using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] private Transform camPoint;
    [SerializeField] private Rigidbody carRB;

    private Vector3 velocity = Vector3.zero;

    private Camera cam;
    public float initialFOV;
    public float FOVScaling;

    private float camTime = .15f;

    void Start()
    {
        cam = GetComponent<Camera>();
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = Vector3.SmoothDamp(transform.position, camPoint.position + new Vector3(0, 6, 0), ref velocity, camTime);
        transform.localRotation = Quaternion.AngleAxis(camPoint.rotation.eulerAngles.y, Vector3.up);

        cam.fieldOfView = initialFOV + (Vector3.Magnitude(carRB.linearVelocity) * FOVScaling);
    }
}
