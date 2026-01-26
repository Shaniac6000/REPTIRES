using System.Collections;
using TMPro;
using UnityEngine;

public class TrackManager : MonoBehaviour
{
    public TextMeshProUGUI timerText;
    public float currentTime = 0f;

    public bool hasStarted = false;
    public bool hasEnded = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        StartCoroutine(Wait(5));
        hasStarted = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (hasStarted && !hasEnded)
        {
            currentTime += Time.deltaTime;

            float min = Mathf.FloorToInt(currentTime / 60);
            float sec = Mathf.FloorToInt(currentTime % 60);
            timerText.text = string.Format("{0:00}:{1:00}", min, sec);
        }
    }

    IEnumerator Wait(float s)
    {
        yield return new WaitForSeconds(s);
    }
}
