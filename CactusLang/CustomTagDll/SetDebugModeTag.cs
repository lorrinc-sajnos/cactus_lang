using CactusLang.Model;
using CactusLang.Tags;

namespace CactusLang.CustomTagDll;

public class SetDebugModeTag : Tag {
    public const string TagId = "debug_mode";
    public override string Id => TagId;

    public override void OnDeclared(CactusFile cactusFile) {
        TagContainer.Global.AddCommonVariable(new CommonVariable("DEBUG_MODE", true, this));
    }
}