using CactusLang.Model.Symbols;
using CactusLang.Model.Types;

namespace CactusLang.Model.CodeStructure.MemberHolders.IncludedFiles;

public class IncludedFile : IMemberHolder {
    public CactusFile IncFile { get; }
    public CactusFile ParentFile { get; }
    public string Alias { get; }
    public string Name => Alias;
    public FileID ParentID => ParentFile.ID;

    public IncludedFile(CactusFile includedFile, string alias, CactusFile parentFile) {
        IncFile = includedFile;
        ParentFile = includedFile;
        Alias = alias;
        MemberType = new Type(this);
    }

    public BaseType MemberType { get; }

    public class Type(IncludedFile parent) : BaseType {
        public IncludedFile Parent { get; } = parent;
        public override string Name => Parent.Name;

        public override int Size => throw new NotImplementedException();
        public override bool IsAbstract => true;


        protected override bool CanImplicitCastInto(BaseType superiorType) {
            throw new NotImplementedException();
        }

        public override string CName => throw new NotImplementedException();
    }
    

    public MemberFunction? GetExactFunction(FunctionSymbol id) => IncFile.GetExactFunction(id);

    public bool ContainsExactFunction(FuncSignature id) => IncFile.ContainsExactFunction(id);

    public MemberFunction? GetExactFunction(FuncSignature sign) => IncFile.GetExactFunction(sign);

    public MemberFunction? GetMatchingFunction(FuncSignature id) => IncFile.GetMatchingFunction(id);

    public bool ContainsMatchingFunction(FuncSignature id) => IncFile.ContainsMatchingFunction(id);

    public FunctionSymbol? GetMatchingId(FuncSignature signature) => IncFile.GetMatchingId(signature);
    public List<MemberFunction> GetFunctions() => IncFile.GetFunctions();

    public MemberField? GetField(string name) => IncFile.GetField(name);

    public List<MemberField> GetFields() => IncFile.GetFields();
}