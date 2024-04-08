namespace ioGame
{
    partial class Form2
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
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
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form2));
            fpsLabel = new Label();
            positionLabel = new Label();
            SuspendLayout();
            // 
            // fpsLabel
            // 
            fpsLabel.AutoSize = true;
            fpsLabel.BackColor = Color.FromArgb(75, 80, 89);
            fpsLabel.ForeColor = SystemColors.ActiveCaption;
            fpsLabel.Location = new Point(10, 8);
            fpsLabel.Margin = new Padding(2, 0, 2, 0);
            fpsLabel.Name = "fpsLabel";
            fpsLabel.Size = new Size(38, 15);
            fpsLabel.TabIndex = 1;
            fpsLabel.Text = "label1";
            // 
            // positionLabel
            // 
            positionLabel.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            positionLabel.AutoSize = true;
            positionLabel.BackColor = Color.FromArgb(75, 80, 89);
            positionLabel.ForeColor = SystemColors.ActiveCaption;
            positionLabel.Location = new Point(13, 835);
            positionLabel.Margin = new Padding(2, 0, 2, 0);
            positionLabel.Name = "positionLabel";
            positionLabel.Size = new Size(38, 15);
            positionLabel.TabIndex = 1;
            positionLabel.Text = "label1";
            positionLabel.TextAlign = ContentAlignment.BottomLeft;
            positionLabel.Click += positionLabel_Click;
            // 
            // Form2
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.FromArgb(30, 31, 34);
            ClientSize = new Size(1584, 861);
            Controls.Add(fpsLabel);
            Controls.Add(positionLabel);
            Icon = (Icon)resources.GetObject("$this.Icon");
            Margin = new Padding(2);
            Name = "Form2";
            Text = "Agario-Gra";
            FormClosing += Form2_FormClosing;
            Load += Form2_Load;
            Paint += Form2_Paint;
            KeyDown += Form2_KeyDown;
            KeyUp += Form2_KeyUp;
            MouseDown += Form2_MouseDown;
            MouseMove += Form2_MouseMove;
            MouseWheel += Form2_MW;
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion
        private System.Windows.Forms.Label fpsLabel;
        private System.Windows.Forms.Label positionLabel;
    }
}