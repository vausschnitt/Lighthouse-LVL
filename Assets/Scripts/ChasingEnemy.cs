using UnityEngine;

public class ChasingEnemy : MonoBehaviour
{
    [Header("References")]
    public PlayerDeathHandler deathHandler;
    public Transform playerCamera;
    public Collider triggerZone;

    [Header("Behavior Settings")]
    public float moveSpeed = 3f;
    public float attackRange = 1.5f;

    private bool isActive = false;
    private Vector3 startPosition;
    private Quaternion startRotation;

    void Start()
    {
        startPosition = transform.position;
        startRotation = transform.rotation;
    }

    void Update()
    {
        if (triggerZone == null || !IsPlayerInZone())
            return;

        if (IsVisibleToCamera())
            return;

        isActive = true;

        if (!isActive)
            return;

        MoveTowardPlayer();
        RotateTowardPlayer();
        StickToGround();
        CheckForAttack();
    }

    bool IsPlayerInZone()
    {
        return triggerZone.bounds.Contains(playerCamera.position);
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

        if (triggerZone != null)
        {
            Bounds bounds = triggerZone.bounds;
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
            Debug.Log("X The enemy got you!");
            if (deathHandler != null)
            {
                deathHandler.ShowDeathScreen();
            }

            enabled = false;
        }
    }

    public void ResetEnemy()
    {
        transform.position = startPosition;
        transform.rotation = startRotation;
        isActive = false;
        enabled = true;
    }
}
