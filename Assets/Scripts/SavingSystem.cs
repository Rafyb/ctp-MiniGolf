using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;
using UnityEngine;

public class SavingSystem : MonoBehaviour
{
    #region Fields

    private MainEditor mainEditor;
    private string _fileName ="defaultMap";

    #endregion

    #region UnityInspector

    [SerializeField] private MapInfos mapInfos;
    [SerializeField] private KeyCode manualSaveKey = KeyCode.F5;
    [SerializeField] private KeyCode manuelLoadKey = KeyCode.F9;

    #endregion

    #region Behaviour

    private void Awake()
    {
        mainEditor = GetComponentInParent<MainEditor>();
    }

    private void Update()
    {
        if(Input.GetKeyDown(manualSaveKey))
        {
            Save();
        }
        if(Input.GetKeyDown(manuelLoadKey))
        {
            Load();
        }
    }

    public void ChangeName(string fileName)
    {
        _fileName = fileName;
    }

    public void Save()
    {
 
        Debug.Log("Save");

        List<TileInfos> tiles = new List<TileInfos>();
        foreach (GameObject tile in mainEditor.GetData())
        {
            TileInfos t = new TileInfos(tile);
            tiles.Add(t);
        }
        mapInfos.SaveDatas(_fileName,tiles);

        string json = JsonUtility.ToJson(mapInfos);
        System.IO.File.WriteAllText(Application.dataPath + "/StreamingFiles/Levels/JSON/" + _fileName+ ".json", json);

    }

    public void Load()
    {
        Debug.Log("Load");

        string json = System.IO.File.ReadAllText(Application.dataPath + "/StreamingFiles/Levels/JSON/" + _fileName+ ".json");
        mapInfos = JsonUtility.FromJson<MapInfos>(json);

        mainEditor.LoadData(mapInfos);
        //return mapInfos;
    }

    #endregion
}
