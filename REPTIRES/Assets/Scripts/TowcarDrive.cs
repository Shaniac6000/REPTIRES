using System.Diagnostics;
using UnityEngine;

public class TowcarDrive : MonoBehaviour
{
    [SerializeField] GameObject PlayerCar;
    [SerializeField] float maxTravelDistance = 590f; // Maximum distance to tow before stopping

    private Rigidbody playerRb;
    private Rigidbody myRb;
    private Vector3 startPosition;
    private bool hasFinishedTowing;
    
    // Flag to track whether the velocity matching should happen
    private bool isTowing = false; 

    void Start()
    {
        if (PlayerCar != null)
        {
            playerRb = PlayerCar.GetComponent<Rigidbody>();
        }
        else
        {
            UnityEngine.Debug.LogError("PlayerCar reference is not set in the inspector.");
        }

        myRb = GetComponent<Rigidbody>();
        startPosition = myRb.position;
    }

    void FixedUpdate()
    {
        // Calculate the distance traveled since towing started
        float distanceTraveled = Vector3.Distance(startPosition, transform.position);
        UnityEngine.Debug.Log(isTowing);

        if (distanceTraveled >= maxTravelDistance)
        {
            // We reached 600 units. Stop towing and halt Z-axis movement.
            isTowing = false;
            hasFinishedTowing = true;
                
            // Bring the object to a stop on the Z axis
            myRb.linearVelocity = new Vector3(myRb.linearVelocity.x, myRb.linearVelocity.y, 0f);
            return; // Exit the function early this frame
        }

        // If we are towing and haven't finished, match the player's Z velocity

        float targetZVelocity = playerRb.linearVelocity.z;
        myRb.linearVelocity = new Vector3(myRb.linearVelocity.x, myRb.linearVelocity.y, Mathf.Clamp(targetZVelocity - 2, -1000f, 0f));
    }

    // This method is called automatically when another collider enters this object's trigger collider
    private void OnTriggerEnter(Collider other)
    {
        // Check if the colliding object has the tag "Player"
        if (other.CompareTag("Player"))
        {
            isTowing = true;
        }
    }
}