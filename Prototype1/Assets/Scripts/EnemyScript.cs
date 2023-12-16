using UnityEngine;

public class EnemyScript : MonoBehaviour
{
    public float moveSpeed = 3f;
    public float detectionRadius = 20f;
    public float attackRadius = 5f;
    public int attackDamage = 20;
    public float attackCooldown = 3f;

    private Transform player;
    private float lastAttackTime;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;

        if (player == null)
        {
            Debug.LogError("Player not found.");
            enabled = false;
        }
    }

    void Update()
    {
        // Check if the player is within the detection radius
        if (player != null && Vector3.Distance(transform.position, player.position) <= detectionRadius)
        {
            // Move towards the player
            MoveTowardsPlayer();

            // Check if the player is within the attack radius and if enough time has passed since the last attack
            if (Vector3.Distance(transform.position, player.position) <= attackRadius && Time.time - lastAttackTime >= attackCooldown)
            {
                // Attack the player
                AttackPlayer();
                lastAttackTime = Time.time;
            }
        }
    }

    void MoveTowardsPlayer()
    {
        // Calculate the direction from the enemy to the player
        Vector3 direction = (player.position - transform.position).normalized;

        // Calculate the desired position based on the direction and speed
        Vector3 targetPosition = transform.position + direction * moveSpeed * Time.deltaTime;

        // Move towards the player
        transform.position = targetPosition;

        FacePlayer(direction);
    }

    void FacePlayer(Vector3 direction)
    {
        // Rotate the enemy to face the player
        Quaternion lookRotation = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f);
    }

    void AttackPlayer()
    {
        // Damage the player using the MainGameManager
        MainGameManager.Instance.DamagePlayer(attackDamage);
    }
}
