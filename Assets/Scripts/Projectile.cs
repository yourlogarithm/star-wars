using System.Collections;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private float lifeTime;
    [SerializeField] private float maxDamage;

    [SerializeField] private AudioClip launchSound;
    [SerializeField] private AudioClip explosionSound;
    
    private Rigidbody2D _rigidbody2D;
    private SpaceShip _source;
    private float _initialHeight;

    private void Awake()
    {
        _rigidbody2D = GetComponent<Rigidbody2D>();
        _initialHeight = transform.localScale.y;
        StartCoroutine(ShrinkOverTime(lifeTime));
        Destroy(gameObject, lifeTime);
    }

    public void Launch(SpaceShip source, Vector3 direction, Quaternion rotation)
    {
        Transform t = transform;
        _source = source;
        t.rotation = rotation;
        _rigidbody2D.velocity = direction * speed;
        AudioSource.PlayClipAtPoint(launchSound, t.position);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        var spaceShip = other.gameObject.GetComponent<SpaceShip>();
        if (spaceShip == _source)
            return;
        if (spaceShip != null)
            spaceShip.TakeHit(maxDamage * (transform.localScale.y / _initialHeight));
        AudioSource.PlayClipAtPoint(explosionSound, transform.position, 1.3f);
        Destroy(gameObject);
    }
    
    IEnumerator ShrinkOverTime(float shrinkTime)
    {
        Vector3 originalScale = transform.localScale;
        Vector3 targetScale = Vector3.zero; // to shrink to nothing; replace with new Vector3(x, y, z) for other sizes

        float currentTime = 0;

        while (currentTime < shrinkTime)
        {
            // Lerp scale from originalScale to targetScale based on the proportion of shrinkTime that has passed
            transform.localScale = Vector3.Lerp(originalScale, targetScale, currentTime / shrinkTime);

            currentTime += Time.deltaTime;

            yield return null; // wait until next frame
        }

        // Ensure it's the correct size at the end of the shrink
        transform.localScale = targetScale;
    }
}
