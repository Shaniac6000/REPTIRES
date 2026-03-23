using UnityEngine;

public class DashboardDoll : MonoBehaviour
{

    public float maxRotation = 30.0f;
    public float rotationSpeed = 1.0f;

    public GameObject car;
    private Rigidbody rb;

    private Quaternion initialRotation;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        initialRotation = transform.rotation;

        rb = car.GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!rb.isKinematic)
        {
            float angle = Mathf.PingPong(Time.time * rotationSpeed, maxRotation * 2) - maxRotation;

            Quaternion offsetRotation = Quaternion.AngleAxis(angle, transform.forward);

            transform.rotation = initialRotation * offsetRotation;
        }
        
    }
}
