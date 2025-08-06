using UnityEngine;
using Zenject;

namespace Quests
{

    public class HaveItemsStep : QuestStepBase
    {
        RequiredItems requiredItems;

        [Inject] readonly InventoryService inventoryService;

        public HaveItemsStep(string id, string stepText, QuestStepBase[] req, QuestStepBase[] next, StepResult compRes, StepResult failRes, string itemID, int amount) : base(id, stepText, req, next, compRes, failRes)
        {
            requiredItems = new RequiredItems(itemID, amount);
        }

        public override void StepBegin()
        {
            base.StepBegin();

            inventoryService.onContentsChanged += InventoryChanged;
            InventoryChanged();
        }

        public override void StepUpdate()
        {
            return;
        }

        public override void StepEnd()
        {
            inventoryService.onContentsChanged -= InventoryChanged;
            this.status = QuestStepStatus.Completed;
            base.StepEnd();
        }

        public override string GetStepText()
        {
            string itemName = inventoryService.GetItemByID(requiredItems.itemID).name.ToLower();
            return $"{stepText} ({requiredItems.currentAmount} / {requiredItems.requiredAmount}) {itemName}";
        }

        void InventoryChanged()
        {
            requiredItems.currentAmount = inventoryService.GetItemCount(requiredItems.itemID);
            if (requiredItems.IsComplete())
                StepEnd();
        }

        struct RequiredItems
        {
            public string itemID;
            public int requiredAmount;
            public int currentAmount;

            public RequiredItems(string itemID, int requiredAmount)
            {
                this.itemID = itemID;
                this.requiredAmount = requiredAmount;
                currentAmount = 0;
            }

            public bool IsComplete() => currentAmount >= requiredAmount;
        }
    }

}
