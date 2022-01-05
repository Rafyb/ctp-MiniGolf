using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

[Serializable]
public struct Tile {
    public int ID;
    public GameObject Prefab;
}


public class MainEditor : MonoBehaviour
{
    private List<GameObject> _tiles;
    private GameObject _preview;
    private Vector3 _pos;
    private float _angle;
    private int _idx;
    
    [Header("Preview")]
    public List<Tile> TilesPrefab;
    public Color Normal;
    public Color Delete;
    
    [Header("Camera")]
    public float minCam;
    public float maxCam;
    public float sensitivity;
    

    void Start()
    {
        Cursor.visible = true;
        //Cursor.lockState = CursorLockMode.Locked;

        _pos = new Vector3(0f, 0.2f, 0f);

        _tiles = new List<GameObject>();
        
        GameObject go = Instantiate(TilesPrefab[_idx].Prefab, Vector3.zero, Quaternion.identity);
        go.name = ""+_idx;
        _tiles.Add(go);

        _idx = 1;
        _preview = Instantiate(TilesPrefab[_idx].Prefab, Vector3.zero, Quaternion.identity);
    }
  

    public List<GameObject> GetData()
    {
        return _tiles;
    }
    
    public void LoadData(MapInfos map)   
    {
        for (int i = _tiles.Count-1; i >= 0; i--)
        {
            Destroy(_tiles[i]);
        }
        _tiles.Clear();

        for (int i = 0; i < map.Tiles.Length; i++)
        {
            TileInfos tile = JsonUtility.FromJson<TileInfos>(map.Tiles[i]);

            GameObject go = Instantiate(_tiles[tile.ID], new Vector3(tile.X, tile.Y, tile.Z), Quaternion.identity);
            Vector3 rot = go.transform.eulerAngles;
            rot.y = tile.Rotate;
            go.transform.eulerAngles = rot;
            go.name = ""+tile.ID;
            
            _tiles.Add(go);
        }
    }



    // Update is called once per frame
    void Update()
    {
        // Controls
        if (Input.GetKeyDown(KeyCode.LeftArrow)) _pos.x--;
        if (Input.GetKeyDown(KeyCode.RightArrow)) _pos.x++;
        if (Input.GetKeyDown(KeyCode.UpArrow)) _pos.z++;
        if (Input.GetKeyDown(KeyCode.DownArrow)) _pos.z--;
        if (Input.GetKeyDown(KeyCode.Space)) TryPut();
        if (Input.GetKeyDown(KeyCode.R)) Rotate();
        if (Input.GetKeyDown(KeyCode.D)) DeleteTile();
        
        // Move
        _preview.transform.position = _pos;
        
        // Rotate
        Vector3 rot = _preview.transform.eulerAngles;
        rot.y = _angle;
        _preview.transform.eulerAngles = rot;

        // Change color
        if (TileExsit((int)_pos.x,(int)_pos.z)){
            foreach (Material mat in _preview.GetComponent<MeshRenderer>().materials)
            {
                mat.color = Delete;
            }
        }else {
            foreach (Material mat in _preview.GetComponent<MeshRenderer>().materials)
            {
                mat.color = Normal;
            }
        }
        
        // Camera
        float size = Camera.main.orthographicSize;
        size += Input.GetAxis("Mouse ScrollWheel") * sensitivity;
        size = Mathf.Clamp(size, minCam, maxCam);
        Camera.main.orthographicSize = size;
        Camera.main.transform.position = _pos;

    }
    
    private bool TileExsit(int posX, int posZ)
    {
        for (int i = 0; i < _tiles.Count; i++)
        {
            if (posX == Mathf.CeilToInt(_tiles[i].transform.position.x) &&
                posZ == Mathf.CeilToInt(_tiles[i].transform.position.z)) return true;
        }

        return false;
    }

    public void Rotate()
    {
        _angle = (_angle+90)%360;
    }

    public void TryPut()
    {
        if( !(Mathf.CeilToInt(_pos.x) == 0 && Mathf.CeilToInt(_pos.z) == 0))
        {
            if (TileExsit((int) _pos.x, (int) _pos.z))
            {
                DeleteTile();
            }

            PutTile();
        }
    }

    public void DeleteTile()
    {
        if (Mathf.CeilToInt(_pos.x) == 0 && Mathf.CeilToInt(_pos.z) == 0)  return;
        
        for (int i = 0; i < _tiles.Count; i++)
        {
            if (Mathf.CeilToInt(_pos.x) == Mathf.CeilToInt(_tiles[i].transform.position.x) &&
                Mathf.CeilToInt(_pos.z) == Mathf.CeilToInt(_tiles[i].transform.position.z))
            {
                GameObject tmp = _tiles[i];
                _tiles.RemoveAt(i);
                Destroy(tmp);
            }
        }

    }

    private void PutTile()
    {
        Vector3 pos = _preview.transform.position;
        pos.y -= 0.2f;

        GameObject go = Instantiate(TilesPrefab[_idx].Prefab, pos, _preview.transform.rotation);
        go.name = ""+_idx;
        _tiles.Add(go);
    }

    public void Clear()
    {
        
    }

    public void CameraRecenter()
    {
        Camera.main.transform.position = new Vector3(0f,0f,0f);
    }

    public void ChangeIdx(int idx)
    {
        Destroy(_preview);
        _idx = idx;
        _preview = Instantiate(TilesPrefab[_idx].Prefab, _pos, Quaternion.identity);
    }
  



   
    
}
