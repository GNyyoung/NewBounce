using System;
using UnityEngine;

/// <summary>
/// 플레이어 조작을 처리하는 클래스입니다.
/// </summary>
public class Control : MonoBehaviour
{
    private float previousTouchDist = 0;
    private Vector2 initialTouchPosition;
    
    public Player player;
    public CameraMove cameraMove;

    public static bool isControllable = true;
    // 드래그로 조작하는 것은 게임 구동이 된 다음 제작한다.

    private void Update()
    {
        if (isControllable)
        {
            if (Input.anyKeyDown)
            {
                Debug.Log("명령");
                if (Input.GetKeyDown(KeyCode.W))
                    player.StartMove(Direction.Up);
                else if (Input.GetKeyDown(KeyCode.A))
                    player.StartMove(Direction.Left);
                else if(Input.GetKeyDown(KeyCode.S))
                    player.StartMove(Direction.Down);
                else if(Input.GetKeyDown(KeyCode.D))
                    player.StartMove(Direction.Right);
            }
        
            float wheelInput = Input.GetAxis("Mouse ScrollWheel");
            if (wheelInput != 0)
            {
                cameraMove.ZoomCamera(wheelInput);
            }

            CheckTouchMove();
            CheckTouchZoom();   
        }
    }

    public void CheckTouchZoom()
    {
        if (Input.touchCount == 2 &&
            (Input.touches[0].phase == TouchPhase.Moved || Input.touches[1].phase == TouchPhase.Moved))
        {
            float touchDist = (Input.touches[0].position - Input.touches[1].position).magnitude;
            if (previousTouchDist == 0)
            {
                previousTouchDist = touchDist;   
            }
            cameraMove.ZoomCamera((previousTouchDist - touchDist) * 0.01f);
            previousTouchDist = touchDist;
        }
        else if(Input.touchCount == 0)
        {
            previousTouchDist = 0;
        }
    }

    public void CheckTouchMove()
    {
        if (Input.touchCount == 1)
        {
            if (Input.touches[0].phase == TouchPhase.Began)
            {
                initialTouchPosition = Input.touches[0].position;
            }
            else if(Input.touches[0].phase == TouchPhase.Ended)
            {
                var distanceVector = Input.touches[0].position - initialTouchPosition;
                Debug.Log(distanceVector);
                if (Mathf.Abs(distanceVector.x) > Mathf.Abs(distanceVector.y))
                {
                    if (Mathf.Abs(distanceVector.x) >= Screen.width * 0.1f)
                    {
                        player.StartMove(player.GetDirectionFromCoor(
                            new Vector2Int((int)Mathf.Sign(distanceVector.x), 0)));   
                    }
                }
                else
                {
                    if (Mathf.Abs(distanceVector.y) >= Screen.width * 0.1f)
                    {
                        player.StartMove(player.GetDirectionFromCoor(
                            new Vector2Int(0, (int)Mathf.Sign(distanceVector.y))));   
                    }
                }
            }   
        }
    }
}