using UnityEngine;
using TMPro;

public class TitleCardTrigger : MonoBehaviour
{
    public TextMeshProUGUI titleText;   // Drag the UI text here
    public string message = "THE BASEMENT"; // The message to show
    public float displayTime = 3f;      // How long to show it

    private bool hasTriggered = false;

    private void OnTriggerEnter(Collider other)
    {
        if (!hasTriggered && other.CompareTag("Player"))
        {
            hasTriggered = true;
            StartCoroutine(ShowTitle());
        }
    }

    private System.Collections.IEnumerator ShowTitle()
    {
        titleText.gameObject.SetActive(true);
        titleText.text = message;

        yield return new WaitForSeconds(displayTime);

        titleText.gameObject.SetActive(false);
    }
}
