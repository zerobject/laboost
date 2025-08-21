#if UNITY_EDITOR
using System.IO;
using UnityEditor;
using UnityEngine;
using Zerobject.Laboost.Runtime.Installers;

namespace Zerobject.Laboost.Editor.Windows
{
    public class InstallerWizard : ScriptableWizard
    {
        public string        InstallerName = "NewInstaller";
        public string        InstallerPath = "Assets";
        public MonoScript    DerivedScript;
        public InstallerType InstallerType = InstallerType.MonoInstaller;

        [MenuItem("Assets/Create/Laboost/Open Installer Wizard")]
        private static void CreateWizard()
            => DisplayWizard<InstallerWizard>("Installer Builder", "Создать");

        private void OnWizardCreate()
        {
            var derivedType = DerivedScript != null ? DerivedScript.GetClass() : null;
            var path        = string.IsNullOrEmpty(InstallerPath) ? "Assets" : InstallerPath;
            path = !Directory.Exists(path) ? Path.GetDirectoryName(path) : path;

            var filePath = Path.Combine(path!, InstallerName + ".cs");
            var template = $@"using Zerobject.Laboost.Runtime.Installers;
using UnityEngine;

public class {InstallerName} : {InstallerType}<{(derivedType != null ? derivedType : InstallerName)}>
{{
    public override void InstallBindings()
    {{
    }}
}}";

            File.WriteAllText(filePath, template);
            AssetDatabase.Refresh();
            Selection.activeObject = AssetDatabase.LoadAssetAtPath<MonoScript>(filePath);
        }

        protected override bool DrawWizardGUI()
        {
            InstallerName = EditorGUILayout.TextField(
                "Installer Name",
                InstallerName);

            InstallerType = (InstallerType)EditorGUILayout.EnumPopup(
                "Installer Type",
                InstallerType);

            DerivedScript = (MonoScript)EditorGUILayout.ObjectField(
                "Derived Script",
                DerivedScript,
                typeof(MonoScript),
                false);

            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.PrefixLabel("Installer Path");
            EditorGUILayout.SelectableLabel(
                InstallerPath,
                EditorStyles.textField,
                GUILayout.Height(EditorGUIUtility.singleLineHeight)
            );

            if (GUILayout.Button("...", GUILayout.Width(30)))
            {
                var folder = EditorUtility.OpenFolderPanel("Выбрать путь сохранения инсталлера", "Assets", "");
                if (!string.IsNullOrEmpty(folder))
                {
                    InstallerPath = folder.StartsWith(Application.dataPath)
                        ? "Assets" + folder[Application.dataPath.Length..]
                        : folder;
                }
            }

            EditorGUILayout.EndHorizontal();

            return true;
        }
    }
}
#endif