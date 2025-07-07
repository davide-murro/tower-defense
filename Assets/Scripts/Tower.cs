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
        BankManager bankManager = FindFirstObjectByType<BankManager>();

        if (bankManager == null) return false; 

        if (bankManager.CurrentBalance >= cost)
        {
            Instantiate(tower, parent.position, Quaternion.identity, parent);
            bankManager.Withdraw(cost);
            return true;
        }

        return false;
    }
}
