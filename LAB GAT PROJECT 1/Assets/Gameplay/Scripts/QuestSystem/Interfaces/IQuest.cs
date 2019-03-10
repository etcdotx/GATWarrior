namespace QuestSystem
{
    public interface IQuest
    {
        bool IsThereAReward { get; }
        void GetRewards();
        void OnCompletion();
        void OnFailed();
    }
}
