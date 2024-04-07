using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;
[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(Animator))]
public class Cow : MonoBehaviour
{
    [SerializeField] private float _jumpPower; 
    private Animator _animator;
    [SerializeField] private GameObject _deadCowPrefab;
    private Rigidbody _rigidbody;
    private Transform _transform;

    private float _jumpTimer = 1f;
   [SerializeField] private float _maxJumpTime = 1f;
   [SerializeField] private float _minJumpTime = 2f;
   private bool _catched = false;

   public void Catched()
   {
       _catched = true;
       _animator.SetBool("Fly",true);
       _rigidbody.isKinematic = true;
   }

   private void Awake()
   {
       _animator = GetComponent<Animator>();
       _rigidbody = GetComponent<Rigidbody>();
       _transform = transform;
   }

   private void Update()
    {
        if (!_catched)
        {
            CheckJump();
        }
    }

   private void CheckJump()
   {
       if (_jumpTimer > 0)
       {
           _jumpTimer -= Time.deltaTime;
           if (_jumpTimer < 0)
           {
               Jump();
               _jumpTimer = Random.Range(_minJumpTime, _maxJumpTime);
           }
       } 
   }
    private void Jump()
    {
        _animator.SetTrigger("Jump");
        _rigidbody.velocity = (Vector3.up + _transform.forward) * _jumpPower;
    }

    private void OnCollisionEnter(Collision other)
    {
        if (_catched == true)
        {
            return;
        }
        var attachedRigidbody = other.collider.attachedRigidbody;
        if (attachedRigidbody == null)
        {
            return;
        }
        if (attachedRigidbody.GetComponent<Player>() != null)
        {
            Instantiate(_deadCowPrefab, _transform.position, _transform.rotation);
            Destroy(gameObject);
        } 
    }
}
