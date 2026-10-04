using MidiSharp;
using MouseLib;
using System;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using TraverseHelperLib;
using DrawHelperLib;
using System.Media;
using NAudio.Wave;

namespace StrongIntervalTheoryLib
{
	public partial class MainForm : Form
	{
		private const double ZoomMult = 0.001;
		private const string FileName = "test2.midi";

		private readonly BpmSignatureForm BpmSignatureForm;

		private const int PianoWidth = 100;
		private const int TimelineTop= 70;
		private const int TimelineHeight = 120;
		private const int NoteHeight = 20;
		private const int BarWidthConst = 80;
		private const int MeterWidth = 200;
		private const int MixerHeight = 30;
		private const int PianoMixerShift = 20;

		private const int NoteCount = 20;

		private PointF IntervalPosition = new PointF(100, 560);
		private PointF MeterPosition = new PointF(100, 610);
		private PointF MixerPosition = new PointF(500, 45);

		private readonly MouseController MouseController = new MouseController();

		public static Color BadColor = Color.Red;
		public static Color GoodColor = Color.Orange;

		private float BeatStart = 0;
		private int NoteStart = 45;

		private float DragBeatStart;
		private int DragNoteStart;

		private int SelectedBeat = 0;

		private Interval SelectedInterval;

		public ProjectJson ProjectJson;

		private float MixerMainVolume = 1;
		private float MixerPianoVolume = 1;


		private bool PlayStatus;
		private float PlayBeatPosition;

		private float BarWidth
		{
			get
			{
				return (float)(BarWidthConst * Math.Exp(ZoomInt * ZoomMult));
			}
		}

		private float BeatWidth
		{
			get
			{
				return (float)(BarWidthConst * Math.Exp(ZoomInt * ZoomMult) / ProjectJson.Track.BeatPerBar);
			}
		}


		private int ZoomInt = 0;

		public MainForm()
		{
			InitializeComponent();

			BpmSignatureForm = new BpmSignatureForm(this);

			this.MouseWheel += Form_MouseWheel;
			MouseController.InitLeftDrag(OnLeftDragStart, OnLeftDragProcess, OnLeftDragProcess);
			MouseController.InitLeftClick(OnLeftClick);
		}

		private void button1_Click(object sender, EventArgs e)
		{
		}

		private void Form1_Paint(object sender, PaintEventArgs e)
		{
			var gr = e.Graphics;


			gr.FillRectangle(Brushes.White, 0, 0, this.Width, this.Height);

			if (ProjectJson == null)
				return;

			DrawGrid(gr);
			DrawNoteAndInterval(gr);
			DrawInterval(gr);
			DrawMeter(gr);
			DrawMixer(gr);
		}

		private bool IsSelected(Interval s)
		{
			var playBar = PlayBeatPosition / ProjectJson.Track.BeatPerBar + 0.15;
			var shift = playBar - (int)playBar;

			return s == SelectedInterval || (PlayStatus && shift < 0.3 && (int)playBar == s.Bar);
		}

