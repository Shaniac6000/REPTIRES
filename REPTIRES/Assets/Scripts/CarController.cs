using System;
using Unity.Mathematics;
using UnityEngine;

public class CarController : MonoBehaviour
{
    [SerializeField] private Transform leftWheel;
    [SerializeField] private Transform rightWheel;
    private Rigidbody rb;

    private Vector3 carVelocity;

    [SerializeField] private float moveSpeed;

    private float wheelRotation = 0;
    private float carRotation = 0;
    private float currentSpeed = 0;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        currentSpeed = Input.GetAxis("Vertical") * moveSpeed;
        wheelRotation -= Input.GetAxisRaw("Horizontal") * Time.deltaTime * 30;
        wheelRotation = Mathf.Clamp(wheelRotation, -45, 45);
        leftWheel.localRotation = Quaternion.Euler(90, 0, 90 + wheelRotation);
        rightWheel.localRotation = Quaternion.Euler(90, 0, 90 + wheelRotation);

        carVelocity = transform.forward * currentSpeed;
        carVelocity.y = rb.linearVelocity.y;
        rb.linearVelocity = carVelocity;
        
        carRotation -= currentSpeed * wheelRotation * Time.deltaTime / 2;
        transform.localRotation = Quaternion.Euler(transform.localRotation.x, carRotation, transform.localRotation.z);
    }
}
