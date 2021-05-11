using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MapEditor
{
    public class EditorCamera : MonoBehaviour
    {
        public float minSize { get; private set; }
        public float maxSize { get; private set; }
        float speedWeight = 1;
        public Camera mainCamera;
        public EditorKeyboard editorKeyboard;

        // Start is called before the first frame update
        void Start()
        {
            minSize = mainCamera.orthographicSize;
            maxSize = 25;
        }

        // Update is called once per frame
        void Update()
        {
            float wheelInput = Input.GetAxis("Mouse ScrollWheel");
            if (wheelInput != 0)
            {
                mainCamera.orthographicSize += wheelInput * editorKeyboard.accelerateValue;
                if (mainCamera.orthographicSize < minSize)
                {
                    mainCamera.orthographicSize = minSize;
                }
                else if(mainCamera.orthographicSize > maxSize)
                {
                    mainCamera.orthographicSize = maxSize;
                }
            }
        }
    }
}

