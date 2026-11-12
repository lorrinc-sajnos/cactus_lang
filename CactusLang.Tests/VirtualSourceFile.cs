using System;
using CactusLang.Compiling.Codefiles;

namespace CactusLang.Tests;

public class VirtualSourceFile : SourceFile {
    private readonly string _codeText;

    public VirtualSourceFile(string path, string codeText) : base("__VIRTUAL/"+path) {
        _codeText = codeText;

        string header = $"---virtual file: {path}---";
        string bottom = "----------------------------------------------------------------------------"[..header.Length];
        
        Console.WriteLine(header);
        
        Console.WriteLine(codeText);
        
        Console.WriteLine(bottom);
    }

    public override void ReadAllCode() {
        if (CodeText != null) return;
        CodeText = _codeText;
    }
    public new void FreeCode() { }
}