using CactusLang.CustomTagDll;

namespace CactusLang.Tags;

//TODO
//This si a placeholder
public class TagSystem {
    public static Tag? CreateStatementFromString(string str) {
        string tagId = str[1..];
        switch (tagId) {
            case PrintDebugTag.TagId:
                return new PrintDebugTag();
            
            case SetDebugModeTag.TagId:
                return new SetDebugModeTag();
            default:
                return null;
        }
    } 
    
}