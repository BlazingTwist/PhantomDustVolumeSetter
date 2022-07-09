using System.Collections.Generic;
using System.Linq;

namespace PhantomDustVolumeSetter.consolearguments {

	public abstract class ConsoleOption {
		public string DisplayName { get; }
		public string InfoText { get; }
		public List<OptionValue> Values { get; } = new();

		protected ConsoleOption(string displayName, string infoText, params OptionValue[] values) {
			DisplayName = displayName;
			InfoText = infoText;
			Values.AddRange(values);
		}

		public abstract void load(string[] args);

		public IEnumerable<string> HelpText(string indentString) {
			yield return indentString + DisplayName + " " + string.Join(" ", Values.Select(value => value.UsageHelpText()));

			string doubleIndent = indentString + indentString;
			yield return doubleIndent + "info: " + InfoText;
			if (Values.Count <= 0) {
				yield return doubleIndent + "Values: -none";
			} else {
				yield return doubleIndent + "Values:";

				string tripleIndent = doubleIndent + indentString;
				foreach (OptionValue optionValue in Values) {
					yield return tripleIndent + optionValue.DetailHelpText();
				}
			}
		}
	}

}