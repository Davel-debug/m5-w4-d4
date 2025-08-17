using UnityEngine;
using System.Collections.Generic;

public class FieldOfView : MonoBehaviour
{
    [Header("Vision Settings")]
    public float viewRadius = 10f;
    [Range(0, 360)]
    public float viewAngle = 90f;

    public LayerMask targetMask;
    public LayerMask obstacleMask;

    public EnemyController enemy;


    [HideInInspector] public Transform visibleTarget;

    void Update()
    {
        FindVisibleTargets();
    }

    void FindVisibleTargets()
    {
        visibleTarget = null;
        Collider[] targetsInViewRadius = Physics.OverlapSphere(transform.position, viewRadius, targetMask);

        foreach (Collider target in targetsInViewRadius)
        {
            Vector3 dirToTarget = (target.transform.position - transform.position).normalized;
            float effectiveViewAngle = viewAngle / 2 + enemy.extraViewAngle; // extraViewAngle lo metti nell'EnemyController
            if (Vector3.Angle(transform.forward, dirToTarget) < effectiveViewAngle)
            {
                float distToTarget = Vector3.Distance(transform.position, target.transform.position);

                if (!Physics.Raycast(transform.position, dirToTarget, distToTarget, obstacleMask))
                {
                    visibleTarget = target.transform;
                    break;
                }
            }

        }
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, viewRadius);

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
