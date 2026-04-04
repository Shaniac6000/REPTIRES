using UnityEngine;

public class TowcarDrive : MonoBehaviour
{
    [SerializeField] GameObject PlayerCar;
    [SerializeField] float maxTravelDistance = 590f; // Maximum distance to tow before stopping

    private Rigidbody playerRb;
    private Rigidbody myRb;
    
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
            Debug.LogWarning("PlayerCar is not assigned in TowcarDrive script!");
        }

        myRb = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        // Calculate the distance traveled since towing started
        float distanceTraveled = Vector3.Distance(startPosition, transform.position);

        if (distanceTraveled >= maxTravelDistance)
        {
            // We reached 600 units. Stop towing and halt Z-axis movement.
            isTowing = false;
            hasFinishedTowing = true;
                
            // Bring the object to a stop on the Z axis
            myRb.linearVelocity = new Vector3(myRb.linearVelocity.x, myRb.linearVelocity.y, 0f);
            return; // Exit the function early this frame
        }

        // If we haven't reached 600 units yet, keep copying the Z velocity
        float targetZVelocity = playerRb.linearVelocity.z;
        myRb.linearVelocity = new Vector3(myRb.linearVelocity.x, myRb.linearVelocity.y, targetZVelocity);
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