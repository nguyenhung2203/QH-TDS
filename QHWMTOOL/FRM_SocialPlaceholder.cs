namespace QHWMTOOL
{
    public partial class FRM_SocialPlaceholder : Form
    {
        public FRM_SocialPlaceholder()
        {
            InitializeComponent();
            ApplyContent(
                "Mô-đun mạng xã hội",
                "Khu vực này là khung chờ để gắn các tác vụ riêng cho từng nền tảng.",
                Color.FromArgb(59, 130, 246));
        }

        public FRM_SocialPlaceholder(string title, string description, Color accentColor)
            : this()
        {
            ApplyContent(title, description, accentColor);
        }

        private void ApplyContent(string title, string description, Color accentColor)
        {
            Text = title;
            LBL_Title.Text = title;
            LBL_Description.Text = description;
            PNL_Accent.BackColor = accentColor;
            PNL_Line1.BackColor = accentColor;
            PNL_Line2.BackColor = accentColor;
            PNL_Line3.BackColor = accentColor;

            LBL_Value1.Text = "Quản lý theo từng nền tảng";
            LBL_Value2.Text = "Sẵn sàng mở rộng thành form tác vụ riêng";
            LBL_Value3.Text = "Có thể thêm dashboard, lịch sử chạy và worker riêng";
        }
    }
}
