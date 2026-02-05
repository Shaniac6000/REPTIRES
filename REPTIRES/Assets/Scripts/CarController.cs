using System;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CarController : MonoBehaviour
{
    [SerializeField] private Transform steeringWheel;
    [SerializeField] private GameObject frontLeftWheel;
    [SerializeField] private GameObject frontRightWheel;

    [SerializeField] private WheelCollider backLeftWheel;
    [SerializeField] private WheelCollider backRightWheel;
    private Rigidbody rb;

    private Vector3 carVelocity;

    private Quaternion tempRotation;

    [SerializeField] private float acceleration = 500;

    [SerializeField] private float brakeForce = 300;
    [SerializeField] private float maxGearSpeed = 10;
    private float currentAcceleration = 0;
    private float currentBrakeForce = 0;
    private float wheelRotation = 0;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        currentAcceleration = acceleration * Input.GetAxis("Vertical");

        if (Input.GetKey(KeyCode.Space) && Input.GetAxisRaw("Vertical") == 0)
        {
            currentBrakeForce = brakeForce;
        }
        else
        {
            currentBrakeForce = 0;
        }

        if (!Input.GetKey(KeyCode.Space))
        {
            frontRightWheel.GetComponent<WheelCollider>().motorTorque = currentAcceleration;
            frontLeftWheel.GetComponent<WheelCollider>().motorTorque = currentAcceleration;
        }

        frontRightWheel.GetComponent<WheelCollider>().brakeTorque = currentBrakeForce;
        frontLeftWheel.GetComponent<WheelCollider>().brakeTorque = currentBrakeForce;
        backRightWheel.brakeTorque = currentBrakeForce;
        backLeftWheel.brakeTorque = currentBrakeForce;

        wheelRotation -= Input.GetAxisRaw("Horizontal") * Time.deltaTime * 30;
        wheelRotation = Mathf.Clamp(wheelRotation, -45, 45);
        steeringWheel.localRotation = Quaternion.Euler(-wheelRotation * 3, 90, 90);
        frontLeftWheel.transform.localRotation = Quaternion.Euler(90, 0, 90 + wheelRotation);
        frontRightWheel.transform.localRotation = Quaternion.Euler(90, 0, 90 + wheelRotation);
        frontRightWheel.GetComponent<WheelCollider>().steerAngle = -wheelRotation;
        frontLeftWheel.GetComponent<WheelCollider>().steerAngle = -wheelRotation;

        rb.linearVelocity = Vector3.ClampMagnitude(rb.linearVelocity, maxGearSpeed);

        if (rb.linearVelocity.magnitude >= maxGearSpeed)
        {
            frontRightWheel.GetComponent<WheelCollider>().brakeTorque = currentBrakeForce;
            frontLeftWheel.GetComponent<WheelCollider>().brakeTorque = currentBrakeForce;
            backRightWheel.brakeTorque = currentBrakeForce;
            backLeftWheel.brakeTorque = currentBrakeForce;
        }

        print(rb.linearVelocity.magnitude);

        //TESTING PURPOSES ONLY, REMOVE AFTER ACTUALLY MAKING LEVELS
        if (Input.GetKey(KeyCode.L))
        {
            LevelManagement.SetHighest(SceneManager.GetActiveScene().buildIndex);
            SceneManager.LoadScene(0);
        }
    }
}
