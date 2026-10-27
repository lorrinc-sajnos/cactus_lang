using System.Runtime.CompilerServices;
using CactusLang.Model.Symbols;
using CactusLang.Model.Types;

namespace CactusLang.Model.CodeStructure.MemberHolders.Structs;

public class FileStruct : IMemberHolder {
    public string Name { get; }
    public FileID ParentID { get; }
    public FieldStore<StructField> Fields { get; }
    public FunctionStore<StructFunction> Functions { get; }
    
    public FileStruct(string id, CactusFile cactusFile) {
        Name = id;
        ParentID = cactusFile.ID;
        _type = new(this);
    }
    public class Type : BaseType {
        public Type(FileStruct fileStruct) {
            FileStruct = fileStruct;
        }

        public FileStruct FileStruct { get; }

        public override string Name => FileStruct.Name;

        public override int Size => -999; //TODO C generálás alapján kiszámolni...

        protected override bool CanImplicitCastInto(BaseType other) {
            //TODO
            return false;
        }

        public override string CName => FileStruct.ParentID.CPrefix + FileStruct.Name;
    }

    private readonly Type _type;
    
    public Type StructType => _type;
    public BaseType MemberType => _type;
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