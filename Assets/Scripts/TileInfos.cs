using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class TileInfos
{
    public int X;
    public int Y;
    public int Z;
    public int ID;
    public int Rotate;

    public TileInfos(GameObject tile)
    {
        ID = int.Parse(tile.name);
        X = Mathf.CeilToInt(tile.transform.position.x);
        Y = Mathf.CeilToInt(tile.transform.position.y);
        Z = Mathf.CeilToInt(tile.transform.position.z);
        Rotate = Mathf.CeilToInt(tile.transform.rotation.y);
    }
}
