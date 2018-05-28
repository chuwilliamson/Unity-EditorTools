using UnityEngine;
using UnityEditor;

namespace Dialogue
{
    public class CreateDialogueRoot
    {
        static string path = "Assets/Dialogue/ScriptableObjects/";

        public static DialogueRootObject Create(string dname)
        {
            var asset = ScriptableObject.CreateInstance<DialogueRootObject>();
            asset.Conversation = new Dialogue.DialogueRoot();
            AssetDatabase.CreateAsset(asset, path + dname + ".asset");
            AssetDatabase.SaveAssets();
            return asset;
        }
    }
}