using UnityEngine;
using UnityEngine.SceneManagement;

public class EndingAngel : MonoBehaviour
{
    [Header("References")]
    public Transform playerCamera;
    public Collider angelZone;

    [Header("Behavior Settings")]
    public float moveSpeed = 3f;
    public float attackRange = 1.5f;

    private Vector3 startPosition;
    private Quaternion startRotation;

    void Start()
    {
        startPosition = transform.position;
        startRotation = transform.rotation;
    }

    void Update()
    {
        if (angelZone == null || !IsPlayerInZone())
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
            SceneManager.LoadScene("Credits");
        }
    }

    public void ResetAngel()
    {
        transform.position = startPosition;
        transform.rotation = startRotation;
        enabled = true;
    }
}