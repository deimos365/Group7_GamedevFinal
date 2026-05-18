using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [Header("Panels")]
    public GameObject mainPanel;
    public GameObject levelSelectPanel;
    public GameObject settingsPanel;

    void Start()
    {
        mainPanel.SetActive(true);

        levelSelectPanel.SetActive(false);

        if (settingsPanel != null)
            settingsPanel.SetActive(false);
    }

    // ---------- LEVEL SELECT ----------

    public void OpenLevelSelect()
    {
        mainPanel.SetActive(false);

        levelSelectPanel.SetActive(true);
    }

    // ---------- RETURN BUTTON ----------

    public void BackToMain()
    {
        levelSelectPanel.SetActive(false);
        settingsPanel.SetActive(false);

        mainPanel.SetActive(true);
    }

    // ---------- LOAD LEVELS ----------

    public void LoadLevel(int sceneIndex)
    {
        SceneManager.LoadScene(sceneIndex);
    }

    // ---------- SETTINGS ----------

    public void OpenSettings()
    {
        Debug.Log("Settings opened");

        settingsPanel.SetActive(true);
    }

    // ---------- EXIT ----------

    public void ExitGame()
    {
        Debug.Log("Game Closed");

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
    Application.Quit();
#endif
    }
}