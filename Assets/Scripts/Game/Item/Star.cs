public class Star : Item
{
    public override void ActiveEffectByStop()
    {
        stageManager.CurrentStarNum += 1;
        Destroy(this.gameObject);
    }

    public override ItemType GetType()
    {
        return ItemType.Star;
    }
}