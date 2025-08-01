using UnityEngine;
using UnityEngine.SceneManagement; // Add this for scene management

public class EndingMainMenu : MonoBehaviour
{
    [Header("References")]
    public Transform respawnPoint;
    public GameObject playerObject;
    public PlayerVoiceLines voiceLines; // Assign in Inspector

    private FPSController fpsController;
    private CharacterController characterController;

    private bool isDead = false;

    void Start()
    {
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
            ReturnToMainMenu();
        }
    }

    public void ShowDeathScreen()
    {
        if (fpsController != null) fpsController.enabled = false;
        if (characterController != null) characterController.enabled = false;

        isDead = true;

        // Instead of showing death screen, go directly to main menu
        ReturnToMainMenu();
    }

    public void ReturnToMainMenu()
    {
        // Load the main menu scene
        SceneManager.LoadScene("Main_Menu");
    }

    // This can be kept if you still want respawn functionality elsewhere
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

        isDead = false;

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