using UnityEngine;

public class TargetLocator : MonoBehaviour
{
    [SerializeField] Transform weaponBase;
    [SerializeField] Vector2 ClampWeaponBase;
    [SerializeField] Transform weaponScope;
    [SerializeField] Vector2 ClampWeaponScope;

    [SerializeField] ParticleSystem projectileParticles;
    [SerializeField] float range = 15f;

    GameObject target;


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
        if (target == null)
        {
            Attack(false);
            return;
        }

        // rotate to the target
        //weapon.LookAt(target.transform);

        // horizontal rotation of the base
        Vector3 targetHorizontalDirection = target.transform.position - weaponBase.position;
        targetHorizontalDirection.y = 0f;
        if (targetHorizontalDirection != Vector3.zero)
        {
            Quaternion lookRotation = Quaternion.LookRotation(targetHorizontalDirection, Vector3.up); // only y

            // clamp
            Vector2 euler = lookRotation.eulerAngles;
            euler.y = ClampAngle(euler.y, ClampWeaponBase.x, ClampWeaponBase.y);
            lookRotation = Quaternion.Euler(euler);

            // set
            weaponBase.rotation = lookRotation;
        }

        // vertical rotation of the scope
        Vector3 targetVerticalDirection = target.transform.position - weaponScope.position;
        targetVerticalDirection.x = 0f; // Lock side-to-side aiming
        if (targetVerticalDirection != Vector3.zero)
        {
            Quaternion lookRotation = Quaternion.LookRotation(targetVerticalDirection);
            lookRotation = Quaternion.Euler(lookRotation.eulerAngles.x, 0f, 0f); // only X

            // clamp
            Vector2 euler = lookRotation.eulerAngles;
            euler.x = ClampAngle(euler.x, ClampWeaponScope.x, ClampWeaponScope.y);
            lookRotation = Quaternion.Euler(euler);

            // set
            weaponScope.localRotation = lookRotation;
        }

        // attack if is enough close
        float targetDistance = Vector3.Distance(transform.position, target.transform.position);
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
        // check if is disable
        if (target != null && !target.activeInHierarchy) target = null;

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
            GameObject closestTarget = null;

            foreach (EnemyController enemy in enemies)
            {
                float targetDistance = Vector3.Distance(transform.position, enemy.transform.position);

                if (targetDistance < maxDistance)
                {
                    closestTarget = enemy.gameObject;
                    maxDistance = targetDistance;
                }
            }

            target = closestTarget;
        }
    }
    float ClampAngle(float angle, float min, float max)
    {
        angle = (angle > 180) ? angle - 360 : angle; // Convert to -180..180
        return Mathf.Clamp(angle, min, max);
    }

    void Attack(bool isActive)
    {
        var emissionModule = projectileParticles.emission;
        emissionModule.enabled = isActive;
    }
}
