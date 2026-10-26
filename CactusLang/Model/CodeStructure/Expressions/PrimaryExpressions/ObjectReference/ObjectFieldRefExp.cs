using CactusLang.Model.Operators.ObjectRefference;
using CactusLang.Model.Types;

namespace CactusLang.Model.CodeStructure.Expressions.PrimaryExpressions.ObjectReference;

public class ObjectFieldRefExp(PrimaryExpression objExpression, ObjectFieldRef fieldRef) : PrimaryExpression {
    public PrimaryExpression Object { get; } = objExpression;
    public ObjectFieldRef FieldRef { get; } = fieldRef;

    public override bool IsLValue() => true;

    public override BaseType GetResultType() => FieldRef.Field!.Type;
}