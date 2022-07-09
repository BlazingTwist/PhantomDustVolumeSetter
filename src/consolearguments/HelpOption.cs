namespace PhantomDustVolumeSetter.consolearguments {

	public class HelpOption : ConsoleOption {
		public bool wasLoaded;

		public HelpOption() : base("help", "prints information on all Options (i.e. this very output)") { }

		public override void load(string[] args) {
			wasLoaded = true;
		}
	}

}