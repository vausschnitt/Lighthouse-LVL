using UnityEngine;
using TMPro;

public class PlayerVoiceLines : MonoBehaviour
{
    [TextArea]
    public string[] subtitleLines; // Add your subtitle text here in Inspector

    public TMP_Text subtitleText;  // Assign your TMP UI text object here
    public float displayTime = 3f;

    public void PlayRandomVoiceLine()
    {
        if (subtitleLines.Length == 0)
        {
            Debug.LogWarning("Subtitle lines array is empty.");
            return;
        }

        int randomIndex = Random.Range(0, subtitleLines.Length);
        string line = subtitleLines[randomIndex];

        subtitleText.text = line;
        subtitleText.gameObject.SetActive(true);

        CancelInvoke(nameof(HideSubtitle));
        Invoke(nameof(HideSubtitle), displayTime);
    }

    private void HideSubtitle()
    {
        subtitleText.gameObject.SetActive(false);
    }
}
