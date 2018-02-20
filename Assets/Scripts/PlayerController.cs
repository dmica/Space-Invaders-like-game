using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour
{
    public delegate void OnHitEnemyAction();
    public delegate void OnKillEnemyAction();

    public OnHitEnemyAction OnHitEnemy;
    public OnKillEnemyAction OnKillEnemy;

    private float SPEED = 6;
    private float SHOOT_COOLDOWN = 0.5f;

    public GameObject bulletPrefab;

    private float shootTimer = 0;

    public void Update()
    {
        shootTimer -= Time.deltaTime;

        ProcessInput();
    }

    private void ProcessInput()
    {
        if (Input.GetKey("left") || Input.GetKey("a"))
        {
            if (this.transform.position.x > -5.5f)
            {
                this.transform.position = new Vector3
                (
                    this.transform.position.x - SPEED * Time.deltaTime,
                    this.transform.position.y,
                    this.transform.position.z
                );
            }
        }
        if (Input.GetKey("right") || Input.GetKey("d"))
        {
            if (this.transform.position.x < 5.5f)
            {
                this.transform.position = new Vector3
                (
                    this.transform.position.x + SPEED * Time.deltaTime,
                    this.transform.position.y,
                    this.transform.position.z
                );
            }
        }

        if (Input.GetKeyDown("space") || Input.GetKeyDown("k"))
        {
            if (shootTimer <= 0f)
            {
                shootTimer = SHOOT_COOLDOWN;
                Shoot();
            }
        }
    }

    private void Shoot()
    {
        if (Time.timeScale == 0) return;

        GameObject bulletObject = GameObject.Instantiate<GameObject>(bulletPrefab);
        bulletObject.transform.SetParent(this.transform.parent);
        bulletObject.transform.position = this.transform.position;

        bulletObject.GetComponent<BulletController>().OnKillEnemy = () =>
        {
            if (this.OnKillEnemy != null)
            {
                this.OnKillEnemy();
            }
        };
    }

    public void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.GetComponent<EnemyController>() != null)
        {
            if (OnHitEnemy != null) { OnHitEnemy(); }
        }
    }
}