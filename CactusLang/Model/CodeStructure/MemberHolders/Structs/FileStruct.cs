using System.Runtime.CompilerServices;
using CactusLang.Model.Stores;
using CactusLang.Model.Symbols;
using CactusLang.Model.Types;
using CactusLang.Model.Types.MemberHolderTypes;

namespace CactusLang.Model.CodeStructure.MemberHolders.Structs;

public class FileStruct : IMemberHolder {
    public FileStruct(string id, CactusFile cactusFile) {
        Name = id;
        Fields = new();
        Functions = new();
        ParentID = cactusFile.ID;
        StructType = new(this);
    }
    
    public string Name { get; }
    public FileID ParentID { get; }
    public FieldStore<StructField> Fields { get; }
    public FunctionStore<StructFunction> Functions { get; }
    
    public MemberHolderType StructType { get; }

    public BaseType MemberType => StructType;
    public MemberFunction? GetExactFunction(FunctionSymbol id) => Functions.GetExactFunction(id);

    public bool ContainsExactFunction(FuncSignature id)  =>  Functions.ContainsExactFunction(id);

    public MemberFunction? GetExactFunction(FuncSignature sign) => Functions.GetExactFunction(sign);
    public MemberFunction? GetMatchingFunction(FuncSignature id) =>  Functions.GetMatchingFunction(id);

    public bool ContainsMatchingFunction(FuncSignature id) => Functions.ContainsMatchingFunction(id);   

    public FunctionSymbol? GetMatchingId(FuncSignature signature) =>  Functions.GetMatchingId(signature);


    public List<MemberFunction> GetFunctions() => Functions.GetFunctions().ConvertAll(MemberFunction (f) => f);

    public MemberField? GetField(string name) => Fields.GetField(name);
    public List<MemberField> GetFields() => Fields.GetFields().ConvertAll<MemberField>(f=> f);
}