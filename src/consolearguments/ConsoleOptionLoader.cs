using System;
using System.Collections.Generic;
using System.Linq;

namespace PhantomDustVolumeSetter.consolearguments {

	public static class ConsoleOptionLoader {
		public static bool LoadOptions(string[] args, params ConsoleOption[] options) {
			try {
				string? currentOption = null;
				List<string>? optionValues = null;

				bool CheckLoadOption() {
					if (optionValues != null) {
						List<ConsoleOption> targetOptions = options
								.Where(option => string.Equals(option.DisplayName, currentOption, StringComparison.OrdinalIgnoreCase))
								.ToList();
						if (targetOptions.Count <= 0) {
							string optionsString = $"['{string.Join("', '", options.Select(option => option.DisplayName))}']";
							Program.PrintGenericError("There was an Error while loading the options.", new Dictionary<string, string> {
									{ "info", $"Unknown option '{currentOption}'. Try one of these: {optionsString} :)" }
							});
							Program.PrintHelp();
							return false;
						}
						string[] optionValuesArray = optionValues.ToArray();
						foreach (ConsoleOption targetOption in targetOptions) {
							targetOption.load(optionValuesArray);
						}
					}
					return true;
				}

				foreach (string arg in args) {
					if (arg.StartsWith("--")) {
						if (!CheckLoadOption()) {
							return false;
						}
						currentOption = arg["..".Length..];
						optionValues = new List<string>();
					} else {
						if (optionValues == null) {
							Program.PrintGenericError("There was an Error while loading the options.", new Dictionary<string, string> {
									{ "info", $"Options must be prefixed with '--'. Try '--{args[0]}' :)" }
							});
							Program.PrintHelp();
							return false;
						}
						optionValues.Add(arg);
					}
				}

				if (!CheckLoadOption()) {
					return false;
				}
			} catch (ConsoleOptionLoadException e) {
				Program.PrintOptionLoadError(e);
				return false;
			}

			return true;
		}
	}

}