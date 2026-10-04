using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace StrongIntervalTheoryLib
{
	public partial class BpmSignatureForm : Form
	{
		private MainForm MainForm;

		public BpmSignatureForm(MainForm mainForm)
		{
			MainForm = mainForm;

			InitializeComponent();
		}


		public void ShowForm()
		{
			this.Show();
			RefreshLabel();
		}

		public void RefreshLabel()
		{
			BpmLabel.Text = string.Format("BPM: {0}", (int)MainForm.ProjectJson.Track.Bpm);
			SignatureLabel.Text = string.Format("Sig: {0}/4", MainForm.ProjectJson.Track.BeatPerBar);
		}

		private void BpmSignatureForm_FormClosing(object sender, FormClosingEventArgs e)
		{
			e.Cancel = true;
			Hide();
		}

		private void SignaturDoubleButton_Click(object sender, EventArgs e)
		{
			MainForm.SignatureDouble();
			RefreshLabel();
		}

		private void SignatureHalfButton_Click(object sender, EventArgs e)
		{
			MainForm.SignatureHalf();
			RefreshLabel();
		}



		private void BpmDoubleButton_Click(object sender, EventArgs e)
		{
			MainForm.BpmDouble();
			RefreshLabel();
		}

		private void BpmHalfButton_Click(object sender, EventArgs e)
		{
			MainForm.BpmHalf();
			RefreshLabel();
		}

		private void ShiftRightButton_Click(object sender, EventArgs e)
		{
			MainForm.ShiftRight();
			RefreshLabel();
		}

		private void ShiftLeftButton_Click(object sender, EventArgs e)
		{
			MainForm.ShiftLeft();
			RefreshLabel();
		}
	}
}
