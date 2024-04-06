using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(Rigidbody))]
public class PositionStabilized : MonoBehaviour
{
    [SerializeField] private float _stabilizerForce;
    
    private Rigidbody _rigidbody;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>(); 
    }

    private void FixedUpdate()
    {
       _rigidbody.AddForce(-_rigidbody.position.z * Vector3.forward * _stabilizerForce,ForceMode.Force); 
    }
}
