namespace Fahp_cbr_app
{
    partial class frm_details
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
            this.dG_details = new System.Windows.Forms.DataGridView();
            this.listBox1 = new System.Windows.Forms.ListBox();
            this.dg_With_stat = new System.Windows.Forms.DataGridView();
            this.dg_Non_stat = new System.Windows.Forms.DataGridView();
            this.lbl_with = new System.Windows.Forms.Label();
            this.lbl_non = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.dG_details)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dg_With_stat)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dg_Non_stat)).BeginInit();
            this.SuspendLayout();
            // 
            // dG_details
            // 
            this.dG_details.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dG_details.Location = new System.Drawing.Point(861, 37);
            this.dG_details.Name = "dG_details";
            this.dG_details.Size = new System.Drawing.Size(497, 417);
            this.dG_details.TabIndex = 0;
            // 
            // listBox1
            // 
            this.listBox1.FormattingEnabled = true;
            this.listBox1.Location = new System.Drawing.Point(861, 461);
            this.listBox1.Name = "listBox1";
            this.listBox1.Size = new System.Drawing.Size(497, 277);
            this.listBox1.TabIndex = 1;
            // 
            // dg_With_stat
            // 
            this.dg_With_stat.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dg_With_stat.Location = new System.Drawing.Point(25, 37);
            this.dg_With_stat.Name = "dg_With_stat";
            this.dg_With_stat.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.dg_With_stat.Size = new System.Drawing.Size(397, 701);
            this.dg_With_stat.TabIndex = 121;
            // 
            // dg_Non_stat
            // 
            this.dg_Non_stat.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dg_Non_stat.Location = new System.Drawing.Point(443, 37);
            this.dg_Non_stat.Name = "dg_Non_stat";
            this.dg_Non_stat.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.dg_Non_stat.Size = new System.Drawing.Size(397, 701);
            this.dg_Non_stat.TabIndex = 120;
            // 
            // lbl_with
            // 
            this.lbl_with.AutoSize = true;
            this.lbl_with.Location = new System.Drawing.Point(158, 9);
            this.lbl_with.Name = "lbl_with";
            this.lbl_with.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.lbl_with.Size = new System.Drawing.Size(151, 13);
            this.lbl_with.TabIndex = 123;
            this.lbl_with.Text = "البدائل باستخدام ++ IK-means";
            // 
            // lbl_non
            // 
            this.lbl_non.AutoSize = true;
            this.lbl_non.Location = new System.Drawing.Point(536, 9);
            this.lbl_non.Name = "lbl_non";
            this.lbl_non.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.lbl_non.Size = new System.Drawing.Size(135, 13);
            this.lbl_non.TabIndex = 122;
            this.lbl_non.Text = "البدائل في حال عدم العنقدة";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(1029, 9);
            this.label1.Name = "label1";
            this.label1.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.label1.Size = new System.Drawing.Size(181, 13);
            this.label1.TabIndex = 124;
            this.label1.Text = "تطابق عدد البدائل الناتج في الطريقتين";
            // 
            // frm_details
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1370, 750);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.lbl_with);
            this.Controls.Add(this.lbl_non);
            this.Controls.Add(this.dg_With_stat);
            this.Controls.Add(this.dg_Non_stat);
            this.Controls.Add(this.listBox1);
            this.Controls.Add(this.dG_details);
            this.Name = "frm_details";
            this.Text = "frm_details";
            this.Load += new System.EventHandler(this.frm_details_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dG_details)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dg_With_stat)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dg_Non_stat)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        public System.Windows.Forms.DataGridView dG_details;
        public System.Windows.Forms.ListBox listBox1;
        public System.Windows.Forms.DataGridView dg_With_stat;
        public System.Windows.Forms.DataGridView dg_Non_stat;
        private System.Windows.Forms.Label lbl_with;
        private System.Windows.Forms.Label lbl_non;
        private System.Windows.Forms.Label label1;
    }
}