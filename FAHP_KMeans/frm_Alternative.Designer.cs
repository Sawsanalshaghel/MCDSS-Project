namespace Fahp_cbr_app
{
    partial class frm_Alternative
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
            this.dg_allcases = new System.Windows.Forms.DataGridView();
            this.dg_bestcluster = new System.Windows.Forms.DataGridView();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.dg_allcases)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dg_bestcluster)).BeginInit();
            this.SuspendLayout();
            // 
            // dg_allcases
            // 
            this.dg_allcases.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.dg_allcases.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dg_allcases.Location = new System.Drawing.Point(12, 28);
            this.dg_allcases.Name = "dg_allcases";
            this.dg_allcases.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.dg_allcases.Size = new System.Drawing.Size(637, 438);
            this.dg_allcases.TabIndex = 119;
            // 
            // dg_bestcluster
            // 
            this.dg_bestcluster.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dg_bestcluster.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dg_bestcluster.Location = new System.Drawing.Point(721, 28);
            this.dg_bestcluster.Name = "dg_bestcluster";
            this.dg_bestcluster.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.dg_bestcluster.Size = new System.Drawing.Size(637, 438);
            this.dg_bestcluster.TabIndex = 118;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(1140, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(103, 13);
            this.label1.TabIndex = 120;
            this.label1.Text = "بدائل العنقود الأنسب";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(221, 9);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(94, 13);
            this.label2.TabIndex = 121;
            this.label2.Text = "بدائل قاعدة البيانات";
            // 
            // frm_Alternative
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1370, 478);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.dg_allcases);
            this.Controls.Add(this.dg_bestcluster);
            this.Name = "frm_Alternative";
            this.Text = "frm_Alternative";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Load += new System.EventHandler(this.frm_Alternative_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dg_allcases)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dg_bestcluster)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        public System.Windows.Forms.DataGridView dg_allcases;
        public System.Windows.Forms.DataGridView dg_bestcluster;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;


    }
}