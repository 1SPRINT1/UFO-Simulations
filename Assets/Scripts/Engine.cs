using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.PackageManager;
using UnityEngine;
using UnityEngine.Rendering;

public class Engine : MonoBehaviour
{
   [HideInInspector] public bool Ovverided = false;
    
    [SerializeField] private LayerMask _layerMask;
    [SerializeField] private float _maxForce;
    [SerializeField] private float _sphereCastRadius;
    
    [SerializeField] private float _maxDistance;
    [SerializeField] private float _damping;
    
   private Rigidbody _targetBody;
   private Transform _transform;
   private float _springSpeed;
   private float _lastDistance;
   private float _altitude;
   private float distance;
   private float _inputY;

   public float GetCurrentAltitude()
   {
       if (Physics.SphereCast(_transform.position, _sphereCastRadius, _transform.forward, out RaycastHit hit,
           _maxDistance,
           _layerMask, QueryTriggerInteraction.Ignore))
       {
          return hit.distance;
       }

       return _maxDistance;
   }
   public void SetAltitude(float altitude)
   {
       _altitude = Mathf.Clamp(altitude, _sphereCastRadius, _maxDistance);
   }
   
   public void Initialize(Rigidbody targetBody)
   {
      _targetBody = targetBody;
      _transform = transform;
   }

   private void FixedUpdate()
   {
       if (_targetBody == null)
       {
           return;
       }

       var _forward = _transform.forward;
       if (Ovverided)
       {
           ForceUpDown(_forward);
       }
       else
       {
           Lift(_forward);
       }
       Damping();
   }

   private void ForceUpDown(Vector3 _forward)
   {
       float forceFactor = (_inputY >= 0) ? 1 : 0;
       _targetBody.AddForce(-_forward * Mathf.Max(forceFactor * _maxForce - _springSpeed * _maxForce * _damping,0f),ForceMode.Force);
   }

   private void Lift(Vector3 _forward)
   {
       if (Physics.SphereCast(_transform.position, _sphereCastRadius, _forward, out RaycastHit hit, _maxDistance,
           _layerMask, QueryTriggerInteraction.Ignore))
       {
            distance = hit.distance;

            var minForceHeight = _altitude + 1f;
           var maxForceHeight = _altitude - 1f;


           var forceFactor = Mathf.Clamp(distance, maxForceHeight,minForceHeight).Remap(maxForceHeight, minForceHeight, 1, 0);
           _targetBody.AddForce(-_forward * Mathf.Max(forceFactor * _maxForce - _springSpeed * _maxForce * _damping,0f),ForceMode.Force);
       }
   }

   private void OnDrawGizmos()
   {
      Gizmos.color = Color.cyan;
      var startPoint = transform.position;
      var endPoint = transform.position + transform.forward * _maxDistance;
      Gizmos.DrawLine(startPoint,endPoint);
      Gizmos.DrawWireCube(transform.position, Vector3.one * 0.2f);
      Gizmos.DrawSphere(transform.position + transform.forward * _maxDistance,_sphereCastRadius);
   }

   public void Damping()
   {
       _springSpeed = (distance - _lastDistance) * Time.fixedTime;
       _springSpeed = Mathf.Max(_springSpeed, 0);
       _lastDistance = distance;
   }

   internal void SetOvverideControls(float inputY)
   {
       _inputY = inputY;
   }
}
