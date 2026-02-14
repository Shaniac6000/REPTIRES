using System.Collections;
using TMPro;
using UnityEngine;

public class TrackManager : MonoBehaviour
{
    public Rigidbody car;
    public CarController controller;
    public TextMeshProUGUI timerText;
    public TextMeshProUGUI centerText;
    public float currentTime = 0f;

    static public bool hasStarted = false;
    static public bool hasEnded = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        centerText.enabled = true;
        StartCoroutine(StartTimer(5));
        car.isKinematic = true;
        controller.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (hasStarted && !hasEnded)
        {
            currentTime += Time.deltaTime;

            float min = Mathf.FloorToInt(currentTime / 60);
            float sec = Mathf.FloorToInt(currentTime % 60);
            float ms = Mathf.FloorToInt((currentTime % 1.00f) * 100f);
            timerText.text = string.Format("{0:00}:{1:00}", min, sec) + ":" + ms.ToString();
        }

        else if (hasEnded)
        {
            centerText.text = "end!";
            centerText.enabled = true;
        }
    }

    IEnumerator StartTimer(int n)
    {
        int timer = n;
        while (timer > 0)
        {
            centerText.text = timer.ToString();
            yield return new WaitForSeconds(1);
            timer--;
        }
        car.isKinematic = false;
        controller.enabled = true;
        centerText.text = "START!";
        hasStarted = true;
        yield return new WaitForSeconds(1);
        centerText.enabled = false;
    }

    public static void EndTrack()
    {
        hasEnded = true;
    }
}
