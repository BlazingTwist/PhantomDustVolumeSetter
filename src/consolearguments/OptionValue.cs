namespace PhantomDustVolumeSetter.consolearguments {

	public abstract class OptionValue {
		public string DisplayName { get; }

		public string InfoText { get; }

		/// <summary>
		/// Optional values are allowed to occur 0 times.
		/// Can be combined with IsVarargs.
		/// </summary>
		public bool IsOptional { get; }

		/// <summary>
		/// Varargs values are allowed to occur more than 1 time.
		/// Can be combined with IsOptional.
		/// </summary>
		public bool IsVarags { get; }

		protected OptionValue(string displayName, string infoText, bool isOptional, bool isVarags) {
			DisplayName = displayName;
			InfoText = infoText;
			IsOptional = isOptional;
			IsVarags = isVarags;
		}

		public string UsageHelpText() {
			string[] modifiers = { "", "?", "+", "*" };
			int modifierIndex = (IsOptional ? 1 : 0) + (IsVarags ? 2 : 0);
			return "[" + DisplayName + "]" + modifiers[modifierIndex];
		}

		public string DetailHelpText() {
			return DisplayName + ": " + InfoText;
		}
	}

}