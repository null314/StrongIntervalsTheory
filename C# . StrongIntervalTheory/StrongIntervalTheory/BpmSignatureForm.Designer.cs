namespace StrongIntervalTheoryLib
{
	partial class BpmSignatureForm
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
			this.BpmLabel = new System.Windows.Forms.Label();
			this.SignatureLabel = new System.Windows.Forms.Label();
			this.SignatureHalfButton = new System.Windows.Forms.Button();
			this.SignaturDoubleButton = new System.Windows.Forms.Button();
			this.BpmDoubleButton = new System.Windows.Forms.Button();
			this.BpmHalfButton = new System.Windows.Forms.Button();
			this.ShiftRightButton = new System.Windows.Forms.Button();
			this.ShiftLeftButton = new System.Windows.Forms.Button();
			this.SuspendLayout();
			// 
			// BpmLabel
			// 
			this.BpmLabel.AutoSize = true;
			this.BpmLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 20.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
			this.BpmLabel.Location = new System.Drawing.Point(36, 46);
			this.BpmLabel.Name = "BpmLabel";
			this.BpmLabel.Size = new System.Drawing.Size(23, 31);
			this.BpmLabel.TabIndex = 0;
			this.BpmLabel.Text = "-";
			// 
			// SignatureLabel
			// 
			this.SignatureLabel.AutoSize = true;
			this.SignatureLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 20.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
			this.SignatureLabel.Location = new System.Drawing.Point(36, 107);
			this.SignatureLabel.Name = "SignatureLabel";
			this.SignatureLabel.Size = new System.Drawing.Size(23, 31);
			this.SignatureLabel.TabIndex = 1;
			this.SignatureLabel.Text = "-";
			// 
			// SignatureHalfButton
			// 
			this.SignatureHalfButton.Location = new System.Drawing.Point(169, 107);
			this.SignatureHalfButton.Name = "SignatureHalfButton";
			this.SignatureHalfButton.Size = new System.Drawing.Size(32, 31);
			this.SignatureHalfButton.TabIndex = 2;
			this.SignatureHalfButton.Text = "/2";
			this.SignatureHalfButton.UseVisualStyleBackColor = true;
			this.SignatureHalfButton.Click += new System.EventHandler(this.SignatureHalfButton_Click);
			// 
			// SignaturDoubleButton
			// 
			this.SignaturDoubleButton.Location = new System.Drawing.Point(207, 107);
			this.SignaturDoubleButton.Name = "SignaturDoubleButton";
			this.SignaturDoubleButton.Size = new System.Drawing.Size(32, 31);
			this.SignaturDoubleButton.TabIndex = 3;
			this.SignaturDoubleButton.Text = "*2";
			this.SignaturDoubleButton.UseVisualStyleBackColor = true;
			this.SignaturDoubleButton.Click += new System.EventHandler(this.SignaturDoubleButton_Click);
			// 
			// BpmDoubleButton
			// 
			this.BpmDoubleButton.Location = new System.Drawing.Point(207, 52);
			this.BpmDoubleButton.Name = "BpmDoubleButton";
			this.BpmDoubleButton.Size = new System.Drawing.Size(32, 31);
			this.BpmDoubleButton.TabIndex = 5;
			this.BpmDoubleButton.Text = "*2";
			this.BpmDoubleButton.UseVisualStyleBackColor = true;
			this.BpmDoubleButton.Click += new System.EventHandler(this.BpmDoubleButton_Click);
			// 
			// BpmHalfButton
			// 
			this.BpmHalfButton.Location = new System.Drawing.Point(169, 52);
			this.BpmHalfButton.Name = "BpmHalfButton";
			this.BpmHalfButton.Size = new System.Drawing.Size(32, 31);
			this.BpmHalfButton.TabIndex = 4;
			this.BpmHalfButton.Text = "/2";
			this.BpmHalfButton.UseVisualStyleBackColor = true;
			this.BpmHalfButton.Click += new System.EventHandler(this.BpmHalfButton_Click);
			// 
			// ShiftRightButton
			// 
			this.ShiftRightButton.Location = new System.Drawing.Point(297, 107);
			this.ShiftRightButton.Name = "ShiftRightButton";
			this.ShiftRightButton.Size = new System.Drawing.Size(32, 31);
			this.ShiftRightButton.TabIndex = 7;
			this.ShiftRightButton.Text = ">";
			this.ShiftRightButton.UseVisualStyleBackColor = true;
			this.ShiftRightButton.Click += new System.EventHandler(this.ShiftRightButton_Click);
			// 
			// ShiftLeftButton
			// 
			this.ShiftLeftButton.Location = new System.Drawing.Point(259, 107);
			this.ShiftLeftButton.Name = "ShiftLeftButton";
			this.ShiftLeftButton.Size = new System.Drawing.Size(32, 31);
			this.ShiftLeftButton.TabIndex = 6;
			this.ShiftLeftButton.Text = "<";
			this.ShiftLeftButton.UseVisualStyleBackColor = true;
			this.ShiftLeftButton.Click += new System.EventHandler(this.ShiftLeftButton_Click);
			// 
			// BpmSignatureForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(372, 174);
			this.Controls.Add(this.ShiftRightButton);
			this.Controls.Add(this.ShiftLeftButton);
			this.Controls.Add(this.BpmDoubleButton);
			this.Controls.Add(this.BpmHalfButton);
			this.Controls.Add(this.SignaturDoubleButton);
			this.Controls.Add(this.SignatureHalfButton);
			this.Controls.Add(this.SignatureLabel);
			this.Controls.Add(this.BpmLabel);
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "BpmSignatureForm";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "Bpm, Time signature";
			this.TopMost = true;
			this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.BpmSignatureForm_FormClosing);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Label BpmLabel;
		private System.Windows.Forms.Label SignatureLabel;
		private System.Windows.Forms.Button SignatureHalfButton;
		private System.Windows.Forms.Button SignaturDoubleButton;
		private System.Windows.Forms.Button BpmDoubleButton;
		private System.Windows.Forms.Button BpmHalfButton;
		private System.Windows.Forms.Button ShiftRightButton;
		private System.Windows.Forms.Button ShiftLeftButton;
	}
}