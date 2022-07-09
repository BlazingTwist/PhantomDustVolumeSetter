using System.Collections.Generic;
using System.Globalization;

namespace PhantomDustVolumeSetter.consolearguments {

	public class ProcessIdOption : ConsoleOption {
		public LoadResult loadResult;

		public ProcessIdOption() : base("processID", "select the process by using one or more processIDs (pid)", new ProcessId()) { }

		public override void load(string[] args) {
			if (args.Length <= 0) {
				throw new ConsoleOptionLoadException("Option requires at least one pid value, but received none", this);
			}
			List<int> processIds = new();
			for (int index = 0; index < args.Length; index++) {
				string pidString = args[index];
				if (!int.TryParse(pidString, NumberStyles.Integer, CultureInfo.InvariantCulture, out int pid)) {
					throw new ConsoleOptionLoadException($"Could not convert pid at index '{index}' ('{pidString}') to an interger value.", this);
				}
				processIds.Add(pid);
			}
			loadResult = new LoadResult(processIds);
		}

		private class ProcessId : OptionValue {
			public ProcessId() : base(
					"pid",
					"the pid provided by the 'PID'-column https://docs.microsoft.com/en-us/windows-hardware/drivers/debugger/finding-the-process-id",
					false,
					true) { }
		}

		public class LoadResult {
			public readonly List<int> processIds;

			public LoadResult(List<int> processIds) {
				this.processIds = processIds;
			}
		}
	}

}