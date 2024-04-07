using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CowCatcher : MonoBehaviour
{
   [SerializeField] private float _catchDistance;
   [SerializeField] private float _catchRadius;
   [SerializeField] private GameObject _effect;
   [SerializeField] private LayerMask _mask;
   [SerializeField] private float _catchTimer;

   private bool _isCatchActionActive = false;
   private Transform _transform;
   private float _catchTime = -1f;
   private Transform _catchedCow;
   private Vector3 _startPosition;
   private Vector3 _startScale;

   private void Awake()
   {
      _transform = transform;
   }

   public void SetInput(PlayerInput input)
   {
      input.CatchPressed += OnCatchPressed;
      input.CatchRealesed += OnCatchRealesed;
   }

   private void OnCatchRealesed()
   {
      if(_catchedCow != null)
         return;

      SetEffects(false);
   }
   private void OnCatchPressed()
   {
      SetEffects(true); 
   }

   private void SetEffects(bool value)
   {
      _effect.SetActive(value);
      _isCatchActionActive = value;
   }

   private void OnDrawGizmosSelected()
   {
      Gizmos.color = Color.yellow;
      Gizmos.DrawSphere(transform.position + transform.forward * _catchDistance,_catchRadius);
   }

   private void FixedUpdate()
   {
      if (!_isCatchActionActive)
      {
         return;
      }
      var colliderCatch = Physics.OverlapSphere(_transform.position + _transform.forward * _catchDistance, _catchRadius, _mask,
         QueryTriggerInteraction.Ignore);
     foreach (var collider in colliderCatch)
     {
        var cow = collider.GetComponentInParent<Cow>();
        if (cow != null)
        {
           cow.Catched();
           _catchedCow = cow.transform;
           _catchedCow.SetParent(_transform);
           _startPosition = _catchedCow.localPosition;
           _startScale = _catchedCow.localScale;
           _catchTimer = 1f;
        }
     }
   }

   private void Update()
   {
      if (_catchTime > 0)
      {
         _catchTime -= Time.deltaTime/_catchTimer;
         if (_catchTime <= 0)
         {
            if (_catchedCow != null)
            {
               Destroy(_catchedCow.gameObject);
               _catchedCow = null;
            }
         }
      }

      if (_catchedCow != null)
      {
         UpdateCowTranform();
      }
   }

   private void UpdateCowTranform()
   {
      float t = _catchTime;
      _catchedCow.transform.localPosition = Vector3.Lerp(Vector3.zero,_startPosition, t);
      _catchedCow.transform.localScale = Vector3.Lerp(Vector3.zero,_startScale, t);
   }
}
