using UnityEngine;

public enum TileType
{
    Ground,
    Wall,
    None
}

/// <summary>
/// 모든 타일의 최상위 클래스입니다.
/// </summary>
public abstract class Tile : MonoBehaviour
{
    public Vector2Int coordinate;
    public Item item;
    public bool isBlockVert;
    public bool isBlockHorz;

    /// <summary>
    /// 타일을 사용 가능한 상태로 세팅합니다.
    /// </summary>
    public void Initialize(TileData tileData)
    {
        coordinate = new Vector2Int(tileData.posX, tileData.posY);
        transform.position = new Vector3(tileData.posX, 0, tileData.posY) * TilePositionSetting.Instance.tileInterval;
    }

    public virtual TileType GetTileType()
    {
        return TileType.None;
    }

    public bool IsEnterable(Vector2Int direction)
    {
        if (direction.x != 0 && !isBlockHorz)
        {
            return true;
        }
        else if (direction.y != 0 && !isBlockVert)
        {
            return true;
        }

        return false;
    }
    
    /// <summary>
    /// 플레이어가 타일 위에 멈출 때 할 행동을 정의.
    /// </summary>
    public abstract void ActiveEffectByStop();
}