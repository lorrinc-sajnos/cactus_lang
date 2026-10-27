using Antlr4.Runtime;
using CactusLang.Compiling.Codefiles;
using CactusLang.Model;
using CactusLang.Model.CodeStructure.Expressions;
using CactusLang.Model.Errors;
using CactusLang.Model.Symbols;
using CactusLang.Model.Types;

namespace CactusLang.Semantics.Errors;

public class ErrorHandler {
    private const bool THROW_ON_ERROR = false;

    public class ParserErrorListener : BaseErrorListener {
        private ErrorHandler _errorHandler;

        public ParserErrorListener(ErrorHandler errorHandler) {
            _errorHandler = errorHandler;
        }

        public override void SyntaxError(TextWriter output, IRecognizer recognizer, IToken offendingSymbol, int line,
            int charPositionInLine,
            string msg, RecognitionException e) {
            base.SyntaxError(output, recognizer, offendingSymbol, line, charPositionInLine, msg, e);

            _errorHandler.AddCompError(
                CctsError.SYNTAX_WRONG_TOKEN.CompTime(_errorHandler, offendingSymbol, offendingSymbol.Text));
        }
    }

    public ParserErrorListener ErrorListener { get; private set; }
    public FileID FileID { get; private set; }

    private readonly List<ErrorInstance> _errors;

    public ErrorHandler(CactusFile file) {
        _errors = new List<ErrorInstance>();
        ErrorListener = new(this);
        //_throwOnError =  throwOnError;
        FileID = file.ID;
    }

    private void AddCompError(ErrorInstance errorInstance) {
        
        _errors.Add(errorInstance);
        if (THROW_ON_ERROR)
            throw new Exception($"Error added:\n{errorInstance.AsPrettyString()}");
    }

    
    public void Error(CctsError error, IToken token, params object[] msgParams) =>
        ErrorInExpressionFromCompTime(error.CompTime(this, token, msgParams));
    public void Error(CctsError error, IToken start, IToken end, params object[] msgParams) =>
        ErrorInExpressionFromCompTime(error.CompTime(this, start, end, msgParams));
    public void Error(CctsError error, ParserRuleContext ctx, params object[] msgParams) =>
        ErrorInExpressionFromCompTime(error.CompTime(this, ctx, msgParams));
    
    
    
    #region Expression
    public Expression.Error ErrorInExpression(CctsError error, IToken token, params object[] msgParams) =>
        ErrorInExpressionFromCompTime(error.CompTime(this, token, msgParams));
    public Expression.Error ErrorInExpression(CctsError error, IToken start, IToken end, params object[] msgParams) =>
        ErrorInExpressionFromCompTime(error.CompTime(this, start, end, msgParams));
    public Expression.Error ErrorInExpression(CctsError error, ParserRuleContext ctx, params object[] msgParams) =>
        ErrorInExpressionFromCompTime(error.CompTime(this, ctx, msgParams));
    private Expression.Error ErrorInExpressionFromCompTime(ErrorInstance errorInstance) {
        AddCompError(errorInstance);
        return new Expression.Error();
    }
    #endregion
    
    #region Type
    public ErrorType ErrorInType(CctsError error, IToken token, params object[] msgParams) =>
        ErrorInTypeFromCompTime(error.CompTime(this, token, msgParams));
    public ErrorType ErrorInType(CctsError error, IToken start, IToken end, params object[] msgParams) =>
        ErrorInTypeFromCompTime(error.CompTime(this, start, end, msgParams));
    public ErrorType ErrorInType(CctsError error, ParserRuleContext ctx, params object[] msgParams) =>
        ErrorInTypeFromCompTime(error.CompTime(this, ctx, msgParams));
    private ErrorType ErrorInTypeFromCompTime(ErrorInstance errorInstance) {
        AddCompError(errorInstance);
        return ErrorType.ERROR;
    }

    public List<ErrorInstance> GetErrors() => _errors;
    
    public bool HasErrors() => _errors.Count > 0;
    #endregion

    public void PrintErrors() {
        if (_errors.Count == 0) {
            Console.WriteLine("-\tNo errors found :)");
            return;
        }

        foreach (var error in _errors) {
            Console.WriteLine(error.AsPrettyString());
        }
    }
}