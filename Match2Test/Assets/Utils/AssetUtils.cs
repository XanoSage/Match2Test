
using UnityEditor;
using UnityEngine;

#if UNITY_EDITOR
public static class AssetUtils
{
    private const string DEFAULT_ASSET_PATH = "Assets/NewAssets";
    private const string ASSET_EXTENSION = ".asset";
    public static T CreateAsset<T>() where T: ScriptableObject
    {
        var path = DEFAULT_ASSET_PATH;

        return CreateAsset<T>(path, "New " + nameof(T));
    }

    public static T CreateAsset<T>(string path, string name) where T : ScriptableObject
    {
        if (string.IsNullOrEmpty(path))
        {
            path = DEFAULT_ASSET_PATH;
        }

        if (!name.EndsWith(ASSET_EXTENSION))
        {
            name += ASSET_EXTENSION;
        }

        var assetPathAndName = AssetDatabase.GenerateUniqueAssetPath(path + "/" + name);

        var asset = ScriptableObject.CreateInstance<T>();
        AssetDatabase.CreateAsset(asset, assetPathAndName);
        AssetDatabase.SaveAssets();
        return asset;
    }

    public static void SelectAssetInProjectView(Object asset)
    {
        EditorUtility.FocusProjectWindow();
        Selection.activeObject = asset;
    }
}
#endif