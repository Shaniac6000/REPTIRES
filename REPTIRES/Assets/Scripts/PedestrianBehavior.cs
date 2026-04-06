using UnityEngine;

public class PedestrianBehavior : MonoBehaviour
{

    public GameObject pointA;
    public GameObject pointB;

    AudioSource scream;

    GameObject target;

    public float speed = 0.5f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        target = pointA;

        scream = GetComponent<AudioSource>();
        speed += Random.Range(-1.0f, 1.0f);
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, target.transform.position, speed * Time.deltaTime);

        if (Vector3.Distance(transform.position, target.transform.position) < 0.1f)
        {
            if (target == pointA) {
                target = pointB;
            } else
            {
                target = pointA;
            }
        }

    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player") && !scream.isPlaying)
        {
            scream.pitch = Random.Range(0.6f, 1.4f);
            scream.Play();
            //Debug.Log("screamed");
        }
    }
}
