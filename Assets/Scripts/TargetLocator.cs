using UnityEngine;

public class TargetLocator : MonoBehaviour
{
    [SerializeField] Transform weaponBase;
    [SerializeField] Transform weaponScope;

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

        // rotate to the target
        //weapon.LookAt(target);

        // horizontal rotation of the base
        Vector3 targetHorizontalDirection = target.position - weaponBase.position;
        targetHorizontalDirection.y = 0f;
        if (targetHorizontalDirection != Vector3.zero)
        {
            weaponBase.rotation = Quaternion.LookRotation(targetHorizontalDirection, Vector3.up);
        }

        // vertical rotation of the scope
        Vector3 targetVerticalDirection = target.position - weaponScope.position;
        targetVerticalDirection.x = 0f; // Lock side-to-side aiming
        if (targetVerticalDirection != Vector3.zero)
        {
            Quaternion lookRotation = Quaternion.LookRotation(targetVerticalDirection);
            weaponScope.localRotation = Quaternion.Euler(lookRotation.eulerAngles.x, 0f, 0f); // only X
        }

        // attack if is enough close
        float targetDistance = Vector3.Distance(transform.position, target.position);
        if (targetDistance <= range)
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
        // if already has a target keep it if is in range
        if (target != null)
        {
            float targetDistance = Vector3.Distance(transform.position, target.transform.position);

            if (targetDistance > range)
            {
                target = null;
            }
        }

        // otherwise find the closest one
        if (target == null)
        {
            float maxDistance = Mathf.Infinity;
            EnemyController[] enemies = FindObjectsByType<EnemyController>(FindObjectsSortMode.None);
            Transform closestTarget = null;

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
    }

    void Attack(bool isActive)
    {
        var emissionModule = projectileParticles.emission;
        emissionModule.enabled = isActive;
    }
}
