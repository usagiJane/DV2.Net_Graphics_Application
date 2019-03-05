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
            this.label_Code = new System.Windows.Forms.Label();
            this.textBox_code = new System.Windows.Forms.TextBox();
            this.tabPage_log = new System.Windows.Forms.TabPage();
            this.groupBox_log = new System.Windows.Forms.GroupBox();
            this.textBox_log = new System.Windows.Forms.TextBox();
            this.tabPage_pms = new System.Windows.Forms.TabPage();
            this.button_FFT = new System.Windows.Forms.Button();
            this.button_FS = new System.Windows.Forms.Button();
            this.textBox_FS = new System.Windows.Forms.TextBox();
            this.label_Type = new System.Windows.Forms.Label();
            this.label_FS = new System.Windows.Forms.Label();
            this.dataStorage = new System.Windows.Forms.TextBox();
            this.comboBox_codeType = new System.Windows.Forms.ComboBox();
            this.groupBox_pic = new System.Windows.Forms.GroupBox();
            this.picBox = new System.Windows.Forms.PictureBox();
            this.tabPage_outPut = new System.Windows.Forms.TabPage();
            this.label_Center = new System.Windows.Forms.Label();
            this.label_Get = new System.Windows.Forms.Label();
            this.label_MovementX = new System.Windows.Forms.Label();
            this.label_posX = new System.Windows.Forms.Label();
            this.label_MovementY = new System.Windows.Forms.Label();
            this.label_posY = new System.Windows.Forms.Label();
            this.textBox_CenterCoordinates = new System.Windows.Forms.TextBox();
            this.textBox_FingerGet = new System.Windows.Forms.TextBox();
            this.tabPage_camera = new System.Windows.Forms.TabPage();
            this.tabPage_dataGridView = new System.Windows.Forms.TabPage();
            this.dataGridView_monitor = new System.Windows.Forms.DataGridView();
            this.dataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn3 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.tabControl_Graphics = new System.Windows.Forms.TabControl();
            this.openFileDialog_CommandFile = new System.Windows.Forms.OpenFileDialog();
            this.tabControl_code.SuspendLayout();
            this.tabPage_code.SuspendLayout();
            this.tabPage_log.SuspendLayout();
            this.groupBox_log.SuspendLayout();
            this.tabPage_pms.SuspendLayout();
            this.groupBox_pic.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picBox)).BeginInit();
            this.tabPage_outPut.SuspendLayout();
            this.tabPage_dataGridView.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView_monitor)).BeginInit();
            this.tabControl_Graphics.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabControl_code
            // 
            this.tabControl_code.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.tabControl_code.CausesValidation = false;
            this.tabControl_code.Controls.Add(this.tabPage_code);
            this.tabControl_code.Controls.Add(this.tabPage_log);
            this.tabControl_code.Controls.Add(this.tabPage_pms);
            this.tabControl_code.Location = new System.Drawing.Point(1116, 12);
            this.tabControl_code.Name = "tabControl_code";
            this.tabControl_code.SelectedIndex = 0;
            this.tabControl_code.Size = new System.Drawing.Size(792, 501);
            this.tabControl_code.TabIndex = 4;
            // 
            // tabPage_code
            // 
            this.tabPage_code.Controls.Add(this.textBox_Input);
            this.tabPage_code.Controls.Add(this.label_Code);
            this.tabPage_code.Controls.Add(this.textBox_code);
            this.tabPage_code.Location = new System.Drawing.Point(4, 22);
            this.tabPage_code.Name = "tabPage_code";
            this.tabPage_code.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage_code.Size = new System.Drawing.Size(784, 475);
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
            // label_Code
            // 
            this.label_Code.AutoSize = true;
            this.label_Code.Location = new System.Drawing.Point(24, 24);
            this.label_Code.Name = "label_Code";
            this.label_Code.Size = new System.Drawing.Size(56, 12);
            this.label_Code.TabIndex = 4;
            this.label_Code.Text = "コード入力";
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
            this.tabPage_log.Size = new System.Drawing.Size(784, 475);
            this.tabPage_log.TabIndex = 1;
            this.tabPage_log.Text = "LogDatas";
            this.tabPage_log.UseVisualStyleBackColor = true;
            // 
            // groupBox_log
            // 
            this.groupBox_log.Controls.Add(this.textBox_log);
            this.groupBox_log.Location = new System.Drawing.Point(24, 24);
            this.groupBox_log.Name = "groupBox_log";
            this.groupBox_log.Size = new System.Drawing.Size(712, 405);
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
            this.textBox_log.Size = new System.Drawing.Size(696, 379);
            this.textBox_log.TabIndex = 0;
            // 
            // tabPage_pms
            // 
            this.tabPage_pms.Controls.Add(this.button_FFT);
            this.tabPage_pms.Controls.Add(this.button_FS);
            this.tabPage_pms.Controls.Add(this.textBox_FS);
            this.tabPage_pms.Controls.Add(this.label_Type);
            this.tabPage_pms.Controls.Add(this.label_FS);
            this.tabPage_pms.Controls.Add(this.dataStorage);
            this.tabPage_pms.Controls.Add(this.comboBox_codeType);
            this.tabPage_pms.Location = new System.Drawing.Point(4, 22);
            this.tabPage_pms.Name = "tabPage_pms";
            this.tabPage_pms.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage_pms.Size = new System.Drawing.Size(784, 475);
            this.tabPage_pms.TabIndex = 2;
            this.tabPage_pms.Text = "システムパラメータ";
            this.tabPage_pms.UseVisualStyleBackColor = true;
            // 
            // button_FFT
            // 
            this.button_FFT.Location = new System.Drawing.Point(653, 134);
            this.button_FFT.Name = "button_FFT";
            this.button_FFT.Size = new System.Drawing.Size(75, 23);
            this.button_FFT.TabIndex = 13;
            this.button_FFT.Text = "機能テスト用";
            this.button_FFT.UseVisualStyleBackColor = true;
            this.button_FFT.Click += new System.EventHandler(this.button_FFT_Click);
            // 
            // button_FS
            // 
            this.button_FS.Location = new System.Drawing.Point(336, 237);
            this.button_FS.Name = "button_FS";
            this.button_FS.Size = new System.Drawing.Size(92, 23);
            this.button_FS.TabIndex = 15;
            this.button_FS.Text = "ファイルの選択";
            this.button_FS.UseVisualStyleBackColor = true;
            this.button_FS.Click += new System.EventHandler(this.button_FS_Click);
            // 
            // textBox_FS
            // 
            this.textBox_FS.Location = new System.Drawing.Point(26, 239);
            this.textBox_FS.Name = "textBox_FS";
            this.textBox_FS.ReadOnly = true;
            this.textBox_FS.Size = new System.Drawing.Size(304, 19);
            this.textBox_FS.TabIndex = 14;
            // 
            // label_Type
            // 
            this.label_Type.AutoSize = true;
            this.label_Type.Location = new System.Drawing.Point(24, 16);
            this.label_Type.Name = "label_Type";
            this.label_Type.Size = new System.Drawing.Size(55, 12);
            this.label_Type.TabIndex = 6;
            this.label_Type.Text = "分裂タイプ";
            // 
            // label_FS
            // 
            this.label_FS.AutoSize = true;
            this.label_FS.Location = new System.Drawing.Point(26, 221);
            this.label_FS.Name = "label_FS";
            this.label_FS.Size = new System.Drawing.Size(107, 12);
            this.label_FS.TabIndex = 16;
            this.label_FS.Text = "コマンドファイルを読込";
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
            // groupBox_pic
            // 
            this.groupBox_pic.Controls.Add(this.picBox);
            this.groupBox_pic.Location = new System.Drawing.Point(128, 88);
            this.groupBox_pic.Name = "groupBox_pic";
            this.groupBox_pic.Size = new System.Drawing.Size(386, 262);
            this.groupBox_pic.TabIndex = 5;
            this.groupBox_pic.TabStop = false;
            this.groupBox_pic.Text = "Analog Signal";
            // 
            // picBox
            // 
            this.picBox.BackColor = System.Drawing.Color.Transparent;
            this.picBox.Location = new System.Drawing.Point(119, 83);
            this.picBox.Name = "picBox";
            this.picBox.Size = new System.Drawing.Size(96, 64);
            this.picBox.TabIndex = 4;
            this.picBox.TabStop = false;
            // 
            // tabPage_outPut
            // 
            this.tabPage_outPut.Controls.Add(this.label_Center);
            this.tabPage_outPut.Controls.Add(this.label_Get);
            this.tabPage_outPut.Controls.Add(this.label_MovementX);
            this.tabPage_outPut.Controls.Add(this.label_posX);
            this.tabPage_outPut.Controls.Add(this.label_MovementY);
            this.tabPage_outPut.Controls.Add(this.label_posY);
            this.tabPage_outPut.Controls.Add(this.textBox_CenterCoordinates);
            this.tabPage_outPut.Controls.Add(this.textBox_FingerGet);
            this.tabPage_outPut.Controls.Add(this.groupBox_pic);
            this.tabPage_outPut.Location = new System.Drawing.Point(4, 22);
            this.tabPage_outPut.Name = "tabPage_outPut";
            this.tabPage_outPut.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage_outPut.Size = new System.Drawing.Size(632, 424);
            this.tabPage_outPut.TabIndex = 0;
            this.tabPage_outPut.Text = "画像出力";
            this.tabPage_outPut.UseVisualStyleBackColor = true;
            // 
            // label_Center
            // 
            this.label_Center.AutoSize = true;
            this.label_Center.Location = new System.Drawing.Point(497, 9);
            this.label_Center.Name = "label_Center";
            this.label_Center.Size = new System.Drawing.Size(53, 12);
            this.label_Center.TabIndex = 12;
            this.label_Center.Text = "中心座標";
            // 
            // label_Get
            // 
            this.label_Get.AutoSize = true;
            this.label_Get.Location = new System.Drawing.Point(479, 34);
            this.label_Get.Name = "label_Get";
            this.label_Get.Size = new System.Drawing.Size(71, 12);
            this.label_Get.TabIndex = 11;
            this.label_Get.Text = "Get用点座標";
            // 
            // label_MovementX
            // 
            this.label_MovementX.AutoSize = true;
            this.label_MovementX.Location = new System.Drawing.Point(52, 24);
            this.label_MovementX.Name = "label_MovementX";
            this.label_MovementX.Size = new System.Drawing.Size(68, 12);
            this.label_MovementX.TabIndex = 6;
            this.label_MovementX.Text = "Movement X";
            // 
            // label_posX
            // 
            this.label_posX.AutoSize = true;
            this.label_posX.Location = new System.Drawing.Point(135, 24);
            this.label_posX.Name = "label_posX";
            this.label_posX.Size = new System.Drawing.Size(11, 12);
            this.label_posX.TabIndex = 6;
            this.label_posX.Text = "0";
            // 
            // label_MovementY
            // 
            this.label_MovementY.AutoSize = true;
            this.label_MovementY.Location = new System.Drawing.Point(52, 46);
            this.label_MovementY.Name = "label_MovementY";
            this.label_MovementY.Size = new System.Drawing.Size(68, 12);
            this.label_MovementY.TabIndex = 6;
            this.label_MovementY.Text = "Movement Y";
            // 
            // label_posY
            // 
            this.label_posY.AutoSize = true;
            this.label_posY.Location = new System.Drawing.Point(135, 46);
            this.label_posY.Name = "label_posY";
            this.label_posY.Size = new System.Drawing.Size(11, 12);
            this.label_posY.TabIndex = 6;
            this.label_posY.Text = "0";
            // 
            // textBox_CenterCoordinates
            // 
            this.textBox_CenterCoordinates.Location = new System.Drawing.Point(556, 6);
            this.textBox_CenterCoordinates.Name = "textBox_CenterCoordinates";
            this.textBox_CenterCoordinates.Size = new System.Drawing.Size(70, 19);
            this.textBox_CenterCoordinates.TabIndex = 9;
            // 
            // textBox_FingerGet
            // 
            this.textBox_FingerGet.Location = new System.Drawing.Point(556, 31);
            this.textBox_FingerGet.Name = "textBox_FingerGet";
            this.textBox_FingerGet.Size = new System.Drawing.Size(70, 19);
            this.textBox_FingerGet.TabIndex = 10;
            // 
            // tabPage_camera
            // 
            this.tabPage_camera.Location = new System.Drawing.Point(4, 22);
            this.tabPage_camera.Name = "tabPage_camera";
            this.tabPage_camera.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage_camera.Size = new System.Drawing.Size(632, 424);
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
            this.tabPage_dataGridView.Size = new System.Drawing.Size(632, 424);
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
            // tabControl_Graphics
            // 
            this.tabControl_Graphics.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.tabControl_Graphics.Controls.Add(this.tabPage_outPut);
            this.tabControl_Graphics.Controls.Add(this.tabPage_camera);
            this.tabControl_Graphics.Controls.Add(this.tabPage_dataGridView);
            this.tabControl_Graphics.Location = new System.Drawing.Point(1180, 519);
            this.tabControl_Graphics.Name = "tabControl_Graphics";
            this.tabControl_Graphics.SelectedIndex = 0;
            this.tabControl_Graphics.Size = new System.Drawing.Size(640, 450);
            this.tabControl_Graphics.TabIndex = 5;
            // 
            // openFileDialog_CommandFile
            // 
            this.openFileDialog_CommandFile.Filter = "All files(*.*)|*.*|txt files(*.txt)|*.txt";
            this.openFileDialog_CommandFile.FilterIndex = 2;
            this.openFileDialog_CommandFile.InitialDirectory = "C:\\Users\\zfbin\\OneDrive\\桌面";
            this.openFileDialog_CommandFile.RestoreDirectory = true;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1920, 980);
            this.Controls.Add(this.tabControl_code);
            this.Controls.Add(this.tabControl_Graphics);
            this.KeyPreview = true;
            this.Name = "MainForm";
            this.Text = "Graphics Form";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Load += new System.EventHandler(this.formloader);
            this.tabControl_code.ResumeLayout(false);
            this.tabPage_code.ResumeLayout(false);
            this.tabPage_code.PerformLayout();
            this.tabPage_log.ResumeLayout(false);
            this.groupBox_log.ResumeLayout(false);
            this.groupBox_log.PerformLayout();
            this.tabPage_pms.ResumeLayout(false);
            this.tabPage_pms.PerformLayout();
            this.groupBox_pic.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.picBox)).EndInit();
            this.tabPage_outPut.ResumeLayout(false);
            this.tabPage_outPut.PerformLayout();
            this.tabPage_dataGridView.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView_monitor)).EndInit();
            this.tabControl_Graphics.ResumeLayout(false);
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
        private System.Windows.Forms.Label label_Type;
        private System.Windows.Forms.Label label_Center;
        private System.Windows.Forms.Label label_Get;
        private System.Windows.Forms.Label label_Code;
        private System.Windows.Forms.Label label_MovementX;
        private System.Windows.Forms.Label label_posX;
        private System.Windows.Forms.Label label_MovementY;
        private System.Windows.Forms.Label label_posY;
        private System.Windows.Forms.GroupBox groupBox_pic;
        //PictureBox
        private System.Windows.Forms.PictureBox picBox;
        private System.Windows.Forms.ComboBox comboBox_codeType;
        private System.Windows.Forms.TextBox textBox_FingerGet;
        private System.Windows.Forms.TextBox textBox_CenterCoordinates;
        private System.Windows.Forms.TextBox dataStorage;
        private System.Windows.Forms.TextBox textBox_Input;
        private System.Windows.Forms.Button button_FFT;
        private System.Windows.Forms.TabPage tabPage_dataGridView;
        private System.Windows.Forms.DataGridView dataGridView_monitor;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn1;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn2;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn3;
        public System.Windows.Forms.TabControl tabControl_code;
        public System.Windows.Forms.TextBox textBox_log;
        public System.Windows.Forms.TextBox textBox_code;
        public System.Windows.Forms.GroupBox groupBox_log;
        private System.Windows.Forms.Button button_FS;
        private System.Windows.Forms.TextBox textBox_FS;
        private System.Windows.Forms.Label label_FS;
        private System.Windows.Forms.OpenFileDialog openFileDialog_CommandFile;
    }
}

