using System.Diagnostics;
using CactusLang.Model;

namespace CactusLang.Compiling;

public class GccInstance {
	public string OutputFileName { get; }

	private CactusGrove _grove;

	public GccInstance(CactusGrove grove,  string outputFileName) {
		_grove = grove;
		OutputFileName = outputFileName;
	}

	public const string CCTS_STD_COMP_INCL = "../../../../ccts_lang/cactus_std/ccts_std_comp_incl/";
	//public const string CCTS_STD_COMP_INCL = "/ccts_lang/cactus_std/ccts_std_comp_incl/";
	
	
	public bool HasError { get; private set; }
	public string ErrorStr { get; private set; } = "";
	public int ExitCode { get; private set; }

	public void Compile() {
		//Pipe into GCC
		string gccCmd = "gcc";

		List<string> args = new();
		foreach (string filename in _grove.RootFile.CFilesGenerated)
			args.Add($"\"{filename}\"");
		
		//Include the CCTS STD library
		args.Add($"-I {CCTS_STD_COMP_INCL}");
		
		//SSTD include
		var stdIncludeFiles = GetIncludeFiles();
		foreach (var stdIncludeFile in stdIncludeFiles) {
			args.Add($"\"{stdIncludeFile}\"");
		}
		
		args.Add($"-o \"{OutputFileName}\"");
		

		string gccArgs =  string.Join(" ", args);

		// Create a new process
		Process gccProc = new Process();
		gccProc.StartInfo.FileName = gccCmd;
		gccProc.StartInfo.Arguments = gccArgs;

		//Print cmd to console:
		
		
		
		// Redirect output so we can read it in C#
		gccProc.StartInfo.RedirectStandardOutput = true;
		gccProc.StartInfo.RedirectStandardError = true;
		gccProc.StartInfo.UseShellExecute = false;
		gccProc.StartInfo.CreateNoWindow = true;

		// Start the process
		gccProc.Start();

		gccProc.WaitForExit();
		// Read output streams
		
		ErrorStr = gccProc.StandardError.ReadToEnd();
		if (!ErrorStr.Equals(""))
			HasError = true;

		ExitCode = gccProc.ExitCode;
	}

	private static string[] GetIncludeFiles() {
		var cFiles = Directory.GetFiles(CCTS_STD_COMP_INCL, "*.c");
		var hFiles = Directory.GetFiles(CCTS_STD_COMP_INCL, "*.h");
		
		return [.. cFiles, .. hFiles];
	}
}