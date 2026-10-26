using CactusLang.Model.Symbols;
using CactusLang.Model.Types;

namespace CactusLang.Model.CodeStructure.MemberHolders.IncludedFiles;

public class IncludedFile(CactusFile file, CactusFile parentFile) : IMemberHolder {
    public CactusFile File { get; } = file;

    public string Name => File.Filename;
    public FileID ParentID => File.FileID;

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

    public BaseType MemberType => throw new NotImplementedException();


    public MemberFunction? GetExactFunction(FunctionSymbol id) => File.Functions.GetExactFunction(id);

    public bool ContainsExactFunction(FuncSignature id) {
        throw new NotImplementedException();
    }

    public MemberFunction? GetExactFunction(FuncSignature sign) {
        throw new NotImplementedException();
    }

    public MemberFunction? GetMatchingFunction(FuncSignature id) {
        throw new NotImplementedException();
    }

    public bool ContainsMatchingFunction(FuncSignature id) {
        throw new NotImplementedException();
    }

    public FunctionSymbol? GetMatchingId(FuncSignature signature) {
        throw new NotImplementedException();
    }

    public List<MemberFunction> GetFunctions() {
        throw new NotImplementedException();
    }

    public MemberField? GetField(string name) {
        throw new NotImplementedException();
    }

    public List<MemberField> GetFields() {
        throw new NotImplementedException();
    }
}