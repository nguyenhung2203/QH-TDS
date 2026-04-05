using Leaf.xNet;
using Newtonsoft.Json;
using Sunny.UI;
using Sunny.UI.Win32;
using System.Text.RegularExpressions;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace QHWMTOOL
{
    public partial class FRM_Chinh : Form
    {
        public FRM_Chinh()
        {
            InitializeComponent();
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
        }

        bool dangChay = false;
        string TDS_token = "";
        string fb_dtsg = "";
        string lsd = "";
        string cookie = "";
        string userId = "";
        string userName = "";
        bool dangLoadForm = true;
        string fields = "facebook_reaction";
        private async void BTN_BatDau_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(TDS_token))
            {
                MessageBox.Show("Vui lòng nhập Token TDS!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (string.IsNullOrEmpty(cookie))
            {
                MessageBox.Show("Vui lòng nhập Cookie FB!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (int.TryParse(TXB_DelayMin.Text, out int delayMin) && int.TryParse(TXB_DelayMax.Text, out int delayMax) && delayMin > delayMax)
            {
                MessageBox.Show("Giá trị Delay Min phải nhỏ hơn hoặc bằng Delay Max!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (!int.TryParse(TXB_TongNhiemVu.Text, out int tongNhiemVu) || tongNhiemVu <= 0)
            {
                MessageBox.Show("Vui lòng nhập số nguyên dương cho Tổng Nhiệm Vụ!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (!int.TryParse(TXB_SoNghiemVuMoiSet.Text, out int soNhiemVuMoiSet) || soNhiemVuMoiSet <= 0)
            {
                MessageBox.Show("Vui lòng nhập số nguyên dương cho Số Nhiệm Vụ Mỗi Set!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (!int.TryParse(TXB_SoDayNghi.Text, out int soDayNghi) || soDayNghi <= 0)
            {
                MessageBox.Show("Vui lòng nhập số nguyên dương cho Số Giây Nghỉ!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (string.IsNullOrEmpty(fb_dtsg) || string.IsNullOrEmpty(userName) || string.IsNullOrEmpty(lsd) || string.IsNullOrEmpty(userId))
            {
                await GetFacebook();
            }

            LBL_CookieFb.Text = "Profile FB";
            TXB_Cookie.Text = $"ID USER : {userId} | NAME USER : {userName}";
            TXB_TokenTDS.PasswordChar = '*';
            RTB_Chinh.Clear();
            dangChay = true;
            BTN_BatDau.Enabled = false;
            BTN_KetThuc.Enabled = true;
            AnHetCacNut();
            RTB_Chinh.AppendText(">>> TOOL BẮT ĐẦU CHẠY <<<" + Environment.NewLine);

            var loaiDaChon = GRB_TheLoai.Controls.OfType<UICheckBox>()
                                       .Where(cb => cb.Checked)
                                       .Select(cb => cb.Text?.ToString())
                                       .ToList();

            string type = "LIKE";
            if (loaiDaChon.Count == 6)
            {
                type = "ALL";
            }
            else if (loaiDaChon.Count > 0)
            {
                type = string.Join(", ", loaiDaChon);
            }
            int NhiemVuDaLam = 0;
            int TongNhiemVu = int.TryParse(TXB_TongNhiemVu.Text, out int result) ? result : 0;
            int nhiemVuMoiSet = int.Parse(TXB_SoNghiemVuMoiSet.Text);
            RTB_Log.AppendText($"Loại nhiệm vụ đã chọn: {type}" + Environment.NewLine);
            if (TongNhiemVu < nhiemVuMoiSet)
            {
                nhiemVuMoiSet = TongNhiemVu / 2;
                TXB_SoNghiemVuMoiSet.Text = nhiemVuMoiSet.ToString();
            }
            while (dangChay && TongNhiemVu > 0)
            {
                List<NhiemVuTDS> danhSachNhiemVu = await LayNhiemVuTDS(type);

                if (danhSachNhiemVu == null)
                {
                    continue;
                }

                if (danhSachNhiemVu.Count == 0)
                {
                    fields = fields == "facebook_reaction" ? "facebook_reaction2" : "facebook_reaction";
                    await ChoNghi(120, "Tạm hết nhiệm vụ, đổi type và đợi đợt mới");
                    continue;
                }

                RTB_Log.AppendText($"Đã lấy được {danhSachNhiemVu.Count} nhiệm vụ từ TDS!" + Environment.NewLine);

                foreach (var nhiemVu in danhSachNhiemVu)
                {
                    if (!dangChay) break;

                    string idBaiViet = nhiemVu.id.Contains("_") ? nhiemVu.id.Split('_')[1] : nhiemVu.id;
                    string idUser = nhiemVu.id.Contains("_") ? nhiemVu.id.Split('_')[0] : "";

                    var ketQua = await React(nhiemVu, nhiemVu.type);
                    if (ketQua)
                    {
                        var (resData, errorMsg) = await NhanXu(fields, nhiemVu.code);
                        if (resData != null)
                        {
                            NhiemVuDaLam++;
                            AppendLog($"[{DateTime.Now:HH:mm:ss}] [{NhiemVuDaLam}] ID USER : {idUser} | ID POST : {idBaiViet} | {resData.msg} | TỔNG XU : {resData.xu:N0}", Color.LimeGreen, RTB_Chinh);

                            this.Invoke(new Action(() =>
                            {
                                TXB_TongXu.Text = resData.xu.ToString("N0");
                            }));
                        }
                        else
                        {

                            AppendLog($"[{DateTime.Now:HH:mm:ss}] {errorMsg} | ID POST : {idBaiViet} | TYPE : {nhiemVu.type}", Color.Red, RTB_Chinh);
                        }
                    }
                    else
                    {
                        AppendLog($"[{DateTime.Now:HH:mm:ss}] FACEBOOK {nhiemVu.type} | ID POST {idBaiViet} thất bại !!!", Color.Red, RTB_Chinh);
                    }

                    RTB_Chinh.SelectionStart = RTB_Chinh.Text.Length;
                    Random rd = new Random();
                    int min = int.Parse(TXB_DelayMin.Text);
                    int max = int.Parse(TXB_DelayMax.Text);
                    int deLay = rd.Next(min, max);
                    int thoiGianChoNhiemVuMoiSet = int.Parse(TXB_SoDayNghi.Text);
                    string thongBaoNghi = $"[{NhiemVuDaLam + 1}] Chờ nghỉ";
                    if (NhiemVuDaLam > 0 && NhiemVuDaLam % nhiemVuMoiSet == 0)
                    {
                        deLay = thoiGianChoNhiemVuMoiSet;
                        thongBaoNghi = $"Đã hoàn thành {NhiemVuDaLam} nhiệm vụ, nghỉ {thoiGianChoNhiemVuMoiSet}s trước khi lấy nhiệm vụ mới";

                    }

                    if (nhiemVu == danhSachNhiemVu.Last())
                    {
                        break;
                    }


                    await ChoNghi(deLay, thongBaoNghi);
                };
                await ChoNghi(120, "Đợi lấy nhiệm vụ đợt mới");
            }

            RTB_Chinh.AppendText($"Đã hoàn thành {TongNhiemVu} nhiệm vụ,tạm biệt và hẹn gặp lại");
        }

        private async Task ChoNghi(int giay, string thongBao = "Chờ nghỉ")
        {
            RTB_Log.AppendText($"[{DateTime.Now:HH:mm:ss}] {thongBao}: ");
            int viTriBatDau = RTB_Log.TextLength;

            for (int i = giay; i >= 0; i--)
            {
                if (!dangChay) break;

                RTB_Log.Select(viTriBatDau, RTB_Log.TextLength - viTriBatDau);
                RTB_Log.SelectedText = $"{i}s...";

                await Task.Delay(1000);
            }
            RTB_Log.AppendText(Environment.NewLine);
        }
        private void AppendLog(string message, Color color, UIRichTextBox rich)
        {
            rich.SelectionStart = rich.TextLength;
            rich.SelectionLength = 0;
            rich.SelectionColor = color;
            rich.AppendText(message + Environment.NewLine);
            rich.SelectionColor = rich.ForeColor;
            rich.ScrollToCaret();
        }

        private void AnHetCacNut()
        {
            foreach (var cb in GRB_TheLoai.Controls.OfType<UICheckBox>())
            {
                cb.ReadOnly = !cb.ReadOnly;
            }
            foreach (var ch in GRB_CauHinh.Controls.OfType<UITextBox>())
            {
                ch.ReadOnly = !ch.ReadOnly;
            }
            BTN_LuuCauHinh.Enabled = !BTN_LuuCauHinh.Enabled;
        }

        private async Task GetFacebook()
        {
            using (var request = new HttpRequest())
            {
                cookie = TXB_Cookie.Text.Trim().ToString();
                SetGetHeaders(request);

                var response = await Task.Run(() => request.Get("https://www.facebook.com/").ToString());

                string patternqeid = @"\\?""qeid\\?"":\\?""([^""\\]+)\\?"",\\?""u\\?"":\\?""\\?"",\\?""t\\?"":\\?""fb_loggedout\\?""";
                string patterndatr = @"datr=([^;]+)";

                Match matchqeid = Regex.Match(response, patternqeid);
                Match matchdatr = Regex.Match(cookie, patterndatr);

                if (matchqeid.Success && matchdatr.Success)
                {
                    string qeid = matchqeid.Groups[1].Value;
                    string datr = matchdatr.Groups[1].Value;
                    if (qeid != datr)
                    {
                        this.Invoke(new Action(() => { MessageBox.Show("Cookie không hợp lệ, vui lòng kiểm tra lại cookie!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error); }));
                        dangChay = false;
                        return;
                    }
                    else
                    {
                        await GetThongTinFacebook(response);
                    }    
                }      
            }
        }

        private async void BTN_KetThuc_Click(object sender, EventArgs e)
        {
            AnHetCacNut();
            BTN_KetThuc.Enabled = !BTN_KetThuc.Enabled;
            dangChay = false;
            BTN_BatDau.Enabled = true;
            TXB_TokenTDS.PasswordChar = '\0';
            LBL_CookieFb.Text = "Cookie FB";
            TXB_Cookie.Text = cookie;
            RTB_Chinh.AppendText(Environment.NewLine + ">>> Đang dừng Tool, vui lòng đợi hết nhiệm vụ hiện tại..." + Environment.NewLine);
        }

        private async void FRM_Chinh_Load(object sender, EventArgs e)
        {
            LoadCauHinh();
            BTN_KetThuc.Enabled = false;
            var profile = await LayProfileTDS();
            if (profile != null)
            {
                TXB_TongXu.Text = long.TryParse(profile.xu, out long s) ? s.ToString("N0") : "0";
                TXB_XuDie.Text = long.TryParse(profile.xudie, out long a) ? a.ToString("N0") : "0";
                SLT_TenAcc.Text = $"Xin chào: {profile.user} chúc một ngày tốt lành";
            }
        }

        private string GetIconId(string reactionType)
        {
            return reactionType.ToUpper() switch
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
        private async Task GetThongTinFacebook(string response)
        {
            string patternDtsg = @"\""dtsg\"":\{\""token\"":\""([^\""]+)\""";
            Match matchDtsg = Regex.Match(response, patternDtsg);

            string patternName = @"""NAME"":""([^""]+)""";
            Match matchName = Regex.Match(response, patternName);

            if (matchDtsg.Success)
            {
                fb_dtsg = matchDtsg.Groups[1].Value;
            }
            else
            {
                MessageBox.Show("Không tìm thấy fb_dtsg trong cookie! Vui lòng kiểm tra lại!!!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                dangChay = false;
                return;
            }

            if (matchName.Success)
            {

                userName = matchName.Groups[1].Value;
                string name = Regex.Unescape(userName);
                userName = name.Trim();
            }
            else
            {
                MessageBox.Show("Không tìm thấy NAME người dùng trong cookie! Vui lòng kiểm tra lại!!!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            Match matchUserId = Regex.Match(cookie, @"c_user=(\d+)");
            if (matchUserId.Success)
            {
                userId = matchUserId.Groups[1].Value;
            }
            else
            {
                MessageBox.Show("Không tìm thấy ID người dùng trong cookie! Vui lòng kiểm tra lại!!!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                dangChay = false;
                return;
            }

            string pattern = @"\[\""LSD\"",\[\],\{\""token\"":\""([^\""]+)\""\},323\]";
            Match match = Regex.Match(response, pattern);

            if (match.Success)
            {
                lsd = match.Groups[1].Value;
            }
            else
            {
                MessageBox.Show("Không tìm thấy lsd trong cookie! Vui lòng kiểm tra lại!!!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                dangChay = false;
                return;
            }
        }

        private async Task<bool> React(NhiemVuTDS nhiemVu, string loaiNhiemVu)
        {
            var IconId = GetIconId(loaiNhiemVu);
            using (var request = new HttpRequest())
            {
                SetPostHeaders(request);

                string idBaiViet = nhiemVu.id.Split('_')[1];
                string chuoiCanMaHoa = "feedback:" + idBaiViet;
                var idbasse64 = Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(chuoiCanMaHoa));

                string variables = SetVariablesLike(idbasse64, IconId, userId);
                var p = SetFormPost(variables);

                string post = await Task.Run(() => request.Post("https://www.facebook.com/api/graphql/", p).ToString());
                bool daLikeThanhCong = post.Contains("feedback_react") && Regex.IsMatch(post, @"\""id\"":\""([^\""]+)\""");

                bool biLoiXoaBai = post.Contains("1446034") || post.Contains("summary\":\"Nội dung không còn tồn tại\"");

                if (daLikeThanhCong && !biLoiXoaBai)
                {
                    return true;
                }

                if (post.Contains("368") || post.Contains("1390008") || post.Contains("Giờ bạn chưa dùng được tính năng này"))
                {
                    this.Invoke(new Action(() => {AppendLog($"[{DateTime.Now:HH:mm:ss}] ACC BỊ CHẶN LIKE/TYM (SPAM)! DỪNG TOOL...", Color.DarkRed, RTB_Log);}));

                    File.AppendAllText("Log_Spam.txt", $"[{DateTime.Now}] Acc bị chặn tính năng do làm quá nhanh.\n");

                    dangChay = false;
                    return false;
                }

                return false;
            }
        }
        private async Task<(TDSDataSuccess? data, string? error)> NhanXu(string type, string id_job)
        {
            using (var request = new HttpRequest())
            {
                try
                {
                    string url = $"https://traodoisub.com/api/coin/?type={type}&id={id_job}&access_token={TDS_token}";
                    string json = await Task.Run(() => request.Get(url).ToString());
                    var result = JsonConvert.DeserializeObject<TDSResSuccess>(json);

                    if (result != null && !string.IsNullOrEmpty(result.error))
                    {
                        return (null, result.error);
                    }
                    return (result?.data, null);
                }
                catch (Exception ex)
                {
                    return (null, ex.Message);
                }
            }
        }

        private async Task<List<NhiemVuTDS>> LayNhiemVuTDS(string type)
        {
            using (var request = new HttpRequest())
            {
                try
                {
                    string url = $"https://traodoisub.com/api/?fields={fields}&access_token={TDS_token}&type={type}";

                    string json = await Task.Run(() => request.Get(url).ToString());

                    var result = JsonConvert.DeserializeObject<TDSResponse>(json);

                    if (result != null && !string.IsNullOrEmpty(result.error))
                    {
                        int giayCho = result.countdown > 0 ? result.countdown : 60;
                        await ChoNghi(giayCho, $"TDS trả lỗi: {result.error}, đợi sau ");
                        return null!;
                    }
                    return result?.data ?? null!;
                }
                catch
                {
                    return new List<NhiemVuTDS>();
                }
            }
        }

        private async Task<TDSProfileData> LayProfileTDS()
        {
            try
            {
                using (var request = new HttpRequest())
                {
                    string url = $"https://traodoisub.com/api/?fields=profile&access_token={TDS_token}";
                    string json = await Task.Run(() => request.Get(url).ToString());

                    if (string.IsNullOrEmpty(json) || json.Contains("\"error\""))
                    {
                        return null;
                    }

                    var result = JsonConvert.DeserializeObject<TDSProfileResponse>(json);
                    return result?.data;
                }
            }
            catch
            {
                return null;
            }
        }

        private void LuuCauHinh()
        {
            try
            {
                string[] data =
                {
                    TXB_TokenTDS.Text,
                    TXB_DelayMin.Text,
                    TXB_DelayMax.Text,
                    TXB_TongNhiemVu.Text,
                    TXB_SoNghiemVuMoiSet.Text,
                    TXB_SoDayNghi.Text,
                    TXB_Cookie.Text,
                    fb_dtsg ?? "",
                    userName ?? "",
                    lsd ?? "",
                    userId ?? ""
                };
                File.WriteAllLines("cauhinh.txt", data);
                RTB_Log.AppendText(">>> Đã lưu cấu hình thành công!" + Environment.NewLine);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi lưu file: " + ex.Message);
            }
        }

        private void LoadCauHinh()
        {
            try
            {
                if (File.Exists("cauhinh.txt"))
                {
                    string[] data = File.ReadAllLines("cauhinh.txt");

                    if (data.Length >= 11)
                    {
                        TXB_TokenTDS.Text = data[0];
                        TXB_DelayMin.Text = data[1];
                        TXB_DelayMax.Text = data[2];
                        TXB_TongNhiemVu.Text = data[3];
                        TXB_SoNghiemVuMoiSet.Text = data[4];
                        TXB_SoDayNghi.Text = data[5];
                        TXB_Cookie.Text = data[6].Trim();
                        cookie = data[6].Trim() ?? "";
                        fb_dtsg = data[7].Trim() ?? "";
                        userName = data[8].Trim() ?? "";
                        lsd = data[9].Trim() ?? "";
                        userId = data[10].Trim() ?? "";
                    }
                    TDS_token = TXB_TokenTDS.Text.ToString();
                    if (string.IsNullOrEmpty(cookie) || cookie.Length == 0)
                    {
                        dangLoadForm = false;
                    }
                }
            }
            catch { }
        }

        private string SetVariablesLike(string feedbackId, string iconId, string userId)
        {
            var data = new
            {
                input = new
                {
                    feedback_id = feedbackId,
                    feedback_reaction_id = iconId,
                    actor_id = userId,
                    client_mutation_id = "1"
                }
            };

            return JsonConvert.SerializeObject(data);
        }


        private RequestParams SetFormPost(string variables)
        {
            var p = new RequestParams();
            p["av"] = userId;
            p["__aaid"] = "0";
            p["__user"] = userId;
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
            p["fb_dtsg"] = fb_dtsg;
            p["jazoest"] = new Random().Next(10000, 99999).ToString();
            p["lsd"] = lsd;
            p["__spin_r"] = "1036236123";
            p["__spin_b"] = "trunk";
            p["__spin_t"] = "1774881387";
            p["__crn"] = "comet.fbweb.CometSinglePostDialogRoute";
            p["fb_api_caller_class"] = "RelayModern";
            p["fb_api_req_friendly_name"] = "CometUFIFeedbackReactMutation";
            p["server_timestamps"] = "true";
            p["variables"] = variables;
            p["doc_id"] = 34430477113234631;

            return p;
        }
        private void SetGetHeaders(HttpRequest request)
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
            request.AddHeader("sec-ch-ua", "Not:A-Brand\";v=\"99\", \"Google Chrome\";v=\"145\", \"Chromium\";v=\"145");
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
        private void SetPostHeaders(HttpRequest request)
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
            request.AddHeader("X-Fb-Lsd", lsd);
            request.AddHeader("Cookie", cookie);
        }

        private async void BTN_LuuCauHinh_Click(object sender, EventArgs e)
        {
            LuuCauHinh();
            LoadCauHinh();
            if (TXB_TongXu.Text.Length == 0)
            {
                var profile = await LayProfileTDS();
                if (profile != null)
                {
                    TXB_TongXu.Text = long.TryParse(profile.xu, out long s) ? s.ToString("N0") : "0";
                    TXB_XuDie.Text = long.TryParse(profile.xudie, out long a) ? a.ToString("N0") : "0";
                    SLT_TenAcc.Text = $"Xin chào: {profile.user} chúc một ngày tốt lành";
                }
            }    
        }

        private async void TXB_Cookie_TextChanged(object sender, EventArgs e)
        {
            if (dangLoadForm) return;
            if (TXB_Cookie.Text.Length > 50)
            {
                await GetFacebook();
            }
        }

        private void RTB_Chinh_DoubleClick(object sender, EventArgs e)
        {
            RTB_Chinh.Visible = false;
            RTB_Log.Visible = true;
            RTB_Log.BringToFront();
            GRP_HienThi.Text = "Nhật ký hệ thống";
        }

        private void RTB_Log_DoubleClick(object sender, EventArgs e)
        {
            RTB_Log.Visible = false;
            RTB_Chinh.Visible = true;
            RTB_Chinh.BringToFront();
            GRP_HienThi.Text = "Nhật ký công việc";
        }
    }

    public class TDSProfileResponse
    {
        public int success { get; set; }
        public TDSProfileData data { get; set; }
    }

    public class TDSProfileData
    {
        public string user { get; set; }
        public string xu { get; set; }
        public string xudie { get; set; }
    }

    public class NhiemVuTDS
    {
        public string id { get; set; }
        public string code { get; set; }
        public string type { get; set; }
    }
    public class TDSResponse
    {
        public int cache { get; set; }
        public string error { get; set; }
        public int countdown { get; set; }
        public List<NhiemVuTDS> data { get; set; } = new List<NhiemVuTDS>();
    }

    public class TDSResSuccess
    {
        public int success { get; set; }
        public string error { get; set; }
        public TDSDataSuccess data { get; set; } = null!;
    }

    public class TDSDataSuccess
    {
        public long xu { get; set; }
        public int job_success { get; set; }
        public string msg { get; set; }
    }
}
