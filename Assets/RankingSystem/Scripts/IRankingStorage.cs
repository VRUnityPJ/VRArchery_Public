namespace RankingSystem.Scripts
{
    public interface IRankingStorage
    {
        public void UpdateData<T>(T data)
            where T : IRankingDataElement<T>;
    }
}
