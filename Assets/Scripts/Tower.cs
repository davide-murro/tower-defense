using TMPro;
using UnityEngine;

public class Tower : MonoBehaviour
{
    [SerializeField] int cost = 75;

    // Start is called before the first frame update
    void Start()
    {
    }
    // Update is called once per frame
    void Update()
    {
    }

    public bool CreateTower(Tower tower, Transform parent)
    {
        Bank bank = FindFirstObjectByType<Bank>();

        if (bank == null) return false; 

        if (bank.CurrentBalance >= cost)
        {
            Instantiate(tower, parent.position, Quaternion.identity, parent);
            bank.Withdraw(cost);
            return true;
        }

        return false;
    }
}
