using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace MapEditor
{
    public class SizeGuide : MonoBehaviour
    {
        private void Start()
        {
            gameObject.SetActive(false);
        }

        public void SetWidthGuide(Vector3 dragStartPosition, int width)
        {
            Vector3 widthScreenPosition;
            if (width >= 10)
            {
                int space = (int)((Input.mousePosition.x - Screen.width) / 128);
                widthScreenPosition =
                    Camera.main.WorldToScreenPoint(dragStartPosition + Vector3.right * (width - (space + 5)));
            }
            else if (width <= -10)
            {
                int space = (int)((Input.mousePosition.x - Screen.width) / 128);
                widthScreenPosition =
                    Camera.main.WorldToScreenPoint(dragStartPosition + Vector3.right * (width + (space + 5)));
            }
            else
            {
                widthScreenPosition =
                    Camera.main.WorldToScreenPoint(dragStartPosition + new Vector3(width / 2, 0, 1));
            }

            // 미리보기가 확장되면서 오브젝트가 밖으로 나가는 것을 방지
            float positionX;
            float positionY;

            if(widthScreenPosition.x > Screen.width - gameObject.GetComponent<RectTransform>().rect.width)
            {
                positionX = Screen.width - gameObject.GetComponent<RectTransform>().rect.width;
            }
            else if(widthScreenPosition.x < gameObject.GetComponent<RectTransform>().rect.width)
            {
                positionX = gameObject.GetComponent<RectTransform>().rect.width;
            }
            else
            {
                positionX = widthScreenPosition.x;
            }

            if(widthScreenPosition.y > Screen.height - gameObject.GetComponent<RectTransform>().rect.height)
            {
                positionY = Screen.height - gameObject.GetComponent<RectTransform>().rect.height;
            }
            else if(widthScreenPosition.y < gameObject.GetComponent<RectTransform>().rect.height)
            {
                positionY = gameObject.GetComponent<RectTransform>().rect.height;
            }
            else
            {
                positionY = widthScreenPosition.y;
            }

            gameObject.transform.position = new Vector3(positionX, positionY, widthScreenPosition.z);
            gameObject.transform.Find("Text").GetComponent<Text>().text = (Mathf.Abs(width) + 1).ToString();
        }

        public void SetHeightGuide(Vector3 dragStartPosition, int height)
        {
            // 세로 크기 가이드를 어느 위치에 표시할지 계산
            Vector3 heightScreenPosition;
            if (height >= 6)
            {
                // 현재 미리보기 사이즈보다 3칸 앞에 가이드가 붙게 함.
                // 마우스 위치 구해서 화면에서 멀리 떨어질 수록 미리보기가 더 앞에 붙도록 해야 할듯.
                int space = (int)((Input.mousePosition.x - Screen.height) / 128);
                heightScreenPosition =
                    Camera.main.WorldToScreenPoint(dragStartPosition + Vector3.forward * (height - (space + 3)));
            }
            else if (height <= -6)
            {
                int space = (int)((Input.mousePosition.x - Screen.height) / 128);
                heightScreenPosition =
                    Camera.main.WorldToScreenPoint(dragStartPosition + Vector3.forward * (height + (space + 3)));
            }
            else
            {
                heightScreenPosition =
                    Camera.main.WorldToScreenPoint(dragStartPosition + new Vector3(-1, 0, height / 2));
            }

            // 미리보기가 확장되면서 오브젝트가 밖으로 나가는 것을 방지
            float positionX;
            float positionY;
            if(heightScreenPosition.x > Screen.width - gameObject.GetComponent<RectTransform>().rect.width)
            {
                positionX = Screen.width - gameObject.GetComponent<RectTransform>().rect.width;
            }
            else if(heightScreenPosition.x < gameObject.GetComponent<RectTransform>().rect.width)
            {
                
                positionX = gameObject.GetComponent<RectTransform>().rect.width;
            }
            else
            {
                positionX = heightScreenPosition.x;
            }
            
            if(heightScreenPosition.y > Screen.height - gameObject.GetComponent<RectTransform>().rect.height)
            {
                positionY = Screen.height - gameObject.GetComponent<RectTransform>().rect.height;
            }
            else if(heightScreenPosition.y < gameObject.GetComponent<RectTransform>().rect.height)
            {
                positionY = gameObject.GetComponent<RectTransform>().rect.height;
            }
            else
            {
                positionY = heightScreenPosition.y;
            }

            gameObject.transform.position = new Vector3(positionX, positionY, heightScreenPosition.z);
            gameObject.transform.Find("Text").GetComponent<Text>().text = (Mathf.Abs(height) + 1).ToString();
        }
    }
}

