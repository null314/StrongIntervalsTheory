namespace StrongIntervalTheoryLib
{
	public class NoteInstance
	{
		public long StartTick;
		public long EndTick;
		public long EndTickVirtual;

		public int Note;


		public float GetStartBeat(int tickByBeat)
		{
			return (float)StartTick / tickByBeat;
		}

		public float GetEndBeat(int tickByBeat)
		{
			return (float)EndTick / tickByBeat;
		}

		public float GetEndVirtualBeat(int tickByBeat)
		{
			return (float)EndTickVirtual / tickByBeat;
		}


		public int GetBar(int tickByBar)
		{
			return (int)((StartTick + tickByBar - 1) / tickByBar);
		}

		public bool IsStrong(int tickByBeat, int shiftBeat, int beatPerBar)
		{
			var barStart = ((StartTick-1) / tickByBeat + beatPerBar - shiftBeat) / beatPerBar;
			var barEnd = ((EndTickVirtual - 1) / tickByBeat + beatPerBar - shiftBeat)/ beatPerBar;
			return barStart < barEnd;
		}

	}
}
