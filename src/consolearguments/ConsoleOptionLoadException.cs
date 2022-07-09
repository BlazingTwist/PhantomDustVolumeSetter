using System;

namespace PhantomDustVolumeSetter.consolearguments {

	public class ConsoleOptionLoadException : Exception {
		public readonly ConsoleOption failingOption;

		public ConsoleOptionLoadException(string? message, ConsoleOption failingOption) : base(message) {
			this.failingOption = failingOption;
		}
	}

}