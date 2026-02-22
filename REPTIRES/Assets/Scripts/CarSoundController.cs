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
    
    private RigidBody rigidBody = this.getComponent(RigidBody);

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
