using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class StartMenu : MonoBehaviour
{
    [SerializeField] private TMP_Text titleText;
    [SerializeField] private string nextSceneName = "LVL_HAZYQ";

    void Start()
    {
        if (titleText != null)
        {
            titleText.text = "VILLAGE 5 H(ORROR)";
        }
    }

    public void OnStartClicked()
    {
        SceneManager.LoadScene(nextSceneName);
    }
}