using CactusLang.Model;
using CactusLang.Model.Symbols;

namespace CactusLang.Compiling.Codefiles;

public class SourceFile(string path) {
    public string Path { get; protected set; } = path;
    public string? CodeText { get; protected set; } = null;

    public virtual void ReadAllCode() {
        if (CodeText != null) return;
        CodeText = File.ReadAllText(Path);
    }
    public void FreeCode() {
        CodeText = null;
    }
}