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

		public readonly List<NoteInstance> NoteList = new List<NoteInstance>();
		public readonly List<Interval> IntervalList = new List<Interval>();
		public readonly MidiSequence Midi;

		public int TickByBar;

		public float Bpm;
		public float Bps;

		public float Rating = 0.5f;


		public Track(MidiSequence midi, int firstNote)
		{
			Midi = midi;

			Init();
			GenNoteList();
			GenSymbolList(firstNote);
			GetQuality();
		}

		private void Init()
		{
			var timeSignatureMetaMidiEvent = Midi.Tracks[0].Events.First(e => e is TimeSignatureMetaMidiEvent) as TimeSignatureMetaMidiEvent;
			var tempoMetaMidiEvent = Midi.Tracks[0].Events.First(e => e is TempoMetaMidiEvent) as TempoMetaMidiEvent;

			BeatPerBar = (int)timeSignatureMetaMidiEvent.Numerator;

			TickByBar = BeatPerBar * Midi.TicksPerBeatOrFrame /*/ timeSignatureMetaMidiEvent.Denominator*/;

			Bpm = 60000000.0f / tempoMetaMidiEvent.Value;
			Bps = 1000000.0f / tempoMetaMidiEvent.Value;
		}

		private void GenNoteList()
		{
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

			BarCount = NoteList.Any() ? ((int)NoteList.Last().GetEndBar(TickByBar)) + 1 : 4;
			var endTicks = BarCount * TickByBar;
			foreach (var n in NoteList)
				if (n.EndTickVirtual == long.MaxValue)
					n.EndTickVirtual = endTicks;
		}

		private void GenSymbolList(int firstNote)
		{
			var lastBar = firstNote >= 0 ? 0 : -1;
			var lastNote = firstNote;

			foreach (var n in NoteList)
			{
				var bar = n.GetBar(TickByBar);
				if (n.IsStrong(TickByBar))
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


	}
}
