using System.Linq;
using UnityEngine;

public class Destination : Item
{
    public override void ActiveEffectByStop()
    {
        if (stageManager.CheckClearCondition())
        {
            StageClear();
        }
    }

    public override ItemType GetType()
    {
        return ItemType.Destination;
    }

    public void StageClear()
    {
        Control.isControllable = false;
        StageClearInterface.Instance.StageClear();
    }
}