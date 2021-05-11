using System.Collections.Generic;
using UnityEngine;

public enum ItemType
{
    Destination,
    Star,
    Key,
    Door,
    ChangeBounce
}

/// <summary>
/// 모든 아이템의 최상위 클래스입니다.
/// </summary>
public abstract class Item : MonoBehaviour
{
    protected Player player;
    protected StageManager stageManager;
    
    public Vector2Int coordinate;
    public int value;
    
    /// <summary>
    /// 아이템을 사용 가능한 상태로 세팅합니다.
    /// </summary>
    /// <param name="itemData"></param>
    public virtual void Initialize(ItemData itemData, Player player, StageManager stageManager)
    {
        coordinate = new Vector2Int(itemData.posX, itemData.posY);
        transform.position = new Vector3(itemData.posX, 0, itemData.posY) * TilePositionSetting.Instance.tileInterval;
        value = itemData.value;
        this.player = player;
        this.stageManager = stageManager;
    }
    
    public abstract void ActiveEffectByStop();
    public abstract ItemType GetType();
}