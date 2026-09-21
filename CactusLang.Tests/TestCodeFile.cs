using CactusLang.Model.Codefiles;

namespace CactusLang.Tests;

public class TestCodeFile : CodeSourceFile {
    public TestCodeFile(string path) : base(path) { }

    public TestCodeFile(string path, string code) : base(path, code) { }
        
}