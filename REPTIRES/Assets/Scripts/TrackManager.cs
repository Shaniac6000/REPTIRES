using System.Collections;
using System.Threading;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TrackManager : MonoBehaviour
{
    public Rigidbody car;
    public CarController controller;
    public TextMeshProUGUI timerText;
    public TextMeshProUGUI centerText;
    public float currentTime = 0f;

    public float goldTime;
    public float silverTime;
    public float bronzeTime;

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
            if (currentTime <= goldTime)
            {
                timerText.color = Color.gold;
            }
            else if (currentTime <= silverTime)
            {
                timerText.color = Color.silver;
            }
            else if (currentTime <= bronzeTime)
            {
                timerText.color = Color.sandyBrown;
            }
            else
            {
                timerText.color = Color.white;
            }
        }

        else if (hasEnded)
        {
            if (currentTime <= goldTime)
            {
                centerText.color = Color.gold;
                centerText.text = "GOLDEN GATOR!!! Fantastic Driving!";
            }
            else if (currentTime <= silverTime)
            {
                centerText.color = Color.silver;
                centerText.text = "SILVER SALAMANDER!!! Nice Work!";
            }
            else if (currentTime <= bronzeTime)
            {
                centerText.color = Color.sandyBrown;
                centerText.text = "BRONZE BOOMSLANG!!! You're getting there!";
            }
            else
            {
                centerText.text = "AT LEAST YOU TRIED!!! :)";
            }
            centerText.enabled = true;
            StartCoroutine(EndLevel());
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

    private IEnumerator EndLevel()
    {
        yield return new WaitForSeconds(5);
        hasEnded = false;
        hasStarted = false;
        LevelManagement.SetHighest(SceneManager.GetActiveScene().buildIndex);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}
