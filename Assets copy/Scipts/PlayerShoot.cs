using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShoot : MonoBehaviour
{
    public GameObject bulletPrefab;
    private Vector3 mousePos;
    private FieldOfView fov;
    public Vector3 shootDir;
    private Camera cam;
    public float bulletVelocity;
    [SerializeField] public Transform gun;
    [SerializeField] public Vector3 shootPos;
    public float delayInSeconds = 0.2f;
    private bool canShoot;
    public Transform shootPointT;
    public Transform lightT;

    [Range(1, 3)]
    public float dist;

    [Range(0f, 0.9f)]
    public float offset;

    private void Start()
    {
        cam = FindObjectOfType<Camera>();
        canShoot = true;
        Cursor.visible = false;
    }

    private void Update()
    {
        mousePos = new Vector3(cam.ScreenToWorldPoint(Input.mousePosition).x, cam.ScreenToWorldPoint(Input.mousePosition).y, 0);
        shootDir = mousePos - new Vector3(transform.position.x, transform.position.y, 0);

        gun.position = new Vector3(mousePos.x, mousePos.y, gun.position.z);

        if ((gun.position - transform.position).magnitude < 2f)
        {
            gun.position = transform.position + new Vector3(shootDir.x, shootDir.y, transform.position.z).normalized * 2f;
        }

        shootPos = transform.position + new Vector3(shootDir.x, shootDir.y, 0).normalized * (dist + offset);
        lightT.position = transform.position + new Vector3(shootDir.x, shootDir.y, 0).normalized * 0.01f;
        lightT.up = new Vector3(shootPos.x, shootPos.y, 0) - lightT.position;

        shootPointT.position = shootPos;

        if (Input.GetMouseButtonDown(0) && canShoot)
        {
            GameObject bullet = Instantiate(bulletPrefab, shootPos, Quaternion.identity);
            bullet.GetComponent<Bullet>().damage = 10;
            bullet.GetComponent<Rigidbody2D>().velocity = new Vector3(shootDir.x, shootDir.y, 0).normalized * bulletVelocity;
            canShoot = false;
            StartCoroutine(ShootDelay());
        }

        gun.up = gun.position - transform.position;
    }

    IEnumerator ShootDelay()
    {
        yield return new WaitForSeconds(delayInSeconds);
        canShoot = true;
    }
}
