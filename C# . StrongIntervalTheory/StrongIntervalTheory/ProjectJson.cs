using JsonLib;
using MidiSharp;
using System;
using System.Collections.Generic;
using System.IO;

namespace StrongIntervalTheoryLib
{
	public class ProjectJson: IDeserializable
	{
		private const string AudioFileNameParam = "AudioFileName";
		private const string MidiFileNameParam = "MidiFileName";
		private const string PianoFileNameParam = "PianoFileName";
		private const string FirstNoteParam = "FirstNote";

		public string AudioFileName;
		public string MidiFileName;
		public string PianoFileName;
		public int FirstNote;



		public MidiSequence Midi;
		public Track Track;
		public SoundFunction MainTrack = new SoundFunction();
		public bool MainExist;
		public SoundFunction PianoTrack = new SoundFunction();
		public bool PianoExist;


		public static ProjectJson LoadMidi(string midiFileName)
		{
			try
			{
				var projectJson = new ProjectJson();

				projectJson.FirstNote = -1;
				projectJson.MidiFileName = midiFileName;
				projectJson.PianoExist = false;
				projectJson.MainExist = false;

				using (Stream inputStream = File.OpenRead(midiFileName))
				{
					projectJson.Midi = MidiSequence.Open(inputStream);
					projectJson.Track = new Track(projectJson.Midi, projectJson.FirstNote);
				}
				return projectJson;
			}
			catch (Exception ex)
			{
				return null;
			}
		}

		public static ProjectJson LoadProject(string projectFileName)
		{
			try
			{
				var json = File.ReadAllText(projectFileName);
				var projectJson = MiniJsonHelper.Deserialize<ProjectJson>(json);

				var path = Path.GetDirectoryName(projectFileName);
				var midiFileName = path + "\\" + projectJson.MidiFileName;
				var audioFileName = path + "\\" + projectJson.AudioFileName;
				var pianoFileName = path + "\\" + projectJson.PianoFileName;

				using (Stream inputStream = File.OpenRead(midiFileName))
				{
					projectJson.Midi = MidiSequence.Open(inputStream);
					projectJson.Track = new Track(projectJson.Midi, projectJson.FirstNote);
				}
				projectJson.MainTrack.Load(audioFileName);

				projectJson.MainExist = true;
				projectJson.PianoExist = projectJson.PianoFileName != "";
				if (projectJson.PianoExist)
					projectJson.PianoTrack.Load(pianoFileName);

				return projectJson;
			}
			catch (Exception exc)
			{
				return null;
			}
		}

		public void Deserialize(Dictionary<string, object> dict)
		{
			MiniJsonHelper.Deserialize(dict, ref AudioFileName, AudioFileNameParam, MiniJsonHelper.ConvertString);
			MiniJsonHelper.Deserialize(dict, ref MidiFileName, MidiFileNameParam, MiniJsonHelper.ConvertString);
			MiniJsonHelper.Deserialize(dict, ref PianoFileName, PianoFileNameParam, MiniJsonHelper.ConvertString);
			MiniJsonHelper.Deserialize(dict, ref FirstNote, FirstNoteParam, MiniJsonHelper.ConvertInt);
		}
	}
}
