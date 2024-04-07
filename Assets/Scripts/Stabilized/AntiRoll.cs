using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(Rigidbody))]
public class AntiRoll : MonoBehaviour
{
    [SerializeField] private float _stabilizerForce;
    [SerializeField] private float _damping;

    private Rigidbody _rigidbody;
    private Transform _transform;
    private float _lastDot;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _transform = transform;
    }

    private void FixedUpdate()
    {
        var up = _transform.up;
        var dot = Vector3.Dot(up,Vector3.up);
        var axis = Vector3.Cross(up, Vector3.up);

        Stabilize(dot,axis);
        Damping(dot,axis);
        AntiRolling(up, dot);
    }

    private void AntiRolling(Vector3 up, float dot)
    {
        if (dot <= 0)
        {
            var needVector = Vector3.ProjectOnPlane(up, Vector3.up).normalized;
            
            Quaternion quaternion = Quaternion.FromToRotation(up,needVector);

            _transform.rotation *= quaternion;
        }
    }

    private void Damping(in float dot, Vector3 axis)
    {
        var difference = (_lastDot - dot) * Time.fixedDeltaTime;

        if (difference > 0)
        {
            _rigidbody.AddTorque(-axis * difference * _damping,ForceMode.Force);
        }
        
        _lastDot = dot;
    }

    private void Stabilize(in float dot, Vector3 axis)
    {
        if (dot > 0)
        {
            _rigidbody.AddTorque(axis * (1-dot) * _stabilizerForce,ForceMode.Force);
        }
    }
}
