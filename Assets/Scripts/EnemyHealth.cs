using UnityEngine;

[RequireComponent(typeof(Enemy))]
[RequireComponent(typeof(EnemyController))]
public class EnemyHealth : MonoBehaviour
{
    [SerializeField] int maxHitPoints = 5;

    [Tooltip("Adds this amount to maxHitPoints when enemy dies.")]
    [SerializeField] int difficultyRamp = 1;

    int currentHitPoints = 0;

    Enemy enemy;
    Animator animator;


    // Start is called before the first frame update
    void Start()
    {
        enemy = GetComponent<Enemy>();
        animator = GetComponent<Animator>();
    }

    // Start is called before the first frame update
    void OnEnable()
    {
        currentHitPoints = maxHitPoints;
    }

    void OnParticleCollision(GameObject other)
    {
        ProcessHit();
    }

    void ProcessHit()
    {
        currentHitPoints--;
        animator.SetTrigger("hit");

        if (currentHitPoints <= 0)
        {
            gameObject.SetActive(false);
            maxHitPoints += difficultyRamp;
            enemy.RewardGold();
        }
    }
}
