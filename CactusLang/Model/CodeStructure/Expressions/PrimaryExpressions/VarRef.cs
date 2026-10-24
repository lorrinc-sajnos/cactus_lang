using CactusLang.Model.Symbols;
using CactusLang.Model.Types;

namespace CactusLang.Model.CodeStructure.Expressions.PrimaryExpressions;

public class VarRef :  PrimaryExpression {
    public VariableSymbol Variable { get; }
    public string  VarName => Variable.Name;

    public VarRef(VariableSymbol variable) {
        Variable = variable;
    }
    public override bool IsLValue() => true;

    public override BaseType GetResultType() => Variable.Type;

    public override string ToString() => $"{Variable.Name}<{Variable.Type}>";
}