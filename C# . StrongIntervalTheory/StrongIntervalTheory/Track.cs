using MidiSharp;
using MidiSharp.Events.Meta;
using MidiSharp.Events.Voice.Note;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using TraverseHelperLib;

namespace StrongIntervalTheoryLib
{
	public class Track
	{
		public int BarCount = 16;
		public int BeatPerBar = 4;

		public TempoMetaMidiEvent TempoMetaMidiEvent;

		public readonly List<NoteInstance> NoteList = new List<NoteInstance>();
		public readonly List<Interval> IntervalList = new List<Interval>();
		public readonly MidiSequence Midi;

		public int TickByBar;
		public int TickByBeat;

		public float Bpm;
		public float Bps;

		public float Rating = 0.5f;

		private int FirstNote;
		private float BpmMult = 1;

		public int StartBeatShift = 0;

		public Track(MidiSequence midi, int firstNote)
		{
			FirstNote = firstNote;
			Midi = midi;

			InitBpb();
			InitBpm();
			GenNoteList();
			GenSymbolList(firstNote);
			GetQuality();
		}

		private void InitBpb()
		{
			var timeSignatureMetaMidiEvent = Midi.Tracks[0].Events.First(e => e is TimeSignatureMetaMidiEvent) as TimeSignatureMetaMidiEvent;
			TempoMetaMidiEvent = Midi.Tracks[0].Events.First(e => e is TempoMetaMidiEvent) as TempoMetaMidiEvent;
			BeatPerBar = (int)timeSignatureMetaMidiEvent.Numerator;
		}

		private void InitBpm()
		{
			TickByBar = (int)(BeatPerBar * Midi.TicksPerBeatOrFrame / BpmMult) /*/ timeSignatureMetaMidiEvent.Denominator*/;
			TickByBeat = (int)(Midi.TicksPerBeatOrFrame / BpmMult);

			Bpm = 60000000.0f / TempoMetaMidiEvent.Value * BpmMult;
			Bps = 1000000.0f / TempoMetaMidiEvent.Value * BpmMult;
		}

		private void GenNoteList()
		{
			NoteList.Clear();
			var activeNoteList = new List<NoteInstance>();

			var prevNote = default(NoteInstance);
			var currentTick = default(long);
			foreach (var track in Midi.Tracks)
			{
				foreach (var ev in track.Events.Where(ev => ev is OnNoteVoiceMidiEvent || ev is OffNoteVoiceMidiEvent))
				{
					if (ev is OnNoteVoiceMidiEvent)
					{
						var onNoteVoiceMidiEvent = ev as OnNoteVoiceMidiEvent;
						currentTick += onNoteVoiceMidiEvent.DeltaTime;

						if (prevNote != null)
							prevNote.EndTickVirtual = currentTick;


						prevNote = new NoteInstance
						{
							StartTick = currentTick,
							EndTick = 0,
							EndTickVirtual = long.MaxValue,
							Note = onNoteVoiceMidiEvent.Note,
						};

						activeNoteList.Add(prevNote);
					}

					if (ev is OffNoteVoiceMidiEvent)
					{
						var offNoteVoiceMidiEvent = ev as OffNoteVoiceMidiEvent;
						currentTick += offNoteVoiceMidiEvent.DeltaTime;

						var ni = activeNoteList.First(n => n.Note == offNoteVoiceMidiEvent.Note);
						ni.EndTick = currentTick;
						activeNoteList.Remove(ni);
						NoteList.Add(ni);
					}
				}
			}

			BarCount = NoteList.Any() ? ((int)NoteList.Last().GetEndBeat(TickByBeat) * BeatPerBar) + 1 : 4;
			var endTicks = BarCount * TickByBar;
			foreach (var n in NoteList)
				if (n.EndTickVirtual == long.MaxValue)
					n.EndTickVirtual = endTicks;
		}

		private void GenSymbolList(int firstNote)
		{
			IntervalList.Clear();
			var lastBar = firstNote >= 0 ? 0 : -1;
			var lastNote = firstNote;

			foreach (var n in NoteList)
			{
				var bar = n.GetBar(TickByBar);
				if (n.IsStrong(TickByBeat, StartBeatShift, BeatPerBar))
				{
					if (lastBar >= 0)
					{
						if (n.Note > lastNote)
						{
							IntervalList.Add(new Interval(Interval.DirectionType.Up, bar, n.StartTick, lastNote, n.Note));
						}
						else if (n.Note < lastNote)
						{
							IntervalList.Add(new Interval(Interval.DirectionType.Down, bar, n.StartTick, lastNote, n.Note));
						}
						else
						{
							IntervalList.Add(new Interval(Interval.DirectionType.Prima, bar, n.StartTick, lastNote, n.Note));
						}
					}
					else
					{
						lastBar = bar;
						lastNote = n.Note;
					}
				}

				lastBar = bar;
				lastNote = n.Note;
			}
		}

		private void GetQuality()
		{
			foreach (var i in IntervalList.Count.Traverse())
			{
				var curSymDirection = IntervalList[i].Direction;
				var nextSymDirection = i < IntervalList.Count - 1 ? IntervalList[i + 1].Direction : Interval.DirectionType.Up;

				if (curSymDirection == Interval.DirectionType.Up)
				{
					if (nextSymDirection == Interval.DirectionType.Down || nextSymDirection == Interval.DirectionType.Prima)
					{
						IntervalList[i].Quality = Interval.QualityType.Gold;
					}
					else
					{
						IntervalList[i].Quality = Interval.QualityType.Shit;
					}
				}
				else if (curSymDirection == Interval.DirectionType.Down || curSymDirection == Interval.DirectionType.Prima)
				{
					IntervalList[i].Quality = Interval.QualityType.Gold;
				}
				else
					throw new System.Exception("imposible");
			}

			Rating = (float)IntervalList.Count(i => i.Quality == Interval.QualityType.Gold) / IntervalList.Count;
		}


		public int GetMinNote()
		{
			return NoteList.Select(n => n.Note).Min();
		}

		public int GetMaxNote()
		{
			return NoteList.Select(n => n.Note).Max();
		}


		public void SignatureDouble()
		{
			BeatPerBar *= 2;
			InitAll();
		}

		public void SignatureHalf()
		{
			if (BeatPerBar % 2 == 0)
			{
				BeatPerBar /= 2;
				if (StartBeatShift >= BeatPerBar)
					StartBeatShift = 0;
				InitAll();
			}
		}

		public void BpmDouble()
		{
			BpmMult *= 2;
			InitAll();
		}

		public void BpmHalf()
		{
			BpmMult /= 2;
			InitAll();
		}

		public void InitAll()
		{
			InitBpm();
			GenNoteList();
			GenSymbolList(FirstNote);
			GetQuality();
		}

		public void ShiftRight()
		{
			if (StartBeatShift < BeatPerBar - 1)
			{
				StartBeatShift++;
				InitAll();
			}
		}

		public void ShiftLeft()
		{
			if (StartBeatShift > 0)
			{
				StartBeatShift--;
				InitAll();
			}
		}
	}
}
