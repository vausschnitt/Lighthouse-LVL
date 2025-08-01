using UnityEngine;

public class ToggleOnTrigger : MonoBehaviour
{
    public GameObject targetObject;  // Object to enable/disable
    public bool activateOnEnter = true; // Set false if you want to deactivate instead

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
