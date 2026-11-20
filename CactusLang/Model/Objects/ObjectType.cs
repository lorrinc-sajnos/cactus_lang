using CactusLang.Model.Types;

namespace CactusLang.Model.Objects;

public class ObjectType(CactusType instanceType) : CactusType
{
    public override string Name => $"object[{InstanceType.Name}]";
    public override int Size => -1;
    
    public CactusType InstanceType { get; } = instanceType;

    public override bool CanImplicitCastInto(CactusType superiorType)
    {
        throw new NotImplementedException();
    }
}