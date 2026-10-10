using CactusLang.Model;

namespace CactusLang.Compiling.Codefiles;

public class SourceFile {
    public string Path { get; private set; }
    public string? CodeText { get; private set; }

    //public CactusFile File ;
    
    protected SourceFile(string path, string codeText) {
        Path = path;
        CodeText = codeText;
    }
    public SourceFile(string path) {
        Path = path;
        CodeText = null;
    }
    
    public void ReadAllCode() {
        if (CodeText != null) return;
        CodeText = File.ReadAllText(Path);
    }
    public void FreeCode() {
        CodeText = null;
    }
}