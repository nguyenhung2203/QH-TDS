namespace QHWMTOOL
{
    partial class FRM_SocialPlaceholder
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
            PNL_Card = new Panel();
            LBL_Note = new Label();
            TLP_Cards = new TableLayoutPanel();
            PNL_Card3 = new Panel();
            LBL_Value3 = new Label();
            LBL_CardTitle3 = new Label();
            PNL_Line3 = new Panel();
            PNL_Card2 = new Panel();
            LBL_Value2 = new Label();
            LBL_CardTitle2 = new Label();
            PNL_Line2 = new Panel();
            PNL_Card1 = new Panel();
            LBL_Value1 = new Label();
            LBL_CardTitle1 = new Label();
            PNL_Line1 = new Panel();
            LBL_Description = new Label();
            LBL_Title = new Label();
            PNL_Accent = new Panel();
            PNL_Card.SuspendLayout();
            TLP_Cards.SuspendLayout();
            PNL_Card3.SuspendLayout();
            PNL_Card2.SuspendLayout();
            PNL_Card1.SuspendLayout();
            SuspendLayout();
            // 
            // PNL_Card
            // 
            PNL_Card.BackColor = Color.White;
            PNL_Card.Controls.Add(LBL_Note);
            PNL_Card.Controls.Add(TLP_Cards);
            PNL_Card.Controls.Add(LBL_Description);
            PNL_Card.Controls.Add(LBL_Title);
            PNL_Card.Controls.Add(PNL_Accent);
            PNL_Card.Dock = DockStyle.Fill;
            PNL_Card.Location = new Point(16, 16);
            PNL_Card.Name = "PNL_Card";
            PNL_Card.Padding = new Padding(34);
            PNL_Card.Size = new Size(1168, 708);
            PNL_Card.TabIndex = 0;
            // 
            // LBL_Note
            // 
            LBL_Note.Dock = DockStyle.Top;
            LBL_Note.Font = new Font("Segoe UI", 10.5F, FontStyle.Italic, GraphicsUnit.Point, 0);
            LBL_Note.ForeColor = Color.FromArgb(100, 116, 139);
            LBL_Note.Location = new Point(34, 298);
            LBL_Note.Name = "LBL_Note";
            LBL_Note.Padding = new Padding(0, 18, 0, 0);
            LBL_Note.Size = new Size(1100, 58);
            LBL_Note.TabIndex = 4;
            LBL_Note.Text = "Form này đang là khung chờ. Khi cần, tôi có thể gắn thêm logic nhiệm vụ, dashboard và lịch sử chạy riêng cho nền tảng này.";
            // 
            // TLP_Cards
            // 
            TLP_Cards.ColumnCount = 3;
            TLP_Cards.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33.33333F));
            TLP_Cards.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33.33333F));
            TLP_Cards.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33.33333F));
            TLP_Cards.Controls.Add(PNL_Card3, 2, 0);
            TLP_Cards.Controls.Add(PNL_Card2, 1, 0);
            TLP_Cards.Controls.Add(PNL_Card1, 0, 0);
            TLP_Cards.Dock = DockStyle.Top;
            TLP_Cards.Location = new Point(34, 138);
            TLP_Cards.Name = "TLP_Cards";
            TLP_Cards.Padding = new Padding(0, 26, 0, 0);
            TLP_Cards.RowCount = 1;
            TLP_Cards.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            TLP_Cards.Size = new Size(1100, 160);
            TLP_Cards.TabIndex = 3;
            // 
            // PNL_Card3
            // 
            PNL_Card3.BackColor = Color.FromArgb(248, 250, 252);
            PNL_Card3.BorderStyle = BorderStyle.FixedSingle;
            PNL_Card3.Controls.Add(LBL_Value3);
            PNL_Card3.Controls.Add(LBL_CardTitle3);
            PNL_Card3.Controls.Add(PNL_Line3);
            PNL_Card3.Dock = DockStyle.Fill;
            PNL_Card3.Location = new Point(737, 29);
            PNL_Card3.Margin = new Padding(5, 3, 0, 3);
            PNL_Card3.Name = "PNL_Card3";
            PNL_Card3.Size = new Size(363, 128);
            PNL_Card3.TabIndex = 2;
            // 
            // LBL_Value3
            // 
            LBL_Value3.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            LBL_Value3.Font = new Font("Segoe UI Semibold", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            LBL_Value3.ForeColor = Color.FromArgb(15, 23, 42);
            LBL_Value3.Location = new Point(24, 45);
            LBL_Value3.Name = "LBL_Value3";
            LBL_Value3.Size = new Size(320, 56);
            LBL_Value3.TabIndex = 2;
            LBL_Value3.Text = "Nội dung";
            // 
            // LBL_CardTitle3
            // 
            LBL_CardTitle3.AutoSize = false;
            LBL_CardTitle3.Font = new Font("Segoe UI", 10F, FontStyle.Regular, GraphicsUnit.Point, 0);
            LBL_CardTitle3.ForeColor = Color.FromArgb(100, 116, 139);
            LBL_CardTitle3.Location = new Point(24, 16);
            LBL_CardTitle3.Name = "LBL_CardTitle3";
            LBL_CardTitle3.Size = new Size(260, 22);
            LBL_CardTitle3.TabIndex = 1;
            LBL_CardTitle3.Text = "Mở rộng";
            // 
            // PNL_Line3
            // 
            PNL_Line3.BackColor = Color.FromArgb(59, 130, 246);
            PNL_Line3.Dock = DockStyle.Left;
            PNL_Line3.Location = new Point(0, 0);
            PNL_Line3.Name = "PNL_Line3";
            PNL_Line3.Size = new Size(8, 126);
            PNL_Line3.TabIndex = 0;
            // 
            // PNL_Card2
            // 
            PNL_Card2.BackColor = Color.FromArgb(248, 250, 252);
            PNL_Card2.BorderStyle = BorderStyle.FixedSingle;
            PNL_Card2.Controls.Add(LBL_Value2);
            PNL_Card2.Controls.Add(LBL_CardTitle2);
            PNL_Card2.Controls.Add(PNL_Line2);
            PNL_Card2.Dock = DockStyle.Fill;
            PNL_Card2.Location = new Point(371, 29);
            PNL_Card2.Margin = new Padding(5, 3, 5, 3);
            PNL_Card2.Name = "PNL_Card2";
            PNL_Card2.Size = new Size(356, 128);
            PNL_Card2.TabIndex = 1;
            // 
            // LBL_Value2
            // 
            LBL_Value2.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            LBL_Value2.Font = new Font("Segoe UI Semibold", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            LBL_Value2.ForeColor = Color.FromArgb(15, 23, 42);
            LBL_Value2.Location = new Point(24, 45);
            LBL_Value2.Name = "LBL_Value2";
            LBL_Value2.Size = new Size(313, 56);
            LBL_Value2.TabIndex = 2;
            LBL_Value2.Text = "Nội dung";
            // 
            // LBL_CardTitle2
            // 
            LBL_CardTitle2.AutoSize = false;
            LBL_CardTitle2.Font = new Font("Segoe UI", 10F, FontStyle.Regular, GraphicsUnit.Point, 0);
            LBL_CardTitle2.ForeColor = Color.FromArgb(100, 116, 139);
            LBL_CardTitle2.Location = new Point(24, 16);
            LBL_CardTitle2.Name = "LBL_CardTitle2";
            LBL_CardTitle2.Size = new Size(260, 22);
            LBL_CardTitle2.TabIndex = 1;
            LBL_CardTitle2.Text = "Trạng thái";
            // 
            // PNL_Line2
            // 
            PNL_Line2.BackColor = Color.FromArgb(59, 130, 246);
            PNL_Line2.Dock = DockStyle.Left;
            PNL_Line2.Location = new Point(0, 0);
            PNL_Line2.Name = "PNL_Line2";
            PNL_Line2.Size = new Size(8, 126);
            PNL_Line2.TabIndex = 0;
            // 
            // PNL_Card1
            // 
            PNL_Card1.BackColor = Color.FromArgb(248, 250, 252);
            PNL_Card1.BorderStyle = BorderStyle.FixedSingle;
            PNL_Card1.Controls.Add(LBL_Value1);
            PNL_Card1.Controls.Add(LBL_CardTitle1);
            PNL_Card1.Controls.Add(PNL_Line1);
            PNL_Card1.Dock = DockStyle.Fill;
            PNL_Card1.Location = new Point(0, 29);
            PNL_Card1.Margin = new Padding(0, 3, 5, 3);
            PNL_Card1.Name = "PNL_Card1";
            PNL_Card1.Size = new Size(361, 128);
            PNL_Card1.TabIndex = 0;
            // 
            // LBL_Value1
            // 
            LBL_Value1.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            LBL_Value1.Font = new Font("Segoe UI Semibold", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            LBL_Value1.ForeColor = Color.FromArgb(15, 23, 42);
            LBL_Value1.Location = new Point(24, 45);
            LBL_Value1.Name = "LBL_Value1";
            LBL_Value1.Size = new Size(318, 56);
            LBL_Value1.TabIndex = 2;
            LBL_Value1.Text = "Nội dung";
            // 
            // LBL_CardTitle1
            // 
            LBL_CardTitle1.AutoSize = false;
            LBL_CardTitle1.Font = new Font("Segoe UI", 10F, FontStyle.Regular, GraphicsUnit.Point, 0);
            LBL_CardTitle1.ForeColor = Color.FromArgb(100, 116, 139);
            LBL_CardTitle1.Location = new Point(24, 16);
            LBL_CardTitle1.Name = "LBL_CardTitle1";
            LBL_CardTitle1.Size = new Size(260, 22);
            LBL_CardTitle1.TabIndex = 1;
            LBL_CardTitle1.Text = "Mục tiêu";
            // 
            // PNL_Line1
            // 
            PNL_Line1.BackColor = Color.FromArgb(59, 130, 246);
            PNL_Line1.Dock = DockStyle.Left;
            PNL_Line1.Location = new Point(0, 0);
            PNL_Line1.Name = "PNL_Line1";
            PNL_Line1.Size = new Size(8, 126);
            PNL_Line1.TabIndex = 0;
            // 
            // LBL_Description
            // 
            LBL_Description.Dock = DockStyle.Top;
            LBL_Description.Font = new Font("Segoe UI", 11F, FontStyle.Regular, GraphicsUnit.Point, 0);
            LBL_Description.ForeColor = Color.FromArgb(71, 85, 105);
            LBL_Description.Location = new Point(34, 66);
            LBL_Description.Name = "LBL_Description";
            LBL_Description.Size = new Size(1100, 72);
            LBL_Description.TabIndex = 2;
            LBL_Description.Text = "Mô tả";
            // 
            // LBL_Title
            // 
            LBL_Title.Dock = DockStyle.Top;
            LBL_Title.Font = new Font("Segoe UI Semibold", 23F, FontStyle.Bold, GraphicsUnit.Point, 0);
            LBL_Title.ForeColor = Color.FromArgb(15, 23, 42);
            LBL_Title.Location = new Point(34, 12);
            LBL_Title.Name = "LBL_Title";
            LBL_Title.Size = new Size(1100, 54);
            LBL_Title.TabIndex = 1;
            LBL_Title.Text = "Tiêu đề";
            // 
            // PNL_Accent
            // 
            PNL_Accent.BackColor = Color.FromArgb(59, 130, 246);
            PNL_Accent.Dock = DockStyle.Top;
            PNL_Accent.Location = new Point(34, 34);
            PNL_Accent.Name = "PNL_Accent";
            PNL_Accent.Size = new Size(1100, 12);
            PNL_Accent.TabIndex = 0;
            // 
            // FRM_SocialPlaceholder
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(242, 246, 252);
            ClientSize = new Size(1200, 740);
            Controls.Add(PNL_Card);
            Font = new Font("Segoe UI", 10F, FontStyle.Regular, GraphicsUnit.Point, 0);
            FormBorderStyle = FormBorderStyle.None;
            Name = "FRM_SocialPlaceholder";
            Padding = new Padding(16);
            Text = "Mô-đun mạng xã hội";
            PNL_Card.ResumeLayout(false);
            TLP_Cards.ResumeLayout(false);
            PNL_Card3.ResumeLayout(false);
            PNL_Card2.ResumeLayout(false);
            PNL_Card1.ResumeLayout(false);
            ResumeLayout(false);
        }

        private Panel PNL_Card;
        private Label LBL_Note;
        private TableLayoutPanel TLP_Cards;
        private Panel PNL_Card3;
        private Label LBL_Value3;
        private Label LBL_CardTitle3;
        private Panel PNL_Line3;
        private Panel PNL_Card2;
        private Label LBL_Value2;
        private Label LBL_CardTitle2;
        private Panel PNL_Line2;
        private Panel PNL_Card1;
        private Label LBL_Value1;
        private Label LBL_CardTitle1;
        private Panel PNL_Line1;
        private Label LBL_Description;
        private Label LBL_Title;
        private Panel PNL_Accent;
    }
}
