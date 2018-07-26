namespace DV2.Net_Graphics_Application
{
    partial class MainForm
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
            Dv2Instance.Disconnect();
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.tabControl_code = new System.Windows.Forms.TabControl();
            this.tabPage_code = new System.Windows.Forms.TabPage();
            this.textBox_Input = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.textBox_code = new System.Windows.Forms.TextBox();
            this.tabPage_log = new System.Windows.Forms.TabPage();
            this.groupBox_log = new System.Windows.Forms.GroupBox();
            this.textBox_log = new System.Windows.Forms.TextBox();
            this.tabPage_pms = new System.Windows.Forms.TabPage();
            this.button1 = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.textBox3 = new System.Windows.Forms.TextBox();
            this.textBox2 = new System.Windows.Forms.TextBox();
            this.dataStorage = new System.Windows.Forms.TextBox();
            this.comboBox_codeType = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.tabPage_outPut = new System.Windows.Forms.TabPage();
            this.groupBox_pic = new System.Windows.Forms.GroupBox();
            this.picBox = new System.Windows.Forms.PictureBox();
            this.tabControl_Graphics = new System.Windows.Forms.TabControl();
            this.tabPage_camera = new System.Windows.Forms.TabPage();
            this.tabPage_dataGridView = new System.Windows.Forms.TabPage();
            this.dataGridView_monitor = new System.Windows.Forms.DataGridView();
            this.dataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn3 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.tabPage_code.SuspendLayout();
            this.tabPage_log.SuspendLayout();
            this.groupBox_log.SuspendLayout();
            this.tabPage_pms.SuspendLayout();
            this.tabPage_outPut.SuspendLayout();
            this.groupBox_pic.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picBox)).BeginInit();
            this.tabControl_code.SuspendLayout();
            this.tabControl_Graphics.SuspendLayout();
            this.tabPage_dataGridView.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView_monitor)).BeginInit();
            this.SuspendLayout();
            // 
            // tabControl_code
            // 
            this.tabControl_code.CausesValidation = false;
            this.tabControl_code.Controls.Add(this.tabPage_code);
            this.tabControl_code.Controls.Add(this.tabPage_log);
            this.tabControl_code.Controls.Add(this.tabPage_pms);
            this.tabControl_code.Location = new System.Drawing.Point(8, 8);
            this.tabControl_code.Name = "tabControl_code";
            this.tabControl_code.SelectedIndex = 0;
            this.tabControl_code.Size = new System.Drawing.Size(768, 496);
            this.tabControl_code.TabIndex = 4;
            // 
            // tabPage_code
            // 
            this.tabPage_code.Controls.Add(this.textBox_Input);
            this.tabPage_code.Controls.Add(this.label4);
            this.tabPage_code.Controls.Add(this.textBox_code);
            this.tabPage_code.Location = new System.Drawing.Point(4, 22);
            this.tabPage_code.Name = "tabPage_code";
            this.tabPage_code.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage_code.Size = new System.Drawing.Size(760, 470);
            this.tabPage_code.TabIndex = 0;
            this.tabPage_code.Text = "画像コード";
            this.tabPage_code.UseVisualStyleBackColor = true;
            // 
            // textBox_Input
            // 
            this.textBox_Input.Location = new System.Drawing.Point(24, 424);
            this.textBox_Input.Name = "textBox_Input";
            this.textBox_Input.Size = new System.Drawing.Size(712, 19);
            this.textBox_Input.TabIndex = 5;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(24, 24);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(56, 12);
            this.label4.TabIndex = 4;
            this.label4.Text = "コード入力";
            // 
            // textBox_code
            // 
            this.textBox_code.Location = new System.Drawing.Point(24, 40);
            this.textBox_code.Multiline = true;
            this.textBox_code.Name = "textBox_code";
            this.textBox_code.ReadOnly = true;
            this.textBox_code.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.textBox_code.Size = new System.Drawing.Size(712, 376);
            this.textBox_code.TabIndex = 4;
            // 
            // tabPage_log
            // 
            this.tabPage_log.Controls.Add(this.groupBox_log);
            this.tabPage_log.Location = new System.Drawing.Point(4, 22);
            this.tabPage_log.Name = "tabPage_log";
            this.tabPage_log.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage_log.Size = new System.Drawing.Size(760, 470);
            this.tabPage_log.TabIndex = 1;
            this.tabPage_log.Text = "LogDatas";
            this.tabPage_log.UseVisualStyleBackColor = true;
            // 
            // groupBox_log
            // 
            this.groupBox_log.Controls.Add(this.textBox_log);
            this.groupBox_log.Location = new System.Drawing.Point(24, 24);
            this.groupBox_log.Name = "groupBox_log";
            this.groupBox_log.Size = new System.Drawing.Size(712, 352);
            this.groupBox_log.TabIndex = 0;
            this.groupBox_log.TabStop = false;
            this.groupBox_log.Text = "Logs";
            // 
            // textBox_log
            // 
            this.textBox_log.Location = new System.Drawing.Point(8, 16);
            this.textBox_log.Multiline = true;
            this.textBox_log.Name = "textBox_log";
            this.textBox_log.ReadOnly = true;
            this.textBox_log.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.textBox_log.Size = new System.Drawing.Size(696, 328);
            this.textBox_log.TabIndex = 0;
            // 
            // tabPage_pms
            // 
            this.tabPage_pms.Controls.Add(this.button1);
            this.tabPage_pms.Controls.Add(this.label3);
            this.tabPage_pms.Controls.Add(this.label2);
            this.tabPage_pms.Controls.Add(this.textBox3);
            this.tabPage_pms.Controls.Add(this.textBox2);
            this.tabPage_pms.Controls.Add(this.dataStorage);
            this.tabPage_pms.Controls.Add(this.comboBox_codeType);
            this.tabPage_pms.Controls.Add(this.label1);
            this.tabPage_pms.Location = new System.Drawing.Point(4, 22);
            this.tabPage_pms.Name = "tabPage_pms";
            this.tabPage_pms.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage_pms.Size = new System.Drawing.Size(760, 470);
            this.tabPage_pms.TabIndex = 2;
            this.tabPage_pms.Text = "システムパラメータ";
            this.tabPage_pms.UseVisualStyleBackColor = true;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(653, 134);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 13;
            this.button1.Text = "機能テスト用";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(24, 120);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(71, 12);
            this.label3.TabIndex = 11;
            this.label3.Text = "Get用点座標";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(24, 72);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(53, 12);
            this.label2.TabIndex = 12;
            this.label2.Text = "中心座標";
            // 
            // textBox3
            // 
            this.textBox3.Location = new System.Drawing.Point(24, 136);
            this.textBox3.Name = "textBox3";
            this.textBox3.Size = new System.Drawing.Size(160, 19);
            this.textBox3.TabIndex = 10;
            // 
            // textBox2
            // 
            this.textBox2.Location = new System.Drawing.Point(24, 88);
            this.textBox2.Name = "textBox2";
            this.textBox2.Size = new System.Drawing.Size(160, 19);
            this.textBox2.TabIndex = 9;
            // 
            // dataStorage
            // 
            this.dataStorage.Location = new System.Drawing.Point(328, 40);
            this.dataStorage.Name = "dataStorage";
            this.dataStorage.ReadOnly = true;
            this.dataStorage.Size = new System.Drawing.Size(400, 19);
            this.dataStorage.TabIndex = 8;
            // 
            // comboBox_codeType
            // 
            this.comboBox_codeType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox_codeType.FormattingEnabled = true;
            this.comboBox_codeType.Location = new System.Drawing.Point(24, 32);
            this.comboBox_codeType.Name = "comboBox_codeType";
            this.comboBox_codeType.Size = new System.Drawing.Size(160, 20);
            this.comboBox_codeType.TabIndex = 7;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(24, 16);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(55, 12);
            this.label1.TabIndex = 6;
            this.label1.Text = "分裂タイプ";
            // 
            // tabPage_outPut
            // 
            this.tabPage_outPut.Controls.Add(this.groupBox_pic);
            this.tabPage_outPut.Location = new System.Drawing.Point(4, 22);
            this.tabPage_outPut.Name = "tabPage_outPut";
            this.tabPage_outPut.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage_outPut.Size = new System.Drawing.Size(632, 454);
            this.tabPage_outPut.TabIndex = 0;
            this.tabPage_outPut.Text = "画像出力";
            this.tabPage_outPut.UseVisualStyleBackColor = true;
            // 
            // groupBox_pic
            // 
            this.groupBox_pic.Controls.Add(this.picBox);
            this.groupBox_pic.Location = new System.Drawing.Point(8, 8);
            this.groupBox_pic.Name = "groupBox_pic";
            this.groupBox_pic.Size = new System.Drawing.Size(616, 440);
            this.groupBox_pic.TabIndex = 5;
            this.groupBox_pic.TabStop = false;
            this.groupBox_pic.Text = "Analog Signal";
            // 
            // picBox
            // 
            this.picBox.Location = new System.Drawing.Point(8, 24);
            this.picBox.Name = "picBox";
            this.picBox.Size = new System.Drawing.Size(600, 400);
            this.picBox.TabIndex = 4;
            this.picBox.TabStop = false;
            // 
            // tabControl_Graphics
            // 
            this.tabControl_Graphics.Controls.Add(this.tabPage_outPut);
            this.tabControl_Graphics.Controls.Add(this.tabPage_camera);
            this.tabControl_Graphics.Controls.Add(this.tabPage_dataGridView);
            this.tabControl_Graphics.Location = new System.Drawing.Point(832, 24);
            this.tabControl_Graphics.Name = "tabControl_Graphics";
            this.tabControl_Graphics.SelectedIndex = 0;
            this.tabControl_Graphics.Size = new System.Drawing.Size(640, 480);
            this.tabControl_Graphics.TabIndex = 5;
            // 
            // tabPage_camera
            // 
            this.tabPage_camera.Location = new System.Drawing.Point(4, 22);
            this.tabPage_camera.Name = "tabPage_camera";
            this.tabPage_camera.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage_camera.Size = new System.Drawing.Size(632, 454);
            this.tabPage_camera.TabIndex = 1;
            this.tabPage_camera.Text = "Real Camera";
            this.tabPage_camera.UseVisualStyleBackColor = true;
            // 
            // tabPage_dataGridView
            // 
            this.tabPage_dataGridView.Controls.Add(this.dataGridView_monitor);
            this.tabPage_dataGridView.Location = new System.Drawing.Point(4, 22);
            this.tabPage_dataGridView.Name = "tabPage_dataGridView";
            this.tabPage_dataGridView.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage_dataGridView.Size = new System.Drawing.Size(632, 454);
            this.tabPage_dataGridView.TabIndex = 2;
            this.tabPage_dataGridView.Text = "DataGridView";
            this.tabPage_dataGridView.UseVisualStyleBackColor = true;
            // 
            // dataGridView_monitor
            // 
            this.dataGridView_monitor.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dataGridView_monitor.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView_monitor.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.dataGridViewTextBoxColumn1,
            this.dataGridViewTextBoxColumn2,
            this.dataGridViewTextBoxColumn3});
            this.dataGridView_monitor.Location = new System.Drawing.Point(8, 8);
            this.dataGridView_monitor.Name = "dataGridView_monitor";
            this.dataGridView_monitor.ReadOnly = true;
            this.dataGridView_monitor.RowTemplate.Height = 23;
            this.dataGridView_monitor.Size = new System.Drawing.Size(616, 440);
            this.dataGridView_monitor.TabIndex = 0;
            // 
            // dataGridViewTextBoxColumn1
            // 
            this.dataGridViewTextBoxColumn1.HeaderText = "ObjName";
            this.dataGridViewTextBoxColumn1.Name = "dataGridViewTextBoxColumn1";
            this.dataGridViewTextBoxColumn1.ReadOnly = true;
            // 
            // dataGridViewTextBoxColumn2
            // 
            this.dataGridViewTextBoxColumn2.HeaderText = "ObjCommand";
            this.dataGridViewTextBoxColumn2.Name = "dataGridViewTextBoxColumn2";
            this.dataGridViewTextBoxColumn2.ReadOnly = true;
            // 
            // dataGridViewTextBoxColumn3
            // 
            this.dataGridViewTextBoxColumn3.HeaderText = "ObjAnalysis";
            this.dataGridViewTextBoxColumn3.Name = "dataGridViewTextBoxColumn3";
            this.dataGridViewTextBoxColumn3.ReadOnly = true;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1476, 593);
            this.Controls.Add(this.tabControl_code);
            this.Controls.Add(this.tabControl_Graphics);
            this.KeyPreview = true;
            this.Name = "MainForm";
            this.Text = "Graphics Form";
            this.Load += new System.EventHandler(this.formloader);
            this.tabControl_code.ResumeLayout(false);
            this.tabPage_code.ResumeLayout(false);
            this.tabPage_code.PerformLayout();
            this.tabPage_log.ResumeLayout(false);
            this.groupBox_log.ResumeLayout(false);
            this.groupBox_log.PerformLayout();
            this.tabPage_pms.ResumeLayout(false);
            this.tabPage_pms.PerformLayout();
            this.tabPage_outPut.ResumeLayout(false);
            this.groupBox_pic.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.picBox)).EndInit();
            this.tabControl_Graphics.ResumeLayout(false);
            this.tabPage_dataGridView.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView_monitor)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.TabPage tabPage_code;
        private System.Windows.Forms.TabPage tabPage_log;
        private System.Windows.Forms.TabPage tabPage_pms;
        private System.Windows.Forms.TabControl tabControl_Graphics;
        private System.Windows.Forms.TabPage tabPage_outPut;
        private System.Windows.Forms.TabPage tabPage_camera;
        //Label
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.GroupBox groupBox_pic;
        //PictureBox
        private System.Windows.Forms.PictureBox picBox;
        private System.Windows.Forms.ComboBox comboBox_codeType;
        private System.Windows.Forms.TextBox textBox3;
        private System.Windows.Forms.TextBox textBox2;
        private System.Windows.Forms.TextBox dataStorage;
        private System.Windows.Forms.TextBox textBox_Input;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.TabPage tabPage_dataGridView;
        private System.Windows.Forms.DataGridView dataGridView_monitor;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn1;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn2;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn3;
        public System.Windows.Forms.TabControl tabControl_code;
        public System.Windows.Forms.TextBox textBox_log;
        public System.Windows.Forms.TextBox textBox_code;
        public System.Windows.Forms.GroupBox groupBox_log;
    }
}

