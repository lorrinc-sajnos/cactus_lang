using CactusLang.Model.CodeStructure;
using CactusLang.Model.CodeStructure.MemberHolders;
using CactusLang.Model.CodeStructure.MemberHolders.Structs;
using CactusLang.Model.Types;

namespace CactusLang.Model.Symbols;

public class MemberFieldRefSymbol(IMemberHolder parent, VariableSymbol symbol) : VariableSymbol(symbol) {
    public IMemberHolder Parent { get; } = parent;
}