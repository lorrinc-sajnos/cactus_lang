using CactusLang.Model.Types;
using CactusLang.Model.Types.PrimitiveTypes;

namespace CactusLang.Model;

public static class CactusLangModel {
    public static bool InitComplete { get; private set; } = false;
    
    public static void InitCactusLang() {
        if (InitComplete) return;
        
        NumberType.InitPrimitives();
        InitComplete =  true;
    }
}