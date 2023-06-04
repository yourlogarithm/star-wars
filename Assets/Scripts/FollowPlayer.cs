using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPlayer : MonoBehaviour
{
    [SerializeField] private Transform target;
    
    private void Update()
    {
        Vector3 position = target.position;
        transform.position = new Vector3(position.x, position.y, transform.position.z);
    }
}
