using UnityEngine;
using System.Collections;

public class DeadState : EnemyState
{
    Chest dropChest = null;

    public DeadState(Enemy enemy) : base(enemy)
    {
    }

    public override void Enter()
    {
        /*
        Debug.Log("Enemy has dieded");
        enemy.agent.isStopped = true;
        string toPlay = enemy.animSet.death;
        enemy.PlayAnimation(toPlay);
        Object.Destroy(enemy.gameObject, 2f);
        */

        enemy.eventBus.Publish(new EnemyKilledEvent(enemy.enemyID));
        enemy.StartCoroutine(Death());
    }

    private IEnumerator Death()
    {
        enemy.agent.isStopped = true;
        string toPlay = enemy.animSet.death;
        enemy.PlayAnimation(toPlay);

        yield return new WaitForSeconds(1f);

        if(enemy.drops.Length == 0 || !enemy.TryGetComponent<Chest>(out dropChest))
        {
            Object.Destroy(enemy.gameObject, 2f);
            yield break;
        }

        dropChest.enabled = true;

        for(int i = 0; i < enemy.drops.Length; i++)
        {
            EnemyItemDrop drop = enemy.drops[i];

            int rand = (int)Random.Range(0, 100);

            if(rand <= drop.dropChance)
            {
                //Debug.Log($"Adding {drop.item.name} x {drop.amount} to container \"{dropChest.container.name}\"");
                enemy.invService.AddItem(drop.item, drop.amount, dropChest.container);
            }
        }

        enemy.invService.onContentsChanged += CheckContainer;
    }

    void CheckContainer()
    {
        int freeSlots = enemy.invService.GetAmountOfFreeSlots(dropChest.container);
        int chestSlots = dropChest.container.slots.Length;
        if (freeSlots == chestSlots)
        {
            enemy.invService.onContentsChanged -= CheckContainer;
            dropChest.enabled = false;
            Object.Destroy(enemy.gameObject, 2f);
        }
    }

    public override void Update() { }

    public override void Exit() { }
}
