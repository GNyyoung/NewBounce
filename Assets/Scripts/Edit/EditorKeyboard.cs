using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MapEditor
{
    public class EditorKeyboard : MonoBehaviour
{
    public EditorManager editorManager;
    public Player player { private get; set; }
    public Camera mainCamera;
    public float accelerateValue = 1;

    delegate void KeyDelegate();
    Dictionary<KeyCode, KeyDelegate> keyDictionary;
    

    // Start is called before the first frame update
    void Start()
    {
        keyDictionary = new Dictionary<KeyCode, KeyDelegate>
        {
            {KeyCode.W, KeyDown_CameraMoveUp },
            {KeyCode.A, KeyDown_CameraMoveLeft },
            {KeyCode.S, KeyDown_CameraMoveDown },
            {KeyCode.D, KeyDown_CameraMoveRight },
            // {KeyCode.UpArrow, KeyDown_PlayerMoveUp },
            // {KeyCode.LeftArrow, KeyDown_PlayerMoveLeft },
            // {KeyCode.DownArrow, KeyDown_PlayerMoveDown },
            // {KeyCode.RightArrow, KeyDown_PlayerMoveRight },
            // {KeyCode.Space, KeyDown_MoveToPlayerPosition },
            {KeyCode.Tab, KeyDown_ResetCameraSize },
            // {KeyCode.Escape, KeyDown_Escape },
            {KeyCode.LeftShift, KeyDown_Acceleration },
            //{KeyCode.Semicolon, KeyDown_Debug }
            {KeyCode.M, () => editorManager.currentMode = EditorMode.Move},
            {KeyCode.N, () => editorManager.currentMode = EditorMode.Normal}
        };
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.anyKey == true)
        {
            foreach(var dic in keyDictionary)
            {
                if(Input.GetKey(dic.Key) == true)
                {
                    dic.Value();
                }
            }
        }
        else if (Input.GetKeyUp(KeyCode.LeftShift) == true)
        {
            Debug.Log("업");
            KeyUp_Acceleration();
        }
    }

    public void SetPlayer()
    {
        player = FindObjectOfType<Player>();
    }

    public void KeyDown_ResetCameraSize()
    {
        mainCamera.orthographicSize = mainCamera.GetComponent<EditorCamera>().minSize;
    }

    public void KeyDown_Acceleration()
    {
        accelerateValue = 3;
    }

    public void KeyDown_CameraMoveUp()
    {
        mainCamera.transform.position += Vector3.forward * 10 * Time.deltaTime * accelerateValue;
    }

    public void KeyDown_CameraMoveDown()
    {
        mainCamera.transform.position += Vector3.back * 10 * Time.deltaTime * accelerateValue;
    }

    public void KeyDown_CameraMoveLeft()
    {
        mainCamera.transform.position += Vector3.left * 10 * Time.deltaTime * accelerateValue;
    }

    public void KeyDown_CameraMoveRight()
    {
        mainCamera.transform.position += Vector3.right * 10 * Time.deltaTime * accelerateValue;
    }

    public void KeyUp_Acceleration()
    {
        accelerateValue = 1;
    }
}
}


