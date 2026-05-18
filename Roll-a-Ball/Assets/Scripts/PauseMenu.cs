using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    [Header("UI Panels")]
    public GameObject mainPausePanel;
    public GameObject settingsPanel;

    bool isPaused = false;
    bool inSettings = false;

    void Start()
    {
        settingsPanel.SetActive(false);
        mainPausePanel.SetActive(false);

        ResumeGame();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (inSettings)
            {
                BackToPauseMenu();
                return;
            }

            if (isPaused)
                ResumeGame();
            else
                PauseGame();
        }
    }

    // ---------------- PAUSE CORE ----------------

    public void PauseGame()
    {
        isPaused = true;

        mainPausePanel.SetActive(true);
        settingsPanel.SetActive(false);

        Time.timeScale = 0f;

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void ResumeGame()
    {
        isPaused = false;
        inSettings = false;

        mainPausePanel.SetActive(false);
        settingsPanel.SetActive(false);

        Time.timeScale = 1f;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    // ---------------- SETTINGS ----------------

    public void OpenSettings()
    {
        inSettings = true;

        mainPausePanel.SetActive(false);
        settingsPanel.SetActive(true);
    }

    public void BackToPauseMenu()
    {
        inSettings = false;

        mainPausePanel.SetActive(true);
        settingsPanel.SetActive(false);
    }

    // ---------------- SCENE ACTIONS ----------------

    public void RestartGame()
    {
        Time.timeScale = 1f;

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void ReturnToMainMenu()
    {
        Time.timeScale = 1f;

        SceneManager.LoadScene(0);
    }
}