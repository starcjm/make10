#if UNITY_EDITOR
#define ENABLE_LOG
#endif

using UnityEngine;

public static class Debug
{
    public static bool isDebugBuild
    {
        get
        {
#if UNITY_EDITOR
            return true;
#else
        return UnityEngine.Debug.isDebugBuild;
#endif
        }
    }

    [System.Diagnostics.Conditional("ENABLE_LOG")]
    public static void Log(object message)
    {
        UnityEngine.Debug.Log(message);
    }

    [System.Diagnostics.Conditional("ENABLE_LOG")]
    public static void Log(object message, UnityEngine.Object context)
    {
        UnityEngine.Debug.Log(message, context);
    }

    [System.Diagnostics.Conditional("ENABLE_LOG")]
    public static void LogError(object message)
    {
        UnityEngine.Debug.LogError(message);
    }

    [System.Diagnostics.Conditional("ENABLE_LOG")]
    public static void LogError(object message, UnityEngine.Object context)
    {
        UnityEngine.Debug.LogError(message, context);
    }

    [System.Diagnostics.Conditional("ENABLE_LOG")]
    public static void LogErrorFormat(string format, params object[] args)
    {
        UnityEngine.Debug.LogErrorFormat(format.ToString(), args);
    }


    [System.Diagnostics.Conditional("ENABLE_LOG")]
    public static void LogWarning(object message)
    {
        UnityEngine.Debug.LogWarning(message.ToString());
    }

    [System.Diagnostics.Conditional("ENABLE_LOG")]
    public static void LogWarning(object message, UnityEngine.Object context)
    {
        UnityEngine.Debug.LogWarning(message.ToString(), context);
    }

    [System.Diagnostics.Conditional("ENABLE_LOG")]
    public static void LogWarningFormat(string format, params object[] args)
    {
        UnityEngine.Debug.LogWarningFormat(format.ToString(), args);
    }

    [System.Diagnostics.Conditional("ENABLE_LOG")]
    public static void LogException(System.Exception e)
    {
        UnityEngine.Debug.LogException(e);
    }

    [System.Diagnostics.Conditional("ENABLE_LOG")]
    public static void LogFormat(string format, params object[] args)
    {
        UnityEngine.Debug.LogFormat(format, args);
    }

    [System.Diagnostics.Conditional("ENABLE_LOG")]
    public static void DrawLine(Vector3 start, Vector3 end, Color color = default(Color), float duration = 0.0f, bool depthTest = true)
    {
        UnityEngine.Debug.DrawLine(start, end, color, duration, depthTest);
    }

    [System.Diagnostics.Conditional("ENABLE_LOG")]
    public static void DrawRay(Vector3 start, Vector3 dir, Color color = default(Color), float duration = 0.0f, bool depthTest = true)
    {
        UnityEngine.Debug.DrawRay(start, dir, color, duration, depthTest);
    }

    [System.Diagnostics.Conditional("ENABLE_LOG")]
    public static void Assert(bool condition, object message)
    {
        UnityEngine.Debug.Assert(condition, message);
        if (!condition)
        {
            throw new System.Exception();
        }
    }

    [System.Diagnostics.Conditional("ENABLE_LOG")]
    public static void AssertFormat(bool condition, string format, params object[] args)
    {
        UnityEngine.Debug.AssertFormat(condition, format, args);
        if (!condition)
        {
            throw new System.Exception();
        }
    }
}
