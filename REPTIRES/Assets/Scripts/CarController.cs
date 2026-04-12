using System;
using System.Runtime.CompilerServices;
using TMPro;
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
    [SerializeField] private Transform speedometerNeedle;
    [SerializeField] private TextMeshProUGUI gearShiftUI;
    [SerializeField] private Animator gator;
    private Transform gatorTransform;
    private SpriteRenderer gatorSprite;
    [SerializeField] private Animator turtle;
    private Transform turtleTransform;
    private Vector3 turtleInitPosition;
    private Vector3 turtleClutchPosition;
    [SerializeField] private Transform gasPedal;
    [SerializeField] private Transform brakePedal;
    [SerializeField] private Transform clutchPedal;
    private Transform currentPedal;
    private float initPedalY;
    private float downPedalY;
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
    private float forceMult = 1300000;
    public bool slowed = false;
    private bool gas;
    private bool braking = false;
    private bool clutch = false;
    private bool turtleMove = false;
    private float heat = 7.5f;
    private float heatSpeed = .75f;
    private float normalRotateDegrees;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        frontRightWheel.GetComponent<WheelCollider>().steerAngle = 90;
        frontLeftWheel.GetComponent<WheelCollider>().steerAngle = 90;
        backRightWheel.steerAngle = 90;
        backLeftWheel.steerAngle = 90;
        gatorTransform = gator.gameObject.transform;
        gatorSprite = gator.gameObject.GetComponent<SpriteRenderer>();
        turtleTransform = turtle.gameObject.transform;
        turtleInitPosition = turtleTransform.localPosition;
        turtleClutchPosition = new Vector3(-5.13999987f,4.01000023f,-6.69999981f);
        currentPedal = gasPedal;
        initPedalY = currentPedal.localPosition.y;
        downPedalY = 2.46f;
        normalRotateDegrees = rotateDegrees;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        frontRightWheel.GetComponent<WheelCollider>().motorTorque = currentAcceleration;
        frontLeftWheel.GetComponent<WheelCollider>().motorTorque = currentAcceleration;        

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

        if (rb.linearVelocity.magnitude <= .01 && !(gas && Input.GetAxisRaw("Vertical") > 0))
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
        if (Input.GetKeyDown(KeyCode.S) && !braking && !clutch)
        {
            turtle.SetBool("Gas", true);
            turtle.SetBool("Brake", false);
            turtle.SetBool("Clutch", false);
            turtleMove = false;
            currentPedal = gasPedal;
        }
        else if (Input.GetKeyDown(KeyCode.Space) && !gas && !clutch)
        {
            turtle.SetBool("Gas", false);
            turtle.SetBool("Brake", true);
            turtle.SetBool("Clutch", false);
            turtleMove = false;
            currentPedal = brakePedal;
        }
        else if (Input.GetKeyDown(KeyCode.Q) && !gas && !braking)
        {
            turtle.SetBool("Gas", false);
            turtle.SetBool("Brake", false);
            turtle.SetBool("Clutch", true);
            turtleMove = true;
            currentPedal = clutchPedal;
        }

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

        if (Input.GetAxisRaw("Vertical") > 0)
        {
            if (turtle.GetBool("Gas"))
            {
                currentAcceleration = Input.GetAxisRaw("Vertical") * acceleration;
                gas = true;
                heat += heatSpeed * Time.deltaTime;
            }
            else if (turtle.GetBool("Brake"))
            {
                currentBrakeForce = brakeForce;
                braking = true;
            }

            else if (turtle.GetBool("Clutch"))
            {
                clutch = true;
            }
            turtle.SetBool("Down", true);
            currentPedal.localPosition = new Vector3(currentPedal.localPosition.x, downPedalY, currentPedal.localPosition.z);
        }
        else
        {
            currentAcceleration = 0;
            currentBrakeForce = 0;
            gas = false;
            braking = false;
            clutch = false;
            turtle.SetBool("Down", false);
            gasPedal.localPosition = new Vector3(gasPedal.localPosition.x, initPedalY, gasPedal.localPosition.z);
            brakePedal.localPosition = new Vector3(brakePedal.localPosition.x, initPedalY, brakePedal.localPosition.z);
            clutchPedal.localPosition = new Vector3(clutchPedal.localPosition.x, initPedalY, clutchPedal.localPosition.z);
        }

        if (clutch && !braking && !gas && !slowed)
        {
            if (Input.GetKeyDown(KeyCode.U) && rb.linearVelocity.magnitude >= currentMaxGearSpeed - gearChangeOffset && gear < 3 && gear > 0)
            {
                gear += 1;
                currentMaxGearSpeed = gear * baseGearSpeed;
                currentMinGearSpeed = (gear - 1) * baseGearSpeed;
                gearShiftUI.text = "GEAR: " + gear;
            }

            if (Input.GetKeyDown(KeyCode.U) && gear == 0 && rb.linearVelocity.magnitude <= .01f) {
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
        else if (slowed && clutch && (Input.GetKeyDown(KeyCode.U) || Input.GetKeyDown(KeyCode.M))) {
            //play sound effect
        }

        if (turtleMove)
        {
            turtleTransform.localPosition = Vector3.Lerp(turtleTransform.localPosition, turtleClutchPosition, 5 * Time.deltaTime);
        }
        else
        {
            turtleTransform.localPosition = Vector3.Lerp(turtleTransform.localPosition, turtleInitPosition, 5 * Time.deltaTime);
        }

        speedometerNeedle.localRotation = Quaternion.Euler(0, 0, 75 - rb.linearVelocity.magnitude * 5);

        if (Input.GetKeyDown(KeyCode.K))
        {
            heat -= heatSpeed * 10f;
        }

        heat = Mathf.Clamp(heat, 0, 20);
        
        if (heat < 5 || heat > 15)
        {
            rotateDegrees = normalRotateDegrees / 3;
        }
        else
        {
            rotateDegrees = normalRotateDegrees;
        }

        if (heat < 7.5f)
        {
            float coldColor = heat / 7.5f;
            coldColor = Mathf.Clamp(coldColor, .4f, 1);
            gatorSprite.color = new Color(coldColor, coldColor, 1);  
        }
        else if (heat > 12.5f)
        {
            float hotColor = (20 - heat) / 7.5f;
            hotColor = Mathf.Clamp(hotColor, .4f, 1);
            gatorSprite.color = new Color(1, hotColor, hotColor);
        }
        else
        {
            gatorSprite.color = new Color(1, 1, 1);
        }

        if (Input.GetKeyDown(KeyCode.R) && !TrackManager.hasEnded)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            TrackManager.hasEnded = false;
            TrackManager.hasStarted = false;
        }

        if (Input.GetKeyDown(KeyCode.F) && rb.linearVelocity.magnitude < 1)
        {
            if (transform.up.y <= .1f)
            {
                rb.AddTorque(transform.right * -forceMult);
            }
            else if (transform.up.y <= .8f)
            {
                if (transform.forward.y >= .25f)
                {
                    rb.AddTorque(transform.right * forceMult / 2);
                }
                else if (transform.forward.y <= -.25f)
                {
                    rb.AddTorque(transform.right * -forceMult / 2);
                }
                else
                {
                    rb.AddTorque(transform.right * -forceMult);
                }
            }
        }
    }
}
