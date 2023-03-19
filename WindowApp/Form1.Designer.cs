namespace WindowApp
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

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.btn_click = new System.Windows.Forms.Button();
            this.txt_input = new System.Windows.Forms.TextBox();
            this.lbl_output = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // btn_click
            // 
            this.btn_click.Location = new System.Drawing.Point(361, 282);
            this.btn_click.Name = "btn_click";
            this.btn_click.Size = new System.Drawing.Size(91, 94);
            this.btn_click.TabIndex = 0;
            this.btn_click.Text = "Click Me";
            this.btn_click.UseVisualStyleBackColor = true;
            this.btn_click.Click += new System.EventHandler(this.button1_Click);
            // 
            // txt_input
            // 
            this.txt_input.Location = new System.Drawing.Point(351, 61);
            this.txt_input.Name = "txt_input";
            this.txt_input.Size = new System.Drawing.Size(118, 20);
            this.txt_input.TabIndex = 1;
            this.txt_input.TextChanged += new System.EventHandler(this.textBox1_TextChanged);
            // 
            // lbl_output
            // 
            this.lbl_output.AutoSize = true;
            this.lbl_output.Location = new System.Drawing.Point(395, 169);
            this.lbl_output.Name = "lbl_output";
            this.lbl_output.Size = new System.Drawing.Size(16, 13);
            this.lbl_output.TabIndex = 2;
            this.lbl_output.Text = "...";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.lbl_output);
            this.Controls.Add(this.txt_input);
            this.Controls.Add(this.btn_click);
            this.Name = "Form1";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btn_click;
        private System.Windows.Forms.TextBox txt_input;
        private System.Windows.Forms.Label lbl_output;
    }
}

