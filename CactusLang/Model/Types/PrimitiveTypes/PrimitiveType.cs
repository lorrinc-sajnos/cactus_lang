namespace CactusLang.Model.Types.PrimitiveTypes;

public abstract class PrimitiveType : CactusType{
	
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