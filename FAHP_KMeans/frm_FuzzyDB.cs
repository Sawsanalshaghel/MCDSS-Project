using System;
using System.Collections.Generic;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;
namespace Fahp_cbr_app
{
    public partial class frm_FuzzyDB : Form
    {
        List<int> Ids = new List<int>();
        public ComboBox cmbox_case;
        private Button button1;
        private DataGridView dG_Prob;
        private DataGridView dg_randomcases;
        private Label label11;
        private Label label10;
        private DataGridView dG_With;
        private Label label4;
        private OpenFileDialog openFileDialog1;
        private Button button23;
        private Button button11;
        private Button button21;
        public static bool ObjCreated = false;
        private DataGridView dG_Non;
        private Label label1;
        private ListBox listBox1;
        private Button button2;

        Statistics statistics ;
        public frm_FuzzyDB()
        {
            InitializeComponent();
            ObjCreated = true;
        }

        private void frm_usageCriteria_Load(object sender, EventArgs e)
        {

        }

        private void InitializeComponent()
        {
            this.cmbox_case = new System.Windows.Forms.ComboBox();
            this.button1 = new System.Windows.Forms.Button();
            this.dG_Prob = new System.Windows.Forms.DataGridView();
            this.dg_randomcases = new System.Windows.Forms.DataGridView();
            this.label11 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.dG_With = new System.Windows.Forms.DataGridView();
            this.label4 = new System.Windows.Forms.Label();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.button23 = new System.Windows.Forms.Button();
            this.button11 = new System.Windows.Forms.Button();
            this.button21 = new System.Windows.Forms.Button();
            this.dG_Non = new System.Windows.Forms.DataGridView();
            this.label1 = new System.Windows.Forms.Label();
            this.listBox1 = new System.Windows.Forms.ListBox();
            this.button2 = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dG_Prob)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dg_randomcases)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dG_With)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dG_Non)).BeginInit();
            this.SuspendLayout();
            // 
            // cmbox_case
            // 
            this.cmbox_case.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.cmbox_case.FormattingEnabled = true;
            this.cmbox_case.Location = new System.Drawing.Point(1122, 11);
            this.cmbox_case.Name = "cmbox_case";
            this.cmbox_case.Size = new System.Drawing.Size(133, 21);
            this.cmbox_case.TabIndex = 41;
            this.cmbox_case.SelectedIndexChanged += new System.EventHandler(this.cmbox_case_SelectedIndexChanged);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(12, 9);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(74, 23);
            this.button1.TabIndex = 56;
            this.button1.Text = "Fuzzify";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // dG_Prob
            // 
            this.dG_Prob.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dG_Prob.Location = new System.Drawing.Point(738, 222);
            this.dG_Prob.Name = "dG_Prob";
            this.dG_Prob.Size = new System.Drawing.Size(287, 516);
            this.dG_Prob.TabIndex = 101;
            this.dG_Prob.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dG_Prob_CellContentClick);
            // 
            // dg_randomcases
            // 
            this.dg_randomcases.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dg_randomcases.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dg_randomcases.Location = new System.Drawing.Point(12, 39);
            this.dg_randomcases.Name = "dg_randomcases";
            this.dg_randomcases.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.dg_randomcases.Size = new System.Drawing.Size(1326, 155);
            this.dg_randomcases.TabIndex = 95;
            this.dg_randomcases.RowHeaderMouseClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.dg_randomcases_RowHeaderMouseClick);
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(12, 204);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(129, 13);
            this.label11.TabIndex = 94;
            this.label11.Text = "Fuzzy AHP , IK-means++";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(395, 204);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(98, 13);
            this.label10.TabIndex = 93;
            this.label10.Text = "AHP , IK-means++";
            // 
            // dG_With
            // 
            this.dG_With.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.dG_With.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dG_With.Location = new System.Drawing.Point(12, 224);
            this.dG_With.Name = "dG_With";
            this.dG_With.Size = new System.Drawing.Size(328, 514);
            this.dG_With.TabIndex = 92;
            this.dG_With.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dG_With_CellContentClick);
            this.dG_With.SelectionChanged += new System.EventHandler(this.dG_With_SelectionChanged);
            // 
            // label4
            // 
            this.label4.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(1293, 16);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(65, 13);
            this.label4.TabIndex = 89;
            this.label4.Tag = "";
            this.label4.Text = "حالة الدراسة";
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            // 
            // button23
            // 
            this.button23.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button23.Location = new System.Drawing.Point(933, 5);
            this.button23.Name = "button23";
            this.button23.Size = new System.Drawing.Size(159, 27);
            this.button23.TabIndex = 90;
            this.button23.Text = "ادخال من ملف";
            this.button23.UseVisualStyleBackColor = true;
            this.button23.Click += new System.EventHandler(this.button23_Click);
            // 
            // button11
            // 
            this.button11.Location = new System.Drawing.Point(1143, 241);
            this.button11.Name = "button11";
            this.button11.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.button11.Size = new System.Drawing.Size(153, 33);
            this.button11.TabIndex = 81;
            this.button11.Text = "AHP and IK-means++";
            this.button11.UseVisualStyleBackColor = true;
            this.button11.Click += new System.EventHandler(this.button11_Click_1);
            // 
            // button21
            // 
            this.button21.Location = new System.Drawing.Point(1143, 285);
            this.button21.Name = "button21";
            this.button21.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.button21.Size = new System.Drawing.Size(153, 33);
            this.button21.TabIndex = 81;
            this.button21.Text = "Fuzzy AHP and IK-means++";
            this.button21.UseVisualStyleBackColor = true;
            this.button21.Click += new System.EventHandler(this.button21_Click);
            // 
            // dG_Non
            // 
            this.dG_Non.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.dG_Non.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dG_Non.Location = new System.Drawing.Point(377, 224);
            this.dG_Non.Name = "dG_Non";
            this.dG_Non.Size = new System.Drawing.Size(346, 514);
            this.dG_Non.TabIndex = 91;
            this.dG_Non.SelectionChanged += new System.EventHandler(this.dG_Non_SelectionChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(794, 204);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(69, 13);
            this.label1.TabIndex = 115;
            this.label1.Text = "Criteria Rank";
            // 
            // listBox1
            // 
            this.listBox1.FormattingEnabled = true;
            this.listBox1.Location = new System.Drawing.Point(1090, 324);
            this.listBox1.Name = "listBox1";
            this.listBox1.Size = new System.Drawing.Size(266, 407);
            this.listBox1.TabIndex = 116;
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(1143, 200);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(153, 33);
            this.button2.TabIndex = 82;
            this.button2.Text = "Generate Comparison";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click_1);
            // 
            // frm_FuzzyDB
            // 
            this.ClientSize = new System.Drawing.Size(1370, 750);
            this.Controls.Add(this.button21);
            this.Controls.Add(this.button11);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.listBox1);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.dG_Prob);
            this.Controls.Add(this.dg_randomcases);
            this.Controls.Add(this.label11);
            this.Controls.Add(this.label10);
            this.Controls.Add(this.dG_With);
            this.Controls.Add(this.dG_Non);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.button23);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.cmbox_case);
            this.Name = "frm_FuzzyDB";
            this.Load += new System.EventHandler(this.frm_usageCriteria_Load_1);
            ((System.ComponentModel.ISupportInitialize)(this.dG_Prob)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dg_randomcases)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dG_With)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dG_Non)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        SugenoFuzzySystem _fsCruiseControl = null;

        int problem_num = 0;

        SugenoFuzzySystem CreateSystem()
        {
            //
            // Create empty Sugeno Fuzzy System
            //
            SugenoFuzzySystem fsCruiseControl = new SugenoFuzzySystem();

            //
            // Create input variables for the system
            //
            FuzzyVariable fvSCPU = new FuzzyVariable("CPU", 500, 4000);
            fvSCPU.Terms.Add(new FuzzyTerm("slow", new TriangularMembershipFunction(500, 1000, 1500)));
            fvSCPU.Terms.Add(new FuzzyTerm("medium", new TriangularMembershipFunction(1400, 1700, 2000)));
            fvSCPU.Terms.Add(new FuzzyTerm("fast", new TriangularMembershipFunction(1800, 2500, 4000)));
            fsCruiseControl.Input.Add(fvSCPU);

            FuzzyVariable fvCore = new FuzzyVariable("Core", 1, 8);
            fvCore.Terms.Add(new FuzzyTerm("low", new TriangularMembershipFunction(1, 2, 3)));
            fvCore.Terms.Add(new FuzzyTerm("medium", new TriangularMembershipFunction(3, 4, 6)));
            fvCore.Terms.Add(new FuzzyTerm("high", new TriangularMembershipFunction(4, 6, 8)));
            fsCruiseControl.Input.Add(fvCore);


            FuzzyVariable fvRam = new FuzzyVariable("Ram", 512, 6000);
            fvRam.Terms.Add(new FuzzyTerm("low", new TriangularMembershipFunction(512, 1000, 1500)));
            fvRam.Terms.Add(new FuzzyTerm("medium", new TriangularMembershipFunction(1200, 2000, 3000)));
            fvRam.Terms.Add(new FuzzyTerm("high", new TriangularMembershipFunction(2500, 4000, 6000)));
            fsCruiseControl.Input.Add(fvRam);

            FuzzyVariable fvPrice = new FuzzyVariable("Price", 70, 3500);
            fvPrice.Terms.Add(new FuzzyTerm("low", new TriangularMembershipFunction(-9.0, -5.0, -1.0)));
            fvPrice.Terms.Add(new FuzzyTerm("medium", new TriangularMembershipFunction(-4.0, -0.0, 4.0)));
            fvPrice.Terms.Add(new FuzzyTerm("high", new TriangularMembershipFunction(1.0, 5.0, 9.0)));
            fsCruiseControl.Input.Add(fvPrice);

            FuzzyVariable fvBattery = new FuzzyVariable("Battery", 350, 12000);
            fvBattery.Terms.Add(new FuzzyTerm("low", new TriangularMembershipFunction(350, 2000, 5000)));
            fvBattery.Terms.Add(new FuzzyTerm("medium", new TriangularMembershipFunction(4000, 6000, 9000)));
            fvBattery.Terms.Add(new FuzzyTerm("high", new TriangularMembershipFunction(8000, 10000, 12000)));
            fsCruiseControl.Input.Add(fvBattery);

            FuzzyVariable fvStorage = new FuzzyVariable("Storage", 8, 256);
            fvStorage.Terms.Add(new FuzzyTerm("low", new TriangularMembershipFunction(8, 60, 100)));
            fvStorage.Terms.Add(new FuzzyTerm("medium", new TriangularMembershipFunction(90, 150, 180)));
            fvStorage.Terms.Add(new FuzzyTerm("high", new TriangularMembershipFunction(170, 200, 256)));
            fsCruiseControl.Input.Add(fvStorage);

            FuzzyVariable fvScreen1 = new FuzzyVariable("Screen1", 400, 4000);
            fvScreen1.Terms.Add(new FuzzyTerm("low", new TriangularMembershipFunction(400, 1000, 1500)));
            fvScreen1.Terms.Add(new FuzzyTerm("medium", new TriangularMembershipFunction(1200, 2000, 2500)));
            fvScreen1.Terms.Add(new FuzzyTerm("high", new TriangularMembershipFunction(2200, 3000, 4000)));
            fsCruiseControl.Input.Add(fvScreen1);


            FuzzyVariable fvScreen2 = new FuzzyVariable("Screen2", 800, 4000);
            fvScreen2.Terms.Add(new FuzzyTerm("low", new TriangularMembershipFunction(400, 1000, 1500)));
            fvScreen2.Terms.Add(new FuzzyTerm("medium", new TriangularMembershipFunction(1200, 2000, 2500)));
            fvScreen2.Terms.Add(new FuzzyTerm("high", new TriangularMembershipFunction(2200, 3000, 4000)));
            fsCruiseControl.Input.Add(fvScreen2);
            //

            FuzzyVariable fvCamera = new FuzzyVariable("Camera",  5, 23);
            fvCamera.Terms.Add(new FuzzyTerm("low", new TriangularMembershipFunction(5, 7, 12)));
            fvCamera.Terms.Add(new FuzzyTerm("medium", new TriangularMembershipFunction(8, 14, 17)));
            fvCamera.Terms.Add(new FuzzyTerm("high", new TriangularMembershipFunction(15, 18, 23)));
            fsCruiseControl.Input.Add(fvCamera);
            // Create output variables for the system
            //
           /* SugenoVariable svAccelerate = new SugenoVariable("Accelerate");
            svAccelerate.Functions.Add(fsCruiseControl.CreateSugenoFunction("zero", new double[] { 0.0, 0.0, 0.0 }));
            svAccelerate.Functions.Add(fsCruiseControl.CreateSugenoFunction("faster", new double[] { 0.0, 0.0, 1.0 }));
            svAccelerate.Functions.Add(fsCruiseControl.CreateSugenoFunction("slower", new double[] { 0.0, 0.0, -1.0 }));
            svAccelerate.Functions.Add(fsCruiseControl.CreateSugenoFunction("func", new double[] { -0.04, -0.1, 0.0 }));
            fsCruiseControl.Output.Add(svAccelerate);

            //
            // Create fuzzy rules
            //
            AddSugenoFuzzyRule(fsCruiseControl, fvSCPU, fvCore, svAccelerate, "slower", "slower", "faster");
            AddSugenoFuzzyRule(fsCruiseControl, fvSCPU, fvCore, svAccelerate, "slower", "zero", "faster");
            AddSugenoFuzzyRule(fsCruiseControl, fvSCPU, fvCore, svAccelerate, "slower", "faster", "zero");
            AddSugenoFuzzyRule(fsCruiseControl, fvSCPU, fvCore, svAccelerate, "zero", "slower", "faster");
            AddSugenoFuzzyRule(fsCruiseControl, fvSCPU, fvCore, svAccelerate, "zero", "zero", "func");
            AddSugenoFuzzyRule(fsCruiseControl, fvSCPU, fvCore, svAccelerate, "zero", "faster", "slower");
            AddSugenoFuzzyRule(fsCruiseControl, fvSCPU, fvCore, svAccelerate, "faster", "slower", "zero");
            AddSugenoFuzzyRule(fsCruiseControl, fvSCPU, fvCore, svAccelerate, "faster", "zero", "slower");
            AddSugenoFuzzyRule(fsCruiseControl, fvSCPU, fvCore, svAccelerate, "faster", "faster", "slower");*/

            //
            // Adding the same rules in text form
            //
            ///////////////////////////////////////////////////////////////////
            //SugenoFuzzyRule rule1 = fsCruiseControl.ParseRule("if (SpeedError is slower) and (SpeedErrorDot is slower) then (Accelerate is faster)");
            //SugenoFuzzyRule rule2 = fsCruiseControl.ParseRule("if (SpeedError is slower) and (SpeedErrorDot is zero) then (Accelerate is faster)");
            //SugenoFuzzyRule rule3 = fsCruiseControl.ParseRule("if (SpeedError is slower) and (SpeedErrorDot is faster) then (Accelerate is zero)");
            //SugenoFuzzyRule rule4 = fsCruiseControl.ParseRule("if (SpeedError is zero) and (SpeedErrorDot is slower) then (Accelerate is faster)");
            //SugenoFuzzyRule rule5 = fsCruiseControl.ParseRule("if (SpeedError is zero) and (SpeedErrorDot is zero) then (Accelerate is func)");
            //SugenoFuzzyRule rule6 = fsCruiseControl.ParseRule("if (SpeedError is zero) and (SpeedErrorDot is faster) then (Accelerate is slower)");
            //SugenoFuzzyRule rule7 = fsCruiseControl.ParseRule("if (SpeedError is faster) and (SpeedErrorDot is slower) then (Accelerate is faster)");
            //SugenoFuzzyRule rule8 = fsCruiseControl.ParseRule("if (SpeedError is faster) and (SpeedErrorDot is zero) then (Accelerate is slower)");
            //SugenoFuzzyRule rule9 = fsCruiseControl.ParseRule("if (SpeedError is faster) and (SpeedErrorDot is faster) then (Accelerate is slower)");

            //fsCruiseControl.Rules.Add(rule1);
            //fsCruiseControl.Rules.Add(rule2);
            //fsCruiseControl.Rules.Add(rule3);
            //fsCruiseControl.Rules.Add(rule4);
            //fsCruiseControl.Rules.Add(rule5);
            //fsCruiseControl.Rules.Add(rule6);
            //fsCruiseControl.Rules.Add(rule7);
            //fsCruiseControl.Rules.Add(rule8);
            //fsCruiseControl.Rules.Add(rule9);

            ///////////////////////////////////////////////////////////////////

            return fsCruiseControl;
        }

        void AddSugenoFuzzyRule(
            SugenoFuzzySystem fs,
            FuzzyVariable fv1,
            FuzzyVariable fv2,
            SugenoVariable sv,
            string value1,
            string value2,
            string result)
        {
            SugenoFuzzyRule rule = fs.EmptyRule();
            rule.Condition.Op = OperatorType.And;
            rule.Condition.ConditionsList.Add(rule.CreateCondition(fv1, fv1.GetTermByName(value1)));
            rule.Condition.ConditionsList.Add(rule.CreateCondition(fv2, fv2.GetTermByName(value2)));
            rule.Conclusion.Var = sv;
            rule.Conclusion.Term = sv.GetFuncByName(result);
            fs.Rules.Add(rule);
        }
        private void frm_usageCriteria_Load_1(object sender, EventArgs e)
        {

        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
           
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
        }

        private void label9_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {


            // Create new fuzzy system
           /* if (_fsCruiseControl == null)
            {
                _fsCruiseControl = CreateSystem();
                if (_fsCruiseControl == null)
                {
                    return;
                }
            }

            //
            // Get variables from the system (for convinience only)
            //
            FuzzyVariable fvCPU = _fsCruiseControl.InputByName("CPU");
            FuzzyVariable fvCore = _fsCruiseControl.InputByName("Core");
            FuzzyVariable fvRam = _fsCruiseControl.InputByName("Ram");
            FuzzyVariable fvPrice = _fsCruiseControl.InputByName("Price");
            FuzzyVariable fvStorage = _fsCruiseControl.InputByName("Storage");
            FuzzyVariable fvBattery = _fsCruiseControl.InputByName("Battery");
            FuzzyVariable fvScreen1 = _fsCruiseControl.InputByName("Screen1");
            FuzzyVariable fvScreen2 = _fsCruiseControl.InputByName("Screen2");
            FuzzyVariable fvCamera = _fsCruiseControl.InputByName("Camera");
           
            //SugenoVariable svAccelerate = _fsCruiseControl.OutputByName("Accelerate");

            //
            // Fuzzify input values
            //
            Dictionary<FuzzyVariable, double> inputValues = new Dictionary<FuzzyVariable, double>();
            //inputValues.Add(fvCPU, (double)nudInputSpeedError.Value);
            inputValues.Add(fvCPU,  2000);
            inputValues.Add(fvCore,  4);
            inputValues.Add(fvRam, 2048);
            inputValues.Add(fvPrice,  300);
            inputValues.Add(fvStorage, 16);
            inputValues.Add(fvBattery, 5000);
            inputValues.Add(fvScreen1,  700);
            inputValues.Add(fvScreen2, 1000);
            inputValues.Add(fvCamera,  8);
            Dictionary<FuzzyVariable, Dictionary<FuzzyTerm, double>> fuzzifiedInput =
               _fsCruiseControl.Fuzzify(inputValues);
            //inputValues.Add(fvSpeedErrorDot, (double)nudInputSpeedErrorDot.Value);

            //
            // Calculate the result
            //
           // Dictionary<SugenoVariable, double> result = _fsCruiseControl.Calculate(inputValues);

           // tbAccelerate.Text = (result[svAccelerate] * 100.0).ToString("f1");*/
        }

        private void cmbox_case_SelectedIndexChanged(object sender, EventArgs e)
        {

            if ((cmbox_case.SelectedValue.ToString() != "System.Data.DataRowView"))
            {
                statistics = new Statistics(cmbox_case.SelectedValue.ToString(), cmbox_case.Text);

                dG_Prob.Columns.Clear();

            }
           
        }

        private void button2_Click(object sender, EventArgs e)
        {

        }

        private void dG_With_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void button8_Click(object sender, EventArgs e)
        {

        }

        private void button11_Click(object sender, EventArgs e)
        {

        }

        private void button14_Click(object sender, EventArgs e)
        {

        }

        private void button23_Click(object sender, EventArgs e)
        {
            dg_randomcases.Columns.Clear();
            dg_randomcases.Rows.Clear();
            if  (cmbox_case.SelectedValue.ToString().Trim() != "")
            {
                //statistics = new Statistics(cmbox_case.SelectedValue.ToString(), cmbox_case.Text);
                statistics.GetRandomCases_File();
                if (statistics.exp_random_cases.Count != 0)
                {
                    for (int j = 0; j < statistics.exp_random_cases[0].GetFeatures().Count; j++)
                    {
                        Feature feature = (Feature)statistics.exp_random_cases[0].GetFeatures()[j];
                        if (feature.GetFeatureName() == "id" || feature.GetFeatureName() == "cluster") continue;
                        dg_randomcases.Columns.Add(feature.GetFeatureName(), feature.GetFeatureName());
                    }
                    dg_randomcases.Rows.Add(statistics.exp_random_cases.Count);

                    for (int i = 0; i < statistics.exp_random_cases.Count; i++)
                    {
                        Case c = statistics.exp_random_cases_nonstandrize[i];
                        for (int j = 0; j < c.GetFeatures().Count; j++)
                        {
                            Feature f = (Feature)c.GetFeatures()[j];
                            if (f.GetFeatureName() == "id" || f.GetFeatureName() == "cluster") continue;
                            dg_randomcases.Rows[i].Cells[f.GetFeatureName()].Value = f.GetFeatureValue();
                        }
                    }
                }
            }
        }

        private void button12_Click(object sender, EventArgs e)
        {

        }

        private void button11_Click_1(object sender, EventArgs e)
        {

            // display
          

            int dG_NonR = 0;
           
            dG_Non.Columns.Clear();
        


            //dG_Non.Columns.Add("Porblem No ", "Porblem No  ");
            dG_Non.Columns.Add("SolNum", "Sol Num  ");
            dG_Non.Columns.Add("Weight  ", "Weight  ");
            dG_Non.Columns.Add("Sim  ", "Sim  ");



            List<Case> solutions = statistics.FindAlternative_WithClustering(statistics.exp_random_cases[problem_num]);
            // call method
            listBox1.Items.Add("Finding Alternatices by specifying best cluster");

            double[] ranks;
            int cri = 0;
            Dictionary<int, double[,]> choices;
            double[,] criteriaarr;

            MyAHPModel myahp = new MyAHPModel(statistics.count_citeria, solutions.Count);
            criteriaarr = GenerateComparison.CriteriaComparisonMatrix;
            myahp.AddCriteria(criteriaarr);
            

            
            if (myahp.CalculatedCriteria())
            {
               choices =GenerateComparison.Create_All_Criteria_Choice_Comparison_Array(solutions);
                myahp.AddCriterionRatedChoices(choices);
                ranks =myahp.CalculatedChoices(out cri);
                for (int i = 0; i < ranks.Length; i++)
                {

                    dG_Non.Rows.Add();
                    dG_Non.Rows[dG_NonR].Cells[0].Value = solutions[i].GetFeature("id").GetFeatureValue().ToString();
                    dG_Non.Rows[dG_NonR].Cells[1].Value = Math.Round(ranks[i] * 100, 2);
                    EuclideanSimilarity s = new EuclideanSimilarity();
                    dG_Non.Rows[dG_NonR].Cells[2].Value = Math.Round(s.Similarity(solutions[i], statistics.exp_random_cases[problem_num]) * 100, 2);
                    dG_NonR++;
                }
            }
            dG_Non.Sort(dG_Non.Columns[1], ListSortDirection.Descending);

            // display Criteria
            if (dG_Prob.Columns.Count == 0)
            {
                dG_Prob.Columns.Add("Criteria", " Criteria ");
                dG_Prob.Columns.Add("AHP", "AHP");
                dG_Prob.Columns.Add("FuzzyAHP", "Fuzzy AHP");
                for (int i = 1; i < solutions[0].GetFeatures().Count - 1; i++)
                {
                    dG_Prob.Rows.Add();
                    Feature f = (Feature)solutions[0].GetFeatures()[i];
                    dG_Prob.Rows[i - 1].Cells[0].Value = f.GetFeatureName().ToString();
                    dG_Prob.Rows[i - 1].Cells[1].Value = Math.Round(myahp.CriteriaWeights[i - 1] * 100, 2);
                }

            }
            else
                for (int i = 1; i < solutions[0].GetFeatures().Count - 1; i++)
                {
                    Feature f = (Feature)solutions[0].GetFeatures()[i];
                    dG_Prob.Rows[i - 1].Cells[1].Value = Math.Round(myahp.CriteriaWeights[i - 1] * 100, 2);
                }
            dG_Prob.AutoResizeColumns();
            dG_Prob.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
            dG_Non.AutoResizeColumns();
            dG_Non.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
        }

        private void button21_Click(object sender, EventArgs e)
        {

            // display


            int dG_WithR = 0;

            dG_With.Columns.Clear();
    


            //dG_Non.Columns.Add("Porblem No ", "Porblem No  ");
            dG_With.Columns.Add("SolNum", "Sol Num  ");
            dG_With.Columns.Add("Weight  ", "Weight  ");
            dG_With.Columns.Add("Sim  ", "Sim  ");



            List<Case> solutions = statistics.FindAlternative_WithClustering(statistics.exp_random_cases[problem_num]);
            // call method
            listBox1.Items.Add("Finding Alternatices by specifying best cluster");

            double[] ranks;
            Dictionary<int, double[,]> choices;
            double[,] criteriaarr;

            MyFuzzyAHP myfahp = new MyFuzzyAHP(statistics.count_citeria, solutions.Count);
            criteriaarr = GenerateComparison.CriteriaComparisonMatrix;
                
            myfahp.AddCriteria(criteriaarr);
            myfahp.CalculatedCriteria();
            choices = GenerateComparison.Create_All_Criteria_Choice_Comparison_Array(solutions);
            myfahp.AddCriterionRatedChoices(choices);
            ranks = myfahp.CalculatedChoices();
            for (int i = 0; i < ranks.Length; i++)
            {

                dG_With.Rows.Add();
                dG_With.Rows[dG_WithR].Cells[0].Value = solutions[i].GetFeature("id").GetFeatureValue().ToString();
                dG_With.Rows[dG_WithR].Cells[1].Value = Math.Round(ranks[i] * 100, 2);
                EuclideanSimilarity s = new EuclideanSimilarity();
                dG_With.Rows[dG_WithR].Cells[2].Value = Math.Round(s.Similarity(solutions[i], statistics.exp_random_cases[problem_num]) * 100, 2);
                dG_WithR++;
            }
           

            dG_With.Sort(dG_With.Columns[1], ListSortDirection.Descending);

            if (dG_Prob.Columns.Count==0)
            {
                dG_Prob.Columns.Add("Criteria", " Criteria ");
                dG_Prob.Columns.Add("AHP", "AHP");
                dG_Prob.Columns.Add("FuzzyAHP", "Fuzzy AHP");
                for (int i = 1; i < solutions[0].GetFeatures().Count - 1; i++)
                {
                    dG_Prob.Rows.Add();
                    Feature f = (Feature)solutions[0].GetFeatures()[i];
                    dG_Prob.Rows[i - 1].Cells[0].Value = f.GetFeatureName().ToString();
                    dG_Prob.Rows[i - 1].Cells[2].Value = Math.Round(myfahp.CriteriaWeights[i - 1] * 100, 2);
                }

            }
            else
            for (int i = 1; i < solutions[0].GetFeatures().Count - 1; i++)
            {
                Feature f = (Feature)solutions[0].GetFeatures()[i];
                dG_Prob.Rows[i - 1].Cells[2].Value = Math.Round(myfahp.CriteriaWeights[i-1] * 100, 2);
            }

            dG_Prob.AutoResizeColumns();
            dG_Prob.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
            dG_With.AutoResizeColumns();
            dG_With.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
         
            // display

        }

        private void button2_Click_1(object sender, EventArgs e)
        {
          GenerateComparison.Create_Criteria_Comparison_Array(statistics.exp_random_cases[0].GetFeatures().Count - 2);
          dG_Prob.Columns.Clear();
          dG_Non.Columns.Clear(); 
           dG_With .Columns.Clear();

        }

        private void dG_Non_SelectionChanged(object sender, EventArgs e)
        {
            string sols = "";

            foreach (DataGridViewRow row in dG_Non.SelectedRows)
            {
                if (row.Cells[2].Value != null)
                    sols = "id  =" + row.Cells["SolNum"].Value.ToString();

                frm_showdatabase fs = new frm_showdatabase();
                fs.dataGridView1.DataSource = Db.get_cases_customized_condition(statistics.tablename, sols);
                fs.Show();

            }
        }

        private void dG_With_SelectionChanged(object sender, EventArgs e)
        {
            string sols = "";

            foreach (DataGridViewRow row in dG_With.SelectedRows)
            {
                if (row.Cells[2].Value != null)
                    sols = "id  =" + row.Cells["SolNum"].Value.ToString();

                frm_showdatabase fs = new frm_showdatabase();
                fs.dataGridView1.DataSource = Db.get_cases_customized_condition(statistics.tablename, sols);
                fs.Show();

            }
        }

        private void dg_randomcases_RowHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            problem_num = e.RowIndex;
        }

        private void dG_Prob_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
    }
}
