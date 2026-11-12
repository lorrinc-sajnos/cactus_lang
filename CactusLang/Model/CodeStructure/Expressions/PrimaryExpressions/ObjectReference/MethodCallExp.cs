using CactusLang.Model.Operators.ObjectRefference;
using CactusLang.Model.Types;

namespace CactusLang.Model.CodeStructure.Expressions.PrimaryExpressions.ObjectReference;

public class MethodCallExp : PrimaryExpression {
    public PrimaryExpression Object { get; }

    public ObjectFuncCall FuncCall { get; }

    List<Expression> _parameters;
    public List<Expression> GetParameters() => _parameters;

    public override bool IsLValue() => true;
    public MethodCallExp(PrimaryExpression objExpression, List<Expression> parameters, ObjectFuncCall funcCall) {
        FuncCall = funcCall;
        Object = objExpression;
        _parameters = parameters;
    }

    public override BaseType GetResultType() => FuncCall.ReturnType;
}