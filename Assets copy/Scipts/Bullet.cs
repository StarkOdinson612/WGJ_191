using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float damage = 5f;
    private Transform playerPos;

    private void Awake()
    {
        playerPos = FindObjectOfType<PlayerShoot>().GetComponent<Transform>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Destroy(gameObject);
    }

    private void Update()
    {
        if ((playerPos.position - transform.position).magnitude > 45f)
        {
            Destroy(gameObject);
        }
    }
}
