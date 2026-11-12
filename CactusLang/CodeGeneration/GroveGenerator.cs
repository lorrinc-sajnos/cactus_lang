using CactusLang.Model;

namespace CactusLang.CodeGeneration;

public class GroveGenerator(CactusGrove grove) {
    
    public CactusGrove Grove { get; } = grove;
    
    
    public void GenerateSourceFiles() {
        var mainCodeGenerator = new SourceFileGenerator(this, Grove.RootFile, Grove.RootFile.SourceFile.Path);

        mainCodeGenerator.GenerateEntryFileCode();
        mainCodeGenerator.GenerateFiles();

        var otherFiles = Grove.GetFiles().ToList();
        otherFiles.Remove(Grove.RootFile);
        foreach (var file in otherFiles) {
            var codeGenerator = new SourceFileGenerator(this, file, file.SourceFile.Path);

            codeGenerator.GenerateCode();
            codeGenerator.GenerateFiles();
        }
    }

    public List<string> CFilesGenerated { get; } = new();
    public List<string> HFilesGenerated { get; } = new();
}