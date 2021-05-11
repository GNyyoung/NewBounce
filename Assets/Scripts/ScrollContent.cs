using System;
using UnityEngine;
using UnityEngine.UI;

public class ScrollContent : MonoBehaviour
{
    private int contentsNum = 0;
    GameObject topObject = null;
    GameObject bottomObject = null;

    public int TopPadding = 512;
    public int BottomPadding = 512;

    private void Start()
    {
        // contentsNum = transform.childCount;
        // SetObjectHeight();
    }

    private void Update()
    {
        if (transform.childCount != contentsNum)
        {
            contentsNum = transform.childCount;
            SetObjectHeight();
        }
    }

    private void SetObjectHeight()
    {
        float topObjectTopPosY = 0;
        float bottomObjectBottomPosY = 0;
        for (int i = 0; i < contentsNum; i++)
        {
            if (topObject == null)
            {
                topObject = transform.GetChild(i).gameObject;
                topObjectTopPosY = topObject.GetComponent<RectTransform>().anchoredPosition.y +
                                   topObject.GetComponent<RectTransform>().rect.height / 2.0f;
                Debug.Log(topObject.GetComponent<RectTransform>().rect.y + ", " + topObject.GetComponent<RectTransform>().rect.height / 2.0f);
            }
            else
            {
                var childTopPosY = transform.GetChild(i).GetComponent<RectTransform>().anchoredPosition.y +
                                   transform.GetChild(i).GetComponent<RectTransform>().rect.height / 2.0f;
                if (childTopPosY > topObjectTopPosY)
                {
                    topObject = transform.GetChild(i).gameObject;
                    topObjectTopPosY = childTopPosY;
                }
            }

            if (bottomObject == null)
            {
                bottomObject = transform.GetChild(i).gameObject;
                bottomObjectBottomPosY = bottomObject.GetComponent<RectTransform>().anchoredPosition.y -
                                         bottomObject.GetComponent<RectTransform>().rect.height / 2.0f;
            }
            else
            {
                var childBottomPosY = transform.GetChild(i).GetComponent<RectTransform>().anchoredPosition.y -
                                      transform.GetChild(i).GetComponent<RectTransform>().rect.height / 2.0f;
                if (childBottomPosY < bottomObjectBottomPosY)
                {
                    bottomObject = transform.GetChild(i).gameObject;
                    bottomObjectBottomPosY = childBottomPosY;
                }
            }
        }

        Debug.Log(topObjectTopPosY);
        Debug.Log(bottomObjectBottomPosY);
        if (topObjectTopPosY < 0)
        {
            GetComponent<RectTransform>().sizeDelta = new Vector2(
                GetComponent<RectTransform>().sizeDelta.x, -bottomObjectBottomPosY + BottomPadding);
        }
        else
        {
            GetComponent<RectTransform>().sizeDelta = new Vector2(
                GetComponent<RectTransform>().sizeDelta.x,topObjectTopPosY - bottomObjectBottomPosY + BottomPadding);
        }
        
    }
}