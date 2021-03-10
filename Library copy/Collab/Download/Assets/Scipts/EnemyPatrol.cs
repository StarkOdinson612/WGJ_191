using UnityEngine;
using UnityEngine.AI;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;

public class EnemyPatrol : MonoBehaviour
{
    public float speed = 10;
    public Transform[] moveSpots;
    private bool hasStopped;

    public Vector3 dir;

    public Transform lightT;
    private EnemyState enemyState;
    private PlayerMovement playerMovement;
    private EnemyVision enemyVision;

    public float timeDelay = 1f;
    private int currentSpot;

    private void Start()
    {
        playerMovement = FindObjectOfType<PlayerMovement>().GetComponent<PlayerMovement>();
        enemyVision = GetComponent<EnemyVision>();

        if (moveSpots.Length != 0)
        {
            currentSpot = 0;
        }
        else
        {
            return;
        }

        hasStopped = false;

        transform.up = new Vector3(moveSpots[currentSpot].position.x, moveSpots[currentSpot].position.y, 0) - transform.position;
        lightT.rotation = transform.rotation;

        enemyState = gameObject.AddComponent<EnemyState>();
        enemyState.isPatrolling = true;
    }

    private void Update()
    {
        if (enemyState.isPatrolling)
        {
            dir = moveSpots[currentSpot].position - transform.position;
            float dirAngle = GetAngleFromVectorFloat(dir);
            Quaternion rotDir = Quaternion.Euler(new Vector3(0, 0, dirAngle - transform.rotation.z - 90));
            transform.rotation = Quaternion.Slerp(transform.rotation, rotDir, 4 * Time.deltaTime);

            lightT.rotation = transform.rotation;
            lightT.position = transform.position;

            if (Vector2.Distance(transform.position, moveSpots[currentSpot].position) < 0.2f)
            {
                StartCoroutine(NextSpot());
            }

            if (!hasStopped)
            {
                transform.position = Vector2.MoveTowards(transform.position, moveSpots[currentSpot].position, speed * Time.deltaTime);
            }
        }
        else if (enemyState.isChasing)
        {
            Vector2.MoveTowards(transform.position, playerMovement.transform.position, float.MaxValue);

            if (enemyVision.CheckPlayerDistance(playerMovement.transform.position, transform.position, 20))
            {
                enemyState.isChasing = false;
                enemyState.isPatrolling = true;
            }
        }
        else if (enemyState.isLocked)
        {
            Debug.DrawLine(transform.position, playerMovement.transform.position, Color.black);
        }

        //transform.up = new Vector3(moveSpots[currentSpot].position.x, moveSpots[currentSpot].position.y, 0) - transform.position;

        if (Input.GetKey(KeyCode.LeftShift))
        {
            if (Input.GetKeyDown(KeyCode.P))
            {
                enemyState.isPatrolling = true;
                enemyState.isChasing = false;
                enemyState.isLocked = false;
            }
            else if (Input.GetKeyDown(KeyCode.C))
            {
                enemyState.isPatrolling = false;
                enemyState.isChasing = true;
                enemyState.isLocked = false;
            }
            else if (Input.GetKeyDown(KeyCode.L))
            {
                enemyState.isPatrolling = false;
                enemyState.isChasing = false;
                enemyState.isLocked = true;
            }
        }
    }

    IEnumerator NextSpot()
    {
        hasStopped = true;

        if (currentSpot < moveSpots.Length - 1)
        {
            currentSpot++;
        }
        else if (currentSpot == moveSpots.Length - 1)
        {
            currentSpot = 0;
        }
        else
        {
            yield break;
        }

        yield return new WaitForSeconds(timeDelay);

        hasStopped = false;
    }

    public static float GetAngleFromVectorFloat(Vector3 dir)
    {
        dir = dir.normalized;
        float n = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;

        if (n < 0)
        {
            n += 360;
        }

        return n;
    }
}