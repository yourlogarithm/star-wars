using System;
using System.Collections.Generic;
using UnityEngine;

public class ScrollingBackground : MonoBehaviour
{
    [SerializeField] private float length;
    
    
    private Transform _cameraTransform;
    private Vector3 _startPosition;

    private void Awake()
    {
        _cameraTransform = Camera.main.transform;
        _startPosition = transform.position;
    }

    private void FixedUpdate()
    {
        // Debug.Log(_cameraTransform.position.y + " " + _startPosition.y + _y);
        if (_cameraTransform.position.y > _startPosition.y + length * 2)
        {
            ScrollUp();
        }
    }

    private void ScrollUp()
    {
        transform.position += Vector3.up * (length * 1.5f);
        _startPosition.y += length * 1.5f;
    }
}