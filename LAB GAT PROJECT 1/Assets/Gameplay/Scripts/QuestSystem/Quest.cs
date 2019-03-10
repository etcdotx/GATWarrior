using System.Collections.Generic;

namespace QuestSystem
{
    public class Quest : IQuest
    {
        private bool isThereAReward;

        public List<IQuestObjective> objectives;
        private bool IsComplete()
        {
            for (int i = 0; i < objectives.Count; i++)
            {
                if (objectives[i].IsComplete == false && objectives[i].IsBonus == false)
                {
                    return false;
                }
            }
            return true;
        }

        public bool IsThereAReward
        {
            get
                {
                    return isThereAReward;
                }
        }

        public void GetRewards() { }
        public void OnCompletion() { }
        public void OnFailed() { }
    }
}
