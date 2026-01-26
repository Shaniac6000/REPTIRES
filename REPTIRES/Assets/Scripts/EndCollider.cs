using UnityEngine;

public class EndCollider : MonoBehaviour
{
    GameObject trackManager;
    TrackManager tmScript;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        trackManager = GameObject.FindGameObjectWithTag("TrackManager");
        tmScript = trackManager.GetComponent<TrackManager>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Debug.Log("track over!");
            TrackManager.EndTrack();
        }
    }
}
