using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using PhantomDustVolumeSetter.consolearguments;

namespace PhantomDustVolumeSetter {

	public static class Program {
		private static readonly List<ConsoleOption> consoleOptions = new() {
				new HelpOption(), new VolumeOption(), new ProcessIdOption(), new ProcessNameOption()
		};

		public static void Main(string[] args) {
			if (args.Length <= 0) {
				PrintHelp();
				return;
			}

			HelpOption helpOption = new();
			VolumeOption volumeOption = new();
			ProcessIdOption processIdOption = new();
			ProcessNameOption processNameOption = new();
			if (!ConsoleOptionLoader.LoadOptions(args, helpOption, volumeOption, processIdOption, processNameOption)) {
				return;
			}

			if (volumeOption.loadResult != null) {
				List<Process> targetProcesses = new();
				if (processIdOption.loadResult == null && processNameOption.loadResult == null) {
					targetProcesses.AddRange(Process.GetProcessesByName("PDUWP"));
					targetProcesses.AddRange(Process.GetProcessesByName("PDUWP.exe"));
				} else {
					if (processIdOption.loadResult != null) {
						targetProcesses.AddRange(processIdOption.loadResult.processIds.Select(Process.GetProcessById));
					}
					if (processNameOption.loadResult != null) {
						foreach (string processName in processNameOption.loadResult.processNames) {
							string processWithoutExtension = Regex.Replace(processName, "^(.+)\\.(.+)", "$1");
							targetProcesses.AddRange(Process.GetProcessesByName(processName));
							targetProcesses.AddRange(Process.GetProcessesByName(processWithoutExtension));
						}
					}
				}

				foreach (Process process in targetProcesses) {
					VolumeMixer volumeMixer = new(process.Id);
					string oldVolumeString = $"[{string.Join(", ", volumeMixer.GetVolume().Select(volume => (int)volume))}]";
					string newVolumeString = $"{volumeOption.loadResult.volumeLevel}";
					volumeMixer.SetVolume(volumeOption.loadResult.volumeLevel);
					Console.WriteLine($"Setting volume level for process with name: '{process.ProcessName}' and pid: '{process.Id}'. Volume was: {oldVolumeString}. Volume now is: {newVolumeString}");
				}

				if (targetProcesses.Count <= 0) {
					Console.WriteLine("WARNING: did not find any running processes, no volume level was set.");
				}
			}

			if (helpOption.wasLoaded) {
				PrintHelp();
			}
		}

		public static void PrintHelp() {
			StringBuilder helpBuilder = new();
			helpBuilder
					.AppendLine("======= Help =======")
					.AppendLine("usage: 'PhantomDustVolumeSetter.exe [ --[Option] [Values]* ]*'")
					.AppendLine("  for example: 'PhantomDustVolumeSetter.exe --noValueOption --oneValueOption val1'")
					.AppendLine("  for example: 'PhantomDustVolumeSetter.exe --twoValueOption val1 val2'")
					.AppendLine("Options:")
					.AppendLine(string.Join("\n", consoleOptions.Select(consoleOption => string.Join("\n", consoleOption.HelpText("  ")))))
					.AppendLine("====================");
			Console.WriteLine(helpBuilder.ToString());
		}

		public static void PrintGenericError(string message, Dictionary<string, string> details) {
			StringBuilder builder = new();
			builder
					.AppendLine("======= ERROR =======")
					.AppendLine(message)
					.AppendLine(string.Join("\n", details.Select(kvp => $"{kvp.Key}: {kvp.Value}")))
					.AppendLine("=====================");
			Console.WriteLine(builder.ToString());
		}

		public static void PrintOptionLoadError(ConsoleOptionLoadException exception) {
			StringBuilder builder = new();
			builder
					.AppendLine("======= ERROR =======")
					.AppendLine($"There was an error while loading values for the Option '{exception.failingOption.DisplayName}'")
					.AppendLine("info: " + exception.Message)
					.AppendLine("usage:")
					.AppendLine(string.Join("\n", exception.failingOption.HelpText("  ")))
					.AppendLine("=====================");
			Console.WriteLine(builder.ToString());
		}
	}

}