using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMove : MonoBehaviour
{
    private float verticalSpace = 0.75f;
    
    public GameObject playerObject;
    public StageManager stageManager;
    public float minZoom = 25;
    public float maxZoom;

    public void MoveCameraCoordinate(int bounceCount, Vector2Int playerCoor, Vector2Int directionCoor)
    {
        var thisTransform = transform;
        Debug.Log(directionCoor);

        int num = 0;
        while (num < bounceCount)
        {
            if (stageManager.GetTile(playerCoor + directionCoor * (num + 1)) == null)
            {
                break;
            }
            
            num += 1;
        }


        Vector2 revisedCameraCoor;
        if (stageManager.GetTile(playerCoor + directionCoor * bounceCount) != null)
        {
            revisedCameraCoor = playerCoor + bounceCount * (Vector2)directionCoor / 2.0f;
        }
        else
        {
            revisedCameraCoor = playerCoor + bounceCount * (Vector2) directionCoor / 2.0f -
                           directionCoor * (bounceCount - num);
        }
        
        revisedCameraCoor.x = directionCoor.x == 0 ? thisTransform.position.x : revisedCameraCoor.x;
        revisedCameraCoor.y = directionCoor.y == 0 ? thisTransform.position.z : revisedCameraCoor.y;
        thisTransform.position = new Vector3(revisedCameraCoor.x, thisTransform.position.y, revisedCameraCoor.y);
    }

    public void SetInitialPosition(Vector2Int playerCoor, int bounceCount)
    {
        var mapSize = stageManager.GetMapSize();
        float posX;
        float posY;
        if (playerCoor.x < mapSize.x / 2.0f)
        {
            posX = playerCoor.x + bounceCount / 2.0f;
        }
        else
        {
            posX = playerCoor.x - bounceCount / 2.0f;
        }

        if (playerCoor.y < mapSize.y / 2.0f)
        {
            posY = playerCoor.y + bounceCount / 2.0f;
        }
        else
        {
            posY = playerCoor.y - bounceCount / 2.0f;
        }
        
        transform.position = new Vector3(posX, transform.position.y, posY);
    }

    public void SetInitialPosition(int width, int height)
    {
        transform.position = new Vector3((width - 1) / 2.0f, transform.position.y, (height / verticalSpace - 1) / 2.0f - 1);
    }

    public void SetCameraSize(int width, int height)
    {
        float cameraSize = 1;
        float ScreenAspect = (float)Screen.height / Screen.width;
        if (height + 2 > (width + 2) * ScreenAspect)
        {
            Debug.Log("세로");
            cameraSize = height / verticalSpace / 2;
        }
        else
        {
            Debug.Log("가로");
            cameraSize = (width + 2) * (float)Screen.height / Screen.width / 2;
        }
        
        GetComponent<Camera>().orthographicSize = cameraSize;
        maxZoom = cameraSize;
        minZoom = maxZoom * 2;
    }

    // 카메라 줌인, 줌아웃에 사용
    public void ZoomCamera(float rate)
    {
        var thisCamera = GetComponent<Camera>(); 
        // thisCamera.orthographicSize += rate;

        Debug.Log($"rate : {rate}");
        thisCamera.orthographicSize = Mathf.Clamp(thisCamera.orthographicSize + rate, maxZoom, minZoom);
    }
}
