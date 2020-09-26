namespace Fahp_cbr_app
{
    partial class frm_result_fuzzy
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
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            this.listBox1 = new System.Windows.Forms.ListBox();
            this.dg_result = new System.Windows.Forms.DataGridView();
            this.button18 = new System.Windows.Forms.Button();
            this.lbl_with = new System.Windows.Forms.Label();
            this.lbl_non = new System.Windows.Forms.Label();
            this.dg_With_stat = new System.Windows.Forms.DataGridView();
            this.label9 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.dg_Non_stat = new System.Windows.Forms.DataGridView();
            this.dg_randomcases = new System.Windows.Forms.DataGridView();
            this.label12 = new System.Windows.Forms.Label();
            this.cmbox_case = new System.Windows.Forms.ComboBox();
            this.dg_criteria = new System.Windows.Forms.DataGridView();
            this.label4 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.dg_result)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dg_With_stat)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dg_Non_stat)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dg_randomcases)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dg_criteria)).BeginInit();
            this.SuspendLayout();
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(1112, 43);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(93, 13);
            this.label2.TabIndex = 147;
            this.label2.Text = "إدخالات المستخدم";
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(1064, 10);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(60, 13);
            this.label1.TabIndex = 146;
            this.label1.Tag = "";
            this.label1.Text = "حالات تجربة";
            // 
            // comboBox1
            // 
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.Items.AddRange(new object[] {
            "حالات تجربة من ملف",
            "توليد بيانات عشوائية"});
            this.comboBox1.Location = new System.Drawing.Point(932, 7);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(121, 21);
            this.comboBox1.TabIndex = 145;
            this.comboBox1.SelectedIndexChanged += new System.EventHandler(this.comboBox1_SelectedIndexChanged);
            // 
            // listBox1
            // 
            this.listBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.listBox1.FormattingEnabled = true;
            this.listBox1.Location = new System.Drawing.Point(944, 582);
            this.listBox1.Name = "listBox1";
            this.listBox1.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.listBox1.Size = new System.Drawing.Size(405, 160);
            this.listBox1.TabIndex = 144;
            // 
            // dg_result
            // 
            this.dg_result.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dg_result.Location = new System.Drawing.Point(14, 297);
            this.dg_result.Name = "dg_result";
            this.dg_result.Size = new System.Drawing.Size(603, 445);
            this.dg_result.TabIndex = 143;
            this.dg_result.RowHeaderMouseClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.dg_result_RowHeaderMouseClick);
            // 
            // button18
            // 
            this.button18.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button18.Location = new System.Drawing.Point(808, 7);
            this.button18.Name = "button18";
            this.button18.Size = new System.Drawing.Size(108, 21);
            this.button18.TabIndex = 142;
            this.button18.Text = "أوجد الحلول";
            this.button18.UseVisualStyleBackColor = true;
            this.button18.Click += new System.EventHandler(this.button18_Click);
            // 
            // lbl_with
            // 
            this.lbl_with.AutoSize = true;
            this.lbl_with.Location = new System.Drawing.Point(657, 281);
            this.lbl_with.Name = "lbl_with";
            this.lbl_with.Size = new System.Drawing.Size(126, 13);
            this.lbl_with.TabIndex = 141;
            this.lbl_with.Text = "Fuzzy AHP, IK-means++";
            // 
            // lbl_non
            // 
            this.lbl_non.AutoSize = true;
            this.lbl_non.Location = new System.Drawing.Point(837, 281);
            this.lbl_non.Name = "lbl_non";
            this.lbl_non.Size = new System.Drawing.Size(95, 13);
            this.lbl_non.TabIndex = 140;
            this.lbl_non.Text = "AHP, IK-means++";
            // 
            // dg_With_stat
            // 
            this.dg_With_stat.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dg_With_stat.Location = new System.Drawing.Point(643, 297);
            this.dg_With_stat.Name = "dg_With_stat";
            this.dg_With_stat.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.dg_With_stat.Size = new System.Drawing.Size(140, 446);
            this.dg_With_stat.TabIndex = 139;
            this.dg_With_stat.SelectionChanged += new System.EventHandler(this.dg_With_stat_SelectionChanged);
            // 
            // label9
            // 
            this.label9.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(1227, 703);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(0, 13);
            this.label9.TabIndex = 138;
            // 
            // label6
            // 
            this.label6.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(24, 281);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(195, 13);
            this.label6.TabIndex = 137;
            this.label6.Text = "تطابق الطريقتين عند نسب تشابه مختلفة";
            // 
            // dg_Non_stat
            // 
            this.dg_Non_stat.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dg_Non_stat.Location = new System.Drawing.Point(808, 297);
            this.dg_Non_stat.Name = "dg_Non_stat";
            this.dg_Non_stat.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.dg_Non_stat.Size = new System.Drawing.Size(124, 446);
            this.dg_Non_stat.TabIndex = 136;
            this.dg_Non_stat.SelectionChanged += new System.EventHandler(this.dg_Non_stat_SelectionChanged);
            // 
            // dg_randomcases
            // 
            this.dg_randomcases.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.dg_randomcases.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dg_randomcases.Location = new System.Drawing.Point(943, 63);
            this.dg_randomcases.Name = "dg_randomcases";
            this.dg_randomcases.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.dg_randomcases.Size = new System.Drawing.Size(405, 517);
            this.dg_randomcases.TabIndex = 135;
            // 
            // label12
            // 
            this.label12.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(1291, 10);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(68, 13);
            this.label12.TabIndex = 134;
            this.label12.Tag = "";
            this.label12.Text = "حالة الدرسة  ";
            // 
            // cmbox_case
            // 
            this.cmbox_case.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.cmbox_case.FormattingEnabled = true;
            this.cmbox_case.Location = new System.Drawing.Point(1138, 7);
            this.cmbox_case.Name = "cmbox_case";
            this.cmbox_case.Size = new System.Drawing.Size(133, 21);
            this.cmbox_case.TabIndex = 133;
            this.cmbox_case.SelectedIndexChanged += new System.EventHandler(this.cmbox_case_SelectedIndexChanged);
            // 
            // dg_criteria
            // 
            this.dg_criteria.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dg_criteria.Location = new System.Drawing.Point(12, 63);
            this.dg_criteria.Name = "dg_criteria";
            this.dg_criteria.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.dg_criteria.Size = new System.Drawing.Size(920, 204);
            this.dg_criteria.TabIndex = 151;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(368, 42);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(42, 13);
            this.label4.TabIndex = 152;
            this.label4.Text = "Criteria";
            // 
            // frm_result_fuzzy
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1370, 750);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.dg_criteria);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.comboBox1);
            this.Controls.Add(this.listBox1);
            this.Controls.Add(this.dg_result);
            this.Controls.Add(this.button18);
            this.Controls.Add(this.lbl_with);
            this.Controls.Add(this.lbl_non);
            this.Controls.Add(this.dg_With_stat);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.dg_Non_stat);
            this.Controls.Add(this.dg_randomcases);
            this.Controls.Add(this.label12);
            this.Controls.Add(this.cmbox_case);
            this.Name = "frm_result_fuzzy";
            this.Text = "frm_userCompare";
            this.Load += new System.EventHandler(this.frm_result_fuzzy_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dg_result)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dg_With_stat)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dg_Non_stat)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dg_randomcases)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dg_criteria)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox comboBox1;
        private System.Windows.Forms.ListBox listBox1;
        private System.Windows.Forms.DataGridView dg_result;
        private System.Windows.Forms.Button button18;
        private System.Windows.Forms.Label lbl_with;
        private System.Windows.Forms.Label lbl_non;
        private System.Windows.Forms.DataGridView dg_With_stat;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.DataGridView dg_Non_stat;
        private System.Windows.Forms.DataGridView dg_randomcases;
        private System.Windows.Forms.Label label12;
        public System.Windows.Forms.ComboBox cmbox_case;
        private System.Windows.Forms.DataGridView dg_criteria;
        private System.Windows.Forms.Label label4;


    }
}