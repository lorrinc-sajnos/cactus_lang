using CactusLang.Model.Operators.ObjectRefference;
using CactusLang.Model.Types;

namespace CactusLang.Model.CodeStructure.Expressions.PrimaryExpressions.ObjectReference;

public class ObjectFieldRefExp : PrimaryExpression {
    public PrimaryExpression Object { get; }

    private ObjectFieldRef _fieldRef;
    public ObjectFieldRef FieldRef => _fieldRef;

    public override bool IsLValue() => true;
    public ObjectFieldRefExp(PrimaryExpression objExpression, ObjectFieldRef fieldRef) {
        _fieldRef = fieldRef;
        Object = objExpression;
    }

    public override BaseType GetResultType() => _fieldRef.Field!.Type;
}