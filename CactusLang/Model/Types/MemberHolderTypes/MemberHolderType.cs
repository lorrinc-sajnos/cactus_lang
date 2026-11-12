using CactusLang.Model.CodeStructure.MemberHolders;
using CactusLang.Model.CodeStructure.MemberHolders.Structs;

namespace CactusLang.Model.Types.MemberHolderTypes;

public class MemberHolderType(IMemberHolder memberHolder) : BaseType {
	public IMemberHolder Parent { get; } = memberHolder;

	public override string Name => Parent.Name;

	public override int Size => -999; //TODO C generálás alapján kiszámolni...

	public override bool CanImplicitCastInto(BaseType other) {
		//TODO
		return false;
	}

	//public override string CName => Parent.ParentID.CPrefix + Parent.Name;
}