using System;
using UnityEngine;

/// <summary>
/// 각 타일 간 간격을 설정합니다.
/// </summary>
public class TilePositionSetting : MonoBehaviour
{
    private TilePositionSetting(){}
    private static TilePositionSetting _instance;

    public static TilePositionSetting Instance
    {
        get
        {
            if (_instance == null)
            {
                var instances = FindObjectsOfType<TilePositionSetting>();
                if (instances.Length == 0)
                {
                    var newInstance = GameObject.Find("Manager").AddComponent<TilePositionSetting>();
                    _instance = newInstance;
                }
                else if (instances.Length >= 1)
                {
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

    public float tileInterval;
}