using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
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

    void OnDestroy()
    {
        EnableWorld();
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
        Tile[] tiles = FindObjectsByType<Tile>(FindObjectsSortMode.None);
        foreach (var tile in tiles) tile.enabled = false;
        //BroadcastMessage(nameof(Tile.DisableInteractions), SendMessageOptions.DontRequireReceiver);
    }

    // enable / unfreeze the world when the menu is close
    void EnableWorld()
    {
        Time.timeScale = 1f;
        AudioListener.pause = false;
        Tile[] tiles = FindObjectsByType<Tile>(FindObjectsSortMode.None);
        foreach (var tile in tiles) tile.enabled = true;
        //BroadcastMessage(nameof(Tile.EnableInteractions), SendMessageOptions.DontRequireReceiver);
    }
}
