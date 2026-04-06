using UnityEngine;

public class SpriteBillboarding : MonoBehaviour
{
    public Camera cam;

    Rigidbody rb;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!rb.isKinematic)
        {
            transform.rotation = cam.transform.rotation;
        }
    }
}
