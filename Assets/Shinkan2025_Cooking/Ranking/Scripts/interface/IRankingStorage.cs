using Ranking.Scripts;

namespace Ranking.Scripts.Interface
{
    public interface IRankingStorage
    {
        public void UpdateData<T>(T data)
            where T : IRankingDataElement<T>;
    }
}
