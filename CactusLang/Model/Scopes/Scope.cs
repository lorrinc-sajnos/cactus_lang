using CactusLang.Model.CodeStructure;
using CactusLang.Model.Symbols;
using CactusLang.Util;

namespace CactusLang.Model.Scopes;

public class Scope {
    private List<Scope> _children;

    private OrderedDictionary<string, VariableSymbol> _variables;

    protected Scope() : this(null) { }

    private Scope(Scope parent) {
        Parent = parent;
        _children = new();
        _variables = new();
    }

    public Scope CreateChild() => AddChild(new Scope(this));

    public Scope AddChild(Scope child) {
        _children.Add(child);
        child.Parent = this;
        return child;
    }

    public Scope Parent { get; private set; }

    public virtual VariableSymbol? GetVariable(string id) {
        if (_variables.ContainsKey(id))
            return _variables[id];
        if (Parent != null)
            return Parent.GetVariable(id);
        return null;
        //throw new Exception($"Variable {id} does not exist.");
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="variable"></param>
    /// <returns>True, if a new variable was added.</returns>
    public bool AddVariable(VariableSymbol variable) {
        if (_variables.ContainsKey(variable.Id)) {
            return false;
        }

        _variables.Add(variable.Id, variable);
        return true;
    }

    public virtual ModelFunction? GetMatchingFunction(FuncSignature signature) {
        return Parent.GetMatchingFunction(signature);
    }

    public virtual int GetScopeDepth() {
        return Parent.GetScopeDepth() + 1;
    }
}