using UnityEngine;

public class PickupUITrigger : MonoBehaviour
{
    public PickupCounterManager pickupManager;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            pickupManager?.ActivateUI();
            gameObject.SetActive(false); // Disable trigger after use
        }
    }
}
