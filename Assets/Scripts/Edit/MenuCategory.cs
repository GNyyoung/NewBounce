using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

namespace MapEditor
{
    /// <summary>
    /// 메뉴 카테고리 전체를 관리하는 클래스
    /// </summary>
    public class MenuCategory : MonoBehaviour
    {
        private MenuCategory(){}
        private static MenuCategory _instance;

        public static MenuCategory Instance
        {
            get
            {
                if (_instance == null)
                {
                    var instances = FindObjectsOfType<MenuCategory>();
                    if (instances.Length == 0)
                    {
                        Debug.LogError("MenuCategory 스크립트가 존재하지 않습니다. CategoryPanel에 추가하십시오.");
                        var newInstance = GameObject.Find("Canvas").AddComponent<MenuCategory>();
                        _instance = newInstance;
                    }
                    else if (instances.Length >= 1)
                    {
                        Debug.LogError("MenuCategory 스크립트가 여러 개 존재합니다. CategoryPanel에만 존재해야 합니다.");
                        for (int i = 1; i > instances.Length; i++)
                        {
                            Destroy(instances[i]);
                        }
                
                        _instance = instances[0];
                    }
                }

                return _instance;
            }
        }
        
        [SerializeField] private CategoryView[] CategoryViews;
        [SerializeField] private ScrollRect categoryScrollRect;
        public Dictionary<string, CategoryView> viewNameDic = new Dictionary<string, CategoryView>();
        private CategoryView openedView;

        private void Start()
        {
            foreach (var view in CategoryViews)
            {
                view.Initialize();
                view.gameObject.SetActive(false);
                viewNameDic.Add(view.gameObject.name, view);
            }
        }

        public void Open(string viewName)
        {
            if (viewNameDic.TryGetValue(viewName, out var view) == true)
            {
                if (view.Equals(openedView) == false)
                {
                    if (openedView != null)
                    {
                        openedView.Hide();
                    }
                    
                    view.Show();
                    openedView = view;
                    categoryScrollRect.content = view.GetComponent<RectTransform>();
                }
            }
            else
            {
                Debug.LogWarning($"잘못된 카테고리 이름입니다. : {viewName}");
            }
        }
    }
}