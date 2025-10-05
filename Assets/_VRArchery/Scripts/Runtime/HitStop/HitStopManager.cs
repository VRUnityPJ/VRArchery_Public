using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;
using System;
using _VRArchery.Scripts.Utility;

/// <summary>
/// ヒットストップを管理する静的クラス
/// </summary>
public static class HitStopManager
{
    // 現在実行中のヒットストップ処理をキャンセルするためのトークンソース
    private static CancellationTokenSource s_cancellationTokenSource;

    /// <summary>
    /// ヒットストップを適用します。
    /// </summary>
    /// <param name="duration">ヒットストップの時間（秒）</param>
    /// <param name="timeScale">ヒットストップ中の時間の倍率（0.0fで停止）</param>
    public static void Apply(float duration, float timeScale = 0.0f)
    {
        // 既に実行中のヒットストップがあればキャンセルする
        s_cancellationTokenSource?.Cancel();
        s_cancellationTokenSource = new CancellationTokenSource();

        // ヒットストップ処理を非同期で開始する（.Forget()で呼び出し元は待機しない）
        HitStopTask(duration, timeScale, s_cancellationTokenSource.Token).Forget();
    }

    private static async UniTask HitStopTask(float duration, float timeScale, CancellationToken token)
    {
        // 時間の流れを遅くする
        Time.timeScale = timeScale;

        try
        {
            // ignoreTimeScale: true を指定し、Time.timeScaleの影響を受けずに実時間で待機する
            await UniTask.Delay(System.TimeSpan.FromSeconds(duration), ignoreTimeScale: true, cancellationToken: token);
            CustomDebug.Log("HitStop");
        }
        catch (OperationCanceledException)
        {
            // タスクがキャンセルされた場合（新しいヒットストップが開始された場合）、
            // Time.timeScaleを元に戻さずに処理を終了する
            return;
        }

        // 時間の流れを元に戻す
        Time.timeScale = 1.0f;
    }
}