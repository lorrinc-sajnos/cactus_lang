using CactusLang.Model.CodeStructure;
using CactusLang.Model.CodeStructure.Structs;
using CactusLang.Model.Types;

namespace CactusLang.Model.Symbols;

public class StructFieldRefSymbol : VariableSymbol {
    public FileStruct Parent { get; }

    public StructFieldRefSymbol(FileStruct parent, VariableSymbol symbol) : base(symbol) {
        Parent = parent;
    }
}