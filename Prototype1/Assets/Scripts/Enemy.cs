using UnityEngine;

public class Enemy : MonoBehaviour
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
        if (player != null && Vector3.Distance(transform.position, player.position) <= detectionRadius)
        {
            MoveTowardsPlayer();

            if (Vector3.Distance(transform.position, player.position) <= attackRadius && Time.time - lastAttackTime >= attackCooldown)
            {
                AttackPlayer();
                lastAttackTime = Time.time;
            }
        }
    }

    void MoveTowardsPlayer()
    {
        Vector3 direction = (player.position - transform.position).normalized;

        Vector3 targetPosition = transform.position + direction * moveSpeed * Time.deltaTime;

        transform.position = targetPosition;

        FacePlayer(direction);
    }

    void FacePlayer(Vector3 direction)
    {
        Quaternion lookRotation = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f);
    }

    void AttackPlayer()
    {
        MainGameManager.Instance.DamagePlayer(attackDamage);
    }
}
