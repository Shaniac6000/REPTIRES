using System;
using TMPro;
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

    [SerializeField] private Transform speedometerNeedle;
    [SerializeField] private TextMeshProUGUI gearShiftUI;
    [SerializeField] private Animator gator;
    private Transform gatorTransform;
    [SerializeField] private float acceleration = 500;

    [SerializeField] private float brakeForce = 300;
    [SerializeField] private float baseGearSpeed = 10;
    [SerializeField] private int gear = 1;
    [SerializeField] private float gearChangeOffset = 2.5f;
     [SerializeField] private float rotateDegrees = 45;
    private float currentAcceleration = 0;
    private float currentBrakeForce = 0;
    private float wheelRotation = 0;
    private float currentMaxGearSpeed = 10;
    private float currentMinGearSpeed = 0;
    private float steeringWheelRotation = 90;
    public bool slowed = false;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        frontRightWheel.GetComponent<WheelCollider>().steerAngle = 90;
        frontLeftWheel.GetComponent<WheelCollider>().steerAngle = 90;
        backRightWheel.steerAngle = 90;
        backLeftWheel.steerAngle = 90;
        gatorTransform = gator.gameObject.transform;
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

        if (!(rb.linearVelocity.y < -1f && Math.Abs(rb.linearVelocity.y) >= rb.linearVelocity.magnitude / 1.5f)) {
            rb.linearVelocity = Vector3.ClampMagnitude(rb.linearVelocity, currentMaxGearSpeed);
        }

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

        wheelRotation -= Input.GetAxisRaw("Horizontal") * Time.deltaTime * rotateDegrees;
        wheelRotation = Mathf.Clamp(wheelRotation, -45, 45);
        steeringWheelRotation = -wheelRotation * 3 + 90;
        frontLeftWheel.transform.parent.localRotation = Quaternion.Euler(0, -wheelRotation, 0);
        frontRightWheel.transform.parent.localRotation = Quaternion.Euler(0, -wheelRotation, 0);
        frontRightWheel.GetComponent<WheelCollider>().steerAngle = 90 -wheelRotation;
        frontLeftWheel.GetComponent<WheelCollider>().steerAngle = 90 -wheelRotation;

        steeringWheel.localRotation = Quaternion.Euler(steeringWheelRotation, 90, 90);
        gator.SetFloat("SteerAngle", steeringWheelRotation);
        if (steeringWheelRotation < -18)
        {
            gatorTransform.localPosition = new Vector3(gatorTransform.localPosition.x, 2.535f, gatorTransform.localPosition.z);
        }
        else if (steeringWheelRotation < 9)
        {
            gatorTransform.localPosition = new Vector3(0.691f, 2.683299f, gatorTransform.localPosition.z);
        }
        else if (steeringWheelRotation > 171)
        {
            gatorTransform.localPosition = new Vector3(1.21f, gatorTransform.localPosition.y, gatorTransform.localPosition.z);
        }
        else if (steeringWheelRotation > 144)
        {
            gatorTransform.localPosition = new Vector3(1.113f, gatorTransform.localPosition.y, gatorTransform.localPosition.z);
        }
        else
        {
            gatorTransform.localPosition = new Vector3(0.8250504f, gatorTransform.localPosition.y, gatorTransform.localPosition.z);
        }
        

        if (slowed)
        {
            currentMaxGearSpeed = Mathf.Lerp(currentMaxGearSpeed, baseGearSpeed * 1.5f, Time.deltaTime / 1.5f);
        }
        else if (gear != 0)
        {
            currentMaxGearSpeed = gear * baseGearSpeed;
        }

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
            if (!slowed)
            {
                currentMaxGearSpeed = gear * baseGearSpeed;
            }
            currentMinGearSpeed = (gear - 1) * baseGearSpeed;
            gearShiftUI.text = "GEAR: " + gear;
        }

        if (Input.GetKey(KeyCode.Q) && !Input.GetKey(KeyCode.Space) && Input.GetAxisRaw("Vertical") == 0 && !slowed)
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
        else if (slowed && Input.GetKey(KeyCode.Q) && (Input.GetKeyDown(KeyCode.K) || Input.GetKeyDown(KeyCode.K))) {
            //play sound effect
        }

        speedometerNeedle.localRotation = Quaternion.Euler(0, 0, 75 - rb.linearVelocity.magnitude * 5);

        if (Input.GetKey(KeyCode.R) && !TrackManager.hasEnded)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            TrackManager.hasEnded = false;
            TrackManager.hasStarted = false;
        }
    }
}
