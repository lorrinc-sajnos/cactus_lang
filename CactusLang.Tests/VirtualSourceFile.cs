using CactusLang.Compiling.Codefiles;

namespace CactusLang.Tests;

public class VirtualSourceFile(string path, string codeText) : SourceFile("__VIRTUAL/"+path) {
    public override void ReadAllCode() {
        if (CodeText != null) return;
        CodeText = codeText;
    }
    public new void FreeCode() { }
}