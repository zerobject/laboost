using System;
using System.Collections.Generic;
using UnityEngine;

namespace Zerobject.Laboost.Core
{
    public interface IInstantiateFunctionProvider
    {
        T Instantiate<T>(IEnumerable<object> args = null);
        object Instantiate(Type type, IEnumerable<object> args = null);

        T InstantiateComponent<T>(GameObject obj, IEnumerable<object> args = null) where T : Component;
        Component InstantiateComponent(Type type, GameObject obj, IEnumerable<object> args = null);

        T InstantiateComponentOnNewGameObject<T>() where T : Component;
        T InstantiateComponentOnNewGameObject<T>(string objName, IEnumerable<object> args = null) where T : Component;

        GameObject CreateNewGameObject(string objName)
        {
            return new GameObject(objName);
        }
    }
}