		private void DrawGrid(Graphics gr)
		{
			var beatWidth = BarWidth / ProjectJson.Track.BeatPerBar;

			foreach (var i in NoteCount.Traverse())
			{
				var notePosition = NoteCount - 1 - i;

				gr.DrawRectangle(Pens.Black, 2, notePosition * NoteHeight + 2 + TimelineHeight, PianoWidth - 4, NoteHeight - 4);

				var note = (NoteStart + i) % 12;
				if (note == 1 || note == 3 || note == 6 || note == 8 || note == 10)
					gr.FillRectangle(Brushes.Black, 2, notePosition * NoteHeight + 2 + TimelineHeight, PianoWidth /2- 4, NoteHeight - 4);
			}

			var weakBeatPen = new Pen(Color.Black, 1);
			weakBeatPen.DashStyle = System.Drawing.Drawing2D.DashStyle.Dot;

			gr.SetClip(new Rectangle(PianoWidth, 0, this.Width - PianoWidth, this.Height));
			gr.TranslateTransform(-BeatStart * beatWidth, 0);

			foreach (var i in ProjectJson.Track.BarCount.Traverse())
			{
				foreach (var o in (ProjectJson.Track.BeatPerBar+1).Traverse())
				{
					gr.DrawLine(weakBeatPen,
						new PointF(PianoWidth + i * BarWidth + o * beatWidth, TimelineHeight),
						new PointF(PianoWidth + i * BarWidth + o * beatWidth, TimelineHeight + NoteCount * NoteHeight));
				}

				gr.DrawLine(Pens.Black,
					new PointF(PianoWidth + (i) * BarWidth + ProjectJson.Track.StartBeatShift * beatWidth, TimelineTop),
					new PointF(PianoWidth + (i) * BarWidth + ProjectJson.Track.StartBeatShift * beatWidth, TimelineHeight + NoteCount * NoteHeight));
			}

			gr.DrawLine(new Pen(Color.Blue, 1),
				new PointF(PianoWidth + PlayBeatPosition * beatWidth + 1, TimelineTop),
				new PointF(PianoWidth + PlayBeatPosition * beatWidth + 1, TimelineHeight + NoteCount * NoteHeight));

			gr.DrawLine(new Pen(Color.Black, 2),
				new PointF(PianoWidth + SelectedBeat * beatWidth + 1, TimelineTop),
				new PointF(PianoWidth + SelectedBeat * beatWidth + 1, TimelineHeight + NoteCount * NoteHeight));

			gr.TranslateTransform(BeatStart * beatWidth, 0);
			gr.ResetClip();
		}

		private void DrawNoteAndInterval(Graphics gr)
		{
			var beatWidth = BarWidth / ProjectJson.Track.BeatPerBar;

			gr.SetClip(new Rectangle(PianoWidth, TimelineHeight, this.Width - PianoWidth, NoteHeight * NoteCount));
			gr.TranslateTransform(-BeatStart * beatWidth, 0);

			foreach (var n in ProjectJson.Track.NoteList)
			{
				var notePosition = NoteStart + NoteCount - n.Note - 1;

				var brush = Brushes.DarkGreen;
				var pen = Pens.DarkGreen;

				if (PlayBeatPosition > n.GetStartBeat(ProjectJson.Track.TickByBeat) &&
					PlayBeatPosition < n.GetEndBeat(ProjectJson.Track.TickByBeat))
				{
					brush = Brushes.LightGreen;
					pen = Pens.LightGreen;
				}

				gr.FillRectangle(brush,
					PianoWidth + n.GetStartBeat(ProjectJson.Track.TickByBeat) * BeatWidth + 2,
					TimelineHeight + notePosition * NoteHeight + 2,
					(n.GetEndBeat(ProjectJson.Track.TickByBeat) - n.GetStartBeat(ProjectJson.Track.TickByBeat)) * BeatWidth - 3,
					NoteHeight - 4);

				gr.DrawRectangle(pen,
					PianoWidth + n.GetStartBeat(ProjectJson.Track.TickByBeat) * BeatWidth + 2,
					TimelineHeight + notePosition * NoteHeight + 2,
					(n.GetEndVirtualBeat(ProjectJson.Track.TickByBeat) - n.GetStartBeat(ProjectJson.Track.TickByBeat)) * BeatWidth - 3,
					NoteHeight - 4);
			}
			if (ProjectJson.FirstNote >= 0)
			{
				var notePosition = NoteStart + NoteCount - ProjectJson.FirstNote - 1;
				var pen = new Pen(Color.Gray);
				pen.DashStyle = System.Drawing.Drawing2D.DashStyle.Dot;

				gr.DrawRectangle(pen,
					PianoWidth + 2,
					TimelineHeight + notePosition * NoteHeight + 2,
					0.25f * BarWidth - 4,
					NoteHeight - 4);
			}

			foreach (var s in ProjectJson.Track.IntervalList)
			{
				var prevNoteIndex = NoteStart + NoteCount - s.PrevNote - 1;
				var curNoteIndex = NoteStart + NoteCount - s.CurNote - 1;

				var arrowPen = new Pen(s.GetColor(), IsSelected(s) ? 4 : 2);
				if (s.Direction == Interval.DirectionType.Prima)
				{
					gr.DrawArrow(arrowPen,
						new PointF(
							PianoWidth + s.GetStartBar(ProjectJson.Track.TickByBar) * BarWidth - 10,
							TimelineHeight + prevNoteIndex * NoteHeight + NoteHeight / 2),
						new PointF(
							PianoWidth + s.GetStartBar(ProjectJson.Track.TickByBar) * BarWidth + 10,
							TimelineHeight + curNoteIndex * NoteHeight + NoteHeight / 2),
						10,
						3);
				}
				else
				{
					gr.DrawArrow(arrowPen,
						new PointF(
							PianoWidth + s.GetStartBar(ProjectJson.Track.TickByBar) * BarWidth,
							TimelineHeight + prevNoteIndex * NoteHeight + NoteHeight / 2),
						new PointF(
							PianoWidth + s.GetStartBar(ProjectJson.Track.TickByBar) * BarWidth,
							TimelineHeight + curNoteIndex * NoteHeight + NoteHeight / 2),
						10,
						3);
				}
			}

			gr.TranslateTransform(BeatStart * beatWidth, 0);
			gr.ResetClip();
		}

