using CactusLang.Model.Stores;
using CactusLang.Model.Symbols;
using CactusLang.Model.Types;
using CactusLang.Model.Types.EntityTypes;

namespace CactusLang.Model.CodeStructure.Entities.Structs;

public class CactusStruct : IEntity {
    public CactusStruct(string id, CactusFile cactusFile) {
        Name = id;
        Fields = new();
        Methods = new();
        ParentID = cactusFile.ID;
        StructType = new(this);
    }
    
    public string Name { get; }
    public FileID ParentID { get; }
    public FieldStore<StructField> Fields { get; }
    public FunctionStore<StructMethod> Methods { get; }
    
    public EntityType StructType { get; }

    #region IEntity Members
    public CactusType MemberType => StructType;
    public CactusMethod? GetExactFunction(FunctionSymbol id) => Methods.GetExactFunction(id);

    public bool ContainsExactFunction(FuncCandidate id)  =>  Methods.ContainsExactFunction(id);

    public CactusMethod? GetExactFunction(FuncCandidate sign) => Methods.GetExactFunction(sign);
    public CactusMethod? GetMatchingFunction(FuncCandidate id) =>  Methods.GetMatchingFunction(id);

    public bool ContainsMatchingFunction(FuncCandidate id) => Methods.ContainsMatchingFunction(id);   

    public FunctionSymbol? GetMatchingId(FuncCandidate signature) =>  Methods.GetMatchingId(signature);
    
    public List<CactusMethod> GetFunctions() => Methods.GetFunctions().ConvertAll(CactusMethod (f) => f);

    public CactusField? GetField(string name) => Fields.GetField(name);
    public List<CactusField> GetFields() => Fields.GetFields().ConvertAll<CactusField>(f=> f);
    
    #endregion
}