using Code.Scrips.FetchAndMatch;
using UnityEditor;
using UnityEngine;

namespace Editors
{
    public class AddComponentsForFetchAndMatch : EditorWindow
    {
        GameObject parentObject;
        private FetchAndMatchManager manager;
        bool addRigidbody = false;
        bool addDropOffPoint = false;

        [MenuItem("Tools/Add Components to Children")]
        public static void ShowWindow()
        {
            GetWindow<AddComponentsForFetchAndMatch>("Add Components");
        }

        void OnGUI()
        {
            GUILayout.Label("Select Parent Object and Components", EditorStyles.boldLabel);

            parentObject = (GameObject)EditorGUILayout.ObjectField("Parent Object", parentObject, typeof(GameObject), true);
            manager = (FetchAndMatchManager)EditorGUILayout.ObjectField("Manager Object", manager, typeof(FetchAndMatchManager), true);
            
            addRigidbody = EditorGUILayout.Toggle("Add Rigidbody", addRigidbody);
            addDropOffPoint = EditorGUILayout.Toggle("Add DropOffPoint", addDropOffPoint);

            if (GUILayout.Button("Add Components"))
            {
                if (parentObject == null)
                {
                    Debug.LogWarning("Please assign a parent object.");
                    return;
                }

                AddComponentsToAllChildren();
            }
        }

        void AddComponentsToAllChildren()
        {
            Transform[] allChildren = parentObject.GetComponentsInChildren<Transform>();

            foreach (Transform child in allChildren)
            {
                if (child == parentObject.transform) continue; // Skip the parent itself

                

                if (addRigidbody && child.GetComponent<Rigidbody>() == null)
                {
                    //check for gravity!!
                    Rigidbody body;
                    body = child.gameObject.AddComponent<Rigidbody>();
                    body.useGravity = false;
                }

                if (addDropOffPoint && child.GetComponent<DropOffPoint>() == null)
                {
                    DropOffPoint obj;
                    obj = child.gameObject.AddComponent<DropOffPoint>();
                    obj.manager = manager;
                }
            }

            Debug.Log("Components added to all children of " + parentObject.name);
        }
    }
}