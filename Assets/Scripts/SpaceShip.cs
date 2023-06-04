using System;
using UnityEngine;

public class SpaceShip : MonoBehaviour
{
    [SerializeField] private bool test;
    
    [SerializeField] private float rotationSpeed;
    
    [SerializeField] private float speed;
    [Range(0, 1)]
    [SerializeField] private float acceleration;
    
    [SerializeField] private float brakeDamping;
    
    [SerializeField] private float shootingCooldown;
    [SerializeField] private Transform projectileSpawnPoint;
    [SerializeField] private Projectile projectilePrefab;

    private Rigidbody2D _rigidbody2D;
    private Camera _camera;

    private Vector3 _aimDirection;
    private float _angle;
    private Quaternion _aimRotation;
    
    public Vector3 AimDirection => _aimDirection;
    public Quaternion AimRotation => _aimRotation;
    
    private bool _isOnBrake;
    private bool _isLocked;
    private float _lastShootTime;

    private void Awake()
    {
        _rigidbody2D = GetComponent<Rigidbody2D>();
        _camera = Camera.main;
    }

    private void Update()
    {
        GetInputDirection();
        GetInputKeys();
    }

    private void FixedUpdate()
    {
        Movement();
        Rotation();
        Brake();
    }


    private void GetInputDirection()
    {
        Vector3 mousePosition = _camera.ScreenToWorldPoint(Input.mousePosition);
        _aimDirection = (mousePosition - transform.position).normalized;
        _angle = Mathf.Atan2(_aimDirection.y, _aimDirection.x) * Mathf.Rad2Deg - 90f;
        _aimRotation = Quaternion.Euler(0, 0, _angle);
    }

    private void GetInputKeys()
    {
        if (Input.GetKey(KeyCode.Space))
            _isOnBrake = true;
        else
            _isOnBrake = false;

        if (Input.GetKey(KeyCode.LeftShift))
            _isLocked = true;
    }

    private void Movement()
    {
        // _rigidbody2D.AddForce(_direction * (speed * Time.deltaTime));
    }

    private void Brake()
    {
        if (_isOnBrake)
            _rigidbody2D.AddForce(-brakeDamping * _rigidbody2D.velocity);
    }

    private void Rotation()
    {
        _rigidbody2D.rotation = Mathf.LerpAngle(_rigidbody2D.rotation, _angle, rotationSpeed * Time.deltaTime);
    }
    

    //
    // private void Shoot()
    // {
    //     if (Input.GetButton("Fire1") && Time.time > _lastShootTime)
    //     {
    //         Projectile projectile = Instantiate(projectilePrefab, projectileSpawnPoint.position, projectileSpawnPoint.rotation);
    //         projectile.Launch(_aimDirection, _aimRotation);
    //         _lastShootTime = Time.time + shootingCooldown;
    //     }
    // }
}
