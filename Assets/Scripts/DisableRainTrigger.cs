using UnityEngine;

public class DisableRainTrigger : MonoBehaviour
{
    [Header("Assign your rain effect GameObject here")]
    public GameObject rainEffect;

    private void OnTriggerEnter(Collider other)
    {
        // Check if the player triggered the hitbox
        if (other.CompareTag("Player"))
        {
            if (rainEffect != null)
            {
                rainEffect.SetActive(false);
                Debug.Log(" Rain disabled after player entered trigger.");
            }
            else
            {
                Debug.LogWarning("RainEffect is not assigned in the Inspector.");
            }

            // Optional: disable the trigger after first use
            gameObject.SetActive(false);
        }
    }
}
