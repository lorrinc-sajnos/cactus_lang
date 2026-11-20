using CactusLang.Model.Symbols;
using CactusLang.Model.Types;

namespace CactusLang.Model.CodeStructure.Entities.IncludedFiles;

public class IncludedFile : IEntity {
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

    public CactusType MemberType { get; }

    public class Type(IncludedFile parent) : CactusType {
        public IncludedFile Parent { get; } = parent;
        public override string Name => Parent.Name;

        public override int Size => throw new NotImplementedException();
        public override bool IsAbstract => true;

        public override bool CanImplicitCastInto(CactusType superiorType) => false;
    }


    public CactusMethod? GetExactFunction(FunctionSymbol id) => CactusFile.GetExactFunction(id);

    public bool ContainsExactFunction(FuncCandidate id) => CactusFile.ContainsExactFunction(id);

    public CactusMethod? GetExactFunction(FuncCandidate sign) => CactusFile.GetExactFunction(sign);

    public CactusMethod? GetMatchingFunction(FuncCandidate id) => CactusFile.GetMatchingFunction(id);

    public bool ContainsMatchingFunction(FuncCandidate id) => CactusFile.ContainsMatchingFunction(id);

    public FunctionSymbol? GetMatchingId(FuncCandidate signature) => CactusFile.GetMatchingId(signature);
    public List<CactusMethod> GetFunctions() => CactusFile.GetFunctions();

    public CactusField? GetField(string name) => CactusFile.GetField(name);

    public List<CactusField> GetFields() => CactusFile.GetFields();

    public IEntity? GetMemberHolder(string name) => CactusFile.DeclStructStore.GetStruct(name);
}