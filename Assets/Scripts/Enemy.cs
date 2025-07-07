using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] int goldReward = 25;
    [SerializeField] int goldPenalty = 20;

    BankManager bankManager;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        bankManager = FindFirstObjectByType<BankManager>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void RewardGold()
    {
        if (bankManager == null) { return; }
        bankManager.Deposit(goldReward);
    }

    public void PenalizeGold()
    {
        if (bankManager == null) { return; }
        bankManager.Withdraw(goldPenalty);
    }
}