		private void DrawInterval(Graphics gr)
		{
			gr.TranslateTransform(IntervalPosition.X, IntervalPosition.Y);

			foreach (var si in ProjectJson.Track.IntervalList.Count.Traverse())
			{
				var s = ProjectJson.Track.IntervalList[si];
				var pos = new PointF(si * 20, 0);

				var arrowPen = new Pen(s.GetColor(), IsSelected(s) ? 4 : 2);

				if (s.Direction == Interval.DirectionType.Up)
				{
					gr.DrawArrow(arrowPen,
						new PointF(0, 10).Plus(pos),
						new PointF(0, -10).Plus(pos), 10, 5);
				}
				if (s.Direction == Interval.DirectionType.Down)
				{
					gr.DrawArrow(arrowPen,
						new PointF(0, -10).Plus(pos),
						new PointF(0, 10).Plus(pos), 10, 5);
				}
				if (s.Direction == Interval.DirectionType.Prima)
				{
					gr.DrawArrow(arrowPen,
						new PointF(-10, 0).Plus(pos),
						new PointF(10, 0).Plus(pos), 10, 5);
				}
			}

			gr.TranslateTransform(-IntervalPosition.X, -IntervalPosition.Y);
		}

		private void DrawMeter(Graphics gr)
		{
			const int segmentCount = 10;

			gr.TranslateTransform(MeterPosition.X, MeterPosition.Y);

			foreach (var i in segmentCount.Traverse())
			{
				var alpha1 = GetAlpha(i, segmentCount + 1);
				var alpha2 = GetAlpha(i+1, segmentCount + 1);
				gr.DrawLine(
					new Pen(Interpolate(BadColor, GoodColor, (alpha1 + alpha2)*.5f)),
					new PointF(Interpolate(0, MeterWidth, alpha1), 0), 
					new PointF(Interpolate(0, MeterWidth, alpha2), 0));
			}
			gr.DrawLine(
				new Pen(BadColor, 2),
				new PointF(0, -5),
				new PointF(0, 5));

			gr.DrawLine(
				new Pen(GoodColor, 2),
				new PointF(MeterWidth, -5),
				new PointF(MeterWidth, 5));


			var color = Interpolate(BadColor, GoodColor, ProjectJson.Track.Rating);
			gr.DrawLine(
				new Pen(color, 3),
				new PointF(MeterWidth * ProjectJson.Track.Rating, -5),
				new PointF(MeterWidth * ProjectJson.Track.Rating, 5));

			gr.DrawString("Shit", this.Font, Brushes.Red, new PointF(-10, 10));
			gr.DrawString("Gold", this.Font, Brushes.Orange, new PointF(MeterWidth-10, 10));

			gr.DrawString(((int)(100 * ProjectJson.Track.Rating)).ToString() + "%", this.Font, new SolidBrush(color),
				new PointF(MeterWidth * ProjectJson.Track.Rating - 10, -20));

			gr.TranslateTransform(-MeterPosition.X, -MeterPosition.Y);
		}

