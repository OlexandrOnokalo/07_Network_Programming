namespace MailSender
{
    partial class Form1
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

        #region Код, автоматически созданный конструктором форм Windows

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.toBox = new System.Windows.Forms.TextBox();
            this.themeBox = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.fromBox = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.attachementsListBox = new System.Windows.Forms.ListBox();
            this.label4 = new System.Windows.Forms.Label();
            this.bodyBox = new System.Windows.Forms.TextBox();
            this.button2 = new System.Windows.Forms.Button();
            this.button3 = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // toBox
            // 
            this.toBox.Location = new System.Drawing.Point(229, 64);
            this.toBox.Margin = new System.Windows.Forms.Padding(8, 7, 8, 7);
            this.toBox.Name = "toBox";
            this.toBox.Size = new System.Drawing.Size(1060, 38);
            this.toBox.TabIndex = 0;
            // 
            // themeBox
            // 
            this.themeBox.Location = new System.Drawing.Point(229, 193);
            this.themeBox.Margin = new System.Windows.Forms.Padding(8, 7, 8, 7);
            this.themeBox.Name = "themeBox";
            this.themeBox.Size = new System.Drawing.Size(1060, 38);
            this.themeBox.TabIndex = 1;
            this.themeBox.Text = "SMTP client in C#";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(35, 64);
            this.label1.Margin = new System.Windows.Forms.Padding(8, 0, 8, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(129, 32);
            this.label1.TabIndex = 2;
            this.label1.Text = "To whom";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(43, 207);
            this.label2.Margin = new System.Windows.Forms.Padding(8, 0, 8, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(102, 32);
            this.label2.TabIndex = 3;
            this.label2.Text = "Theme";
            // 
            // fromBox
            // 
            this.fromBox.Location = new System.Drawing.Point(229, 312);
            this.fromBox.Margin = new System.Windows.Forms.Padding(8, 7, 8, 7);
            this.fromBox.Name = "fromBox";
            this.fromBox.Size = new System.Drawing.Size(1060, 38);
            this.fromBox.TabIndex = 4;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(35, 326);
            this.label3.Margin = new System.Windows.Forms.Padding(8, 0, 8, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(161, 32);
            this.label3.TabIndex = 5;
            this.label3.Text = "From whom";
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(229, 422);
            this.button1.Margin = new System.Windows.Forms.Padding(8, 7, 8, 7);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(291, 55);
            this.button1.TabIndex = 6;
            this.button1.Text = "Attach a file";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // attachementsListBox
            // 
            this.attachementsListBox.FormattingEnabled = true;
            this.attachementsListBox.ItemHeight = 31;
            this.attachementsListBox.Location = new System.Drawing.Point(1373, 69);
            this.attachementsListBox.Margin = new System.Windows.Forms.Padding(8, 7, 8, 7);
            this.attachementsListBox.Name = "attachementsListBox";
            this.attachementsListBox.Size = new System.Drawing.Size(775, 376);
            this.attachementsListBox.TabIndex = 7;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(1493, 21);
            this.label4.Margin = new System.Windows.Forms.Padding(8, 0, 8, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(266, 32);
            this.label4.TabIndex = 8;
            this.label4.Text = "List of attached files";
            // 
            // bodyBox
            // 
            this.bodyBox.Location = new System.Drawing.Point(51, 551);
            this.bodyBox.Margin = new System.Windows.Forms.Padding(8, 7, 8, 7);
            this.bodyBox.Multiline = true;
            this.bodyBox.Name = "bodyBox";
            this.bodyBox.Size = new System.Drawing.Size(2069, 446);
            this.bodyBox.TabIndex = 9;
            this.bodyBox.Text = "Free text here";
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(1920, 1134);
            this.button2.Margin = new System.Windows.Forms.Padding(8, 7, 8, 7);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(200, 55);
            this.button2.TabIndex = 10;
            this.button2.Text = "send";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(51, 1134);
            this.button3.Margin = new System.Windows.Forms.Padding(8, 7, 8, 7);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(776, 55);
            this.button3.TabIndex = 11;
            this.button3.Text = "New letter";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(16F, 31F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(2153, 1262);
            this.Controls.Add(this.button3);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.bodyBox);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.attachementsListBox);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.fromBox);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.themeBox);
            this.Controls.Add(this.toBox);
            this.Margin = new System.Windows.Forms.Padding(8, 7, 8, 7);
            this.Name = "Form1";
            this.Text = "MailSender";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox toBox;
        private System.Windows.Forms.TextBox themeBox;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox fromBox;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.ListBox attachementsListBox;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox bodyBox;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button button3;
    }
}

