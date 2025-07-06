using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{
    [SerializeField] GameObject menuPanel;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        CloseMenu();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnPauseClick()
    {
        OpenMenu();
    }

    public void OnHomeClick()
    {
        SceneManager.LoadScene("Home");
    }

    public void OnRestartClick()
    {
        int sceneIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(sceneIndex);
    }

    public void OnResumeButton()
    {
        CloseMenu();
    }

    void OpenMenu()
    {
        menuPanel.SetActive(true);
        DisableWorld();
    }
    void CloseMenu()
    {
        menuPanel.SetActive(false);
        EnableWorld();
    }


    // disable / freeze the world when the menu is open
    void DisableWorld()
    {
        Time.timeScale = 0f;
        AudioListener.pause = true;
        //if (playerInput) playerInput.enabled = false;
    }

    // enable / unfreeze the world when the menu is close
    void EnableWorld()
    {
        Time.timeScale = 1f;
        AudioListener.pause = false;
        //if (playerInput) playerInput.enabled = true;
    }
}
