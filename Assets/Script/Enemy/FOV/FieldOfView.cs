using UnityEngine;
using System.Collections.Generic;

public class FieldOfView : MonoBehaviour
{
    [Header("Vision Settings")]
    public float viewRadius = 10f;
    [Range(0, 360)]
    public float viewAngle = 90f;

    [Header("Light Settings")]
    public Light torchLight;
    public Light extraLight;

    [Header("Debug Settings")]
    public bool showDebugGizmos = true;

    public LayerMask targetMask;
    public LayerMask obstacleMask;

    public EnemyController enemy;

    [HideInInspector] public Transform visibleTarget;

    void Update()
    {
        UpdateLights();
        FindVisibleTargets();
    }

    void FindVisibleTargets()
    {
        visibleTarget = null;
        Collider[] targetsInViewRadius = Physics.OverlapSphere(transform.position, viewRadius + enemy.extraViewRadius, targetMask);

        foreach (Collider target in targetsInViewRadius)
        {
            Vector3 dirToTarget = (target.transform.position - transform.position).normalized;
            float distToTarget = Vector3.Distance(transform.position, target.transform.position);

            // dentro al cono normale
            if (Vector3.Angle(transform.forward, dirToTarget) < viewAngle / 2)
            {
                if (!Physics.Raycast(transform.position, dirToTarget, distToTarget, obstacleMask))
                {
                    visibleTarget = target.transform;
                    break;
                }
            }
            // dentro al cerchio extra
            else if (distToTarget <= enemy.extraViewRadius)
            {
                if (!Physics.Raycast(transform.position, dirToTarget, distToTarget, obstacleMask))
                {
                    visibleTarget = target.transform;
                    break;
                }
            }
        }
    }
    void UpdateLights()
    {
        if (torchLight != null)
        {
            torchLight.spotAngle = viewAngle;
            torchLight.range = viewRadius;
            torchLight.transform.rotation = transform.rotation; // segue forward del nemico
        }

        if (extraLight != null)
        {
            extraLight.spotAngle = 360f;
            extraLight.range = enemy.extraViewRadius;
        }
    }

    void OnDrawGizmos()
    {
        if (!showDebugGizmos) return;
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, viewRadius);

        
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, enemy != null ? enemy.extraViewRadius : 0f);

        Vector3 leftBoundary = DirFromAngle(-viewAngle / 2, false);
        Vector3 rightBoundary = DirFromAngle(viewAngle / 2, false);

        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, transform.position + leftBoundary * viewRadius);
        Gizmos.DrawLine(transform.position, transform.position + rightBoundary * viewRadius);

        if (visibleTarget != null)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawLine(transform.position, visibleTarget.position);
        }
    }

    public Vector3 DirFromAngle(float angleInDegrees, bool angleIsGlobal)
    {
        if (!angleIsGlobal)
        {
            angleInDegrees += transform.eulerAngles.y;
        }
        return new Vector3(Mathf.Sin(angleInDegrees * Mathf.Deg2Rad), 0, Mathf.Cos(angleInDegrees * Mathf.Deg2Rad));
    }
}
