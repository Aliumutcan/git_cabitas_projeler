namespace VerificationCertificate
{
    partial class Form1
    {
        /// <summary>
        ///Gerekli tasarımcı değişkeni.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///Kullanılan tüm kaynakları temizleyin.
        /// </summary>
        ///<param name="disposing">yönetilen kaynaklar dispose edilmeliyse doğru; aksi halde yanlış.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer üretilen kod

        /// <summary>
        /// Tasarımcı desteği için gerekli metot - bu metodun 
        ///içeriğini kod düzenleyici ile değiştirmeyin.
        /// </summary>
        private void InitializeComponent()
        {
            this.btn_decrypt_file = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.txt_pin_kod = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.txt_password = new System.Windows.Forms.TextBox();
            this.btn_privatekey = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // btn_decrypt_file
            // 
            this.btn_decrypt_file.Location = new System.Drawing.Point(211, 5);
            this.btn_decrypt_file.Name = "btn_decrypt_file";
            this.btn_decrypt_file.Size = new System.Drawing.Size(81, 73);
            this.btn_decrypt_file.TabIndex = 0;
            this.btn_decrypt_file.Text = "Doğrula";
            this.btn_decrypt_file.UseVisualStyleBackColor = true;
            this.btn_decrypt_file.Click += new System.EventHandler(this.btn_decrypt_file_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 9);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(28, 13);
            this.label2.TabIndex = 1;
            this.label2.Text = "Pin: ";
            // 
            // txt_pin_kod
            // 
            this.txt_pin_kod.Location = new System.Drawing.Point(105, 5);
            this.txt_pin_kod.Name = "txt_pin_kod";
            this.txt_pin_kod.Size = new System.Drawing.Size(100, 20);
            this.txt_pin_kod.TabIndex = 2;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 35);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(93, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Kapalı anahtarınız:";
            // 
            // txt_password
            // 
            this.txt_password.Location = new System.Drawing.Point(105, 58);
            this.txt_password.Name = "txt_password";
            this.txt_password.Size = new System.Drawing.Size(100, 20);
            this.txt_password.TabIndex = 2;
            // 
            // btn_privatekey
            // 
            this.btn_privatekey.Location = new System.Drawing.Point(105, 29);
            this.btn_privatekey.Name = "btn_privatekey";
            this.btn_privatekey.Size = new System.Drawing.Size(100, 23);
            this.btn_privatekey.TabIndex = 0;
            this.btn_privatekey.Text = "Kapalı anahtar";
            this.btn_privatekey.UseVisualStyleBackColor = true;
            this.btn_privatekey.Click += new System.EventHandler(this.btn_privatekey_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(12, 65);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(76, 13);
            this.label3.TabIndex = 1;
            this.label3.Text = "Anahtar şifresi:";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(298, 92);
            this.Controls.Add(this.txt_password);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.txt_pin_kod);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.btn_privatekey);
            this.Controls.Add(this.btn_decrypt_file);
            this.Name = "Form1";
            this.Text = "Form1";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btn_decrypt_file;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txt_pin_kod;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txt_password;
        private System.Windows.Forms.Button btn_privatekey;
        private System.Windows.Forms.Label label3;
    }
}