		private void DrawMixer(Graphics gr)
		{
			gr.TranslateTransform(MixerPosition.X, MixerPosition.Y);
			gr.DrawLine(ProjectJson.MainExist ? Pens.Black : Pens.LightGray, 0, -MixerHeight/2, 0, MixerHeight/2);
			gr.DrawLine(PianoCheckBox.Checked ? Pens.Black : Pens.LightGray, PianoMixerShift, -MixerHeight/2, PianoMixerShift, MixerHeight/2);

			var pen = new Pen(ProjectJson.MainExist ? Color.Black : Color.LightGray, 3);
			var pen2 = new Pen(PianoCheckBox.Checked ? Color.Black : Color.LightGray, 3);
			var mainY = Interpolate(MixerHeight / 2, -MixerHeight / 2, MixerMainVolume);
			var pianoY = Interpolate(MixerHeight / 2, -MixerHeight / 2, MixerPianoVolume);
			gr.DrawLine(pen, -5, mainY, 5, mainY);
			gr.DrawLine(pen2, PianoMixerShift - 5, pianoY, PianoMixerShift + 5, pianoY);

			gr.TranslateTransform(-MixerPosition.X, -MixerPosition.Y);
		}

		private Color Interpolate(Color  x1, Color  x2, float alpha)
		{
			return Color.FromArgb
				(
				(int)Interpolate(x1.R, x2.R, alpha),
				(int)Interpolate(x1.G, x2.G, alpha),
				(int)Interpolate(x1.B, x2.B, alpha));
		}

		private float GetAlpha(int a, int max)
		{
			return (float)a / (max - 1);
		}

		private float Interpolate(float x1, float x2, float alpha)
		{
			return x1 * (1-alpha) + x2 * alpha;
		}

		private float BackInterpolate(float x1, float x2, float value)
		{
			return (value - x1) / (x2 - x1);
		}

		private void Form1_MouseDown(object sender, MouseEventArgs e)
		{
			MouseController.MouseDown(e);
		}

		private void Form1_MouseUp(object sender, MouseEventArgs e)
		{
			MouseController.MouseUp(e);
		}

		private void Form1_MouseMove(object sender, MouseEventArgs e)
		{
			MouseController.MouseMove(e);
		}

		private void OnLeftClick(PointF point)
		{
			SelectedInterval = FindSymbol(point);
			if (SelectedInterval == null)
			{
				var s = FindBeat(point);
				if (s != -1)
				{
					if (SelectedBeat == s)
						SelectedBeat = 0;
					else
						SelectedBeat = s;

					PlayBeatPosition = SelectedBeat;
				}
			}

			if(ProjectJson.MainExist)
				ClickMixer(point);

			Invalidate();
		}

		private int FindBeat(PointF point)
		{
			if (ProjectJson == null)
				return -1;

			if (point.Y < TimelineHeight && point.Y > TimelineTop)
			{
				var beatWidth = BarWidth / ProjectJson.Track.BeatPerBar;

				var i = (int)((point.X - PianoWidth) / beatWidth + BeatStart + 0.5f);
				return i;
			}
			return -1;
		}

