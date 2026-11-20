using CactusLang.Model;
using CactusLang.Model.CodeStructure.Expressions;
using CactusLang.Model.CodeStructure.Expressions.PrimaryExpressions;
using CactusLang.Model.CodeStructure.Expressions.PrimaryExpressions.LiteralExpressions;
using CactusLang.Model.CodeStructure.Expressions.PrimaryExpressions.ObjectReference;
using CactusLang.Model.CodeStructure.Entities.Structs;
using CactusLang.Model.Operators.ObjectRefference;
using CactusLang.Model.Operators.Unary;
using CactusLang.Model.Visitors;

namespace CactusLang.CodeGeneration;

public class ExpressionCodeFactory(NameResolver resolver, Expression expression, CactusFileGenerator.State state)
    : CodeModelVisitor<string>("", null!) {
    public static string Manufacture(NameResolver resolver, Expression expression,
        CactusFileGenerator.State state = new()) {
        ExpressionCodeFactory factory = new(resolver, expression, state);
        return factory.Manufacture();
    }

    public string Manufacture() {
        string str = VisitExpression(expression);
        return str;
    }

    protected override string AggregateResults(params string[] statuses) {
        string result = "";
        foreach (var str in statuses) {
            result += str ?? throw new Exception("Bad implementation in Visitor!!!!");
        }

        return result;
    }

    protected override string VisitOperationExpression(OperationExpression operationExpression) {
        var lhExp = operationExpression.LeftExpression;
        var rhExp = operationExpression.RightExpression;

        var op = operationExpression.Operator;

        string lhStr = VisitExpression(lhExp);
        string rhStr = VisitExpression(rhExp);

        return $"{lhStr} {op.GetId()} {rhStr}";
    }


    #region Primary expression

    protected override string VisitVarRefExpression(VarRef varRef) => varRef.VarName;


    protected override string VisitFuncCallExpression(FuncCallExp funcCall) {
        string call = resolver.GetFunc(funcCall.Function) + '(';

        var paramExps = funcCall.GetParameters();
        if (paramExps.Any()) {
            for (int i = 0; i < paramExps.Count() - 1; i++)
                call += VisitExpression(paramExps[i]) + ", ";

            call += VisitExpression(paramExps[^1]);
        }

        return call + ')';
    }

    protected override string VisitLocalVarRefExpression(LocalVarRef structFieldRef) =>
        $"this->{resolver.GetLocalVar(structFieldRef)}";

    protected override string VisitLiteralExpression(LiteralExpression literalExpression) {
        return literalExpression.Value;
    }

    protected override string VisitObjectFieldRefExp(ObjectFieldRefExp objectFieldRefExp) {
        switch (objectFieldRefExp.Object) {
            case IncludedFileRef fileRef:
                return resolver.GetMemberField(objectFieldRefExp.FieldRef.Variable);
            default:
                string objPtr = VisitExpression(objectFieldRefExp.Object);
                string fieldName = resolver.GetField(objectFieldRefExp.FieldRef.Variable);

                if (objectFieldRefExp.FieldRef.AccessType == AccessType.REFERENCE)
                    return $"{objPtr}->{fieldName}";
                return $"{objPtr}.{fieldName}";
        }
    }

    protected override string VisitObjectFuncCallExp(MethodCallExp methodCallExp) {
        string funcName;
        List<string> funcParams = [];

        switch (methodCallExp.FuncCall.Function) {
            case StructMethod structFunc: {
                funcName = resolver.GetStructRefFunc(structFunc);

                //If 
                string objPtr = VisitExpression(methodCallExp.Object);
                funcParams.Add(objPtr);
                break;
            }
            //If methodCall just needs a prefix
            default:
                var fn = methodCallExp.FuncCall.Function;
                funcName = resolver.GetMemberFunction(fn);
                break;
        }

        foreach (var param in methodCallExp.GetParameters()) {
            var factory = new ExpressionCodeFactory(resolver, param, state);
            funcParams.Add(factory.Manufacture());
        }

        string joinedParams = string.Join(NameResolver.PARAM_SEP, funcParams);

        return $"{funcName}({joinedParams})";
    }

    protected override string VisitAlloc(Alloc alloc) => $"malloc(sizeof({resolver.GetBaseType(alloc.AllocType)}))";

    protected override string VisitUnaryOperator(UnaryOp op) => op.GetId();

    #endregion
}