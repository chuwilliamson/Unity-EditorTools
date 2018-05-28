using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;
using UnityEditor;
using UnityEngine;

namespace Dialogue
{


    public class DialogueEditor : EditorWindow
    {
        public static List<DialogueRootObject> DialogueList;

        private readonly string _dialogueName = string.Empty;

        public DialogueRootObject CurrentDialogue;

        public GUIStyle DialogueGuiStyle = new GUIStyle();

        public Dictionary<string, List<string>> EmoteList = new Dictionary<string, List<string>>();

        private Vector2 _scrollPosition = Vector2.zero;

        private int _viewIndex = 1;

        [MenuItem("Tools/ChuTools/Dialogue Editor")]
        private static void Init()
        {
            var w = (DialogueEditor)GetWindow(t: typeof(DialogueEditor));
            w.PopulateExistingConversations();
        }

        [ContextMenu("Clear")]
        private void ClearDialogue()
        {
            var allfiles = new List<string>();
            var absPath = Application.dataPath + "/Dialogue/ScriptableObjects/";

            // datapath includes the "Assets" string so take it off to get the relative path...
            var relPath = absPath.Substring(startIndex: Application.dataPath.Length - "Assets".Length);

            allfiles.AddRange(collection: Directory.GetFiles(relPath, "*.asset"));

            foreach (var s in allfiles) AssetDatabase.DeleteAsset(s);

            DialogueList.Clear();
            CurrentDialogue = null;
        }

        private void CreateDialogue(string conversationId)
        {
            if (conversationId == string.Empty) conversationId = "CON001-NAME";
            DialogueList = new List<DialogueRootObject>();
            var dro = CreateDialogueRoot.Create(dname: conversationId + "-" + DialogueList.Count);
            CurrentDialogue = dro;
            DialogueList.Add(CurrentDialogue);
            Selection.activeObject = CurrentDialogue;
        }

        [ContextMenu("Delete Item")]
        private void DeleteDialogueItem()
        {
            CurrentDialogue.Conversation.RemoveAt(_viewIndex);
        }

        private void DisplayConversation()
        {
            _scrollPosition = GUILayout.BeginScrollView(_scrollPosition, false, true);

            GUILayout.BeginHorizontal();

            GUILayout.Space(50);

            GUILayout.EndHorizontal();

            if (CurrentDialogue.Conversation == null) Debug.Log("No Conversation....");

            DialogueGuiStyle.normal.textColor = Color.white;
            if (CurrentDialogue.Conversation.Count > 0)
            {
                for (var i = 0; i < CurrentDialogue.Conversation.Count; i++)
                {
                    GUILayout.Space(10);
                    GUILayout.BeginHorizontal();
                    GUILayout.Label(text: string.Format("Line {0}", i));

                    GUILayout.EndHorizontal();
                    CurrentDialogue.Conversation[i].ParticipantName = EditorGUILayout.TextField("Participant Name",
                        CurrentDialogue.Conversation[i].ParticipantName);
                    CurrentDialogue.Conversation[i].Side = EditorGUILayout.EnumPopup("Side",
                            selected: (Side)Enum.Parse(enumType: typeof(Side),
                                value: CurrentDialogue.Conversation[i].Side))
                        .ToString();
                    CurrentDialogue.Conversation[i].Line =
                        EditorGUILayout.TextField("Line", CurrentDialogue.Conversation[i].Line);
                    CurrentDialogue.Conversation[i].EmoteType = EditorGUILayout.EnumPopup("Emote",
                        selected: (EmoteType)Enum.Parse(enumType: typeof(EmoteType),
                            value: CurrentDialogue.Conversation[i].EmoteType)).ToString();
                    EditorUtility.SetDirty(CurrentDialogue);
                }
            }
            else
            {
                GUILayout.Space(10);
                GUILayout.Label("This Dialogue List is Empty.");
            }

            GUILayout.EndScrollView();
        }

