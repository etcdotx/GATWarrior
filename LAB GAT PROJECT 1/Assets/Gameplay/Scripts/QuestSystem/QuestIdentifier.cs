namespace QuestSystem
{
    public class QuestIdentifier : IQuestIdentifier
    {

        private int sourceID;
        private int chainQuestID;
        private int id;

        public QuestIdentifier(int sourceID, int chainQuestID, int id) {
            this.sourceID = sourceID;
            this.chainQuestID = chainQuestID;
            this.id = id;
        }

        public int SourceID
        {
            get
            {
                return sourceID;
            }
        }

        public int ChainQuestID
        {
            get
            {
                return chainQuestID;
            }
        }

        public int ID
        {
            get
            {
                return id;
            }
        }
    }
}
