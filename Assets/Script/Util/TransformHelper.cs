using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class TransformHelper
{
    public static Transform FindChildByRecursively(this Transform a, string targetName)
    {
        return Find(targetName, a);
    }

    public static Transform Find(string targetName, Transform rootTransform)
    {
        int count = rootTransform.childCount;
        for (int i = 0; i < count; ++i)
        {
            Transform child = rootTransform.GetChild(i);
            if (child.name.Equals(targetName))
            {
                return child;
            }
            var result = Find(targetName, child);
            if (result)
            {
                return result;
            }
        }
        return null;
    }
}
