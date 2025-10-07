using System.Threading;
using Cysharp.Threading.Tasks;

namespace KeyBoard
{
    /// <summary>
    /// 何か操作を完了したことを通知するインターフェース
    /// </summary>
    public interface ICompletable
    {
        UniTask OnComplete(CancellationToken token);
    }
}