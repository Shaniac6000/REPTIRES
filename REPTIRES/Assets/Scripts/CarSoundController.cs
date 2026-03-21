using UnityEngine;

public class CarSoundController : MonoBehaviour
{
    [SerializeField] private AudioSource decelerate1;
    [SerializeField] private AudioSource speed1;
    [SerializeField] private AudioSource accelerate1;

    [SerializeField] private AudioSource decelerate2;
    [SerializeField] private AudioSource speed2;
    [SerializeField] private AudioSource accelerate2;

    [SerializeField] private AudioSource decelerate3;
    [SerializeField] private AudioSource speed3;
    [SerializeField] private AudioSource accelerate3;
    
    [SerializeField] private float minPitch = 0.8f;
    [SerializeField] private float maxPitch = 1.5f;
    [SerializeField] private float maxSpeed = 10f;
    [SerializeField] private float fadeSpeed = 2f;
    
    private Rigidbody rb;
    private float targetVolume = 0f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        speed1.loop = true;
        speed1.volume = 0f;
        if (!speed1.isPlaying)
        {
            speed1.Play();
        }
    }

    // Update is called once per frame
    void Update()
    {
        float currentSpeed = rb.linearVelocity.magnitude;
        
        if (currentSpeed > 1f)
        {
            // Set target volume to full
            targetVolume = 0.4f;
            
            // Adjust pitch based on speed
            float speedFraction = Mathf.Clamp01(currentSpeed / maxSpeed);
            speed1.pitch = Mathf.Lerp(minPitch, maxPitch, speedFraction);
        }
        else
        {
            // Set target volume to silent
            targetVolume = 0f;
        }
        
        // Fade volume towards target
        speed1.volume = Mathf.Lerp(speed1.volume, targetVolume, fadeSpeed * Time.deltaTime);
    }
}
