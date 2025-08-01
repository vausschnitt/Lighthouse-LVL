using UnityEngine;
using TMPro;

public class PickupCounterManager : MonoBehaviour
{
    public int requiredPickups = 5;
    private int currentPickups = 0;

    public TMP_Text pickupUIText; // Assign this in the Inspector
    public GameObject pickupUI; // Assign this in the inspector
    public AudioClip pickupSound; // Assign your pickup sound in the Inspector
    private AudioSource puSound; // Reference to the AudioSource component

    public bool HasCollectedAll()
    {
        return currentPickups >= requiredPickups;
    }

    public void ActivateUI()
    {
        if (pickupUI != null)
            pickupUI.SetActive(true);
    }

    private void Start()
    {
        UpdatePickupUI();
        // Try to get AudioSource on this object first
        puSound = GetComponent<AudioSource>();

        // If not found, try to find "puSound" object
        if (puSound == null)
        {
            GameObject soundObject = GameObject.Find("puSound");
            if (soundObject != null)
            {
                puSound = soundObject.GetComponent<AudioSource>();
            }
        }

        // Create new AudioSource if none exists
        if (puSound == null)
        {
            puSound = gameObject.AddComponent<AudioSource>();
        }
    }

    public void RegisterPickup()
    {
        currentPickups++;
        UpdatePickupUI();

        // Play pickup sound if available
        if (puSound != null && pickupSound != null)
        {
            puSound.PlayOneShot(pickupSound);
        }

        if (currentPickups >= requiredPickups)
        {
            Debug.Log("All items collected. Player can now escape.");
            // You can trigger escape logic here
        }
    }

    private void UpdatePickupUI()
    {
        if (pickupUIText != null)
        {
            pickupUIText.text = $"Objective:\nCollect CASE's to escape.\nItem collected: {currentPickups}/{requiredPickups}";
        }
    }
}