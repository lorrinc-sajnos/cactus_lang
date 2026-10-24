using CactusLang.Model.Symbols;
using CactusLang.Model.Types;

namespace CactusLang.Model.CodeStructure.Expressions.PrimaryExpressions;

public class LocalVarRef(StructFieldRefSymbol variable) : PrimaryExpression {
    public StructFieldRefSymbol Variable { get; } = variable;

    public string  VarName => Variable.Name;

    public override bool IsLValue() => true;

    public override BaseType GetResultType() => Variable.Type;

    public override string ToString() => Variable.Name;
}