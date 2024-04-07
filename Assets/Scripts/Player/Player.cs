using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.SceneManagement;
using UnityEngine;
[RequireComponent(typeof(PlayerInput))]
[RequireComponent(typeof(Rigidbody))]
public class Player : MonoBehaviour
{
   private PlayerInput _playerInput;
   private Rigidbody _rigidbody;
   private Transform _transform;

   private ConstantForce _constantForce;

   [SerializeField] private Engine _engine;
   [SerializeField] private float _constantForcePower;
   [SerializeField] private CowCatcher _catcher;

   private void Awake()
   {
      _transform = transform;
      _rigidbody = GetComponent<Rigidbody>();
      _playerInput = GetComponent<PlayerInput>();
      _constantForce = GetComponent<ConstantForce>();
      _catcher.SetInput(_playerInput);
      _engine.Initialize(_rigidbody);
   }

   private void FixedUpdate()
   {
       _constantForce.force = -Vector3.right * _playerInput.Controls.x * _constantForcePower + Physics.gravity * _rigidbody.mass;
   }

   private void Update()
   {
       var isVerticalAxisActive = !Mathf.Approximately(_playerInput.Controls.y, 0); 
       if (isVerticalAxisActive)
       {
           _engine.SetAltitude(_engine.GetCurrentAltitude());
           _engine.SetOvverideControls(_playerInput.Controls.y);
       }

       _engine.Ovverided = isVerticalAxisActive;
   }
}
