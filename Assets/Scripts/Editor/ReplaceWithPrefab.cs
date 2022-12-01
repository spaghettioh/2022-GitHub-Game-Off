using UnityEngine;
using UnityEditor;

public class ReplaceWithPrefab : EditorWindow
{
    [SerializeField] private GameObject _prefab;

    [MenuItem("Tools/Replace With Prefab")]
    private static void CreateReplaceWithPrefab()
    {
        var window = GetWindow<ReplaceWithPrefab>();
    }

    private void OnGUI()
    {
        _prefab = (GameObject)EditorGUILayout.ObjectField(
            "Prefab", _prefab, typeof(GameObject), false);

        if (GUILayout.Button($"Replace {Selection.objects.Length} object(s)"))
        {
            var selection = Selection.gameObjects;

            for (var i = selection.Length - 1; i >= 0; --i)
            {
                var selected = selection[i];
                var prefabType = PrefabUtility.GetPrefabAssetType(_prefab);
                GameObject newObject;

                //if (prefabType == PrefabAssetType.Variant)
                //{
                    newObject = (GameObject)PrefabUtility
                        .InstantiatePrefab(_prefab);
                //}
                //else
                //{
                //    newObject = Instantiate(_prefab);
                //    newObject.name = _prefab.name;
                //}

                if (newObject == null)
                {
                    Debug.LogError("Error instantiating prefab");
                    break;
                }

                Undo.RegisterCreatedObjectUndo(
                    newObject, "Replace with prefab");
                newObject.transform.parent = selected.transform.parent;
                newObject.transform.localPosition =
                    selected.transform.localPosition;
                newObject.transform.localRotation =
                    selected.transform.localRotation;
                newObject.transform.localScale = selected.transform.localScale;
                newObject.transform
                    .SetSiblingIndex(selected.transform.GetSiblingIndex());
                Undo.DestroyObjectImmediate(selected);
            }
        }

        GUI.enabled = false;
    }
}