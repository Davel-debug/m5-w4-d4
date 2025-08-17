using Unity.AI.Navigation;
using UnityEngine;
using UnityEngine.AI;
using System.Collections.Generic;

public class DoorSwitch : MonoBehaviour
{
    [Header("Riferimenti")]
    public Transform player;
    public NavMeshSurface navmeshSurface;

    [Header("Porte")]
    public List<Transform> porteDaScendere = new List<Transform>();
    public List<Transform> porteDaSalire = new List<Transform>();

    [Header("Impostazioni")]
    public float distanzaInterazione = 3f;
    public float velocitaMovimento = 2f;

    private bool vicino = false;
    private bool attivato = false;
    private bool porteInMovimento = false;

    private Dictionary<Transform, Vector3> posInizialiDaScendere = new Dictionary<Transform, Vector3>();
    private Dictionary<Transform, Vector3> posInizialiDaSalire = new Dictionary<Transform, Vector3>();
    private Dictionary<Transform, Vector3> posTarget = new Dictionary<Transform, Vector3>();

    void Start()
    {
        foreach (Transform porta in porteDaScendere)
            posInizialiDaScendere[porta] = porta.position;

        foreach (Transform porta in porteDaSalire)
            posInizialiDaSalire[porta] = porta.position;
    }

    void Update()
    {
        if (player == null) return;

        vicino = Vector3.Distance(player.position, transform.position) <= distanzaInterazione;

        if (vicino)
            Debug.Log("Premi E per attivare l'interruttore");

        if (vicino && Input.GetKeyDown(KeyCode.E) && !porteInMovimento)
        {
            attivato = !attivato; // toggle stato
            porteInMovimento = true;
            posTarget.Clear();

            // target porte da scendere
            foreach (Transform porta in porteDaScendere)
            {
                Renderer rend = porta.GetComponent<Renderer>();
                float altezza = (rend != null) ? rend.bounds.size.y : 1f;
                float targetY = attivato ? porta.position.y - altezza - 1f : posInizialiDaScendere[porta].y;
                posTarget[porta] = new Vector3(porta.position.x, targetY, porta.position.z);
            }

            // target porte da salire
            foreach (Transform porta in porteDaSalire)
            {
                Renderer rend = porta.GetComponent<Renderer>();
                float altezza = (rend != null) ? rend.bounds.size.y : 1f;
                float targetY = attivato ? posInizialiDaSalire[porta].y : posInizialiDaSalire[porta].y + altezza + 1f;
                posTarget[porta] = new Vector3(porta.position.x, targetY, porta.position.z);
            }
        }//non funziona correggi

        if (porteInMovimento)
        {
            bool tutteArrivate = true;
            foreach (var kvp in posTarget)
            {
                Transform porta = kvp.Key;
                Vector3 target = kvp.Value;
                porta.position = Vector3.MoveTowards(porta.position, target, velocitaMovimento * Time.deltaTime);
                if (porta.position != target)
                    tutteArrivate = false;
            }

            if (tutteArrivate)
            {
                porteInMovimento = false;
                if (navmeshSurface != null)
                    navmeshSurface.BuildNavMesh();
            }
        }
    }
}
