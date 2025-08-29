
using Unity.AI.Navigation;
using UnityEditorInternal;
using UnityEngine;
using UnityEngine.AI;

public class LevelGenerator : MonoBehaviour
{
    public NavMeshSurface surface;

    [SerializeField] public int levelWidth = 10;
    [SerializeField] public int levelHeight = 10;

    [SerializeField] public GameObject wall;
    [SerializeField] public GameObject player;

    private bool playerSpawned = false;
    void Awake()
    {
        if (surface == null)
        {
            surface = FindObjectOfType<NavMeshSurface>();
            if (surface == null)
                Debug.LogWarning("Nessuna NavMeshSurface trovata nella scena!");
        }

        GenerateLevel();
        Debug.Log("navmesh creata");
        if (surface != null)
            surface.BuildNavMesh();
    }

    void GenerateLevel()
    {
        
    }
}
