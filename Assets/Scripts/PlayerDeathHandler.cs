using UnityEngine;
using UnityEngine.UI;

public class PlayerDeathHandler : MonoBehaviour
{
    [Header("References")]
    public GameObject deathScreen;
    public Transform respawnPoint;
    public GameObject playerObject;
    public Button respawnButton;
    public PlayerVoiceLines voiceLines; // Assign in Inspector


    private FPSController fpsController;
    private CharacterController characterController;

    private bool isDead = false;

    void Start()
    {
        deathScreen.SetActive(false);

        if (playerObject != null)
        {
            fpsController = playerObject.GetComponent<FPSController>();
            characterController = playerObject.GetComponent<CharacterController>();
        }
    }

    void Update()
    {
        if (isDead && Input.GetKeyDown(KeyCode.Space))
        {
            respawnButton.onClick.Invoke();
        }
    }

    public void ShowDeathScreen()
    {
        if (fpsController != null) fpsController.enabled = false;
        if (characterController != null) characterController.enabled = false;

        deathScreen.SetActive(true);
        isDead = true;
    }

    public void RespawnPlayer()
    {
        // Reposition player
        if (characterController != null)
        {
            characterController.enabled = false;
            playerObject.transform.position = respawnPoint.position;
            characterController.enabled = true;
        }
        else
        {
            playerObject.transform.position = respawnPoint.position;
        }

        // Reactivate player controller
        if (fpsController != null)
        {
            fpsController.enabled = true;
        }

        // Reset all WeepingAngels using new Unity API
        var angels = Object.FindObjectsByType<WeepingAngelEnemy>(FindObjectsSortMode.None);
        foreach (var angel in angels)
        {
            angel.ResetAngel();
            angel.enabled = true; // re-enable angel if it was disabled after attack
        }

        deathScreen.SetActive(false);
        isDead = false;

        Debug.Log("Respawn logic complete. About to play voice line.");
        

        if (voiceLines != null)
        {
            voiceLines.PlayRandomVoiceLine();
        }
        else
        {
            Debug.LogWarning("voiceLines is NULL");
        }
    }
}
