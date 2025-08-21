#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading.Tasks;
using UnityEditor;
using UnityEditor.Compilation;
using Zerobject.Laboost.Runtime.Attributes;
using Zerobject.Laboost.Runtime.Extensions.Internal;
using Zerobject.Laboost.Runtime.Injection;
using Zerobject.Laboost.Runtime.Injection.Units;
using Assembly = System.Reflection.Assembly;

namespace Zerobject.Laboost.Editor.Services
{
    public static class DependencyScanner
    {
        private static bool     m_IsScanning;
        private static DateTime m_LastScanTime;

        private const BindingFlags InjectionFlags
            = BindingFlags.Instance
            | BindingFlags.Public
            | BindingFlags.NonPublic;

        static DependencyScanner()
        {
            CompilationPipeline.compilationFinished += OnCompilationFinished;
            EditorApplication.delayCall             += Init;
        }

        private static async void Init()
        {
            if (InjectionMap.DefinitionsByType.Count == 0)
                await ScanAssembliesAsync();
        }

        private static async void OnCompilationFinished(object obj)
            => await ScanAssembliesAsync();

        private static async Task ScanAssembliesAsync()
        {
            if (m_IsScanning) return;
            m_IsScanning = true;

            try
            {
                await Task.Run(() =>
                {
                    var assemblies = AppDomain.CurrentDomain.GetAssemblies();
                    var scanResult = ScanAssembiles(assemblies);

                    InjectionMap.UpdateCache(scanResult);
                    m_LastScanTime = DateTime.Now;
                });
            }
            finally
            {
                m_IsScanning = false;
            }
        }

        private static IEnumerable<InjectionDefinition> ScanAssembiles(params Assembly[] assemblies)
        {
            if (assemblies == null || assemblies.Length == 0)
                yield break;

            foreach (var assembly in assemblies)
            {
                if (ShouldSkipAssembly(assembly)) continue;

                foreach (var type in assembly.GetTypes())
                {
                    var injectList = ScanTypeForInjections(type);
                    if (injectList.Count > 0)
                        yield return new(type, injectList);
                }
            }
        }

        private static List<BaseInjectUnit> ScanTypeForInjections(Type targetType)
        {
            List<BaseInjectUnit> injectList = new();

            foreach (var field in targetType.GetFields(InjectionFlags))
            {
                if (field.TryGetAttribute(out InjectAttribute injectAttr))
                    injectList.Add(new FieldInjectUnit(targetType, field.FieldType, injectAttr.Id, field));
            }

            foreach (var prop in targetType.GetProperties(InjectionFlags))
            {
                if (prop.TryGetAttribute(out InjectAttribute injectAttr))
                    injectList.Add(new PropertyInjectUnit(targetType, prop.PropertyType, injectAttr.Id, prop));
            }

            foreach (var method in targetType.GetMethods(InjectionFlags))
            {
                if (method.IsStatic || method.IsAbstract) continue;

                if (method.TryGetAttribute(out InjectAttribute injectAttr))
                    injectList.Add(new MethodInjectUnit(targetType, method.ReturnType, injectAttr.Id, method));

                foreach (var param in method.GetParameters())
                {
                    if (param.TryGetAttribute(out InjectAttribute paramInjectAttr))
                        injectList.Add(new ParamInjectUnit(targetType, param.ParameterType, injectAttr.Id, param));
                }
            }

            return injectList;
        }

        private static bool ShouldSkipAssembly(Assembly assembly)
        {
            if (assembly == null || assembly.IsDynamic)
                return true;

            var asmName = assembly.GetName().Name;
            return asmName.StartsWith(nameof(System))
                || asmName.StartsWith(nameof(Unity))
                || asmName.StartsWith(nameof(Microsoft))
                || asmName.StartsWith("netstandard")
                || asmName.StartsWith("mscorlib");
        }
    }
}
#endif