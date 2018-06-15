using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;
using System.Linq;

namespace Dialogue
{
    public class DialogueDictionaryObject : ScriptableObject
    {
        private readonly Dictionary<string, DialogueRootObject> _dialogueDictionary = new Dictionary<string, DialogueRootObject>();

        public bool AddDialogue(string nodeName, DialogueRootObject dialogueRootObject)
        {
            DialogueRootObject dro;
            var contains = _dialogueDictionary.TryGetValue(nodeName, out dro);
            if (!contains) _dialogueDictionary.Add(nodeName, dialogueRootObject);
            return contains;
        }

        public bool RemoveNode(string nodeName)
        {
            DialogueRootObject dro;
            var contains = _dialogueDictionary.TryGetValue(nodeName, out dro);
            var success = _dialogueDictionary.Remove(nodeName);
            return contains && success;
        }

        private bool PopulateExistingConversations()
        {
            /*
             * persistent path: C:/Users/chuwi/AppData/LocalLow/DefaultCompany/LoboDestroyo - PC, 
             * data path: C:/Users/chuwi/Documents/GitLab/LobodestroyoUnity/Assets
             */
            var allfiles = new List<string>();
            allfiles.AddRange(collection: Directory.GetFiles(path: Application.dataPath + "/Dialogue/ScriptableObjects/", searchPattern: "t:DialogueRootObject"));
            
            // if you add a regular file here it will break
            if (allfiles == null) return false;

            var DialogueList = new List<DialogueRootObject>();
            foreach (var file in allfiles)
            {
                var relpath = file.Substring(Application.dataPath.Length - "Assets".Length);
                var d = AssetDatabase.LoadAssetAtPath<DialogueRootObject>(relpath);
                DialogueList.Add(d);
                EditorUtility.SetDirty(d);
            }

            return true;
        }


    }

}
