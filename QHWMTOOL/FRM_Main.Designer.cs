namespace QHWMTOOL
{
    partial class FRM_Main
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            PNL_Menu = new Panel();
            LBL_MenuFooter = new Label();
            FLP_MenuButtons = new FlowLayoutPanel();
            BTN_Facebook = new Button();
            BTN_Instagram = new Button();
            BTN_TikTok = new Button();
            LBL_Brand = new Label();
            PNL_Header = new Panel();
            LBL_Subtitle = new Label();
            LBL_Title = new Label();
            PNL_Content = new Panel();
            PNL_Menu.SuspendLayout();
            FLP_MenuButtons.SuspendLayout();
            PNL_Header.SuspendLayout();
            SuspendLayout();
            // 
            // PNL_Menu
            // 
            PNL_Menu.BackColor = Color.FromArgb(17, 24, 39);
            PNL_Menu.Controls.Add(LBL_MenuFooter);
            PNL_Menu.Controls.Add(FLP_MenuButtons);
            PNL_Menu.Controls.Add(LBL_Brand);
            PNL_Menu.Dock = DockStyle.Left;
            PNL_Menu.Location = new Point(0, 0);
            PNL_Menu.Name = "PNL_Menu";
            PNL_Menu.Padding = new Padding(18, 22, 18, 22);
            PNL_Menu.Size = new Size(260, 920);
            PNL_Menu.TabIndex = 0;
            // 
            // LBL_MenuFooter
            // 
            LBL_MenuFooter.Dock = DockStyle.Bottom;
            LBL_MenuFooter.Font = new Font("Segoe UI", 9F, FontStyle.Regular, GraphicsUnit.Point, 0);
            LBL_MenuFooter.ForeColor = Color.FromArgb(148, 163, 184);
            LBL_MenuFooter.Location = new Point(18, 838);
            LBL_MenuFooter.Name = "LBL_MenuFooter";
            LBL_MenuFooter.Size = new Size(224, 60);
            LBL_MenuFooter.TabIndex = 3;
            LBL_MenuFooter.Text = "Nguyen Quoc Hung";
            LBL_MenuFooter.TextAlign = ContentAlignment.BottomLeft;
            // 
            // FLP_MenuButtons
            // 
            FLP_MenuButtons.AutoSize = true;
            FLP_MenuButtons.BackColor = Color.Transparent;
            FLP_MenuButtons.Controls.Add(BTN_Facebook);
            FLP_MenuButtons.Controls.Add(BTN_Instagram);
            FLP_MenuButtons.Controls.Add(BTN_TikTok);
            FLP_MenuButtons.Dock = DockStyle.Top;
            FLP_MenuButtons.FlowDirection = FlowDirection.TopDown;
            FLP_MenuButtons.Location = new Point(18, 74);
            FLP_MenuButtons.Margin = new Padding(0);
            FLP_MenuButtons.Name = "FLP_MenuButtons";
            FLP_MenuButtons.Padding = new Padding(0, 24, 0, 0);
            FLP_MenuButtons.Size = new Size(224, 258);
            FLP_MenuButtons.TabIndex = 2;
            FLP_MenuButtons.WrapContents = false;
            // 
            // BTN_Facebook
            // 
            BTN_Facebook.BackColor = Color.FromArgb(31, 41, 55);
            BTN_Facebook.Cursor = Cursors.Hand;
            BTN_Facebook.FlatAppearance.BorderSize = 0;
            BTN_Facebook.FlatStyle = FlatStyle.Flat;
            BTN_Facebook.Font = new Font("Segoe UI Semibold", 11F, FontStyle.Bold, GraphicsUnit.Point, 0);
            BTN_Facebook.ForeColor = Color.White;
            BTN_Facebook.Location = new Point(0, 24);
            BTN_Facebook.Margin = new Padding(0, 0, 0, 12);
            BTN_Facebook.Name = "BTN_Facebook";
            BTN_Facebook.Padding = new Padding(14, 0, 14, 0);
            BTN_Facebook.Size = new Size(224, 66);
            BTN_Facebook.TabIndex = 0;
            BTN_Facebook.TextAlign = ContentAlignment.MiddleLeft;
            BTN_Facebook.UseVisualStyleBackColor = false;
            BTN_Facebook.Click += BTN_Facebook_Click;
            // 
            // BTN_Instagram
            // 
            BTN_Instagram.BackColor = Color.FromArgb(31, 41, 55);
            BTN_Instagram.Cursor = Cursors.Hand;
            BTN_Instagram.FlatAppearance.BorderSize = 0;
            BTN_Instagram.FlatStyle = FlatStyle.Flat;
            BTN_Instagram.Font = new Font("Segoe UI Semibold", 11F, FontStyle.Bold, GraphicsUnit.Point, 0);
            BTN_Instagram.ForeColor = Color.White;
            BTN_Instagram.Location = new Point(0, 102);
            BTN_Instagram.Margin = new Padding(0, 0, 0, 12);
            BTN_Instagram.Name = "BTN_Instagram";
            BTN_Instagram.Padding = new Padding(14, 0, 14, 0);
            BTN_Instagram.Size = new Size(224, 66);
            BTN_Instagram.TabIndex = 1;
            BTN_Instagram.TextAlign = ContentAlignment.MiddleLeft;
            BTN_Instagram.UseVisualStyleBackColor = false;
            BTN_Instagram.Click += BTN_Instagram_Click;
            // 
            // BTN_TikTok
            // 
            BTN_TikTok.BackColor = Color.FromArgb(31, 41, 55);
            BTN_TikTok.Cursor = Cursors.Hand;
            BTN_TikTok.FlatAppearance.BorderSize = 0;
            BTN_TikTok.FlatStyle = FlatStyle.Flat;
            BTN_TikTok.Font = new Font("Segoe UI Semibold", 11F, FontStyle.Bold, GraphicsUnit.Point, 0);
            BTN_TikTok.ForeColor = Color.White;
            BTN_TikTok.Location = new Point(0, 180);
            BTN_TikTok.Margin = new Padding(0, 0, 0, 12);
            BTN_TikTok.Name = "BTN_TikTok";
            BTN_TikTok.Padding = new Padding(14, 0, 14, 0);
            BTN_TikTok.Size = new Size(224, 66);
            BTN_TikTok.TabIndex = 2;
            BTN_TikTok.TextAlign = ContentAlignment.MiddleLeft;
            BTN_TikTok.UseVisualStyleBackColor = false;
            BTN_TikTok.Click += BTN_TikTok_Click;
            // 
            // LBL_Brand
            // 
            LBL_Brand.Dock = DockStyle.Top;
            LBL_Brand.Font = new Font("Segoe UI Semibold", 20F, FontStyle.Bold, GraphicsUnit.Point, 0);
            LBL_Brand.ForeColor = Color.White;
            LBL_Brand.Location = new Point(18, 22);
            LBL_Brand.Name = "LBL_Brand";
            LBL_Brand.Size = new Size(224, 52);
            LBL_Brand.TabIndex = 0;
            LBL_Brand.Text = "QH-TDS";
            LBL_Brand.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // PNL_Header
            // 
            PNL_Header.Controls.Add(LBL_Subtitle);
            PNL_Header.Controls.Add(LBL_Title);
            PNL_Header.Dock = DockStyle.Top;
            PNL_Header.Location = new Point(260, 0);
            PNL_Header.Name = "PNL_Header";
            PNL_Header.Padding = new Padding(26, 20, 26, 10);
            PNL_Header.Size = new Size(1220, 92);
            PNL_Header.TabIndex = 1;
            // 
            // LBL_Subtitle
            // 
            LBL_Subtitle.Dock = DockStyle.Top;
            LBL_Subtitle.Font = new Font("Segoe UI", 10.5F, FontStyle.Regular, GraphicsUnit.Point, 0);
            LBL_Subtitle.ForeColor = Color.FromArgb(71, 85, 105);
            LBL_Subtitle.Location = new Point(26, 62);
            LBL_Subtitle.Name = "LBL_Subtitle";
            LBL_Subtitle.Size = new Size(1168, 32);
            LBL_Subtitle.TabIndex = 1;
            LBL_Subtitle.Text = "Quản lý tác vụ đang hoạt động trong khu vực này.";
            // 
            // LBL_Title
            // 
            LBL_Title.Dock = DockStyle.Top;
            LBL_Title.Font = new Font("Segoe UI Semibold", 22F, FontStyle.Bold, GraphicsUnit.Point, 0);
            LBL_Title.ForeColor = Color.FromArgb(15, 23, 42);
            LBL_Title.Location = new Point(26, 20);
            LBL_Title.Name = "LBL_Title";
            LBL_Title.Size = new Size(1168, 42);
            LBL_Title.TabIndex = 0;
            LBL_Title.Text = "Facebook";
            // 
            // PNL_Content
            // 
            PNL_Content.BackColor = Color.FromArgb(242, 246, 252);
            PNL_Content.Dock = DockStyle.Fill;
            PNL_Content.Location = new Point(260, 92);
            PNL_Content.Name = "PNL_Content";
            PNL_Content.Padding = new Padding(22, 0, 22, 22);
            PNL_Content.Size = new Size(1220, 828);
            PNL_Content.TabIndex = 2;
            // 
            // FRM_Main
            // 
            AutoScaleDimensions = new SizeF(9F, 23F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(242, 246, 252);
            ClientSize = new Size(1480, 920);
            Controls.Add(PNL_Content);
            Controls.Add(PNL_Header);
            Controls.Add(PNL_Menu);
            Font = new Font("Segoe UI", 10F, FontStyle.Regular, GraphicsUnit.Point, 0);
            MinimumSize = new Size(1320, 840);
            Name = "FRM_Main";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "QH-TDS | Trung tâm nhiệm vụ mạng xã hội";
            PNL_Menu.ResumeLayout(false);
            PNL_Menu.PerformLayout();
            FLP_MenuButtons.ResumeLayout(false);
            PNL_Header.ResumeLayout(false);
            ResumeLayout(false);
        }

        private Panel PNL_Menu;
        private Label LBL_MenuFooter;
        private FlowLayoutPanel FLP_MenuButtons;
        private Button BTN_TikTok;
        private Button BTN_Instagram;
        private Button BTN_Facebook;
        private Label LBL_Brand;
        private Panel PNL_Header;
        private Label LBL_Subtitle;
        private Label LBL_Title;
        private Panel PNL_Content;
    }
}
