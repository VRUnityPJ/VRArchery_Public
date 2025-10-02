/// <summary>
/// DebugModeかどうかのパラメータを保持するクラス
/// </summary>
public static class DebugMode
{
    private static bool isDebugMode = false;
    /// <summary>
    /// DebugModeかどうか
    /// 一度参照されるとfalseになる
    /// </summary>
    public static bool IsDebugModeOneTime
    {
        get
        {
            //結果を返したのちにfalseにする
            var result = isDebugMode;
            isDebugMode = false;
            return result;
        }
        set
        {
            isDebugMode = value;
        }
    }
}
