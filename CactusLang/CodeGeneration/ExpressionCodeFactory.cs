using CactusLang.Model;
using CactusLang.Model.CodeStructure.Expressions;
using CactusLang.Model.CodeStructure.Expressions.PrimaryExpressions;
using CactusLang.Model.CodeStructure.Expressions.PrimaryExpressions.LiteralExpressions;
using CactusLang.Model.CodeStructure.Expressions.PrimaryExpressions.ObjectReference;
using CactusLang.Model.CodeStructure.MemberHolders;
using CactusLang.Model.CodeStructure.MemberHolders.Structs;
using CactusLang.Model.Operators.ObjectRefference;
using CactusLang.Model.Operators.Unary;
using CactusLang.Model.Visitors;

namespace CactusLang.CodeGeneration;

public class ExpressionCodeFactory : CodeModelVisitor<string> {
	private readonly Expression _expression;
	private readonly SourceFileGenerator.State _modelState;
	private readonly NameResolver _resolver;

	public static string Manufacture(NameResolver resolver, Expression expression, SourceFileGenerator.State state = new()) {
		ExpressionCodeFactory factory = new(resolver, expression, state);
		return factory.Manufacture();
	}

	public ExpressionCodeFactory(NameResolver resolver, Expression expression, SourceFileGenerator.State state) : base("", null!) {
		_modelState = state;
		_expression = expression;
		_resolver = resolver;
	}

	public string Manufacture() {
		string str = VisitExpression(_expression);
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
		string call = _resolver.GetFunc( funcCall.Function ) + '(';

		var paramExps = funcCall.GetParameters();
		if (paramExps.Any()) {
			for (int i = 0; i < paramExps.Count() - 1; i++)
				call += VisitExpression(paramExps[i]) + ", ";

			call += VisitExpression(paramExps[^1]);
		}

		return call + ')';
	}

	protected override string VisitLocalVarRefExpression(LocalVarRef structFieldRef) =>
		$"this->{  _resolver.GetLocalVar( structFieldRef)}";

	protected override string VisitLiteralExpression(LiteralExpression literalExpression) {
		return literalExpression.Value;
	}

	protected override string VisitObjectFieldRefExp(ObjectFieldRefExp objectFieldRefExp) {
		switch (objectFieldRefExp.Object) {
			case IncludedFileRef fileRef:
				return _resolver.GetMemberField(objectFieldRefExp.FieldRef.Field);
			default:
				string objPtr = VisitExpression(objectFieldRefExp.Object);
				string fieldName = _resolver.GetField(objectFieldRefExp.FieldRef.Field);
				
				if (objectFieldRefExp.FieldRef.AccessType== AccessType.REFERENCE)
					return $"{objPtr}->{fieldName}";
				return $"{objPtr}.{fieldName}";
		}
		
	}

	protected override string VisitObjectFuncCallExp(MethodCallExp methodCallExp) {
		string funcName;
		List<string> funcParams = [];

		switch (methodCallExp.FuncCall.Function) {
			case StructFunction structFunc: {
				funcName = _resolver.GetStructRefFunc(structFunc);
				
				//If 
				string objPtr = VisitExpression(methodCallExp.Object);
				funcParams.Add(objPtr);
				break;	
			}
			//If methodCall just needs a prefix
			default:
				var fn = methodCallExp.FuncCall.Function;
				funcName = _resolver.GetMemberFunction(fn);
				break;
		}

		foreach (var param in methodCallExp.GetParameters()) {
			var factory = new ExpressionCodeFactory(_resolver,param, _modelState);
			funcParams.Add( factory.Manufacture());
		}
		string joinedParams = string.Join(NameResolver.PARAM_SEP, funcParams);

		return $"{funcName}({joinedParams})";
	}

	protected override string VisitAlloc(Alloc alloc) => $"malloc(sizeof({_resolver.GetBaseType( alloc.AllocType)}))";

	protected override string VisitUnaryOperator(UnaryOp op) => op.GetId();
	

    
    
	#endregion
}