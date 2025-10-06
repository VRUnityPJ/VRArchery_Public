using UnityEngine;
using System.Diagnostics;
using System;

namespace _VRArchery.Scripts.Utility
{
    /// <summary>
    /// 汎用デバッグクラス
    /// </summary>
    public static class CustomDebug
    {
        /// <summary>
        /// エディタ起動時のみ実行する. ビルド時にはコンパイラが呼び出しをスキップしてくれる
        /// </summary>
        /// <param name="o">確認したい内容</param>
        [Conditional("UNITY_EDITOR")]
        public static void Log(object o) => UnityEngine.Debug.Log(o);
        [Conditional("UNITY_EDITOR")]
        public static void LogWarning(object message)
        {
            UnityEngine.Debug.LogWarning(message);
        }
        [Conditional("UNITY_EDITOR")]
        public static void LogError(object message)
        {
            UnityEngine.Debug.LogError(message);
        }
        [Conditional("UNITY_EDITOR")]
        public static void LogException(Exception e, UnityEngine.Object message)
        {
            UnityEngine.Debug.LogException(e, message);
        }
        [Conditional("UNITY_EDITOR")]
        public static void Assert(bool boolean, object message)
        {
            UnityEngine.Debug.Assert(boolean, message);
        }
        [Conditional("UNITY_EDITOR")]
        public static void DrawLine(Vector3 start, Vector3 dir)
        {
            DrawLine(start, dir, Color.magenta);
        }
        [Conditional("UNITY_EDITOR")]
        public static void DrawLine(Vector3 start, Vector3 dir, Color color)
        {
            DrawLine(start, dir, color, 0.0f);
        }
        [Conditional("UNITY_EDITOR")]
        public static void DrawLine(Vector3 start, Vector3 dir, Color color, float duration)
        {
            DrawLine(start, dir, color, duration, true);
        }
        [Conditional("UNITY_EDITOR")]
        public static void DrawLine(Vector3 start, Vector3 dir, Color color, float duration, bool depthTest)
        {
            DrawLine(start, dir, color, duration, depthTest);
        }
        [Conditional("UNITY_EDITOR")]
        public static void DrawRay(Vector3 start, Vector3 dir)
        {
            DrawRay(start, dir, Color.magenta);
        }
        [Conditional("UNITY_EDITOR")]
        public static void DrawRay(Vector3 start, Vector3 dir, Color color)
        {
            DrawRay(start, dir, color, 0.0f);
        }
        [Conditional("UNITY_EDITOR")]
        public static void DrawRay(Vector3 start, Vector3 dir, Color color, float duration)
        {
            DrawRay(start, dir, color, duration, true);
        }
        [Conditional("UNITY_EDITOR")]
        public static void DrawRay(Vector3 start, Vector3 dir, Color color, float duration, bool depthTest)
        {
            DrawRay(start, dir, color, duration, depthTest);
        }
        [Conditional("UNITY_EDITOR")]
        public static void Break()
        {
            UnityEngine.Debug.Break();
        }
    }
}