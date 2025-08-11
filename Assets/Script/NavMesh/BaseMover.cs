using UnityEngine;
using UnityEngine.AI;

public class BaseMover : MonoBehaviour
{
    protected NavMeshAgent agent;

    protected virtual void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    // Imposta la destinazione del NavMeshAgent
    public virtual void MoveTo(Vector3 destination)
    {
        if (agent != null && agent.isOnNavMesh)
        {
            agent.SetDestination(destination);
        }
    }

    // Controlla se l'agente è arrivato alla destinazione
    public bool HasReachedDestination(float threshold = 0.1f)
    {
        if (agent == null || !agent.hasPath)
            return false;

        return !agent.pathPending && agent.remainingDistance <= threshold;
    }
}
