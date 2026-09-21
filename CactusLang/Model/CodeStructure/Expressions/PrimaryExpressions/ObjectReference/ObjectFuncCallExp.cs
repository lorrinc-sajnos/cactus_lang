using CactusLang.Model.Operators.ObjectRefference;
using CactusLang.Model.Types;

namespace CactusLang.Model.CodeStructure.Expressions.PrimaryExpressions.ObjectReference;

public class ObjectFuncCallExp : PrimaryExpression {
    public PrimaryExpression Object { get; }

    private ObjectFuncCall _funcCall;
    public ObjectFuncCall FuncCall => _funcCall;
    
    List<Expression> _parameters;
    public List<Expression> GetParameters() => _parameters;

    public override bool IsLValue() => true;
    public ObjectFuncCallExp(PrimaryExpression objExpression, List<Expression> parameters, ObjectFuncCall funcCall) {
        _funcCall = funcCall;
        Object = objExpression;
        _parameters = parameters;
    }

    public override BaseType GetResultType() => _funcCall.ReturnType;
}