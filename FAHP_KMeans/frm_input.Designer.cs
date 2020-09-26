namespace Fahp_cbr_app
{
    partial class frm_input
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
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.rbtn_nonekmeans = new System.Windows.Forms.RadioButton();
            this.label2 = new System.Windows.Forms.Label();
            this.rbtn_kmeansPlus = new System.Windows.Forms.RadioButton();
            this.txt_cluster = new System.Windows.Forms.TextBox();
            this.rbtn_simpleKmean = new System.Windows.Forms.RadioButton();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.rbtn_none = new System.Windows.Forms.RadioButton();
            this.rbtn_minmax = new System.Windows.Forms.RadioButton();
            this.rbtn_normalize = new System.Windows.Forms.RadioButton();
            this.button3 = new System.Windows.Forms.Button();
            this.progressBar1 = new System.Windows.Forms.ProgressBar();
            this.button1 = new System.Windows.Forms.Button();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.label1 = new System.Windows.Forms.Label();
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            this.button2 = new System.Windows.Forms.Button();
            this.dataGridView2 = new System.Windows.Forms.DataGridView();
            this.label3 = new System.Windows.Forms.Label();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.listBox1 = new System.Windows.Forms.ListBox();
            this.groupBox2.SuspendLayout();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView2)).BeginInit();
            this.SuspendLayout();
            // 
            // groupBox2
            // 
            this.groupBox2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox2.Controls.Add(this.rbtn_nonekmeans);
            this.groupBox2.Controls.Add(this.label2);
            this.groupBox2.Controls.Add(this.rbtn_kmeansPlus);
            this.groupBox2.Controls.Add(this.txt_cluster);
            this.groupBox2.Controls.Add(this.rbtn_simpleKmean);
            this.groupBox2.Location = new System.Drawing.Point(857, 243);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.groupBox2.Size = new System.Drawing.Size(206, 133);
            this.groupBox2.TabIndex = 32;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "التجزئة العنقودية";
            this.groupBox2.Enter += new System.EventHandler(this.groupBox2_Enter);
            // 
            // rbtn_nonekmeans
            // 
            this.rbtn_nonekmeans.AutoSize = true;
            this.rbtn_nonekmeans.Location = new System.Drawing.Point(159, 69);
            this.rbtn_nonekmeans.Name = "rbtn_nonekmeans";
            this.rbtn_nonekmeans.Size = new System.Drawing.Size(36, 17);
            this.rbtn_nonekmeans.TabIndex = 40;
            this.rbtn_nonekmeans.Text = "بلا";
            this.rbtn_nonekmeans.UseVisualStyleBackColor = true;
            // 
            // label2
            // 
            this.label2.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(143, 107);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(52, 13);
            this.label2.TabIndex = 39;
            this.label2.Text = "عدد العقد";
            // 
            // rbtn_kmeansPlus
            // 
            this.rbtn_kmeansPlus.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.rbtn_kmeansPlus.AutoSize = true;
            this.rbtn_kmeansPlus.Location = new System.Drawing.Point(107, 46);
            this.rbtn_kmeansPlus.Name = "rbtn_kmeansPlus";
            this.rbtn_kmeansPlus.Size = new System.Drawing.Size(88, 17);
            this.rbtn_kmeansPlus.TabIndex = 1;
            this.rbtn_kmeansPlus.Text = " ++K-means ";
            this.rbtn_kmeansPlus.UseVisualStyleBackColor = true;
            // 
            // txt_cluster
            // 
            this.txt_cluster.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.txt_cluster.Location = new System.Drawing.Point(31, 97);
            this.txt_cluster.Multiline = true;
            this.txt_cluster.Name = "txt_cluster";
            this.txt_cluster.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.txt_cluster.Size = new System.Drawing.Size(73, 23);
            this.txt_cluster.TabIndex = 38;
            this.txt_cluster.Text = "3";
            this.txt_cluster.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // rbtn_simpleKmean
            // 
            this.rbtn_simpleKmean.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.rbtn_simpleKmean.AutoSize = true;
            this.rbtn_simpleKmean.Checked = true;
            this.rbtn_simpleKmean.Location = new System.Drawing.Point(96, 23);
            this.rbtn_simpleKmean.Name = "rbtn_simpleKmean";
            this.rbtn_simpleKmean.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.rbtn_simpleKmean.Size = new System.Drawing.Size(99, 17);
            this.rbtn_simpleKmean.TabIndex = 0;
            this.rbtn_simpleKmean.TabStop = true;
            this.rbtn_simpleKmean.Text = "Simple K-means";
            this.rbtn_simpleKmean.UseVisualStyleBackColor = true;
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this.rbtn_none);
            this.groupBox1.Controls.Add(this.rbtn_minmax);
            this.groupBox1.Controls.Add(this.rbtn_normalize);
            this.groupBox1.Location = new System.Drawing.Point(857, 131);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.groupBox1.Size = new System.Drawing.Size(206, 106);
            this.groupBox1.TabIndex = 31;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "معالجة البيانات Data Standardization";
            // 
            // rbtn_none
            // 
            this.rbtn_none.AutoSize = true;
            this.rbtn_none.Location = new System.Drawing.Point(159, 65);
            this.rbtn_none.Name = "rbtn_none";
            this.rbtn_none.Size = new System.Drawing.Size(36, 17);
            this.rbtn_none.TabIndex = 30;
            this.rbtn_none.TabStop = true;
            this.rbtn_none.Text = "بلا";
            this.rbtn_none.UseVisualStyleBackColor = true;
            // 
            // rbtn_minmax
            // 
            this.rbtn_minmax.AutoSize = true;
            this.rbtn_minmax.Location = new System.Drawing.Point(95, 42);
            this.rbtn_minmax.Name = "rbtn_minmax";
            this.rbtn_minmax.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.rbtn_minmax.Size = new System.Drawing.Size(100, 17);
            this.rbtn_minmax.TabIndex = 29;
            this.rbtn_minmax.TabStop = true;
            this.rbtn_minmax.Text = "Min-Max scaling";
            this.rbtn_minmax.UseVisualStyleBackColor = true;
            this.rbtn_minmax.CheckedChanged += new System.EventHandler(this.radioButton2_CheckedChanged);
            // 
            // rbtn_normalize
            // 
            this.rbtn_normalize.AutoSize = true;
            this.rbtn_normalize.Checked = true;
            this.rbtn_normalize.Location = new System.Drawing.Point(59, 19);
            this.rbtn_normalize.Name = "rbtn_normalize";
            this.rbtn_normalize.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.rbtn_normalize.Size = new System.Drawing.Size(136, 17);
            this.rbtn_normalize.TabIndex = 28;
            this.rbtn_normalize.TabStop = true;
            this.rbtn_normalize.Text = " Z-score normalization  \r\n";
            this.rbtn_normalize.UseVisualStyleBackColor = true;
            // 
            // button3
            // 
            this.button3.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.button3.Location = new System.Drawing.Point(888, 387);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(114, 42);
            this.button3.TabIndex = 30;
            this.button3.Text = "عنقدة";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // progressBar1
            // 
            this.progressBar1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.progressBar1.Location = new System.Drawing.Point(109, 541);
            this.progressBar1.Name = "progressBar1";
            this.progressBar1.Size = new System.Drawing.Size(1033, 23);
            this.progressBar1.TabIndex = 28;
            // 
            // button1
            // 
            this.button1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button1.Location = new System.Drawing.Point(884, 59);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(106, 42);
            this.button1.TabIndex = 27;
            this.button1.Text = "حالة جديدة";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // dataGridView1
            // 
            this.dataGridView1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Location = new System.Drawing.Point(12, 12);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.dataGridView1.Size = new System.Drawing.Size(821, 387);
            this.dataGridView1.TabIndex = 33;
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(996, 15);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(53, 13);
            this.label1.TabIndex = 35;
            this.label1.Text = "فرز حسب";
            // 
            // comboBox1
            // 
            this.comboBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.comboBox1.BackColor = System.Drawing.SystemColors.MenuBar;
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.Location = new System.Drawing.Point(857, 15);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(133, 21);
            this.comboBox1.TabIndex = 36;
            this.comboBox1.SelectedIndexChanged += new System.EventHandler(this.comboBox1_SelectedIndexChanged_1);
            // 
            // button2
            // 
            this.button2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.button2.Location = new System.Drawing.Point(888, 467);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(114, 42);
            this.button2.TabIndex = 37;
            this.button2.Text = "حفظ";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click_1);
            // 
            // dataGridView2
            // 
            this.dataGridView2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dataGridView2.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView2.Location = new System.Drawing.Point(12, 405);
            this.dataGridView2.Name = "dataGridView2";
            this.dataGridView2.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.dataGridView2.Size = new System.Drawing.Size(821, 140);
            this.dataGridView2.TabIndex = 38;
            this.dataGridView2.RowHeaderMouseClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.dataGridView2_RowHeaderMouseClick);
            this.dataGridView2.SelectionChanged += new System.EventHandler(this.dataGridView2_SelectionChanged);
            // 
            // label3
            // 
            this.label3.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(926, 441);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(35, 13);
            this.label3.TabIndex = 39;
            this.label3.Text = "label3";
            // 
            // textBox1
            // 
            this.textBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.textBox1.Location = new System.Drawing.Point(888, 515);
            this.textBox1.Multiline = true;
            this.textBox1.Name = "textBox1";
            this.textBox1.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.textBox1.Size = new System.Drawing.Size(73, 23);
            this.textBox1.TabIndex = 40;
            this.textBox1.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label4
            // 
            this.label4.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(967, 522);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(19, 13);
            this.label4.TabIndex = 41;
            this.label4.Text = "FK";
            // 
            // listBox1
            // 
            this.listBox1.FormattingEnabled = true;
            this.listBox1.Location = new System.Drawing.Point(56, 414);
            this.listBox1.Name = "listBox1";
            this.listBox1.Size = new System.Drawing.Size(284, 95);
            this.listBox1.TabIndex = 42;
            // 
            // frm_input
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Window;
            this.ClientSize = new System.Drawing.Size(1075, 576);
            this.Controls.Add(this.listBox1);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.dataGridView2);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.comboBox1);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.dataGridView1);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.button3);
            this.Controls.Add(this.progressBar1);
            this.Controls.Add(this.button1);
            this.Name = "frm_input";
            this.Text = "test";
            this.Load += new System.EventHandler(this.frm_input_Load);
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView2)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.RadioButton rbtn_kmeansPlus;
        private System.Windows.Forms.RadioButton rbtn_simpleKmean;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.RadioButton rbtn_minmax;
        private System.Windows.Forms.RadioButton rbtn_normalize;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.ProgressBar progressBar1;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txt_cluster;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox comboBox1;
        private System.Windows.Forms.RadioButton rbtn_none;
        private System.Windows.Forms.RadioButton rbtn_nonekmeans;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.DataGridView dataGridView2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ListBox listBox1;
    }
}