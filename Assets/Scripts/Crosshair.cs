using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crosshair : MonoBehaviour
{

    private SpaceShip _pivot;
    private float _offset;
    
    void Awake()
    {
        _pivot = GetComponentInParent<SpaceShip>();
        _offset = Vector3.Distance(transform.position, _pivot.transform.position);
    }

    void Update()
    {
        transform.position = _pivot.transform.position + ((Vector3)_pivot.AimDirection * _offset);
        transform.rotation = _pivot.AimRotation;
    }
}
