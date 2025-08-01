using UnityEngine;
using UnityEngine.SceneManagement;

public class ExitZoneTrigger : MonoBehaviour
{
    public string nextSceneName = "NextScene"; // Set in Inspector
    public PickupCounterManager pickupManager; // Assign in Inspector

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (pickupManager != null && pickupManager.HasCollectedAll())
            {
                Debug.Log(" All items collected. Escaping...");
                SceneManager.LoadScene(nextSceneName);
            }
            else
            {
                Debug.Log(" You must collect all items before escaping!");
            }
        }
    }
}
