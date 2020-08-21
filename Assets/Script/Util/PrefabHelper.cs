using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class PrefabHelper
{
    public static GameObject Instantiate(string path, Transform parent)
    {
        GameObject prefab = Resources.Load<GameObject>(path);
        if (prefab != null)
        {
            var go = GameObject.Instantiate<GameObject>(prefab);
            bool isWorldPosStay = !(parent is RectTransform);
            go.transform.SetParent(parent, isWorldPosStay);
            go.transform.localPosition = Vector3.zero;
            go.transform.localRotation = Quaternion.identity;
            go.transform.localScale = Vector3.one;
            return go;
        }
        return null;
    }

    public static T Instantiate<T>(string path, Transform parent) where T : Component
    {
        GameObject go = Instantiate(path, parent);
        if (go != null)
        {
            T component = go.GetComponent<T>();
            if (component == null)
            {
                GameObject.DestroyImmediate(go);
            }
            return component;
        }
        return null;
    }

    public static T InstantiateObejct<T>(string path) where T : UnityEngine.Object
    {
        var prefab = Resources.Load(path, typeof(T)) as T;
        if (prefab)
        {
            var go = GameObject.Instantiate(prefab);
            if (!(go is T))
            {
                GameObject.DestroyImmediate(go);
            }
            return (T)go;
        }
        return null;
    }
}
