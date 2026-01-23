using System;
using Unity.Mathematics;
using UnityEngine;

public class CarController : MonoBehaviour
{
    [SerializeField] private Transform steeringWheel;
    [SerializeField] private Transform leftWheel;
    [SerializeField] private Transform rightWheel;
    private Rigidbody rb;

    private Vector3 carVelocity;

    private Quaternion tempRotation;

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
        steeringWheel.localRotation = Quaternion.Euler(-wheelRotation * 3, 90, 90);
        leftWheel.localRotation = Quaternion.Euler(90, 0, 90 + wheelRotation);
        rightWheel.localRotation = Quaternion.Euler(90, 0, 90 + wheelRotation);

        carVelocity = transform.forward * currentSpeed;
        carVelocity.y = rb.linearVelocity.y;
        rb.linearVelocity = carVelocity;
        
        carRotation -= currentSpeed * wheelRotation * Time.deltaTime / 4;
        tempRotation = Quaternion.AngleAxis(carRotation, Vector3.up);
        tempRotation.x = transform.localRotation.x;
        tempRotation.z = transform.localRotation.z;
        transform.localRotation = tempRotation;
    }
}
