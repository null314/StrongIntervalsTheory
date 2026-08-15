namespace StrongIntervalTheoryLib
{
	public class NoteInstance
	{
		public long StartTick;
		public long EndTick;
		public long EndTickVirtual;

		public int Note;


		public float GetStartBar(int tickByBar)
		{
			return (float)StartTick / tickByBar;
		}

		public float GetEndBar(int tickByBar)
		{
			return (float) EndTick / tickByBar;
		}

		public float GetEndVirtualBar(int tickByBar)
		{
			return (float) EndTickVirtual / tickByBar;
		}

		public int GetBar(int tickByBar)
		{
			return (int)((StartTick + tickByBar - 1) / tickByBar);
		}

		public bool IsStrong(int tickByBar)
		{
			var barStart = (StartTick-1) / tickByBar;
			var barEnd = (EndTickVirtual - 1) / tickByBar;
			return barStart < barEnd;
		}
	}
}
