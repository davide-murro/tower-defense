using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    [SerializeField] GameObject menuPanel;

    bool isOpen;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        CloseMenu();
    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnDestroy()
    {
        EnableWorld();
    }

    public void OnPauseClick()
    {
        if (!isOpen) OpenMenu();
        else CloseMenu();
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

    public void OnResumeClick()
    {
        CloseMenu();
    }

    void OpenMenu()
    {
        isOpen = true;
        menuPanel.SetActive(true);
        DisableWorld();
    }
    void CloseMenu()
    {
        isOpen = false;
        menuPanel.SetActive(false);
        EnableWorld();
    }


    // disable / freeze the world when the menu is open
    void DisableWorld()
    {
        Time.timeScale = 0f;
        AudioListener.pause = true;
        //BroadcastMessage(nameof(Tile.DisableInteractions), SendMessageOptions.DontRequireReceiver);
    }

    // enable / unfreeze the world when the menu is close
    void EnableWorld()
    {
        Time.timeScale = 1f;
        AudioListener.pause = false;
        //BroadcastMessage(nameof(Tile.EnableInteractions), SendMessageOptions.DontRequireReceiver);
    }
}
