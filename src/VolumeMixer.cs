using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Runtime.InteropServices;
using PhantomDustVolumeSetter.soundcoreapi;

namespace PhantomDustVolumeSetter {

	// shamelessly stolen from https://stackoverflow.com/questions/62840338/windows-core-audio-api-changing-app-volume-wrong-audio-interface-selected-for
	public class VolumeMixer {
		private readonly List<ISimpleAudioVolume> volumes = new();

		[SuppressMessage("ReSharper", "SuspiciousTypeConversion.Global", Justification = "Accessing undocumented APIs do be like that.")]
		public VolumeMixer(int processId) {
			IMMDeviceEnumerator deviceEnumerator = (IMMDeviceEnumerator)new MMDeviceEnumerator();
			deviceEnumerator.GetDefaultAudioEndpoint(EDataFlow.eRender, ERole.eMultimedia, out IMMDevice outputDevice);

			Guid audioSessionManagerGuid = typeof(IAudioSessionManager2).GUID;
			outputDevice.Activate(ref audioSessionManagerGuid, 0, IntPtr.Zero, out object sessionManagerObject);
			IAudioSessionManager2 sessionManager = (IAudioSessionManager2)sessionManagerObject;

			sessionManager.GetSessionEnumerator(out IAudioSessionEnumerator sessionEnumerator);
			sessionEnumerator.GetCount(out int sessionCount);

			for (int sessionIndex = 0; sessionIndex < sessionCount; sessionIndex++) {
				sessionEnumerator.GetSession(sessionIndex, out IAudioSessionControl2 sessionControl);
				sessionControl.GetProcessId(out int sessionProcessId);

				if (sessionProcessId == processId && sessionControl is ISimpleAudioVolume simpleVolume) {
					volumes.Add(simpleVolume);
				} else {
					ReleaseComObject(sessionControl);
				}
			}

			ReleaseComObject(sessionEnumerator);
			ReleaseComObject(sessionManagerObject);
			ReleaseComObject(outputDevice);
			ReleaseComObject(deviceEnumerator);
		}

		/// <summary>
		/// Sets the volume level for the volume-controls registered with this VolumeMixer
		/// </summary>
		/// <param name="level">volume level in range [0, 100]</param>
		public void SetVolume(float level) {
			foreach (ISimpleAudioVolume volume in volumes) {
				Guid guid = Guid.Empty;
				volume.SetMasterVolume(level / 100, ref guid);
			}
		}

		/// <summary>
		/// Gets the volume levels of the volume-controls registered with this VolumeMixer
		/// </summary>
		/// <returns>list of volume levels in range [0, 100]</returns>
		public List<float> GetVolume() {
			return volumes
					.Select(volume => {
						volume.GetMasterVolume(out float level);
						return level * 100;
					}).ToList();
		}

		~VolumeMixer() {
			foreach (ISimpleAudioVolume volume in volumes) {
				ReleaseComObject(volume);
			}
		}

		private static void ReleaseComObject(object objectToRelease) {
			if (OperatingSystem.IsWindows()) {
				Marshal.ReleaseComObject(objectToRelease);
			}
		}
	}

}