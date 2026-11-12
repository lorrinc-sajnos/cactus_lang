using CactusLang.Model.Symbols;
using CactusLang.Model.Types;

namespace CactusLang.Model.CodeStructure.MemberHolders.IncludedFiles;

public class IncludedFile : IMemberHolder {
    public CactusFile CactusFile { get; }
    public CactusFile ParentFile { get; }
    public string Alias { get; }
    public string Name => Alias;
    public FileID ParentID => ParentFile.ID;

    public IncludedFile(CactusFile includedFile, string alias, CactusFile parentFile) {
        CactusFile = includedFile;
        ParentFile = parentFile;
        Alias = alias;
        MemberType = new Type(this);
    }

    public BaseType MemberType { get; }

    public class Type(IncludedFile parent) : BaseType {
        public IncludedFile Parent { get; } = parent;
        public override string Name => Parent.Name;

        public override int Size => throw new NotImplementedException();
        public override bool IsAbstract => true;

        public override bool CanImplicitCastInto(BaseType superiorType) => false;
    }
    

    public MemberFunction? GetExactFunction(FunctionSymbol id) => CactusFile.GetExactFunction(id);

    public bool ContainsExactFunction(FuncSignature id) => CactusFile.ContainsExactFunction(id);

    public MemberFunction? GetExactFunction(FuncSignature sign) => CactusFile.GetExactFunction(sign);

    public MemberFunction? GetMatchingFunction(FuncSignature id) => CactusFile.GetMatchingFunction(id);

    public bool ContainsMatchingFunction(FuncSignature id) => CactusFile.ContainsMatchingFunction(id);

    public FunctionSymbol? GetMatchingId(FuncSignature signature) => CactusFile.GetMatchingId(signature);
    public List<MemberFunction> GetFunctions() => CactusFile.GetFunctions();

    public MemberField? GetField(string name) => CactusFile.GetField(name);

    public List<MemberField> GetFields() => CactusFile.GetFields();

    public IMemberHolder? GetMemberHolder(string name) => CactusFile.GetStruct(name);
}