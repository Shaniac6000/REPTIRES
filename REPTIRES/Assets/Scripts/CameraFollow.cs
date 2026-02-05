using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] private Transform camPoint;

    private Vector3 velocity = Vector3.zero;

    private float camTime = .1f;

    // Update is called once per frame
    void Update()
    {
        transform.position = Vector3.SmoothDamp(transform.position, camPoint.position + new Vector3(0, 5, 0), ref velocity, camTime);
        transform.localRotation = Quaternion.AngleAxis(camPoint.rotation.eulerAngles.y, Vector3.up);
    }
}
