using Leaf.xNet;
using Newtonsoft.Json;
using System.Text;
using System.Text.RegularExpressions;

namespace QHWMTOOL
{
    public sealed class FacebookService
    {
        private const string HomeUrl = "https://www.facebook.com/";
        private const string GraphQlUrl = "https://www.facebook.com/api/graphql/";

        public async Task<FacebookValidationResult> ValidateSessionAsync(string rawCookie)
        {
            if (string.IsNullOrWhiteSpace(rawCookie))
            {
                return new FacebookValidationResult { ErrorMessage = "Vui lòng nhập Cookie Facebook." };
            }

            var cookie = rawCookie.Trim();

            try
            {
                using var request = new HttpRequest();
                SetGetHeaders(request, cookie);

                var response = await ExecuteAsync(() => request.Get(HomeUrl).ToString());

                var qeidMatch = Regex.Match(response, @"\\?""qeid\\?"":\\?""([^""\\]+)\\?"",\\?""u\\?"":\\?""\\?"",\\?""t\\?"":\\?""fb_loggedout\\?""");
                var datrMatch = Regex.Match(cookie, @"datr=([^;]+)");

                if (!qeidMatch.Success || !datrMatch.Success)
                {
                    return new FacebookValidationResult { ErrorMessage = "Cookie Facebook không hợp lệ." };
                }

                if (!string.Equals(qeidMatch.Groups[1].Value, datrMatch.Groups[1].Value, StringComparison.Ordinal))
                {
                    return new FacebookValidationResult { ErrorMessage = "Cookie Facebook không hợp lệ hoặc đã hết hạn." };
                }

                var dtsgMatch = Regex.Match(response, @"\""dtsg\"":\{\""token\"":\""([^\""]+)\""");
                var nameMatch = Regex.Match(response, @"""NAME"":""([^""]+)""");
                var userIdMatch = Regex.Match(cookie, @"c_user=(\d+)");
                var lsdMatch = Regex.Match(response, @"\[\""LSD\"",\[\],\{\""token\"":\""([^\""]+)\""\},323\]");

                if (!dtsgMatch.Success)
                {
                    return new FacebookValidationResult { ErrorMessage = "Không tìm thấy fb_dtsg từ cookie Facebook." };
                }

                if (!nameMatch.Success)
                {
                    return new FacebookValidationResult { ErrorMessage = "Không tìm thấy tên tài khoản Facebook." };
                }

                if (!userIdMatch.Success)
                {
                    return new FacebookValidationResult { ErrorMessage = "Không tìm thấy User ID trong cookie Facebook." };
                }

                if (!lsdMatch.Success)
                {
                    return new FacebookValidationResult { ErrorMessage = "Không tìm thấy LSD từ cookie Facebook." };
                }

                return new FacebookValidationResult
                {
                    Session = new FacebookSession
                    {
                        Cookie = cookie,
                        FbDtsg = dtsgMatch.Groups[1].Value,
                        UserName = Regex.Unescape(nameMatch.Groups[1].Value).Trim(),
                        UserId = userIdMatch.Groups[1].Value,
                        Lsd = lsdMatch.Groups[1].Value
                    }
                };
            }
            catch (Exception ex)
            {
                return new FacebookValidationResult { ErrorMessage = ex.Message };
            }
        }

        public async Task<FacebookReactResult> ReactAsync(FacebookSession session, NhiemVuTDS task, string reactionType)
        {
            try
            {
                using var request = new HttpRequest();
                SetPostHeaders(request, session);

                var postId = ExtractPostId(task.id);
                var feedbackId = Convert.ToBase64String(Encoding.UTF8.GetBytes($"feedback:{postId}"));
                var variables = JsonConvert.SerializeObject(new
                {
                    input = new
                    {
                        feedback_id = feedbackId,
                        feedback_reaction_id = GetReactionIconId(reactionType),
                        actor_id = session.UserId,
                        client_mutation_id = "1"
                    }
                });

                var postData = BuildFormPost(session, variables);
                var response = await ExecuteAsync(() => request.Post(GraphQlUrl, postData).ToString());

                var success = response.Contains("feedback_react", StringComparison.OrdinalIgnoreCase)
                    && Regex.IsMatch(response, @"\""id\"":\""([^\""]+)\""");

                var deleted = response.Contains("1446034", StringComparison.Ordinal)
                    || response.Contains("summary\":\"Noi dung khong con ton tai\"", StringComparison.Ordinal)
                    || response.Contains("summary\":\"Nội dung không còn tồn tại\"", StringComparison.Ordinal);

                if (success && !deleted)
                {
                    return new FacebookReactResult { Success = true };
                }

                if (response.Contains("368", StringComparison.Ordinal)
                    || response.Contains("1390008", StringComparison.Ordinal)
                    || response.Contains("Giờ bạn chưa dùng được tính năng này", StringComparison.Ordinal)
                    || response.Contains("Gio ban chua dung duoc tinh nang nay", StringComparison.Ordinal))
                {
                    return new FacebookReactResult
                    {
                        IsBlocked = true,
                        ErrorMessage = "Tài khoản Facebook đã bị chặn thả cảm xúc (Spam)."
                    };
                }

                return new FacebookReactResult
                {
                    ErrorMessage = "Facebook không xác nhận thao tác (Có thể do mạng hoặc bài viết lỗi)."
                };
            }
            catch (Exception ex)
            {
                return new FacebookReactResult { ErrorMessage = ex.Message };
            }
        }

        private static string ExtractPostId(string rawId)
        {
            return rawId.Contains('_') ? rawId.Split('_')[1] : rawId;
        }

        private static string GetReactionIconId(string reactionType)
        {
            return reactionType.ToUpperInvariant() switch
            {
                "LIKE" => "1635855486666999",
                "LOVE" => "1678524932434102",
                "CARE" => "613557422527858",
                "HAHA" => "115940658764963",
                "WOW" => "478547315650144",
                "SAD" => "908563459236466",
                "ANGRY" => "444813342392137",
                _ => "1635855486666999"
            };
        }

        private static RequestParams BuildFormPost(FacebookSession session, string variables)
        {
            var p = new RequestParams();
            p["av"] = session.UserId;
            p["__aaid"] = "0";
            p["__user"] = session.UserId;
            p["__a"] = "1";
            p["__req"] = "16";
            p["__hs"] = "20542.HCSV2:comet_pkg.2.1...0";
            p["dpr"] = "1";
            p["__ccg"] = "EXCELLENT";
            p["__rev"] = "1036236123";
            p["__s"] = "enohtk:6pzb0r:kjrjcp";
            p["__hsi"] = "7623057512211941975";
            p["__dyn"] = "7xeUjGU5a5Q1hyoyEqxemh0noeEb8nwgUao4ubyQdwSAx-bwNw9G2Saw8i2S1DwUx60GE5O0BU2_CxS320qa321Rwwwg8a8462mcw8a1TwgEcEhwGxu782lwj8bU9kbxS2617wnE6a1awhUC7Udo5qfK0zEkxe2Gexe5E5e7oqBwJK14xm3y3aexfxmu3W3rwxwhFVovUaU3VwLyEbUGdG1QwVwwwOg2ZwhA4UjyUaUbGxe6Uak0zXxS9wkopg4-6o4e4UO2m3Gfxm2yVU-4FqwIK6E4-mEbU3cwgo-1gweW2K3abxG6E2Kyo3jw";
            p["__csr"] = "gaQ5Y9Ol1Hd2sj6hBEA8syPOOOjY8Rbv2yPN25nczPlOhth7O8Yvli4tpdX_uZEG5b9l4WuCBk-J-rWVp7iqjeXFqAC_HgDvWih25JlWrChaALLF48_8SFlJ99plKBXQnzemDWz8FAXKBgyijWGt6yqbhbiQB9haGl4UxqBpeiWmleOBgkGjBOd2qFeZ6zHAKWBDV22pbgCh6BHQUlBKAE-GBCKVaGGG58GuuVpGzuGCCDQnKqU-iF8OAimcBAnDKaGazrGUGm4F99FUCcSq4FpVAifDjx5bxa9xpalbyVV8gZk8F38zxOdhaBwyVEigtggyWGmbyKWS9G4ayEozoy9yaBAAhaAGi6ECAVe9yTzWBwCx2iqqbCzaJ1YwKimbx66USmQmFVoyq4Uhx2fmeyXD-V8dVoqByEkyoK2GbDyo84exq9xiuQfzoyFXwBBzE98nx6ECu2qaxa2W7U8E8oy7EpoZK264-qdwxgrwpGwtpo5qewmU3jK58521yzawFwwU663PwhE4WcxG2y19wBwRwMyEgwVwwGh1W224o2gwi8cVU6mbxeUlF1-dnuE2wxe4oK5GwDxSiibBwHxq4E4KmfxS2SqUsz-F8R4zVEuwGAG7Hw4IwKBTHpkKSBxSOo9iQp7mKA1xx21_g1pk0big76m4u0q2dw3Yo0pCw2AC4Wgx1qawg80oyeibm971m3l0aedw5lwbp1Jp8J3p984V3Vo2bw5TQ1yo2sw3OoowOm9yp847o1Porx53QnhU09qo05Xm1qC88o0QO8u2G0No0szxW07545831nByQdBc04UU2Dyp4iU1-k0Zm4Ube16gdqPH_weG3qkE33wcK0oO3N3Uig1qo3Sm2S1rw8S8o5W0B84K0oi0iifg14U2VwOnxmGU4ii091x-aAK487zgCaK0eVUfVUK0TE2lz60kQOxt4ct7NmkGwpouUakiaxK5p83kgty98x0f3whUt84U1eC18xt1Hl0baQ0sO6UB0aCucAgx344OamQ4qCTOa5Q6Jw282xi3m0hCcz4i0gR0Dw3BUaykigi5U14e1_y83bc2S3a26084g2_5UF3re0d5g08g81ro2CwWwgo6O0d_ge_wkUbo2cGE0zq9la0x9EGagS9wiof92w8W0ES1HAUy363q8gfA1shu0oe1mw32o0gJy74o5q2a2O0azAwmU4a0EE11Eck084w9W1aa3O1kG0li09EwFUEo1hBo4e";
            p["__hsdp"] = "g5ZQgizEd8ogmwMo89E8Ey2aq446A1wy6aB8G4cqwjQw_iaxFqaz9n236EG3Am43jMSg4yDsj9114A3sT8xlhaiGkhBBp0zpcwQz8W10F7b78h2N7mzDfeIqz9IWsbiE86iChAW6cgMmE63px0UOC5IF1O512Bgmaicb61kk9AoGEB8wB18wsxS9iyxiai9ADhkElDotUhFkhekMgUwmFpmu6ZBbAabyV34GFF4gip3kkOa5ne8H2KQ9zQ4966CfKfh4gHoolwJe5d8joEg5olQA4A9wAgyawzCCKqq9zFU98vHFjowOjAglokU4qu8wVyUW3iu58J0xykL8KbgOA2O3Kh0TwygOGyHzsw_hA2pAorwg8y5ofCNgrxaF8pUWcw8yunJ38SbAxydxm2S11wzwVg8Gy3wjocobo2myFia2a489EiK2CcwEwQ_F6wio4q3hFQ2i0DbxO1PxO1hwDjyUlKbwWwSwio3wwPG9wnaCw8G1LwPwSzbxSq5k1xwq84e0D8c8vwVw9OUoxa16xh0k9FEfU4a2G3yUGewto26wXw47wPwmEswee261bwgEmyU1BUpzojxa1Lw4zw45xy2-i2e5US1FKU4p0iUkwIwHwmVouwRw4vwayawq824yU7mewww9SE760_UnwNwqoswnoeU8lwxwQwnoeUrwIw5XxV6whUuwbW0MUpweK1zwpecyojwWwhVEhxK0D9-0ha16xq4O09q1dw4pxi4oCawgE9UaUbE4a262y1ew4mwlUjwc-0j-2i1fyU421myU4O";
            p["__hblp"] = "0Dqg4S5t0a26S3e0xEbU5V7CBwZwlEK2e0VE9oGczFE8U4259pE29wt8fUy6UiwmoN0Sxa1axCiuvyEuwxyd127EpyGwjo9o5q4E-mm1bgvwTzo25CDw865Utxi18DwjEW1twywBgfo423Ba7EOJyrwyG3nxK1vw9-0HEKcw8vwPwtUqy84K1DwiEe8tyU8EgwpUCEa8dHF6wio4q1zwTwOwRxO1Pwn8hwDwsodE1aEb9GwpGCwq88oiwn98Gm7o2MwQzUf8SdwgU4O6UOfxqfwwx2eG3q5EmwiUfUiCw_wIwVCwhE4a1Dwo8nxqawGzo4505qxO4898460UU8ouwIxK2q5EK220Eo6i3G3i2u4UiwrU23w9-1Jx61uwCxy5FlzV88Unzo984iXK6Eqz42q2i58qx66EgwVxSVouwRxe4889oW2l0ywlE8Qcgeo5-awkElw8ibwtobU2wwl8swlE2Exu361vyomLwhEmwXxmewxG362a3G3KbzUixC0ge1TDxp6yE4OewEw9m0wQ3W6rwzwExm48G2-8wLx-2m7of89-cyojg8orwgQm4orw48BzUnyU6ifxeUbqxq4O0Ewro4S1rw9yexm1hxC4E9V8nGbKEjxe2q48G2q58Wim12mcy8qwpXwqoS1nxe1yxW327e12Cwzyaz40zEvCg8UtwNCAwLx26o9UhGbG2ui9yoyfw";
            p["__sjsp"] = "g5ZQgizEd8ogmwMo89E8Ey2aq446A1wCsGkyEgNGscNd8YbQyEqmyCiik8cnEG358k4jjMSg4yDsj9114A3sT8xlvuqFh6mlA2dASep264PiOi24yi8gx4b4qWJyqEwjIEyQS3mVEOgwmAwl9VzzTyE9UowCz62KbxWhyk4A3O61iai9zpk1ng3bzo6B1B0Owfx055g2Fwbfw4ow";
            p["__comet_req"] = "15";
            p["fb_dtsg"] = session.FbDtsg;
            p["jazoest"] = Random.Shared.Next(10000, 99999).ToString();
            p["lsd"] = session.Lsd;
            p["__spin_r"] = "1036236123";
            p["__spin_b"] = "trunk";
            p["__spin_t"] = "1774881387";
            p["__crn"] = "comet.fbweb.CometSinglePostDialogRoute";
            p["fb_api_caller_class"] = "RelayModern";
            p["fb_api_req_friendly_name"] = "CometUFIFeedbackReactMutation";
            p["server_timestamps"] = "true";
            p["variables"] = variables;
            p["doc_id"] = "34430477113234631";
            return p;
        }

        private static void SetGetHeaders(HttpRequest request, string cookie)
        {
            request.ClearAllHeaders();
            request.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/145.0.0.0 Safari/537.36";
            request.KeepAlive = true;
            request.AllowAutoRedirect = true;
            request.AddHeader("accept", "text/html,application/xhtml+xml,application/xml;q=0.9,image/avif,image/webp,image/apng,*/*;q=0.8,application/signed-exchange;v=b3;q=0.7");
            request.AddHeader("accept-language", "vi,en;q=0.9");
            request.AddHeader("cache-control", "max-age=0");
            request.AddHeader("dpr", "1.25");
            request.AddHeader("priority", "u=0,i");
            request.AddHeader("sec-ch-prefers-color-scheme", "dark");
            request.AddHeader("sec-ch-ua", "Not:A-Brand\";v=\"99\", \"Google Chrome\";v=\"145\", \"Chromium\";v=\"145\"");
            request.AddHeader("sec-ch-ua-full-version-list", "\"Chromium\";v=\"146.0.7680.165\", \"Not-A.Brand\";v=\"24.0.0.0\", \"Google Chrome\";v=\"146.0.7680.165\"");
            request.AddHeader("sec-ch-ua-mobile", "?0");
            request.AddHeader("sec-ch-ua-platform", "\"Windows\"");
            request.AddHeader("sec-ch-ua-platform-version", "\"19.0.0\"");
            request.AddHeader("sec-fetch-dest", "document");
            request.AddHeader("sec-fetch-mode", "navigate");
            request.AddHeader("sec-fetch-site", "same-origin");
            request.AddHeader("sec-fetch-user", "?1");
            request.AddHeader("upgrade-insecure-requests", "1");
            request.AddHeader("viewport-width", "444");
            request.AddHeader("Cookie", cookie);
        }

        private static void SetPostHeaders(HttpRequest request, FacebookSession session)
        {
            request.ClearAllHeaders();
            request.UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/146.0.0.0 Safari/537.36";
            request.KeepAlive = true;
            request.AllowAutoRedirect = false;
            request.AddHeader("accept", "*/*");
            request.AddHeader("accept-language", "vi,en;q=0.9");
            request.AddHeader("content-type", "application/x-www-form-urlencoded");
            request.AddHeader("origin", "https://www.facebook.com");
            request.AddHeader("priority", "u=0, i");
            request.AddHeader("referer", "https://www.facebook.com/");
            request.AddHeader("sec-ch-prefers-color-scheme", "dark");
            request.AddHeader("sec-ch-ua", "\"Chromium\";v=\"146\", \"Not-A.Brand\";v=\"24\", \"Google Chrome\";v=\"146\"");
            request.AddHeader("sec-ch-ua-full-version-list", "\"Chromium\";v=\"146.0.7680.165\", \"Not-A.Brand\";v=\"24.0.0.0\", \"Google Chrome\";v=\"146.0.7680.165\"");
            request.AddHeader("sec-ch-ua-mobile", "?0");
            request.AddHeader("sec-ch-ua-platform", "\"Windows\"");
            request.AddHeader("sec-ch-ua-platform-version", "\"19.0.0\"");
            request.AddHeader("sec-fetch-dest", "empty");
            request.AddHeader("sec-fetch-mode", "cors");
            request.AddHeader("sec-fetch-site", "same-origin");
            request.AddHeader("x-asbd-id", "359341");
            request.AddHeader("x-fb-friendly-name", "CometUFIFeedbackReactMutation");
            request.AddHeader("X-Fb-Lsd", session.Lsd);
            request.AddHeader("Cookie", session.Cookie);
        }

        private static Task<string> ExecuteAsync(Func<string> action)
        {
            return Task.Run(action).WaitAsync(TimeSpan.FromSeconds(20));
        }
    }
}