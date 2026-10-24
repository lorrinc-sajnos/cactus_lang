using CactusLang.Model.Symbols;

namespace CactusLang.Compiling.Codefiles;

public struct CodePos(FileID fileID, int line, int column) {
    public FileID FileID { get; private set; } = fileID;
    public int Line { get; private set; } = line;
    public int Column { get; private set; } = column;
    public override string ToString() => $"{FileID}|{Line}:{Column}";
}