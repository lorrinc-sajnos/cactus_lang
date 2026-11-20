using CactusLang.Model.CodeStructure.Entities.IncludedFiles;

namespace CactusLang.Model.Types;

public class IncludedType(IncludedFile origin, CactusType type) : CactusType
{
    public IncludedFile Origin { get; } = origin;
    public CactusType InnerType { get; } = type;
    
    public override string Name  => Origin.Name + "." + InnerType.Name;
    public override int Size { get; } = type.Size;
    public override bool CanImplicitCastInto(CactusType superiorType) => InnerType.CanImplicitCastInto(superiorType);
}