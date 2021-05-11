using UnityEngine;
using UnityEngine.UI;

namespace MapEditor
{
    public class ItemCreateView : CategoryView
    {
        public GameObject buttonPrefab;
        public override void Initialize()
        {
            var itemPrefabs = Resources.LoadAll<GameObject>("Prefabs/Edit/Item");
            foreach (var itemPrefab in itemPrefabs)
            {
                var buttonObject = Instantiate(buttonPrefab, this.transform);
                buttonObject.GetComponent<Image>().sprite = itemPrefab.GetComponent<SpriteRenderer>().sprite;
                var clickedEvent = new Button.ButtonClickedEvent();
                clickedEvent.AddListener(() =>
                {
                    editorManager.objectPrefab = itemPrefab;
                    editorManager.currentMode = EditorMode.Create;
                    InfoPanel.Instance.ShowCreatePanel(itemPrefab.GetComponent<EditorItem>().itemType.ToString());
                });
                buttonObject.GetComponent<Button>().onClick = clickedEvent;
            }
            gameObject.SetActive(false);
        }

        public override void Show()
        {
            gameObject.SetActive(true);
        }

        public override void Hide()
        {
            gameObject.SetActive(false);
        }
    }
}