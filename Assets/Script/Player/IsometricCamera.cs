using UnityEngine;

public class IsometricCamera : MonoBehaviour
{
    public Transform target;      // Il player da seguire
    public Vector3 offset = new Vector3(0, 15, -15); // Posizione fissa rispetto al player
    public float smoothSpeed = 0.1f;

    void LateUpdate()
    {
        if (target == null) return;

        Vector3 desiredPosition = target.position + offset;
        transform.position = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);

        // Rotazione fissa isometrica: angolo di circa 45° in alto e 45° sull'asse Y
        transform.rotation = Quaternion.Euler(60, 50, 0);
    }
}
