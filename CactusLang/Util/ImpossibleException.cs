namespace CactusLang.Util;

public class ImpossibleException(string msg = 
	"This expression should be impossible, if it was raised, the program contains logic errors!")
    : Exception(msg);