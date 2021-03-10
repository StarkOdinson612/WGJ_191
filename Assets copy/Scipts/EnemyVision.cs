using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEditor.PackageManager;
using UnityEngine;

public class EnemyVision : MonoBehaviour
{
    public Transform player;
    [SerializeField] private GameObject pFieldOfView;
    public FieldOfView fov;
    [SerializeField] private EnemyPatrol enemyPatrol;

    [SerializeField] float fovAngle;
    [SerializeField] float viewDistance;
    [SerializeField] Transform enemyLook;

    // Start is called before the first frame update
    void Start()
    {
        fov = Instantiate(pFieldOfView, Vector3.zero, Quaternion.identity).GetComponent<FieldOfView>();
        fov.SetFOV(fovAngle);
        fov.SetViewDistance(viewDistance);
        player = FindObjectOfType<PlayerMovement>().GetComponent<Transform>();
    }

    // Update is called once per frame
    void Update()
    {
        fov.SetOrigin(transform.position);
        try
        {
            fov.SetAimDirectionV(enemyLook.position - transform.position);
        }
        catch
        {
            Debug.Log("Error");
        }

        if (CheckPlayerDistance(player.position, transform.position, viewDistance) && CheckPlayerAngle() && CheckLineOfSight(player, transform))
        {
            StartCoroutine(PlayerSeen());
            
        }
    }

    public bool CheckPlayerDistance(Vector3 playerPos, Vector3 enemyPos, float distance)
    {
        playerPos = new Vector3(playerPos.x, playerPos.y, 0);
        enemyPos = new Vector3(enemyPos.x, enemyPos.y, 0);

        return Vector3.Distance(playerPos, enemyPos) < distance;
    }

    private Vector3 GetVectorDir(Vector3 vector1, Vector3 vector2)
    {
        vector1 = new Vector3(vector1.x, vector1.y, 0);
        vector2 = new Vector3(vector2.x, vector2.y, 0);

        return (vector1 - vector2).normalized;
    }

    private bool CheckPlayerAngle()
    {
        Vector3 dirToPlayer = GetVectorDir(player.position, transform.position);
        Vector2 enemyLookDir = GetVectorDir(enemyLook.position, transform.position);

        return Vector3.Angle(enemyLookDir, dirToPlayer) < (fovAngle / 2);
    }

    public bool CheckLineOfSight(Transform body1, Transform body2)
    {
        RaycastHit2D hit = Physics2D.Raycast(body1.position, body2.position);

        if (hit.collider == null)
        {
            return false;
        }

        if (hit.collider.gameObject.GetComponent<PlayerMovement>() == null)
        {
            return false;
        }
        return true;
    }

    IEnumerator PlayerSeen()
    {
        yield return new WaitForSeconds(5);
    }
}
