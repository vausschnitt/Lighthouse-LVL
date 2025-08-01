using UnityEngine;

public class WeepingAngelEnemy : MonoBehaviour
{
    [Header("References")]
    public PlayerDeathHandler deathHandler;
    public Transform playerCamera;
    public Collider angelZone;

    [Header("Behavior Settings")]
    public float moveSpeed = 3f;
    public float attackRange = 1.5f;

    private bool hasSeenPlayer = false;

    // New fields for reset
    private Vector3 startPosition;
    private Quaternion startRotation;
    //private bool isChasingPlayer = false;
    //private float cooldownTimer = 0f;
    //private bool hasLineOfSight = false;

    void Start()
    {
        startPosition = transform.position;
        startRotation = transform.rotation;
    }

    void Update()
    {
        if (angelZone == null || !IsPlayerInZone())
            return;

        if (IsVisibleToCamera())
            return;

        if (!hasSeenPlayer)
            return;

        MoveTowardPlayer();
        RotateTowardPlayer();
        StickToGround();
        CheckForAttack();
    }

    bool IsPlayerInZone()
    {
        return angelZone.bounds.Contains(playerCamera.position);
    }

    bool IsVisibleToCamera()
    {
        Vector3 toEnemy = (transform.position - playerCamera.position).normalized;
        float dot = Vector3.Dot(playerCamera.forward, toEnemy);

        if (dot > 0.4f)
        {
            Ray ray = new Ray(playerCamera.position, toEnemy);
            if (Physics.Raycast(ray, out RaycastHit hit, 100f))
            {
                if (hit.transform.root == transform)
                {
                    Debug.Log("O Player is looking at the enemy (hit: " + hit.transform.name + ")");
                    hasSeenPlayer = true;
                    return true;
                }
            }
        }

        return false;
    }

    void MoveTowardPlayer()
    {
        Vector3 direction = (playerCamera.position - transform.position).normalized;
        Vector3 newPosition = transform.position + direction * moveSpeed * Time.deltaTime;

        if (angelZone != null)
        {
            Bounds bounds = angelZone.bounds;
            newPosition.x = Mathf.Clamp(newPosition.x, bounds.min.x, bounds.max.x);
            newPosition.z = Mathf.Clamp(newPosition.z, bounds.min.z, bounds.max.z);
        }

        transform.position = newPosition;
    }

    void RotateTowardPlayer()
    {
        Vector3 direction = playerCamera.position - transform.position;
        direction.y = 0f;

        if (direction != Vector3.zero)
        {
            Quaternion lookRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, 5f * Time.deltaTime);
        }
    }

    void StickToGround()
    {
        if (Physics.Raycast(transform.position + Vector3.up, Vector3.down, out RaycastHit hit, 5f))
        {
            transform.position = new Vector3(transform.position.x, hit.point.y, transform.position.z);
        }
    }

    void CheckForAttack()
    {
        float distance = Vector3.Distance(transform.position, playerCamera.position);
        if (distance <= attackRange)
        {
            Debug.Log("X The angel got you!");
            if (deathHandler != null)
            {
                deathHandler.ShowDeathScreen();
            }

            enabled = false;
        }
    }

    public void ResetAngel()
    {
        transform.position = startPosition;
        transform.rotation = startRotation;
        hasSeenPlayer = false;
        enabled = true;
    }
}
