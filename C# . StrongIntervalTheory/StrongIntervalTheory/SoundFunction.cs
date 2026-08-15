using System;
using NAudio.Wave;

namespace StrongIntervalTheoryLib
{
	public class SoundFunction: IDisposable
	{
		private AudioFileReader AudioFile;
		private WaveOutEvent OutputDevice;

		private string Path;

		public SoundFunction()
		{
		}

		public void Load(string path)
		{
			Path = path;
			if (AudioFile != null)
				AudioFile.Dispose();

			if (OutputDevice != null)
				OutputDevice.Dispose();

			AudioFile = new AudioFileReader(path);
			OutputDevice = new WaveOutEvent();
			OutputDevice.Init(AudioFile);
		}

		public float GetPositionSeconds()
		{
			if (AudioFile != null)
				return (float)AudioFile.CurrentTime.TotalSeconds;

			return 0;
		}

		public void SetPositionSeconds(float posSeconds)
		{
			AudioFile.CurrentTime = TimeSpan.FromSeconds(posSeconds);
		}

		public void SetVolume(float volume)
		{
			AudioFile.Volume = volume;
		}

		public void Play()
		{
			if(OutputDevice != null)
				OutputDevice.Play();
		}

		public void Stop()
		{
			if(Path != null)
				Load(Path);
		}

		public void Pause()
		{
			if (OutputDevice != null)
				OutputDevice.Pause();
		}

		public void Dispose()
		{
			if (AudioFile != null)
				AudioFile.Dispose();

			if (OutputDevice != null)
				OutputDevice.Dispose();
		}
	}
}
