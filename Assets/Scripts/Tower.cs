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

    public bool CreateTower(Tower tower, Vector3 position)
    {
        Bank bank = FindFirstObjectByType<Bank>();

        if (bank == null) return false; 

        if (bank.CurrentBalance >= cost)
        {
            Instantiate(tower, position, Quaternion.identity);
            bank.Withdraw(cost);
            return true;
        }

        return false;
    }
}
