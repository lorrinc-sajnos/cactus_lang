namespace CactusLang.Model.Types.PrimitiveTypes;

public abstract class PrimitiveType : BaseType{
	
	public enum PrimType {
		Float,
		Integer,
		Uint,
		Char,
		Bool,
		Void
	}
	public PrimType Kind { get; protected set; }
}