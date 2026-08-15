using System.Drawing;

namespace StrongIntervalTheoryLib
{
	public class Interval
	{
		public enum DirectionType
		{
			Up, 
			Prima,
			Down,
		}

		public enum QualityType
		{
			Gold, 
			Shit,
		}

		public readonly DirectionType Direction;
		public readonly int Bar;
		public readonly long StartTick;
		public readonly int PrevNote;
		public readonly int CurNote;

		public QualityType Quality;

		public Interval(DirectionType direction, int bar, long startTick, int prevNote, int curNote)
		{
			Direction = direction;
			Bar = bar;
			StartTick = startTick;
			PrevNote = prevNote;
			CurNote = curNote;
		}

		public float GetStartBar(int tickByBar)
		{
			return (float)StartTick / tickByBar;
		}

		public Color GetColor()
		{
			switch(Quality)
			{
				case QualityType.Gold: return MainForm.GoodColor;
				case QualityType.Shit: return MainForm.BadColor;
				default: throw new System.Exception("imposible");
			}
		}

	}
}
