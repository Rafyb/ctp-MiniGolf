using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class MapInfos
{
    public string Name;
    public string[] Tiles;
    
    public void SaveDatas(string name, List<TileInfos> saveTab)
    {
        Name = name;
        
        Tiles = new string[saveTab.Count];
        for (int i = 0; i < saveTab.Count; i++)
        {
            Tiles[i] = JsonUtility.ToJson(saveTab[i]);
            
        }
    }
}
