using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Schema;
using UnityEngine;

public class Ball : MonoBehaviour
{
    public GameObject Preview;
    public Color LowShot;
    public Color NormalShot;
    public Color StrongShot;

    [HideInInspector] public Game game;

    public float MaxStrength;
    public float MinStength;

    private float _force;
    private Vector3 _dir;
    private Rigidbody _rigidbody;
    private Vector3 _previousShot;
    private bool _isMoving;
    void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _isMoving = false;
        _rigidbody.isKinematic = true;
        
        
    }
    
    void Update()
    {
        if (_dir != null)
        {
            
            Vector3 scale = Preview.transform.localScale;
            Vector3 pos = new Vector3(0f, 0f, 0f);
            scale.z = _force / 1000;
            pos.z = (scale.z*10)/2;
            Preview.transform.localScale = scale;
            Preview.transform.localPosition = pos;
            

            Renderer rd = Preview.GetComponent<Renderer>();
            if (_force > 60) rd.material.color = StrongShot;
            else if(_force < 20) rd.material.color = LowShot;
            else rd.material.color = NormalShot;
        }
        

        if (_rigidbody.velocity.magnitude < 0.02 && _rigidbody.velocity.magnitude > 0) StartCoroutine(KillVelocity());
        /*_rigidbody.velocity *= 0.6f;
        _rigidbody.angularVelocity *= 0.6f;*/

        if (transform.position.y < -1) Respawn();

    }

    IEnumerator KillVelocity()
    {
        yield return new WaitForSeconds(0.3f);
        if (_rigidbody.velocity.magnitude < 0.02)
        {
            _rigidbody.velocity = Vector3.zero;
            _rigidbody.isKinematic = true;
            _isMoving = false;
        }
    }

    

    public void StartJauge(Vector3 dir)
    {
        Debug.Log(_rigidbody);
        
        Preview.SetActive(true);
        _dir = dir;
        transform.LookAt(transform.position + _dir + Vector3.up);
        _rigidbody.isKinematic = true;
        
    }

    public void UpdateForce(float f)
    {
        _force += f;
        if (_force < 0) _force = 0;

        if (_force > MaxStrength) _force = MaxStrength;

        //Debug.Log(_force);
    }

    public void LaunchBall()
    {
        _rigidbody.isKinematic = false;
        Preview.SetActive(false);
        
        if (_force < MinStength) return;

        _previousShot = transform.position;
        _rigidbody.AddForce(_dir * _force * 8);
        _isMoving = true;
        _force = 0;

        game.AddHit();
    }

    public void Respawn()
    {
        _rigidbody.velocity = Vector3.zero;
        _rigidbody.isKinematic = true;
        transform.position = _previousShot;
    }

    public bool IsMoving()
    {
        return _isMoving;
    }
}
