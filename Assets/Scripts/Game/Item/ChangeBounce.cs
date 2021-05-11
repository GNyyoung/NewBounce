using System.Collections.Generic;
using UnityEngine;

public class ChangeBounce : Item
{
    public override void ActiveEffectByStop()
    {
        player.BounceCount = value;
        Destroy(this.gameObject);
    }

    public override ItemType GetType()
    {
        return ItemType.ChangeBounce;
    }
}