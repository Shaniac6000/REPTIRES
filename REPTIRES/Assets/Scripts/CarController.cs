using System;
using TMPro;
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

    [SerializeField] private TextMeshProUGUI speedometer;
    [SerializeField] private Transform speedometerNeedle;
    [SerializeField] private TextMeshProUGUI gearShiftUI;

    [SerializeField] private float acceleration = 500;

    [SerializeField] private float brakeForce = 300;
    [SerializeField] private float baseGearSpeed = 10;
    [SerializeField] private int gear = 1;
    [SerializeField] private float gearChangeOffset = 2.5f;
    private float currentAcceleration = 0;
    private float currentBrakeForce = 0;
    private float wheelRotation = 0;
    private float currentMaxGearSpeed = 10;
    private float currentMinGearSpeed = 0;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        frontRightWheel.GetComponent<WheelCollider>().steerAngle = 90;
        frontLeftWheel.GetComponent<WheelCollider>().steerAngle = 90;
        backRightWheel.steerAngle = 90;
        backLeftWheel.steerAngle = 90;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (!Input.GetKey(KeyCode.Space) && !Input.GetKey(KeyCode.Q))
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
        frontLeftWheel.transform.parent.localRotation = Quaternion.Euler(0, -wheelRotation, 0);
        frontRightWheel.transform.parent.localRotation = Quaternion.Euler(0, -wheelRotation, 0);
        frontRightWheel.GetComponent<WheelCollider>().steerAngle = 90 -wheelRotation;
        frontLeftWheel.GetComponent<WheelCollider>().steerAngle = 90 -wheelRotation;

        rb.linearVelocity = Vector3.ClampMagnitude(rb.linearVelocity, currentMaxGearSpeed);

        if (rb.linearVelocity.magnitude >= currentMaxGearSpeed)
        {
            frontRightWheel.GetComponent<WheelCollider>().brakeTorque = currentBrakeForce;
            frontLeftWheel.GetComponent<WheelCollider>().brakeTorque = currentBrakeForce;
            backRightWheel.brakeTorque = currentBrakeForce;
            backLeftWheel.brakeTorque = currentBrakeForce;
        }

        if (rb.linearVelocity.magnitude <= .01 && Input.GetAxisRaw("Vertical") == 0)
        {
            frontRightWheel.GetComponent<WheelCollider>().motorTorque = 0;
            frontLeftWheel.GetComponent<WheelCollider>().motorTorque = 0;
            frontRightWheel.GetComponent<WheelCollider>().brakeTorque = 0;
            frontLeftWheel.GetComponent<WheelCollider>().brakeTorque = 0;
            backRightWheel.brakeTorque = 0;
            backLeftWheel.brakeTorque = 0;
        }
    }

    void Update()
    {
        currentAcceleration = acceleration * Input.GetAxisRaw("Vertical");

        if (Input.GetKey(KeyCode.Space) && Input.GetAxisRaw("Vertical") == 0 && !Input.GetKey(KeyCode.Q))
        {
            currentBrakeForce = brakeForce;
        }
        else
        {
            currentBrakeForce = 0;
        }

        if (rb.linearVelocity.magnitude <= currentMinGearSpeed - gearChangeOffset)
        {
            gear -= 1;
            currentMaxGearSpeed = gear * baseGearSpeed;
            currentMinGearSpeed = (gear - 1) * baseGearSpeed;
            gearShiftUI.text = "GEAR: " + gear;
        }

        if (Input.GetKey(KeyCode.Q) && !Input.GetKey(KeyCode.Space) && Input.GetAxisRaw("Vertical") == 0)
        {
            if (Input.GetKeyDown(KeyCode.K) && rb.linearVelocity.magnitude >= currentMaxGearSpeed - gearChangeOffset && gear < 3 && gear > 0)
            {
                gear += 1;
                currentMaxGearSpeed = gear * baseGearSpeed;
                currentMinGearSpeed = (gear - 1) * baseGearSpeed;
                gearShiftUI.text = "GEAR: " + gear;
            }

            if (Input.GetKeyDown(KeyCode.K) && gear == 0 && rb.linearVelocity.magnitude <= .01f) {
                gear += 1;
                acceleration *= -1;
                gearShiftUI.text = "GEAR: " + gear;
            }

            if (Input.GetKeyDown(KeyCode.M) && rb.linearVelocity.magnitude <= currentMinGearSpeed + gearChangeOffset && gear > 1)
            {
                gear -= 1;
                currentMaxGearSpeed = gear * baseGearSpeed;
                currentMinGearSpeed = (gear - 1) * baseGearSpeed;
                gearShiftUI.text = "GEAR: " + gear;
            }

            if (Input.GetKeyDown(KeyCode.M) && gear == 1 && rb.linearVelocity.magnitude <= .01f) {
                gear -= 1;
                acceleration *= -1;
                gearShiftUI.text = "GEAR: R";
            }
        }

        speedometer.text = "SPEED: " + Math.Round(rb.linearVelocity.magnitude, 2);
        speedometerNeedle.localRotation = Quaternion.Euler(0, 0, 75 - rb.linearVelocity.magnitude * 5);

        if (Input.GetKey(KeyCode.R))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            TrackManager.hasEnded = false;
            TrackManager.hasStarted = false;
        }

        //TESTING PURPOSES ONLY, REMOVE AFTER ACTUALLY MAKING LEVELS
        if (Input.GetKey(KeyCode.P))
        {
            LevelManagement.SetHighest(SceneManager.GetActiveScene().buildIndex);
            SceneManager.LoadScene(0);
        }
    }
}
