using UnityEngine;

public class TerrainDetector : MonoBehaviour
{
    private CarController player;
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<CarController>();
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
