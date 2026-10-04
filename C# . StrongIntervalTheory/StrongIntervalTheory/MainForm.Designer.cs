namespace StrongIntervalTheoryLib
{
	partial class MainForm
	{
		/// <summary>
		/// Обязательная переменная конструктора.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// Освободить все используемые ресурсы.
		/// </summary>
		/// <param name="disposing">истинно, если управляемый ресурс должен быть удален; иначе ложно.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing && (components != null))
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Код, автоматически созданный конструктором форм Windows

		/// <summary>
		/// Требуемый метод для поддержки конструктора — не изменяйте 
		/// содержимое этого метода с помощью редактора кода.
		/// </summary>
		private void InitializeComponent()
		{
			this.components = new System.ComponentModel.Container();
			this.timer1 = new System.Windows.Forms.Timer(this.components);
			this.PlayButton = new System.Windows.Forms.Button();
			this.StopButton = new System.Windows.Forms.Button();
			this.PauseButton = new System.Windows.Forms.Button();
			this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
			this.PianoCheckBox = new System.Windows.Forms.CheckBox();
			this.menuStrip1 = new System.Windows.Forms.MenuStrip();
			this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.openProjectToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.openMidiToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.openFileDialog2 = new System.Windows.Forms.OpenFileDialog();
			this.BpmLabel = new System.Windows.Forms.Label();
			this.button1 = new System.Windows.Forms.Button();
			this.menuStrip1.SuspendLayout();
			this.SuspendLayout();
			// 
			// timer1
			// 
			this.timer1.Enabled = true;
			this.timer1.Interval = 25;
			this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
			// 
			// PlayButton
			// 
			this.PlayButton.Location = new System.Drawing.Point(267, 31);
			this.PlayButton.Name = "PlayButton";
			this.PlayButton.Size = new System.Drawing.Size(75, 23);
			this.PlayButton.TabIndex = 2;
			this.PlayButton.Text = "PLAY";
			this.PlayButton.UseVisualStyleBackColor = true;
			this.PlayButton.Click += new System.EventHandler(this.button2_Click);
			// 
			// StopButton
			// 
			this.StopButton.Location = new System.Drawing.Point(186, 31);
			this.StopButton.Name = "StopButton";
			this.StopButton.Size = new System.Drawing.Size(75, 23);
			this.StopButton.TabIndex = 3;
			this.StopButton.Text = "STOP";
			this.StopButton.UseVisualStyleBackColor = true;
			this.StopButton.Click += new System.EventHandler(this.button3_Click);
			// 
			// PauseButton
			// 
			this.PauseButton.Location = new System.Drawing.Point(348, 31);
			this.PauseButton.Name = "PauseButton";
			this.PauseButton.Size = new System.Drawing.Size(75, 23);
			this.PauseButton.TabIndex = 4;
			this.PauseButton.Text = "PAUSE";
			this.PauseButton.UseVisualStyleBackColor = true;
			this.PauseButton.Click += new System.EventHandler(this.button4_Click);
			// 
			// openFileDialog1
			// 
			this.openFileDialog1.DefaultExt = "json";
			this.openFileDialog1.FileName = "project.json";
			this.openFileDialog1.Filter = "project|*.json";
			// 
			// PianoCheckBox
			// 
			this.PianoCheckBox.AutoSize = true;
			this.PianoCheckBox.BackColor = System.Drawing.Color.White;
			this.PianoCheckBox.Location = new System.Drawing.Point(526, 35);
			this.PianoCheckBox.Name = "PianoCheckBox";
			this.PianoCheckBox.Size = new System.Drawing.Size(52, 17);
			this.PianoCheckBox.TabIndex = 5;
			this.PianoCheckBox.Text = "piano";
			this.PianoCheckBox.UseVisualStyleBackColor = false;
			// 
			// menuStrip1
			// 
			this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem});
			this.menuStrip1.Location = new System.Drawing.Point(0, 0);
			this.menuStrip1.Name = "menuStrip1";
			this.menuStrip1.Size = new System.Drawing.Size(1264, 24);
			this.menuStrip1.TabIndex = 6;
			this.menuStrip1.Text = "menuStrip1";
			// 
			// fileToolStripMenuItem
			// 
			this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.openProjectToolStripMenuItem,
            this.openMidiToolStripMenuItem});
			this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
			this.fileToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
			this.fileToolStripMenuItem.Text = "File";
			// 
			// openProjectToolStripMenuItem
			// 
			this.openProjectToolStripMenuItem.Name = "openProjectToolStripMenuItem";
			this.openProjectToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
			this.openProjectToolStripMenuItem.Text = "Open Project";
			this.openProjectToolStripMenuItem.Click += new System.EventHandler(this.openProjectToolStripMenuItem_Click);
			// 
			// openMidiToolStripMenuItem
			// 
			this.openMidiToolStripMenuItem.Name = "openMidiToolStripMenuItem";
			this.openMidiToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
			this.openMidiToolStripMenuItem.Text = "Open Midi";
			this.openMidiToolStripMenuItem.Click += new System.EventHandler(this.openMidiToolStripMenuItem_Click);
			// 
			// openFileDialog2
			// 
			this.openFileDialog2.FileName = "openFileDialog2";
			this.openFileDialog2.Filter = "midi files|*.mid|midi files|*.midi";
			// 
			// BpmLabel
			// 
			this.BpmLabel.AutoSize = true;
			this.BpmLabel.BackColor = System.Drawing.Color.White;
			this.BpmLabel.ForeColor = System.Drawing.Color.Black;
			this.BpmLabel.Location = new System.Drawing.Point(689, 35);
			this.BpmLabel.Name = "BpmLabel";
			this.BpmLabel.Size = new System.Drawing.Size(10, 13);
			this.BpmLabel.TabIndex = 7;
			this.BpmLabel.Text = "-";
			// 
			// button1
			// 
			this.button1.Location = new System.Drawing.Point(852, 31);
			this.button1.Name = "button1";
			this.button1.Size = new System.Drawing.Size(119, 23);
			this.button1.TabIndex = 8;
			this.button1.Text = "BPM, Time signature";
			this.button1.UseVisualStyleBackColor = true;
			this.button1.Click += new System.EventHandler(this.button1_Click_1);
			// 
			// MainForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(1264, 681);
			this.Controls.Add(this.button1);
			this.Controls.Add(this.BpmLabel);
			this.Controls.Add(this.PianoCheckBox);
			this.Controls.Add(this.PauseButton);
			this.Controls.Add(this.StopButton);
			this.Controls.Add(this.PlayButton);
			this.Controls.Add(this.menuStrip1);
			this.DoubleBuffered = true;
			this.MainMenuStrip = this.menuStrip1;
			this.Name = "MainForm";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "Strong Interval Theory";
			this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.MainForm_FormClosed);
			this.Paint += new System.Windows.Forms.PaintEventHandler(this.Form1_Paint);
			this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.Form1_MouseDown);
			this.MouseMove += new System.Windows.Forms.MouseEventHandler(this.Form1_MouseMove);
			this.MouseUp += new System.Windows.Forms.MouseEventHandler(this.Form1_MouseUp);
			this.menuStrip1.ResumeLayout(false);
			this.menuStrip1.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion
		private System.Windows.Forms.Timer timer1;
		private System.Windows.Forms.Button PlayButton;
		private System.Windows.Forms.Button StopButton;
		private System.Windows.Forms.Button PauseButton;
		private System.Windows.Forms.OpenFileDialog openFileDialog1;
		private System.Windows.Forms.CheckBox PianoCheckBox;
		private System.Windows.Forms.MenuStrip menuStrip1;
		private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem openProjectToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem openMidiToolStripMenuItem;
		private System.Windows.Forms.OpenFileDialog openFileDialog2;
		private System.Windows.Forms.Label BpmLabel;
		private System.Windows.Forms.Button button1;
	}
}

