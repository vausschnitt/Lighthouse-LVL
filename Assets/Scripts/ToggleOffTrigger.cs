using UnityEngine;

public class ToggleOffTrigger : MonoBehaviour
{
    public GameObject targetObject;  // Object to enable/disable
    public bool activateOnEnter = false; // Set false if you want to deactivate instead

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            targetObject.SetActive(activateOnEnter);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            targetObject.SetActive(!activateOnEnter);
        }
    }
}
