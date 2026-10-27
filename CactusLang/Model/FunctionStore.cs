using CactusLang.Model.CodeStructure;
using CactusLang.Model.Symbols;
using CactusLang.Util;

namespace CactusLang.Model;

public class FunctionStore<TFunc> : IFunctionHolder<TFunc> where TFunc : ModelFunction {
    private readonly OrderedDictionary<FunctionSymbol, TFunc> _functions = new();

    public TFunc? GetExactFunction(FunctionSymbol id) {
        if (!_functions.ContainsKey(id)) {
            //throw new Exception($"Function {id} does not exist.");

            //:(
            foreach (var manualSearch in _functions.Values)
                if (manualSearch.Symbol.Equals(id))
                    return manualSearch;

            return null;
        }

        return _functions[id];
    }

    public bool ContainsExactFunction(FuncSignature id) =>
        _functions.Keys.Any(f => f.ExactSignatureMatch(id));

    public TFunc? GetExactFunction(FuncSignature sign) {
        var matches = _functions.Keys.Where(f => f.ExactSignatureMatch(sign)
        );
        var functionSymbols = matches as FunctionSymbol[] ?? matches.ToArray();
        if (functionSymbols.Count() != 1) return null;
        return _functions[functionSymbols.First()];
    }


    public TFunc? GetMatchingFunction(FuncSignature id) {
        FunctionSymbol? guessedId = GetMatchingId(id);

        if (guessedId == null)
            return null;

        return _functions[guessedId];
    }

    public bool ContainsMatchingFunction(FuncSignature id) {
        if (ContainsExactFunction(id)) return true;

        FunctionSymbol? guessedId = GetMatchingId(id);
        return guessedId != null;
    }

    public FunctionSymbol? GetMatchingId(FuncSignature signature) {
        //Check for ones with same name and parameter count
        var matchingFunctions =
            _functions.Keys
                .Where(f => f.Name.Equals(signature.Name) && f.Parameters.Count == signature.Params.Count)
                .ToList();

        if (!matchingFunctions.Any())
            return null;

        foreach (var possibleFunc in matchingFunctions) {
            if (possibleFunc.DoesMatchSignature(signature))
                return possibleFunc;
        }


        return null;
    }

    public bool AddFunction(TFunc variable) {
        if (_functions.ContainsKey(variable.Symbol)) {
            //throw new Exception($"Function {variable.Name} already exists");
            return false;
        }

        //If has functions, check for correct overload
        //if(_fileFunctions.Count>0) {
        var overloads = _functions.Values
            .Where(f => f.Name.Equals(variable.Name));

        if (overloads.Any() && overloads.First().ReturnType != variable.ReturnType)
            //Error!
            return false;


        _functions.Add(variable.Symbol, variable);
        return true;
    }

    public List<TFunc> GetFunctions() => _functions.Values.ToList();
}