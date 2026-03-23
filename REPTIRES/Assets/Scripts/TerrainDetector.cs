using UnityEngine;

public class TerrainDetector : MonoBehaviour
{
    private CarController player;
    private BoxCollider bc;
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<CarController>();
        bc = GetComponent<BoxCollider>();
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Slowing"))
        {
            player.slowed = true;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Slowing"))
        {
            player.slowed = false;
        }
    }
}
