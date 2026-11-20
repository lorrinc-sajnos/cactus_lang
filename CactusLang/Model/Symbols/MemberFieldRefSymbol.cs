using CactusLang.Model.CodeStructure;
using CactusLang.Model.CodeStructure.Entities;
using CactusLang.Model.Types;

namespace CactusLang.Model.Symbols;

public class MemberFieldRefSymbol(IEntity parent, VariableSymbol symbol) : VariableSymbol(symbol) {
    public IEntity Parent { get; } = parent;
}