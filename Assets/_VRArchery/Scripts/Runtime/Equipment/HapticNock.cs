using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;
using UnityEngine.XR.Interaction.Toolkit.Interactors;
using Cysharp.Threading.Tasks;
using System.Threading;


[RequireComponent(typeof(Collider))]
/// <summary>
/// 矢をつがえるときのコントローラーの振動を制御するクラス
/// </summary>
public class HapticNock : MonoBehaviour
{
    [Header("On Enter Haptics (一瞬の振動)")]
    [Range(0f, 1f)] public float enterAmplitude = 0.5f;
    public float enterDuration = 0.1f;

    [Header("On Exit Haptics (弱まる振動)")]
    [Range(0f, 1f)] public float exitStartAmplitude = 0.5f;
    public float exitFadeDuration = 0.3f;
    [Tooltip("振動させる対象のInteractor")]
    [SerializeField]
    private XRBaseInputInteractor _interactor;

    // 非同期処理をキャンセルするためのトークンソース
    private CancellationTokenSource _hapticCts;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Nocker"))
        {
            
            if (_interactor != null)
            {
                // 実行中のフェードアウト処理があればキャンセル
                CancelHaptics();
                
                // 一瞬振動させる
                _interactor.SendHapticImpulse(enterAmplitude, enterDuration);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Nocker"))
        {
            if (_interactor != null)
            {
                CancelHaptics();

                // キャンセルトークンを新しく生成
                _hapticCts = new CancellationTokenSource();
                
                // 0.3秒かけて弱まる振動を非同期で開始 (Forgetで投げっぱなしにする)
                FadeOutHapticsAsync(_interactor, _hapticCts.Token).Forget();
            }
        }
    }

    private void OnDestroy()
    {
        // オブジェクト破棄時に確実にキャンセルする
        CancelHaptics();
    }

    private void CancelHaptics()
    {
        if (_hapticCts != null)
        {
            _hapticCts.Cancel();
            _hapticCts.Dispose();
            _hapticCts = null;
        }
    }

    // UniTaskによる振動を徐々に弱める非同期メソッド
    private async UniTask FadeOutHapticsAsync(XRBaseInputInteractor interactor, CancellationToken token)
    {
        float elapsedTime = 0f;

        // キャンセルされるか、指定時間が経過するまでループ
        while (elapsedTime < exitFadeDuration)
        {
            // 時間経過に応じて振幅を減衰 (exitStartAmplitude -> 0)
            float currentAmplitude = Mathf.Lerp(exitStartAmplitude, 0f, elapsedTime / exitFadeDuration);
            
            // XRIの仕様上、短いインパルスを毎フレーム送信し直す
            interactor.SendHapticImpulse(currentAmplitude, Time.deltaTime);

            elapsedTime += Time.deltaTime;
            
            // 次のフレームまで待機 (キャンセルリクエストがあればここでOperationCanceledExceptionがスローされる)
            await UniTask.Yield(PlayerLoopTiming.Update, token);
        }
    }
}