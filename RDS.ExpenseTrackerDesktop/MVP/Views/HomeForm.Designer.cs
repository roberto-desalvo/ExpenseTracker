namespace RDS.ExpenseTrackerDesktop
{
    partial class HomeForm
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            LoadExcelDataBtn = new Button();
            panel1 = new Panel();
            SellaLabel = new Label();
            AccountGroupBox = new GroupBox();
            HypeLabel = new Label();
            ContantiLabel = new Label();
            SatispayLabel = new Label();
            SellaAvailabilityLabel = new Label();
            HypeAvailabilityLabel = new Label();
            SatispayAvailabilityLabel = new Label();
            ContantiAvailabilityLabel = new Label();
            panel1.SuspendLayout();
            AccountGroupBox.SuspendLayout();
            SuspendLayout();
            // 
            // LoadExcelDataBtn
            // 
            LoadExcelDataBtn.BackColor = SystemColors.WindowFrame;
            LoadExcelDataBtn.Font = new Font("Segoe UI Emoji", 9F, FontStyle.Regular, GraphicsUnit.Point);
            LoadExcelDataBtn.ForeColor = SystemColors.ButtonHighlight;
            LoadExcelDataBtn.Location = new Point(1116, 4);
            LoadExcelDataBtn.Margin = new Padding(3, 4, 3, 4);
            LoadExcelDataBtn.Name = "LoadExcelDataBtn";
            LoadExcelDataBtn.Padding = new Padding(2, 0, 0, 2);
            LoadExcelDataBtn.Size = new Size(144, 31);
            LoadExcelDataBtn.TabIndex = 0;
            LoadExcelDataBtn.Text = "Load Excel Data";
            LoadExcelDataBtn.UseVisualStyleBackColor = false;
            LoadExcelDataBtn.Click += LoadExcelDataBtn_Click;
            // 
            // panel1
            // 
            panel1.Controls.Add(AccountGroupBox);
            panel1.Controls.Add(LoadExcelDataBtn);
            panel1.Location = new Point(8, 10);
            panel1.Name = "panel1";
            panel1.Size = new Size(1263, 150);
            panel1.TabIndex = 1;
            // 
            // SellaLabel
            // 
            SellaLabel.AutoSize = true;
            SellaLabel.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point);
            SellaLabel.Location = new Point(6, 23);
            SellaLabel.Name = "SellaLabel";
            SellaLabel.Size = new Size(53, 28);
            SellaLabel.TabIndex = 1;
            SellaLabel.Text = "Sella";
            // 
            // AccountGroupBox
            // 
            AccountGroupBox.Controls.Add(ContantiAvailabilityLabel);
            AccountGroupBox.Controls.Add(SatispayAvailabilityLabel);
            AccountGroupBox.Controls.Add(HypeAvailabilityLabel);
            AccountGroupBox.Controls.Add(SellaAvailabilityLabel);
            AccountGroupBox.Controls.Add(SatispayLabel);
            AccountGroupBox.Controls.Add(ContantiLabel);
            AccountGroupBox.Controls.Add(HypeLabel);
            AccountGroupBox.Controls.Add(SellaLabel);
            AccountGroupBox.Location = new Point(4, 4);
            AccountGroupBox.Name = "AccountGroupBox";
            AccountGroupBox.Size = new Size(347, 136);
            AccountGroupBox.TabIndex = 2;
            AccountGroupBox.TabStop = false;
            AccountGroupBox.Text = "Accounts";
            // 
            // HypeLabel
            // 
            HypeLabel.AutoSize = true;
            HypeLabel.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point);
            HypeLabel.Location = new Point(6, 90);
            HypeLabel.Name = "HypeLabel";
            HypeLabel.Size = new Size(58, 28);
            HypeLabel.TabIndex = 2;
            HypeLabel.Text = "Hype";
            // 
            // ContantiLabel
            // 
            ContantiLabel.AutoSize = true;
            ContantiLabel.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point);
            ContantiLabel.Location = new Point(196, 90);
            ContantiLabel.Name = "ContantiLabel";
            ContantiLabel.Size = new Size(87, 28);
            ContantiLabel.TabIndex = 3;
            ContantiLabel.Text = "Contanti";
            // 
            // SatispayLabel
            // 
            SatispayLabel.AutoSize = true;
            SatispayLabel.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point);
            SatispayLabel.Location = new Point(196, 23);
            SatispayLabel.Name = "SatispayLabel";
            SatispayLabel.Size = new Size(85, 28);
            SatispayLabel.TabIndex = 4;
            SatispayLabel.Text = "Satispay";
            // 
            // SellaAvailabilityLabel
            // 
            SellaAvailabilityLabel.AutoSize = true;
            SellaAvailabilityLabel.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point);
            SellaAvailabilityLabel.ForeColor = SystemColors.ActiveCaptionText;
            SellaAvailabilityLabel.Location = new Point(65, 23);
            SellaAvailabilityLabel.Name = "SellaAvailabilityLabel";
            SellaAvailabilityLabel.Size = new Size(0, 28);
            SellaAvailabilityLabel.TabIndex = 5;
            // 
            // label1
            // 
            HypeAvailabilityLabel.AutoSize = true;
            HypeAvailabilityLabel.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point);
            HypeAvailabilityLabel.ForeColor = SystemColors.ActiveCaptionText;
            HypeAvailabilityLabel.Location = new Point(59, 90);
            HypeAvailabilityLabel.Name = "label1";
            HypeAvailabilityLabel.Size = new Size(0, 28);
            HypeAvailabilityLabel.TabIndex = 6;
            // 
            // label2
            // 
            SatispayAvailabilityLabel.AutoSize = true;
            SatispayAvailabilityLabel.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point);
            SatispayAvailabilityLabel.ForeColor = SystemColors.ActiveCaptionText;
            SatispayAvailabilityLabel.Location = new Point(281, 23);
            SatispayAvailabilityLabel.Name = "label2";
            SatispayAvailabilityLabel.Size = new Size(0, 28);
            SatispayAvailabilityLabel.TabIndex = 7;
            // 
            // label3
            // 
            ContantiAvailabilityLabel.AutoSize = true;
            ContantiAvailabilityLabel.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point);
            ContantiAvailabilityLabel.ForeColor = SystemColors.ActiveCaptionText;
            ContantiAvailabilityLabel.Location = new Point(281, 90);
            ContantiAvailabilityLabel.Name = "label3";
            ContantiAvailabilityLabel.Size = new Size(0, 28);
            ContantiAvailabilityLabel.TabIndex = 8;
            // 
            // HomeForm
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = SystemColors.Window;
            ClientSize = new Size(1283, 624);
            Controls.Add(panel1);
            ForeColor = SystemColors.ActiveCaption;
            Margin = new Padding(3, 4, 3, 4);
            Name = "HomeForm";
            Text = "Home";
            panel1.ResumeLayout(false);
            AccountGroupBox.ResumeLayout(false);
            AccountGroupBox.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private Button LoadExcelDataBtn;
        private Panel panel1;
        private GroupBox AccountGroupBox;
        private Label ContantiAvailabilityLabel;
        private Label SatispayAvailabilityLabel;
        private Label HypeAvailabilityLabel;
        private Label SellaAvailabilityLabel;
        private Label SatispayLabel;
        private Label ContantiLabel;
        private Label HypeLabel;
        private Label SellaLabel;
    }
}