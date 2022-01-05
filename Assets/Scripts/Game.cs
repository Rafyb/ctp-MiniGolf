using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Game : MonoBehaviour
{
    [Header("Setup")]
    public GameObject PlayerBall;
    public GameObject StartPos;
    public List<string> Levels;
    
    [Header("Controls")]
    public float MouseSpeedX;
    public float MouseSpeedY;
    public float ZoomSpeed;
    
    private Vector3 _offset;
    private Transform _resetCam;
    private int _hits;
    private int _idx;
    private bool _active;

    void Start()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        StartPos.transform.position += new Vector3(0f, 0.03f, 0f);
        
        PlayerBall = Instantiate(PlayerBall,StartPos.transform.position,Quaternion.identity);
        PlayerBall.GetComponent<Ball>().game = this;
        
        _offset = new Vector3(PlayerBall.transform.position.x, PlayerBall.transform.position.y + 1.5f, PlayerBall.transform.position.z + 6.0f);

        _idx = 0;
    }


    void LoadLevel()
    {
        string levelName = Levels[_idx];
        
        PlayerBall.transform.position = StartPos.transform.position;
        
        _hits = 0;
    }
    


    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Camera.main.transform.position = _resetCam.position;
            Camera.main.transform.rotation = _resetCam.rotation;
        }

        Ball ball = PlayerBall.GetComponent<Ball>();
        if (Input.GetMouseButtonDown(0) && !ball.IsMoving())
        {
            Vector3 dir = PlayerBall.transform.position - Camera.main.transform.position;
            dir.y = 0;
            _active = true;
            ball.StartJauge(dir.normalized);
        }
        if (Input.GetMouseButton(0)) ball.UpdateForce(Input.GetAxis("Mouse Y") * MouseSpeedY);
        if (Input.GetMouseButtonUp(0) && _active)
        {
            ball.LaunchBall();
            _active = false;
        }
    }

    void LateUpdate()
    {
        
        // Zoom
        float zoom = Camera.main.fieldOfView;
        zoom += Input.GetAxis("Mouse ScrollWheel") * ZoomSpeed;
        Camera.main.fieldOfView = zoom;

        // Rotate
        //if (Input.GetMouseButton(0)) return;
        //if (Camera.main.transform.position.y + Input.GetAxis("Mouse Y") <= 0.5) return;
            
        if (!Input.GetMouseButton(0)) _offset = Quaternion.AngleAxis (Input.GetAxis("Mouse Y") * MouseSpeedY, Vector3.right) * _offset;
        _offset = Quaternion.AngleAxis (Input.GetAxis("Mouse X") * MouseSpeedX, Vector3.up) * _offset;
        
        Camera.main.transform.position = PlayerBall.transform.position + _offset;
        if (Camera.main.transform.position.y <= 0.5)
        {
            Vector3 pos = Camera.main.transform.position;
            pos.y = 0.5f;
            Camera.main.transform.position = pos;
        }
        Camera.main.transform.LookAt(PlayerBall.transform.position);
        
        if(_resetCam == null) _resetCam = Camera.main.transform;
    }

    public void AddHit()
    {
        _hits++;
        UI.Instance.ShowHit(_hits);
    }
}