		private Interval FindSymbol(PointF point)
		{
			if (ProjectJson == null)
				return null;

			var beatWidth = BarWidth / ProjectJson.Track.BeatPerBar;

			foreach (var s in ProjectJson.Track.IntervalList)
			{
				var prevNoteIndex = NoteStart + NoteCount - s.PrevNote - 1;
				var curNoteIndex = NoteStart + NoteCount - s.CurNote - 1;

				var arrowPen = new Pen(s.GetColor(), 3);
				var x = PianoWidth + s.GetStartBar(ProjectJson.Track.TickByBar) * BarWidth - 10 - BeatStart * beatWidth;
				var y1 = TimelineHeight + prevNoteIndex * NoteHeight + NoteHeight / 2;
				var y2 = TimelineHeight + curNoteIndex * NoteHeight + NoteHeight / 2;

				if (point.X > x - 20 &&
					point.X < x + 20 &&
					point.Y > Math.Min(y1, y2) - 20 &&
					point.Y < Math.Max(y1, y2) + 20)
					return s;
			}

			return null;
		}

		private void ClickMixer(PointF point)
		{
			var delta = point.Minus(MixerPosition);

			if (delta.X > -5 && delta.X < 5 && delta.Y > -MixerHeight / 2 && delta.Y < MixerHeight / 2)
			{
				MixerMainVolume = BackInterpolate(MixerHeight / 2, -MixerHeight / 2, delta.Y);
				ProjectJson.MainTrack.SetVolume(MixerMainVolume);
			}

			if (delta.X > PianoMixerShift + -5 && delta.X < PianoMixerShift + 5 && delta.Y > -MixerHeight / 2 && delta.Y < MixerHeight / 2)
			{
				MixerPianoVolume = BackInterpolate(MixerHeight / 2, -MixerHeight / 2, delta.Y);
				ProjectJson.PianoTrack.SetVolume(MixerPianoVolume);
			}
		}

		private bool OnLeftDragStart()
		{
			if (ProjectJson == null)
				return false;

			DragBeatStart = BeatStart;
			DragNoteStart = NoteStart;

			return true;
		}

		private void OnLeftDragProcess(PointF point)
		{
			var beatWidth = BarWidth / ProjectJson.Track.BeatPerBar;
			BeatStart = DragBeatStart - (int)(point.X / beatWidth);
			NoteStart = DragNoteStart + (int)(point.Y / NoteHeight);

			if (NoteStart < 40)
				NoteStart = 40;

			if (NoteStart > 60)
				NoteStart = 60;

			if (BeatStart + (int)((this.Width - PianoWidth) / BarWidth) * ProjectJson.Track.BeatPerBar > ProjectJson.Track.BarCount * ProjectJson.Track.BeatPerBar)
				BeatStart = (ProjectJson.Track.BarCount - (int)((this.Width - PianoWidth) / BarWidth)) * ProjectJson.Track.BeatPerBar;

			if (BeatStart < 0)
				BeatStart = 0;


			Invalidate();
		}

		private void Form_MouseWheel(object sender, MouseEventArgs e)
		{
			ZoomInt += e.Delta;
			Invalidate();
		}

		private void timer1_Tick(object sender, EventArgs e)
		{
			if (ProjectJson != null)
			{
				if (PlayStatus)
				{
					PlayBeatPosition = ProjectJson.MainTrack.GetPositionSeconds() * ProjectJson.Track.Bps;

					var beatWidth = BarWidth / ProjectJson.Track.BeatPerBar;
					if ((PlayBeatPosition - BeatStart) * beatWidth > this.Width/2)
					{
						BeatStart = PlayBeatPosition - this.Width / 2 / beatWidth;
					}

					if (PlayBeatPosition > ProjectJson.Track.BarCount * ProjectJson.Track.BeatPerBar)
					{
						PlayBeatPosition = SelectedBeat;
						PlayStatus = false;
						ProjectJson.MainTrack.Stop();
						ProjectJson.PianoTrack.Stop();
					}
				}
				Invalidate();
			}
		}

