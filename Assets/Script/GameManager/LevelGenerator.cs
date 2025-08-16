
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
    void Start()
    {
        GenerateLevel();

        surface.BuildNavMesh();
    }

    void GenerateLevel()
    {
        
    }
}
