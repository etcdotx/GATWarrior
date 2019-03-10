using UnityEngine;

namespace QuestSystem
{
    public class CollectionObjective : IQuestObjective
    {
        private string title;
        private string description;
        private bool isComplete;
        private bool isBonus;
        private string verb;
        private int collectionAmount; //total amount of whatever we need
        private int currentAmount; //start at 0
        private GameObject itemToCollect;


        /// <summary>
        /// This constructor builds a collection objective.
        /// </summary>
        /// <param name="titleVerb">Describe the type of collection</param>
        /// <param name="totalAmount">Amount required to complete objective</param>
        /// <param name="item">Item to be collected</param>
        /// <param name="descrip">Describe what will be collected</param>
        /// <param name="bonus">Is this a bonus objective?</param>
        public CollectionObjective(string titleVerb, int totalAmount, GameObject item, string descrip, bool bonus)
        {
            title = titleVerb + " " + totalAmount + " " + item.name;
            verb = titleVerb;
            description = descrip;
            itemToCollect = item;
            collectionAmount = totalAmount;
            currentAmount = 0;
            isBonus = bonus;
            CheckProgress();
        }

        public string Title
        {
            get
            {
                return title;
            }
        }

        public string Description
        {
            get
            {
                return description;
            }
        }

        public bool IsComplete
        {
            get
            {
                return isComplete;
            }
        }

        public bool IsBonus
        {
            get
            {
                return isBonus;
            }
        }

        public int CollectionAmount
        {
            get
            {
                return collectionAmount;
            }
        }

        public int CurrentAmount
        {
            get
            {
                return currentAmount;
            }
        }

        public GameObject ItemToCollect
        {
            get
            {
                return itemToCollect;
            }
        }

        public void CheckProgress()
        {
            if (currentAmount >= collectionAmount)
                isComplete = true;
            else
                isComplete = false;
        }

        public void UpdateProgress()
        {
            throw new System.NotImplementedException();
        }

        public override string ToString()
        {
            return currentAmount + "/" + collectionAmount + " " + itemToCollect.name + " " + verb + "ed!";
        }
    }
}
