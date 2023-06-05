using System;
using System.Collections.Generic;
using UnityEngine;

public class SpaceShip : MonoBehaviour
{
    [SerializeField] private bool test;

    [SerializeField] private float health;
    [SerializeField] private float armor;
    
    [SerializeField] private float rotationSpeed;
    
    [SerializeField] private float speed;
    [SerializeField] private float acceleration;
    
    [Range(0, 1f)]
    [SerializeField] private float brakeDamping;

    [SerializeField] private float fov;
    [SerializeField] private float shootingCooldown;
    [SerializeField] private Transform projectileSpawnPoint;
    [SerializeField] private Projectile projectilePrefab;

    private Rigidbody2D _rigidbody2D;
    private SpriteRenderer _spriteRenderer;
    private Camera _camera;

    private Vector2 _aimDirection;
    private Quaternion _aimRotation;
    private float _aimAngle;
    private Vector2 _movementDirection;
    private float _movementAngle;
    private Quaternion _movementRotation;
    
    public Vector2 AimDirection => _aimDirection;
    public Quaternion AimRotation => _aimRotation;
    
    private bool _isOnBrake;
    private bool _isLocked;
    private float _lastShootTime;

    private void Awake()
    {
        _rigidbody2D = GetComponent<Rigidbody2D>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _camera = Camera.main;
    }

    private void Update()
    {
        if (test)
            return;
        GetInputDirection();
        GetInputKeys();
    }

    private void FixedUpdate()
    {
        if (test)
            return;
        Movement();
        Rotation();
        Shoot();
    }

    private void GetInputDirection()
    {
        Vector3 mousePosition = _camera.ScreenToWorldPoint(Input.mousePosition);
        mousePosition.z = 0;
        _aimDirection = (mousePosition - transform.position).normalized;
        Vector2 forward = _movementRotation * Vector2.up;
        float angle = Vector2.Angle(forward, _aimDirection);
        if (angle > fov / 2)
        {
            float sign = (Vector3.Cross(forward, _aimDirection).z < 0) ? -1 : 1;
            float edgeAngle = sign * fov / 2;
            _aimDirection = Quaternion.Euler(0, 0, edgeAngle) * forward;
        }
        _aimAngle = Mathf.Atan2(_aimDirection.y, _aimDirection.x) * Mathf.Rad2Deg - 90f;
        _aimRotation = Quaternion.Euler(0, 0, _aimAngle);
        if (_isLocked)
            return;
        _movementDirection = _aimDirection;
        _movementAngle = _aimAngle;
        _movementRotation = _aimRotation;
    }

    private void GetInputKeys()
    {
        if (Input.GetKey(KeyCode.Space))
            _isOnBrake = true;
        else
            _isOnBrake = false;

        if (Input.GetKey(KeyCode.LeftShift))
            _isLocked = true;
        else
            _isLocked = false;
    }

    private void Movement()
    {
        if (_isOnBrake)
            _rigidbody2D.velocity = Vector2.Lerp(_rigidbody2D.velocity, Vector2.zero, brakeDamping * 10 * Time.fixedDeltaTime);
        else
        {
            _rigidbody2D.AddForce(_movementDirection * acceleration);
            if (_rigidbody2D.velocity.magnitude > speed)
                _rigidbody2D.velocity = Vector2.ClampMagnitude(_rigidbody2D.velocity, speed); 
        }
    }

    private void Rotation()
    {
        _rigidbody2D.rotation = Mathf.LerpAngle(_rigidbody2D.rotation, _movementAngle, rotationSpeed * Time.deltaTime);
    }

    private void Shoot()
    {
        if (Input.GetButton("Fire1") && Time.time > _lastShootTime)
        {
            Projectile projectile = Instantiate(projectilePrefab, projectileSpawnPoint.position, projectileSpawnPoint.rotation);
            projectile.Launch(this, _aimDirection, _aimRotation);
            _lastShootTime = Time.time + shootingCooldown;
        }
    }

    public void TakeHit(float damage)
    {
        health -= damage;
        StartCoroutine(FlashRed(damage));
        if (health <= 0)
        {
            Destroy(gameObject);
        }
    }

    private IEnumerator<WaitForSeconds> FlashRed(float damage)
    {
        Color originalColor = _spriteRenderer.color;
        Color redColor = new Color(1, 0, 0, Mathf.Min(damage / (health / 2), 1));
        float flashDuration = 0.1f;
        _spriteRenderer.color = redColor;
        float elapsedTime = 0;
        while (elapsedTime < flashDuration)
        {
            // calculate how far through the flash we are (between 0 and 1)
            float lerpAmount = elapsedTime / flashDuration;
            // blend towards the original color
            _spriteRenderer.color = Color.Lerp(redColor, originalColor, lerpAmount);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        // revert back to the original color
        _spriteRenderer.color = originalColor;
    }
}
