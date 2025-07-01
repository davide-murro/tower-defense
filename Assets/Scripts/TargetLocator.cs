using UnityEngine;

public class TargetLocator : MonoBehaviour
{
    [SerializeField] Transform weapon;

    [SerializeField] ParticleSystem projectileParticles;
    [SerializeField] float range = 15f;

    Transform target;


    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        FindClosestTarget();
        AimWeapon();
    }

    void AimWeapon()
    {
        if (target == null) return;

        float targetDistance = Vector3.Distance(transform.position, target.position);

        //weapon.LookAt(target);

        

        Vector3 targetDirection = target.position - transform.position;

        Vector3 directionHorizontal = targetDirection;
        directionHorizontal.y = 0f;
        weapon.rotation = Quaternion.LookRotation(directionHorizontal, Vector3.up);

        
        Vector3 directionVertical = targetDirection;
        directionHorizontal.x = 0f;
        directionHorizontal.z = 0f;
        weapon.Find("cannon").rotation = Quaternion.LookRotation(directionVertical, Vector3.right);
        
        //weapon.Find("cannon").LookAt(target);



        /*
        // --- Horizontal Rotation (Y Axis) ---
        Vector3 targetPosY = new Vector3(target.position.x, weaponBase.position.y, target.position.z);
        Vector3 dirY = targetPosY - weaponBase.position;
        if (dirY != Vector3.zero)
        {
            Quaternion lookRotationY = Quaternion.LookRotation(dirY);
            weaponBase.rotation = Quaternion.Euler(0, lookRotationY.eulerAngles.y, 0);
        }

        // --- Vertical Rotation (X Axis) ---
        Transform cannon = target.Find("cannon");
        if (cannon != null)
        {
            Vector3 dirX = cannon.position - weaponBarrel.position;
            if (dirX != Vector3.zero)
            {
                Quaternion lookRotationX = Quaternion.LookRotation(dirX);
                weaponBarrel.localRotation = Quaternion.Euler(lookRotationX.eulerAngles.x, 0, 0);
            }
        }*/


        if (targetDistance < range)
        {
            Attack(true);
        }
        else
        {
            Attack(false);
        }
    }

    void FindClosestTarget()
    {
        EnemyController[] enemies = FindObjectsByType<EnemyController>(FindObjectsSortMode.None);
        Transform closestTarget = null;
        float maxDistance = Mathf.Infinity;

        foreach (EnemyController enemy in enemies)
        {
            float targetDistance = Vector3.Distance(transform.position, enemy.transform.position);

            if (targetDistance < maxDistance)
            {
                closestTarget = enemy.transform;
                maxDistance = targetDistance;
            }
        }

        target = closestTarget;
    }

    void Attack(bool isActive)
    {
        var emissionModule = projectileParticles.emission;
        emissionModule.enabled = isActive;
    }
}
