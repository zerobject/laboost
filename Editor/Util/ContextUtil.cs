using UnityEditor;
using UnityEngine;
using Zerobject.Laboost.Runtime.Contexts;

namespace Zerobject.Laboost.Editor.Util
{
    public static class ContextUtil
    {
        [MenuItem("GameObject/Laboost/Create Project Context")]
        public static void CreateProjectContext()
        {
            GameObject   prefabSrc = new(nameof(ProjectContext), typeof(ProjectContext));
            const string prefabDir = "Assets/Resources";

            if (!AssetDatabase.IsValidFolder(prefabDir))
                AssetDatabase.CreateFolder("Assets", "Resources");

            var prefabPath = $"{prefabDir}/{nameof(ProjectContext)}.prefab";

            PrefabUtility.SaveAsPrefabAsset(prefabSrc, prefabPath);
            Object.DestroyImmediate(prefabSrc);

            Selection.activeObject = AssetDatabase.LoadAssetAtPath<Object>(prefabPath);
        }

        [MenuItem("GameObject/Laboost/Create Scene Context")]
        public static void CreateSceneContext()
            => _ = new GameObject("Scene Context", typeof(SceneContext));

        [MenuItem("GameObject/Laboost/Create GameObject Context")]
        public static void CreateGameObjectContext()
            => _ = new GameObject("GameObject Context", typeof(GameObjectContext));
    }
}