using CactusLang.Model.Types;

namespace CactusLang.Model;

public static class CactusLangModel {
    public static bool InitComplete { get; private set; } = false;
    
    public static void InitCactusLang() {
        if (InitComplete) return;
        
        PrimitiveType.InitPrimitives();
        InitComplete =  true;
    }
}