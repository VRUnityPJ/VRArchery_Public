using System.Diagnostics;

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
    }
}