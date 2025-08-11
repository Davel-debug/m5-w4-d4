using UnityEngine;
using UnityEngine.AI;

public class PlayerController : BaseMover
{
    public Camera mainCamera;
    public float inputThreshold = 0.1f;

    protected override void Awake()
    {
        base.Awake();

        if (mainCamera == null)
            mainCamera = Camera.main;
    }

    void Update()
    {
        //Prendo i movimenti horizzontali e verticali in base alla telecamera escludendo y
        Vector3 forward = Vector3.Normalize(new Vector3(mainCamera.transform.forward.x, 0f, mainCamera.transform.forward.z));
        Vector3 right = Vector3.Normalize(new Vector3(mainCamera.transform.right.x, 0f, mainCamera.transform.right.z));
        
        //Creo il vettore direzione come somma di vettori e tolgo y
        Vector3 inputDirection = (right * Input.GetAxis("Horizontal")) + (forward * Input.GetAxis("Vertical"));
        inputDirection.y = 0f;

        if (inputDirection.magnitude > inputThreshold)
        {
            // Movimento WASD: sposta il player nella direzione dell'input
            Vector3 targetPosition = transform.position + inputDirection.normalized;
            MoveTo(targetPosition);
        }
        else if (Input.GetMouseButtonDown(0))
        {
            // Punta e clicca
            Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                MoveTo(hit.point);
            }
        }
    }
}
