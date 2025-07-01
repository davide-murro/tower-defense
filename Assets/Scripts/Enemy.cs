using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] int goldReward = 25;
    [SerializeField] int goldPenalty = 20;

    Bank bank;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        bank = FindFirstObjectByType<Bank>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void RewardGold()
    {
        if (bank == null) { return; }
        bank.Deposit(goldReward);
    }

    public void PenalizeGold()
    {
        if (bank == null) { return; }
        bank.Withdraw(goldPenalty);
    }
}
