using System.Runtime.InteropServices;

namespace PhantomDustVolumeSetter.soundcoreapi {

	/// <summary>
	/// The long awaited sequel to IAudioSessionManager1
	/// </summary>
	[Guid("77AA99A0-1BD6-484F-8BC7-2C654C9A9B6F"), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	internal interface IAudioSessionManager2 {
		int NotImpl1();
		int NotImpl2();

		[PreserveSig]
		int GetSessionEnumerator(out IAudioSessionEnumerator SessionEnum);
	}

}