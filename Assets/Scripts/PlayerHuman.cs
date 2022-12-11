using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(BoxCollider))]
public class PlayerHuman : MonoBehaviour
{
    private Rigidbody _rigidbody;
    private Animator _animator;

    private Vector3 _distans;

    private bool _isMove = false;

    [SerializeField] private ParticleSystem _particleSystem;
    [SerializeField] private float _speed;
    [SerializeField] private float _speedRotation;
    [SerializeField] private Vector3 _forceJump;


    private void Awake()
    {
        _particleSystem = GetComponentInChildren<ParticleSystem>();
        _particleSystem.Stop();

        _rigidbody = GetComponent<Rigidbody>();
        _animator = GetComponent<Animator>();


    }

    void Update()
    {
        _distans = new Vector3(Input.GetAxis("Horizontal"), 0f, Input.GetAxis("Vertical"));

        if (_distans.x != 0f || _distans.z != 0f)
        {
            _isMove = true;
            _animator.SetBool("isFast", _isMove);            
        }
        else
        {
            _isMove = false;
            _animator.SetBool("isFast", _isMove);
        }


        if (Input.GetButtonDown("Jump"))       
            _animator.Play("Jump");
        

        if (Input.GetButtonDown("Fire1"))
            _animator.Play("Kick");

        
         
    }

    private void FixedUpdate()
    {
        MovementHuman(_distans);
        MoveJump(_forceJump);
        
    }

    // Работа ParticleSystem

    private void OnCollisionStay(Collision collision)
    {
       
        if (collision.gameObject.layer == 3)
        {
            if (_distans.x != 0f || _distans.z != 0f)
            {

                var shape = _particleSystem.shape;
                shape.rotation = Quaternion.LookRotation(_distans.normalized).eulerAngles;
                _particleSystem.Play();

            }
            else
            {
                _particleSystem.Stop();
            }
        }
        else
        {
            _particleSystem.Stop();
        }
    }

    // Движение
    private void MovementHuman(Vector3 distans)
    {
        var forwardDistans = distans * _speed * Time.deltaTime;
        transform.position += forwardDistans;
        _rigidbody.MovePosition(transform.position);

        if(distans.magnitude > Mathf.Abs(0.05f))
        _rigidbody.MoveRotation(Quaternion.Lerp(transform.rotation ,Quaternion.LookRotation(distans),_speedRotation*Time.deltaTime));
     
    }
    // Прыжек
    private void MoveJump(Vector3 forceJump)
    {
        if(Input.GetButtonDown("Jump"))
          _rigidbody.AddForce(forceJump);          
    }    
}
