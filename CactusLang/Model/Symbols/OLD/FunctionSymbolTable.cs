/*
 * using CactusLang.Util;

namespace CactusLang.Model.Symbols;

public class FunctionSymbolTable {
    private readonly HashSet<FunctionSymbol> _functions;

    public FunctionSymbolTable() {
        _functions = new ();
    }


    public bool ContainsExactFunction(FuncSignature id) => _functions.Contains(id);
    
    
    public FunctionSymbol? GetMatchingFunction(FuncSignature signature) {
        if (ContainsExactFunction(symbol))
            return symbol;

        FuncID? guessedId = GetMatchingId(id);

        if (guessedId == null)
            return null;

        return _functions[guessedId.Value];
    }
    public bool ContainsMatchingFunction(FuncSignature id) {
        if (ContainsExactFunction(id)) return true;

        FuncID? guessedId = GetMatchingId(id);
        return guessedId != null;
    }

    public FuncID? GetMatchingId(FuncID id) {
        if (_functions.ContainsKey(id))
            return id;

        //If exact match is not found, check for ones with same name and parameter count
        var matchingFunctions =
            _functions
                .Where(f => f.Name.Equals(id.Name) && f.Parameters.Count == id.Params.Count)
                .ToList();

        if (!matchingFunctions.Any())
            return null;

        foreach (var possibleFunc in matchingFunctions) {
            if (id.DoesMatch(possibleFunc))
                return possibleFunc;
        }


        return null;
    }

    public bool AddFunction(FunctionSymbol variable) {
        if (_functions.Contains(variable)) {
            //throw new Exception($"Function {variable.Name} already exists");
            return false;
        }
        
        return true;
    }
    
    public List<FunctionSymbol> GetFunctions() => _functions.ToList();
}

*/