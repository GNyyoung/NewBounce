public class Key : Item
{
    public override void ActiveEffectByStop()
    {
        foreach (var item in stageManager.itemList)
        {
            if (item.GetType() == ItemType.Door && item.value == this.value)
            {
                Destroy(item.gameObject);
                break;
            }
        }
        
        Destroy(this.gameObject);
    }

    public override ItemType GetType()
    {
        return ItemType.Key;
    }
}