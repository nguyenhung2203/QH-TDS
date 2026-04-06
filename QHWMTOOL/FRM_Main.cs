using System.ComponentModel;

namespace QHWMTOOL
{
    public partial class FRM_Main : Form
    {
        private readonly Dictionary<string, Form> _sections = new();
        private Button? _activeButton;

        public FRM_Main()
        {
            InitializeComponent();
            ApplyMenuButtonStyle(BTN_Facebook, "Facebook");
            ApplyMenuButtonStyle(BTN_Instagram, "Instagram");
            ApplyMenuButtonStyle(BTN_TikTok, "TikTok");

            if (!IsInDesignMode())
            {
                OpenSection("facebook");
            }
        }

        private bool IsInDesignMode()
        {
            return LicenseManager.UsageMode == LicenseUsageMode.Designtime || DesignMode;
        }

        private static void ApplyMenuButtonStyle(Button button, string title)
        {
            button.UseCompatibleTextRendering = true;
            button.Text = $"{title}{Environment.NewLine}";
        }

        private void BTN_Facebook_Click(object? sender, EventArgs e)
        {
            OpenSection("facebook");
        }

        private void BTN_Instagram_Click(object? sender, EventArgs e)
        {
            OpenSection("instagram");
        }

        private void BTN_TikTok_Click(object? sender, EventArgs e)
        {
            OpenSection("tiktok");
        }

        private void OpenSection(string key)
        {
            if (IsInDesignMode())
            {
                return;
            }

            var form = GetOrCreateSection(key);

            foreach (var section in _sections.Values)
            {
                section.Hide();
            }

            HighlightButton(key);
            UpdateHeader(key);

            if (!PNL_Content.Controls.Contains(form))
            {
                form.TopLevel = false;
                form.FormBorderStyle = FormBorderStyle.None;
                form.Dock = DockStyle.Fill;
                form.Visible = true;
                PNL_Content.Controls.Add(form);
            }

            form.Show();
            form.BringToFront();
        }

        private Form GetOrCreateSection(string key)
        {
            if (_sections.TryGetValue(key, out var existing))
            {
                return existing;
            }

            Form form = key switch
            {
                "facebook" => new FRM_Facebook(),
                "instagram" => new FRM_SocialPlaceholder(
                    "Instagram",
                    "Khu vực này đã sẵn sàng để gắn tác vụ Instagram. Có thể thêm đăng nhập, lấy nhiệm vụ, dashboard và lịch sử chạy riêng.",
                    Color.FromArgb(225, 48, 108)),
                "tiktok" => new FRM_SocialPlaceholder(
                    "TikTok",
                    "Khu vực này đã sẵn sàng để gắn tác vụ TikTok. Có thể thêm profile worker, thống kê phiên và điều khiển hàng đợi riêng.",
                    Color.FromArgb(15, 23, 42)),
                _ => new FRM_SocialPlaceholder(
                    "Mô-đun",
                    "Khu vực này đang được khởi tạo.",
                    Color.FromArgb(59, 130, 246))
            };

            _sections[key] = form;
            return form;
        }

        private void HighlightButton(string key)
        {
            var button = key switch
            {
                "facebook" => BTN_Facebook,
                "instagram" => BTN_Instagram,
                "tiktok" => BTN_TikTok,
                _ => BTN_Facebook
            };

            if (_activeButton is not null)
            {
                _activeButton.BackColor = Color.FromArgb(31, 41, 55);
            }

            button.BackColor = Color.FromArgb(37, 99, 235);
            _activeButton = button;
        }

        private void UpdateHeader(string key)
        {
            switch (key)
            {
                case "facebook":
                    LBL_Title.Text = "Facebook";
                    LBL_Subtitle.Text = "Khu vực vận hành tác vụ Facebook và bảng điều khiển chính.";
                    break;
                case "instagram":
                    LBL_Title.Text = "Instagram";
                    LBL_Subtitle.Text = "Không gian dành cho các tác vụ Instagram khi anh triển khai tiếp.";
                    break;
                case "tiktok":
                    LBL_Title.Text = "TikTok";
                    LBL_Subtitle.Text = "Không gian dành cho các tác vụ TikTok khi anh triển khai tiếp.";
                    break;
                default:
                    LBL_Title.Text = "Mạng xã hội";
                    LBL_Subtitle.Text = "Chọn một khu vực từ menu bên trái.";
                    break;
            }
        }
    }
}
