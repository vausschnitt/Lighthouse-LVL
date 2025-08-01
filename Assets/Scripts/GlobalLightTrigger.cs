using UnityEngine;

public class GlobalLightTrigger : MonoBehaviour
{
    public Light globalLight; // Drag your Directional or main light here
    private bool hasTriggered = false;

    private void OnTriggerEnter(Collider other)
    {
        if (!hasTriggered && other.CompareTag("Player"))
        {
            hasTriggered = true;
            if (globalLight != null)
            {
                globalLight.enabled = false;
                Debug.Log(" Global light turned off.");
            }
        }
    }
}
