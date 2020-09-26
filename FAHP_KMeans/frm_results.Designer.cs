namespace Fahp_cbr_app
{
    partial class frm_results
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
            this.label12 = new System.Windows.Forms.Label();
            this.cmbox_case = new System.Windows.Forms.ComboBox();
            this.dg_randomcases = new System.Windows.Forms.DataGridView();
            this.dg_Non_stat = new System.Windows.Forms.DataGridView();
            this.label6 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.dg_With_stat = new System.Windows.Forms.DataGridView();
            this.lbl_non = new System.Windows.Forms.Label();
            this.lbl_with = new System.Windows.Forms.Label();
            this.button18 = new System.Windows.Forms.Button();
            this.dg_result = new System.Windows.Forms.DataGridView();
            this.listBox1 = new System.Windows.Forms.ListBox();
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.comboBox2 = new System.Windows.Forms.ComboBox();
            this.button1 = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            this.dg_criteria = new System.Windows.Forms.DataGridView();
            ((System.ComponentModel.ISupportInitialize)(this.dg_randomcases)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dg_Non_stat)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dg_With_stat)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dg_result)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dg_criteria)).BeginInit();
            this.SuspendLayout();
            // 
            // label12
            // 
            this.label12.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(1295, 6);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(68, 13);
            this.label12.TabIndex = 29;
            this.label12.Tag = "";
            this.label12.Text = "حالة الدرسة  ";
            // 
            // cmbox_case
            // 
            this.cmbox_case.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.cmbox_case.FormattingEnabled = true;
            this.cmbox_case.Location = new System.Drawing.Point(1142, 3);
            this.cmbox_case.Name = "cmbox_case";
            this.cmbox_case.Size = new System.Drawing.Size(133, 21);
            this.cmbox_case.TabIndex = 28;
            this.cmbox_case.SelectedIndexChanged += new System.EventHandler(this.cmbox_case_SelectedIndexChanged);
            // 
            // dg_randomcases
            // 
            this.dg_randomcases.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.dg_randomcases.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dg_randomcases.Location = new System.Drawing.Point(947, 59);
            this.dg_randomcases.Name = "dg_randomcases";
            this.dg_randomcases.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.dg_randomcases.Size = new System.Drawing.Size(405, 591);
            this.dg_randomcases.TabIndex = 46;
            // 
            // dg_Non_stat
            // 
            this.dg_Non_stat.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dg_Non_stat.Location = new System.Drawing.Point(597, 276);
            this.dg_Non_stat.Name = "dg_Non_stat";
            this.dg_Non_stat.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.dg_Non_stat.Size = new System.Drawing.Size(339, 463);
            this.dg_Non_stat.TabIndex = 57;
            this.dg_Non_stat.SelectionChanged += new System.EventHandler(this.dg_Non_stat_SelectionChanged);
            // 
            // label6
            // 
            this.label6.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(26, 250);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(195, 13);
            this.label6.TabIndex = 58;
            this.label6.Text = "تطابق الطريقتين عند نسب تشابه مختلفة";
            this.label6.Click += new System.EventHandler(this.label6_Click);
            // 
            // label9
            // 
            this.label9.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(1231, 699);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(0, 13);
            this.label9.TabIndex = 61;
            // 
            // dg_With_stat
            // 
            this.dg_With_stat.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dg_With_stat.Location = new System.Drawing.Point(247, 276);
            this.dg_With_stat.Name = "dg_With_stat";
            this.dg_With_stat.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.dg_With_stat.Size = new System.Drawing.Size(339, 463);
            this.dg_With_stat.TabIndex = 119;
            this.dg_With_stat.SelectionChanged += new System.EventHandler(this.dg_With_stat_SelectionChanged);
            // 
            // lbl_non
            // 
            this.lbl_non.AutoSize = true;
            this.lbl_non.Location = new System.Drawing.Point(653, 250);
            this.lbl_non.Name = "lbl_non";
            this.lbl_non.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.lbl_non.Size = new System.Drawing.Size(177, 13);
            this.lbl_non.TabIndex = 120;
            this.lbl_non.Text = "نتائج AHP  عند عدم استخدام العنقدة";
            this.lbl_non.Click += new System.EventHandler(this.lbl_non_Click);
            // 
            // lbl_with
            // 
            this.lbl_with.AutoSize = true;
            this.lbl_with.Location = new System.Drawing.Point(319, 250);
            this.lbl_with.Name = "lbl_with";
            this.lbl_with.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.lbl_with.Size = new System.Drawing.Size(177, 13);
            this.lbl_with.TabIndex = 121;
            this.lbl_with.Text = "نتائج AHP مع استخدام ++ IK-means";
            // 
            // button18
            // 
            this.button18.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button18.Location = new System.Drawing.Point(812, 3);
            this.button18.Name = "button18";
            this.button18.Size = new System.Drawing.Size(108, 21);
            this.button18.TabIndex = 123;
            this.button18.Text = "أوجد الحلول";
            this.button18.UseVisualStyleBackColor = true;
            this.button18.Click += new System.EventHandler(this.button18_Click_1);
            // 
            // dg_result
            // 
            this.dg_result.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dg_result.Location = new System.Drawing.Point(18, 276);
            this.dg_result.Name = "dg_result";
            this.dg_result.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.dg_result.Size = new System.Drawing.Size(218, 463);
            this.dg_result.TabIndex = 125;
            // 
            // listBox1
            // 
            this.listBox1.FormattingEnabled = true;
            this.listBox1.Location = new System.Drawing.Point(948, 656);
            this.listBox1.Name = "listBox1";
            this.listBox1.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.listBox1.Size = new System.Drawing.Size(405, 82);
            this.listBox1.TabIndex = 126;
            // 
            // comboBox1
            // 
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.Items.AddRange(new object[] {
            "حالات تجربة من ملف",
            "توليد بيانات عشوائية"});
            this.comboBox1.Location = new System.Drawing.Point(936, 3);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(121, 21);
            this.comboBox1.TabIndex = 127;
            this.comboBox1.SelectedIndexChanged += new System.EventHandler(this.comboBox1_SelectedIndexChanged);
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(1068, 6);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(60, 13);
            this.label1.TabIndex = 128;
            this.label1.Tag = "";
            this.label1.Text = "حالات تجربة";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(1116, 39);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(93, 13);
            this.label2.TabIndex = 129;
            this.label2.Text = "إدخالات المستخدم";
            this.label2.Click += new System.EventHandler(this.label2_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(638, 10);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(52, 13);
            this.label3.TabIndex = 130;
            this.label3.Text = "فرز النتائج";
            // 
            // comboBox2
            // 
            this.comboBox2.FormattingEnabled = true;
            this.comboBox2.Items.AddRange(new object[] {
            "حسب رقم التجربة",
            "حسب نسبة التشابه"});
            this.comboBox2.Location = new System.Drawing.Point(486, 3);
            this.comboBox2.Name = "comboBox2";
            this.comboBox2.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.comboBox2.Size = new System.Drawing.Size(121, 21);
            this.comboBox2.TabIndex = 131;
            this.comboBox2.SelectedIndexChanged += new System.EventHandler(this.comboBox2_SelectedIndexChanged);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(341, 3);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 132;
            this.button1.Text = "تفاصيل";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(400, 39);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(42, 13);
            this.label4.TabIndex = 154;
            this.label4.Text = "Criteria";
            // 
            // dg_criteria
            // 
            this.dg_criteria.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dg_criteria.Location = new System.Drawing.Point(21, 59);
            this.dg_criteria.Name = "dg_criteria";
            this.dg_criteria.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.dg_criteria.Size = new System.Drawing.Size(920, 188);
            this.dg_criteria.TabIndex = 153;
            // 
            // frm_results
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1370, 750);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.dg_criteria);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.comboBox2);
            this.Controls.Add(this.label3);
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
            this.Name = "frm_results";
            this.Text = "frm_results";
            this.Load += new System.EventHandler(this.frm_results_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dg_randomcases)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dg_Non_stat)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dg_With_stat)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dg_result)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dg_criteria)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label12;
        public System.Windows.Forms.ComboBox cmbox_case;
        private System.Windows.Forms.DataGridView dg_randomcases;
        private System.Windows.Forms.DataGridView dg_Non_stat;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.DataGridView dg_With_stat;
        private System.Windows.Forms.Label lbl_non;
        private System.Windows.Forms.Label lbl_with;
        private System.Windows.Forms.Button button18;
        private System.Windows.Forms.DataGridView dg_result;
        private System.Windows.Forms.ListBox listBox1;
        private System.Windows.Forms.ComboBox comboBox1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ComboBox comboBox2;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.DataGridView dg_criteria;
    }
}