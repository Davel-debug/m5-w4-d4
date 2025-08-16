using UnityEngine;

public class CharacterDetection : MonoBehaviour
{
    [SerializeField]
    [Range(0, 360)]
    public float _viewAngle = 45f;
    [SerializeField] private float _sightDistance = 10f;    
    [SerializeField] public Transform _target;

    private void Update()
    {
        if (CanSeeTarget())
        {
            Debug.Log($"{_target.gameObject.name} è visibile da {gameObject.name}");
        }
        else
        {
            Debug.Log($"{_target.gameObject.name} non è visibile da {gameObject.name}");
        }
    }


    public bool CanSeeTarget()
    {
        Vector3 toTarget = _target.position - transform.position;
        float sqrDistance = toTarget.sqrMagnitude;

        if(sqrDistance > _sightDistance * _sightDistance )
        {
            Debug.Log($"{_target.gameObject.name} è troppo lontano da {gameObject.name}");
            return false;
        }
        if (Vector3.Dot(transform.forward, toTarget) < Mathf.Cos(_viewAngle * Mathf.Deg2Rad))
        {
            Debug.Log($"{_target.gameObject.name} non è visto da {gameObject.name}");
            return false;
        }
    

        Debug.Log($"{_target.gameObject.name} è visto da {gameObject.name}");
        return true;
    }

}