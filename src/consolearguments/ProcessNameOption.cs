using System.Collections.Generic;
using System.Linq;

namespace PhantomDustVolumeSetter.consolearguments {

	public class ProcessNameOption : ConsoleOption {
		public LoadResult loadResult;

		public ProcessNameOption() : base("processName", "select the process by using one or more process names", new ProcessName()) { }

		public override void load(string[] args) {
			if (args.Length <= 0) {
				throw new ConsoleOptionLoadException($"Option requires at least one processName value, but received none, try '--{DisplayName} PDUWP.exe' :)", this);
			}
			loadResult = new LoadResult(args.ToList());
		}

		private class ProcessName : OptionValue {
			public ProcessName() : base(
					"name",
					"the name provided by the 'Image Name'-column https://docs.microsoft.com/en-us/windows-hardware/drivers/debugger/finding-the-process-id",
					false,
					true) { }
		}

		public class LoadResult {
			public readonly List<string> processNames;

			public LoadResult(List<string> processNames) {
				this.processNames = processNames;
			}
		}
	}

}