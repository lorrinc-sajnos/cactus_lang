using Antlr4.Runtime;
using CactusLang.Semantics.Errors;

namespace CactusLang.Model.Errors;

public class CactusError {
    public int Code { get; private set; }
    public string Title { get; private set; }
    public string Msg { get; private set; }
    public Exception Exception { get; private set; }

    private readonly List<CactusError> _errors = [];

    private CactusError(int code, string title, string msg) {
        Code = code;
        Title = title;
        Msg = msg;
        Exception = new Exception(msg);

        if (_errors.Any(e => e.Code == code)) {
            var err = _errors.First(e => e.Code == code);
            throw new Exception($"Two errors with the code {code}:"
                                + $"\n{Title}:\n{Msg}"
                                + $"\n{err.Title}:\n{err.Msg}"
            );
        }

        _errors.Add(this);
    }

    public ErrorInstance CompTime(ErrorHandler errorHandler, ParserRuleContext ctx, params object[] msgParams) =>
        new(this, errorHandler, ctx.Start, ctx.Stop, msgParams);

    public ErrorInstance CompTime(ErrorHandler errorHandler, IToken token, params object[] msgParams) =>
        new(this, errorHandler, token, token, msgParams);

    public ErrorInstance CompTime(ErrorHandler errorHandler, IToken start, IToken end, params object[] msgParams) =>
        new(this, errorHandler, start, end, msgParams);

    public static readonly CactusError TODO_ERROR = new(0, "Todo Error", "Todo Error ");
    public static readonly CactusError ID_NOT_FOUND = new(1, "ID \"{0}\" Not Found", "Could not find ID \"{0}\".");

    public static readonly CactusError ALREADY_DEFINED =
        new(2, "ID \"{0}\" already defined", "\"{0}\" is already defined");

    public static readonly CactusError OP_MISMATCH = new(3, "Operator type mismatch", "No operator exists.");

    public static readonly CactusError LITERAL_ERROR =
        new(4, "Malformed literal #{0}#", "The value of \"{0}\" could not be parsed.");

    public static readonly CactusError TYPE_NOT_FOUND = new(5, "Type {0} Not Found", "Could not find type {0}.");

    public static readonly CactusError OVERLOAD_DOESNT_MATCH_RET_TYPE = new(6,
        "Overload of {0} doesn't match return type \"{1}\"",
        "Overload of function \"{0}\" return type has to be \"{1}\"");

    public static readonly CactusError BAD_OBJ_REF =
        new(7, "Invalid reference: {0}", "Object reference {0} is not valid.");

    public static readonly CactusError TYPE_MISMATCH =
        new(8, "Cannot assign type {0} =/> {1}", "Cannot assign type {0} to {1}.");

    public static readonly CactusError RVALUE_ASG = new(9, "Cannot assign to rvalue #{0}#",
        "Cannot assign value into #{0}#, as it is an rvalue, thus not accessible.");

    public static readonly CactusError FREE_ERROR =
        new(9, "Cannot free {0}", "Cannot free {0} as it is not a pointer.");

    public static readonly CactusError CONDITION_ERROR =
        new(10, "Condition type is {0}, not #bool#", "Condition type should be #bool#, instead of {0}");
    
    public static readonly CactusError FILE_ERROR =
        new (101, """Error when trying to include "{0}". """, """A error occured, while trying to include file "{0}" """);
    public static readonly CactusError FILE_DOENST_EXISTS_ON_DISK =
        new (102, """File "{0}" does not exist""", """File "{0}" does not exist.""");
    public static readonly CactusError FILE_DOENST_EXISTS =
        new (103, """File "{0}" is not declared""", """File "{0}" is not included in this file.""");
    public static readonly CactusError GROVE_DOENST_EXISTS =
        new (103, """Grove "{0}" is not declared""", """Grove "{0}" is not included in this file.""");
    
    
    public static readonly CactusError EXPRESSION_ERROR =
        new(201, "Expression error", "Error when evaluating the following expression: \"{0}\"");
    public static readonly CactusError SYNTAX_WRONG_TOKEN =
        new(9001, "Syntax error: Unexpected token \"{0}\"", "Token \"{0}\" was unexpected.");
    
    
    
}