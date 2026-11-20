using CactusLang.Model.CodeStructure.Entities;

namespace CactusLang.Model.Types.EntityTypes;

public class EntityType(IEntity entity) : CactusType {
	public IEntity Parent { get; } = entity;

	public override string Name => Parent.Name;

	public override int Size => -999; //TODO C generálás alapján kiszámolni...

	public override bool CanImplicitCastInto(CactusType other) {
		//TODO
		return false;
	}

	//public override string CName => Parent.ParentID.CPrefix + Parent.Name;
}