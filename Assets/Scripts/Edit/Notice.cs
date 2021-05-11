using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Notice : MonoBehaviour
{
    private Notice() { }
    private static Notice _instance;

    public static Notice Instance
    {
        get
        {
            if(_instance == null)
            {
                _instance = FindObjectOfType<Notice>();

                if (_instance == null)
                {
                    
                    _instance = GameObject.Find("Edit").transform.Find("Canvas").Find("NoticePanel")
                        .gameObject.AddComponent<Notice>();
                }
            }

            return _instance;
        }
    }

    Coroutine currentCoroutine;
    bool isTextChange;
    const float showTime = 3.0f;

    public enum NoticeType { SaveComplete, SaveNameOverlapError, SaveTileTypeNoneError, MaxDragRange }

    public void ShowNotice(NoticeType noticeType)
    {
        string noticeText = null;

        switch (noticeType)
        {
            case NoticeType.SaveComplete:
                noticeText = "저장 완료";
                break;
            case NoticeType.SaveNameOverlapError:
                noticeText = "이름이 겹치는 파일이 있습니다.";
                break;
            case NoticeType.SaveTileTypeNoneError:
                noticeText = "타입이 설정되지 않은 타일이 있습니다.";
                break;
            case NoticeType.MaxDragRange:
                noticeText = "드래그 최대 한도에 도달했습니다.";
                break;
        }

        gameObject.SetActive(true);


        if(currentCoroutine != null)
        {
            transform.Find("Notice").GetComponent<Text>().text = noticeText;
            isTextChange = true;
        }
        else
        {
            currentCoroutine = StartCoroutine(ActivePanel(noticeText));
        }
    }

    private IEnumerator ActivePanel(string text)
    {
        transform.Find("Notice").GetComponent<Text>().text = text;

        yield return new WaitForSeconds(3.0f);

        int showFrame = (int)(showTime / Time.fixedDeltaTime);
        int currentFrame = 0;
        while(currentFrame < showFrame)
        {
            if(isTextChange == true)
            {
                showFrame = currentFrame + (int)(showTime / Time.fixedDeltaTime);
                isTextChange = false;
            }
            currentFrame++;
        }

        currentCoroutine = null;
        gameObject.SetActive(false);
    }

    public void CloseNotice()
    {
        if(currentCoroutine != null)
        {
            StopCoroutine(currentCoroutine);
            currentCoroutine = null;
            gameObject.SetActive(false);
        }
    }
}
