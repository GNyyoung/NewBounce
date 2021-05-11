using UnityEngine;
using UnityEngine.UI;

namespace MapEditor
{
    public class TileCreateView : CategoryView
    {
        public GameObject buttonPrefab;
        public override void Initialize()
        {
            var tilePrefabs = Resources.LoadAll<GameObject>("Prefabs/Edit/Tile");
            foreach (var tilePrefab in tilePrefabs)
            {
                var buttonObject = Instantiate(buttonPrefab, this.transform);
                var buttonImage = buttonObject.GetComponent<Image>();
                var tileImage = tilePrefab.GetComponent<SpriteRenderer>();
                buttonImage.sprite = tileImage.sprite;
                buttonImage.color = tileImage.color;
                var clickedEvent = new Button.ButtonClickedEvent();
                clickedEvent.AddListener(() =>
                {
                    editorManager.objectPrefab = tilePrefab;
                    editorManager.currentMode = EditorMode.Create;
                    InfoPanel.Instance.ShowCreatePanel(tilePrefab.GetComponent<EditorTile>().tileType.ToString());
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