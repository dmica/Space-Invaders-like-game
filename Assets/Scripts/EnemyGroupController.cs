using UnityEngine;
using System.Collections;

public class EnemyGroupController : MonoBehaviour
{
    private float HORIZONTAL_MOVE_AMOUNT = 0.25f;
    private float VERTICAL_MOVE_AMOUNT = 0.25f;
    private float MAXIMUM_INTERVAL = 0.300f;
    private float MINIMUM_INTERVAL = 0.025f;
    private float RIGHT_BOUND = 5.5f;
    private float LEFT_BOUND = -5.5f;

    private EnemyController[] enemies;
    private float moveTimer;
    private int totalEnemies;
    private bool movingRight;

    public void Start()
    {
        enemies = this.GetComponentsInChildren<EnemyController>();

        totalEnemies = enemies.Length;
    }

    public void Update()
    {
        if (Time.timeScale == 0) return;

        moveTimer -= Time.deltaTime;
        if (moveTimer <= 0f)
        {
            enemies = this.GetComponentsInChildren<EnemyController>();

            int enemyCount = enemies.Length;

            // Calculates next interval for movement

            float difficultyPercentage = 1 - ((float)enemyCount / totalEnemies);
            float interval = MAXIMUM_INTERVAL + (MINIMUM_INTERVAL - MAXIMUM_INTERVAL) * difficultyPercentage;

            moveTimer = interval;

            // Checks whether enemy group reached screen bounds

            float minimumX = 0;
            float maximumX = 0;

            foreach (EnemyController enemy in enemies)
            {
                if (enemy.transform.position.x < minimumX) minimumX = enemy.transform.position.x;
                else if (enemy.transform.position.x > maximumX) maximumX = enemy.transform.position.x;
            }

            if (movingRight && maximumX >= RIGHT_BOUND || !movingRight && minimumX <= LEFT_BOUND)
            {
                // Reached bounds! Move down

                this.transform.position = new Vector3
                (
                    this.transform.position.x,
                    this.transform.position.y - VERTICAL_MOVE_AMOUNT,
                    this.transform.position.z
                );

                movingRight = !movingRight;
            }
            else
            {
                // Didn't reach bounds, move further

                this.transform.position = new Vector3
                (
                    this.transform.position.x + HORIZONTAL_MOVE_AMOUNT * (movingRight ? 1 : -1),
                    this.transform.position.y,
                    this.transform.position.z
                );
            }
        }
    }
}
