using System.Collections;
using UnityEngine;

public enum Direction
{
    None,
    Up,
    Down,
    Left,
    Right
}

/// <summary>
/// 플레이어 오브젝트가 가지는 클래스입니다.
/// </summary>
public class Player : MonoBehaviour
{
    public delegate IEnumerator MoveAction(Vector2Int directionCoor);
    private Coroutine moveCoroutine = null;
    [SerializeField] private StageManager stageManager;
    
    // 플레이어가 위치한 좌표
    public Vector2Int currentCoor;
    // 플레이어가 한번에 이동하는 칸 수
    private int bounceCount;
    // 이동 간 시간 간격
    public float moveTimeInterval;
    public MoveAction moveAction;
    public CameraMove cameraMove;

    public int BounceCount
    {
        get => bounceCount;
        set
        {
            bounceCount = value;
            stageManager.gameInformation.UpdateBounceCount(value);
        }
    }

    // 플레이어 좌표를 설정하고, 좌표와 일치하는 곳에 배치합니다.
    public void SetPosition(int x, int y)
    {
        currentCoor = new Vector2Int(x, y);
        transform.position = new Vector3(currentCoor.x, 0, currentCoor.y) * TilePositionSetting.Instance.tileInterval;
        cameraMove.SetInitialPosition(currentCoor, bounceCount);
    }

    // 플레이어를 이동시킵니다.
    public void StartMove(Direction direction)
    {
        if (moveCoroutine == null)
        {
            moveCoroutine = StartCoroutine(Move(GetCoorFromDirection(direction)));
        }
    }

    private IEnumerator Move(Vector2Int directionCoor)
    {
        int currentBounceCount = 0;

        stageManager.CurrentMoveTurn += 1;
        
        while (currentBounceCount < bounceCount)
        {
            // 이동에 필요한 상세 코드 입력.
            var nextTile = stageManager.GetTile(currentCoor + directionCoor);

            if (nextTile == null ||
                nextTile.IsEnterable(directionCoor) == false)
            {
                yield return StartCoroutine(MoveBounce(directionCoor));
                directionCoor = -directionCoor;
            }
            else
            {
                yield return StartCoroutine(MoveStraight(directionCoor));
                cameraMove.MoveCameraCoordinate(bounceCount, currentCoor, directionCoor);
            }
            
            currentBounceCount += 1;
        }

        var currentTile = stageManager.GetTile(currentCoor); 
        // 멈춘 타일의 아이템 효과 발동
        var item = currentTile.item;
        if (item != null)
        {
            item.ActiveEffectByStop();
        }
        
        // 멈춘 타일의 효과 발동
        currentTile.ActiveEffectByStop();

        moveCoroutine = null;
    }

    public Vector2Int GetCoorFromDirection(Direction direction)
    {
        Vector2Int directionCoor;
        
        switch (direction)
        {
            case Direction.Up:
                directionCoor = Vector2Int.up;
                break;
            case Direction.Down:
                directionCoor = Vector2Int.down;
                break;
            case Direction.Left:
                directionCoor = Vector2Int.left;
                break;
            case Direction.Right:
                directionCoor = Vector2Int.right;
                break;
            default:
                directionCoor = Vector2Int.zero;
                break;
        }

        return directionCoor;
    }

    public Direction GetDirectionFromCoor(Vector2Int directionCoor)
    {
        if (directionCoor.x == 1)
        {
            return Direction.Right;
        }
        else if (directionCoor.x == -1)
        {
            return Direction.Left;
        }
        else if (directionCoor.y == 1)
        {
            return Direction.Up;
        }
        else if (directionCoor.y == -1)
        {
            return Direction.Down;
        }
        else
        {
            Debug.LogWarning($"directionCoor이 잘못됐습니다. : {directionCoor}");
            return Direction.None;
        }
    }
    
    /// <summary>
    /// 일직선으로 이동합니다.
    /// </summary>
    /// <param name="directionCoor"></param>
    /// <returns></returns>
    public IEnumerator MoveStraight(Vector2Int directionCoor)
    {
        var waitForFixedUpdate = new WaitForFixedUpdate();
        float progress = Time.fixedDeltaTime;
        Vector3 previousPlayerPosition = transform.position;
        Vector3 nextPlayerPosition = previousPlayerPosition +
                                     (new Vector3(directionCoor.x, 0, directionCoor.y) *
                                      TilePositionSetting.Instance.tileInterval);
        while (progress <= moveTimeInterval)
        {
            transform.position = Vector3.Lerp(previousPlayerPosition, nextPlayerPosition, progress / moveTimeInterval);
            progress += Time.fixedDeltaTime;
            yield return waitForFixedUpdate;
        }

        currentCoor += directionCoor;
    }
    
    /// <summary>
    /// 진행방향으로 이동하다 튕겨서 원래 자리로 돌아옵니다.
    /// </summary>
    /// <param name="directionCoor"></param>
    /// <returns></returns>
    private IEnumerator MoveBounce(Vector2Int directionCoor)
    {
        var waitForFixedUpdate = new WaitForFixedUpdate();
        float progress = Time.fixedDeltaTime;
        Vector3 previousPlayerPosition = transform.position;
        Vector3 nextPlayerPosition = previousPlayerPosition +
                                     (new Vector3(directionCoor.x, 0, directionCoor.y) *
                                      TilePositionSetting.Instance.tileInterval);
        
        while (progress <= moveTimeInterval)
        {
            if (progress <= moveTimeInterval / 2.0f)
            {
                Vector3.Lerp(previousPlayerPosition, nextPlayerPosition, progress / moveTimeInterval);    
            }
            else
            {
                Vector3.Lerp(nextPlayerPosition, previousPlayerPosition, progress / moveTimeInterval);
            }
            progress += Time.fixedDeltaTime;
            yield return waitForFixedUpdate;
        }
    }
}