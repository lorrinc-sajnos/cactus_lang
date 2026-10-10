using CactusLang.Compiling.Codefiles;

namespace CactusLang.Tests;

public class TestCactusFile : SourceFile {
    public TestCactusFile(string path) : base(path) { }

    public TestCactusFile(string path, string codeText) : base(path, codeText) { }
        
}