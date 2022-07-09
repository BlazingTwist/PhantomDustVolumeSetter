using System.Globalization;

namespace PhantomDustVolumeSetter.consolearguments {

	public class VolumeOption : ConsoleOption {
		public LoadResult? loadResult;

		public VolumeOption() : base("volume", "specify the volume level to assign to the process", new VolumeLevel()) { }

		public override void load(string[] args) {
			if (args.Length <= 0) {
				throw new ConsoleOptionLoadException($"Option requires volumeLevel value to be set, but received no values. Try '--{DisplayName} 25' :)", this);
			}
			if (args.Length > 1) {
				string message = $"Option can accept at most one volumeLevel value, but received multiple ({args.Length}: '{string.Join(" ", args)}'). Try '--{DisplayName} 25' :)";
				throw new ConsoleOptionLoadException(message, this);
			}
			if (!int.TryParse(args[0], NumberStyles.Integer, CultureInfo.InvariantCulture, out int volumeLevel)) {
				throw new ConsoleOptionLoadException($"Could not convert '{args[0]}' to an interger value. Try '--{DisplayName} 25' :)", this);
			}
			loadResult = new LoadResult(volumeLevel);
		}

		private class VolumeLevel : OptionValue {
			public VolumeLevel() : base("volumeLevel", "specify the volume level to assign to the process", false, false) { }
		}

		public class LoadResult {
			public readonly int volumeLevel;

			public LoadResult(int volumeLevel) {
				this.volumeLevel = volumeLevel;
			}
		}
	}

}