using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Bank : MonoBehaviour
{
    [SerializeField] int startingBalance = 100;
    [SerializeField] int currentBalance = 0;

    [SerializeField] TextMeshProUGUI balanceTextbox;

    public int CurrentBalance
    {
        get { return currentBalance; }
    }


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        currentBalance = startingBalance;
    }

    // Update is called once per frame
    void Update()
    {
        UpdateUI();
    }

    public void Deposit(int amount)
    {
        currentBalance += Mathf.Abs(amount);
    }

    public void Withdraw(int amount)
    {
        currentBalance -= Mathf.Abs(amount);
        if (currentBalance < 0)
        {
            ReloadScene();
        }
    }

    void ReloadScene()
    {
        Scene currentScene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(currentScene.buildIndex);
    }

    void UpdateUI()
    {
        balanceTextbox.text = $"Gold: {currentBalance}"; 
    }
}
