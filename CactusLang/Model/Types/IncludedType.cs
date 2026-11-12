using CactusLang.Model.CodeStructure.MemberHolders.IncludedFiles;

namespace CactusLang.Model.Types;

public class IncludedType(IncludedFile origin, BaseType type) : BaseType
{
    public IncludedFile Origin { get; } = origin;
    public BaseType InnerType { get; } = type;
    
    public override string Name  => Origin.Name + "." + InnerType.Name;
    public override int Size { get; } = type.Size;
    public override bool CanImplicitCastInto(BaseType superiorType) => InnerType.CanImplicitCastInto(superiorType);
}