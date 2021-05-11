using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMove : MonoBehaviour
{
    public GameObject playerObject;
    public StageManager stageManager;
    public float minZoom = 25;
    public float maxZoom;
    
    private void Update()
    {
        // var playerPosition = playerObject.transform.position;
        // transform.position = new Vector3(
        //     playerPosition.x, 
        //     transform.position.y, 
        //     playerPosition.z - (transform.position.y / Mathf.Tan(transform.localEulerAngles.x * Mathf.Deg2Rad)));
    }

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

    public void SetCameraSize(int bounceCount)
    {
        var cameraSize = (bounceCount + 3) * (float)Screen.height / Screen.width / 2;
        GetComponent<Camera>().orthographicSize = cameraSize;
        maxZoom = cameraSize;
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