		private void MainForm_FormClosed(object sender, FormClosedEventArgs e)
		{
			if (ProjectJson != null)
			{
				ProjectJson.MainTrack.Dispose();
				ProjectJson.PianoTrack.Dispose();
			}
		}

		private void button2_Click(object sender, EventArgs e)
		{
			if (ProjectJson != null)
			{
				ProjectJson.MainTrack.SetPositionSeconds(PlayBeatPosition / ProjectJson.Track.Bps);
				ProjectJson.MainTrack.Play();

				if (ProjectJson.PianoExist && PianoCheckBox.Checked)
				{
					ProjectJson.PianoTrack.SetPositionSeconds(PlayBeatPosition / ProjectJson.Track.Bps);
					ProjectJson.PianoTrack.Play();
				}
				PlayStatus = true;
			}
		}

		private void button3_Click(object sender, EventArgs e)
		{
			if (ProjectJson != null)
			{
				ProjectJson.MainTrack.Stop();
				ProjectJson.PianoTrack.Stop();
				PlayStatus = false;
				PlayBeatPosition = SelectedBeat;
			}
		}

		private void button4_Click(object sender, EventArgs e)
		{
			if (ProjectJson != null)
			{
				ProjectJson.MainTrack.Pause();
				ProjectJson.PianoTrack.Pause();
				PlayStatus = false;
			}
		}

		private void openProjectToolStripMenuItem_Click(object sender, EventArgs e)
		{
			if (openFileDialog1.ShowDialog() == DialogResult.OK)
			{
				ProjectJson = ProjectJson.LoadProject(openFileDialog1.FileName);

				BeatStart = 0;
				NoteStart = (ProjectJson.Track.GetMinNote() + ProjectJson.Track.GetMaxNote()) / 2 - NoteCount / 2;

				StopButton.Enabled = true;
				PlayButton.Enabled = true;
				PauseButton.Enabled = true;

				RefreshBpmLabel();
			}

			Invalidate();
		}

		private void RefreshBpmLabel()
		{
			BpmLabel.Text = string.Format("BPM: {0}, {1}/4", (int)ProjectJson.Track.Bpm, ProjectJson.Track.BeatPerBar);
		}

		private void openMidiToolStripMenuItem_Click(object sender, EventArgs e)
		{
			if (openFileDialog2.ShowDialog() == DialogResult.OK)
			{
				ProjectJson = ProjectJson.LoadMidi(openFileDialog2.FileName);
				BeatStart = 0;
				NoteStart = (ProjectJson.Track.GetMinNote() + ProjectJson.Track.GetMaxNote()) / 2 - NoteCount / 2;

				StopButton.Enabled = false;
				PlayButton.Enabled = false;
				PauseButton.Enabled = false;
			}

			Invalidate();
		}

		private void button1_Click_1(object sender, EventArgs e)
		{
			if(ProjectJson != null)
				BpmSignatureForm.ShowForm();
		}


		public void ShiftRight()
		{
			ProjectJson.Track.ShiftRight();
			Invalidate();
		}

		public void ShiftLeft()
		{
			ProjectJson.Track.ShiftLeft();
			Invalidate();
		}

		public void SignatureDouble()
		{
			ProjectJson.Track.SignatureDouble();
			MultZoom(2);
			Invalidate();
			RefreshBpmLabel();
		}

		public void SignatureHalf()
		{
			ProjectJson.Track.SignatureHalf();
			MultZoom(0.5f);
			Invalidate();
			RefreshBpmLabel();
		}

		private void MultZoom(float mult)
		{
			ZoomInt += (int)(Math.Log(mult) / ZoomMult);
		}

		public void BpmDouble()
		{
			ProjectJson.Track.BpmDouble();
			MultZoom(0.5f);
			Invalidate();
			RefreshBpmLabel();
		}

		public void BpmHalf()
		{
			ProjectJson.Track.BpmHalf();
			MultZoom(2);
			Invalidate();
			RefreshBpmLabel();
		}
	}
}