        private void LoadFromFile()
        {
            var absPath = EditorUtility.OpenFilePanel("Select Dialogue List",
                directory: Application.dataPath + "/Dialogue/", extension: "xml");
            if (absPath == string.Empty)
            {
                Debug.LogWarning("No File Selected");
                return;
            }

            var relPath = absPath.Substring(startIndex: Application.dataPath.Length - "Assets".Length);
            if (DialogueList == null) DialogueList = new List<DialogueRootObject>();

            DialogueTree tree;

            var serializer = new XmlSerializer(type: typeof(DialogueTree));
            using (Stream reader = new FileStream(relPath, FileMode.Open))
            {
                tree = (DialogueTree)serializer.Deserialize(reader);
            }

            for (var i = 0; i < tree.DialogueRoots.Count - 1; i++)
            {
                var nodes = tree.DialogueRoots[i].DialogueNodes;
                var dro = CreateDialogueRoot.Create(tree.DialogueRoots[i].Current.ConversationID);
                if (dro.Conversation == null) dro.Conversation = new DialogueRoot();

                foreach (var node in nodes) dro.Conversation.Add(node);
                DialogueList.Add(dro);
                EditorUtility.SetDirty(dro);
                AssetDatabase.SaveAssets();
            }
        }

        private void OnFocus()
        {
            if (PopulateExistingConversations()) CurrentDialogue = DialogueList[index: _viewIndex - 1];
        }

        private void OnGUI()
        {
            DialogueGuiStyle.alignment = TextAnchor.UpperCenter;
            DialogueGuiStyle.fontStyle = FontStyle.BoldAndItalic;
            DialogueGuiStyle.fontSize = 25;
            DialogueGuiStyle.normal.textColor = Color.white;

            GUILayout.Label("Dialogue Editor", DialogueGuiStyle);
            GUILayout.Space(25);
            if (GUILayout.Button("Jamies Help Guide Button"))
                Application.OpenURL(url: "file:///" + Application.dataPath + "/Dialogue/DialogueUserGuide.pdf");
            GUILayout.BeginHorizontal();
            {
                GUILayout.Space(10);

                if (GUILayout.Button("Create Dialogue", GUILayout.ExpandWidth(false))) CreateDialogue(_dialogueName);

                GUILayout.Space(10);
                if (GUILayout.Button("Load From File", GUILayout.ExpandWidth(false))) LoadFromFile();

                GUILayout.Space(25);

                GUILayout.EndHorizontal();
            }

            GUILayout.Space(25);


            if (DialogueList == null || CurrentDialogue == null) return;

            CurrentDialogue = DialogueList[index: _viewIndex - 1];
            EditorGUILayout.LabelField("Current Conversation", CurrentDialogue.name);
            EditorGUILayout.LabelField("Lines: ", label2: CurrentDialogue.Conversation.Count.ToString());
            GUILayout.BeginHorizontal();
            if (DialogueList.Count > 0)
                if (GUILayout.Button("Prev", GUILayout.ExpandWidth(false))) if (_viewIndex > 1) _viewIndex--;

            GUILayout.Space(10);
            if (GUILayout.Button("Next", GUILayout.ExpandWidth(false))) if (_viewIndex < DialogueList.Count) _viewIndex++;

            GUILayout.EndHorizontal();


            GUILayout.Space(25);
            if (DialogueList != null && DialogueList.Count > 0 && CurrentDialogue != null) DisplayConversation();
        }

        private bool PopulateExistingConversations()
        {
            /*
             * persistent path: C:/Users/chuwi/AppData/LocalLow/DefaultCompany/LoboDestroyo - PC, 
             * data path: C:/Users/chuwi/Documents/GitLab/LobodestroyoUnity/Assets
             */
            var allfiles = new List<string>();
            allfiles.AddRange(collection: Directory.GetFiles(path: Application.dataPath + "/Dialogue/ScriptableObjects/",
                searchPattern: "*.asset"));

            // if you add a regular file here it will break

            DialogueList = new List<DialogueRootObject>();
            foreach (var file in allfiles)
            {
                var relpath = file.Substring(startIndex: Application.dataPath.Length - "Assets".Length);
                var d = AssetDatabase.LoadAssetAtPath<DialogueRootObject>(relpath);
                DialogueList.Add(d);
                EditorUtility.SetDirty(d);
            }

            return true;
        }
    }
}