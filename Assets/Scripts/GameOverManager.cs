using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverManager : MonoBehaviour
{
    [SerializeField] GameObject gameOverPanel;

    bool isGameOver = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (!isGameOver) UpdateGameOver();
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

    public void OnNextLevelClick()
    {
        int sceneIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(sceneIndex + 1);
    }

    void UpdateGameOver()
    {
        // if enemies end you win
        int poolsCount = FindObjectsByType<ObjectPool>(FindObjectsSortMode.None).Length;
        if (poolsCount <= 0)
        {
            // game over
            isGameOver = true;
            gameOverPanel.SetActive(true);
            Tile[] tiles = FindObjectsByType<Tile>(FindObjectsSortMode.None);
            foreach (var tile in tiles) tile.enabled = false;

            // set fields
            Transform endTextbox = gameOverPanel.transform.Find("ContentPanel").Find("EndTextbox");
            endTextbox.GetComponent<TextMeshProUGUI>().text = "You won!";

            int currentIndex = SceneManager.GetActiveScene().buildIndex;
            int nextIndex = currentIndex + 1;
            if (nextIndex < SceneManager.sceneCountInBuildSettings)
            {
                Transform nextLevelButton = gameOverPanel.transform.Find("ContentPanel").Find("ButtonGroup").Find("NextLevelButton");
                nextLevelButton.gameObject.SetActive(true);
            }

            return;
        }

        // if you finish money you lose, easy!
        int bankAmount = FindFirstObjectByType<BankManager>().CurrentBalance;
        if (bankAmount < 0)
        {
            // game over
            isGameOver = true;
            gameOverPanel.SetActive(true);

            // set fields
            Transform endTextbox = gameOverPanel.transform.Find("ContentPanel").Find("EndTextbox");
            endTextbox.GetComponent<TextMeshProUGUI>().text = "You lost!";

            Transform nextLevelButton = gameOverPanel.transform.Find("ContentPanel").Find("ButtonGroup").Find("NextLevelButton");
            nextLevelButton.gameObject.SetActive(false);

            return;
        }
    }
}
