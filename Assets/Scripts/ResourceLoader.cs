using System.IO;
using MapEditor;
using UnityEngine;

/// <summary>
/// 리소스를 불러올 때 사용하는 클래스.
/// </summary>
public class ResourceLoader
{
    private ResourceLoader(){}
    private static ResourceLoader instance;
    public static ResourceLoader Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new ResourceLoader();
            }

            return instance;
        }
    }

    private const string TilePrefabFolderPath = "Prefabs/Tile";
    private const string ItemPrefabFolderPath = "Prefabs/Item";
    private const string DecoPrefabFolderPath = "Prefabs/Decoration";
    private const string StageFolderPath = "Data/Stage";
    
    public GameObject GetTilePrefab(TileType tileType)
    {
        var tileObject = Resources.Load<GameObject>($"{TilePrefabFolderPath}/{tileType}");
        return tileObject;
    }

    public GameObject GetItemPrefab(ItemType itemType, int value)
    {
        GameObject itemObject;
        if(value == 0)
            itemObject = Resources.Load<GameObject>($"{ItemPrefabFolderPath}/{itemType}");
        else
            itemObject = Resources.Load<GameObject>($"{ItemPrefabFolderPath}/{itemType}{value}");
        
        return itemObject;
    }

    public GameObject GetDecorationPrefab(int decorationID)
    {
        var decorationObject = Resources.Load<GameObject>($"{DecoPrefabFolderPath}/{decorationID}");
        return decorationObject;
    }

    public TextAsset GetStageJsonData(int group, int num)
    {
        var stageJson = Resources.Load<TextAsset>($"{StageFolderPath}/{group}-{num}");
        return stageJson;
    }

    public Sprite GetSprite(string name, string path = "Sprites")
    {
        var sprite = Resources.Load<Sprite>($"{path}/name");
        return sprite;
    }

    public string GetSaveData()
    {
        return File.ReadAllText($"{Application.persistentDataPath}/SaveData.json");
    }
}