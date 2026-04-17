using UnityEngine;

public class MusicHandler : MonoBehaviour
{

    private AudioSource[] playlist;
    public int currentlyPlaying = 0;
    public float playVolume = 80f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        playlist = this.GetComponents<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        for (int i = 0; i < playlist.Length; i++)
        {
            if (currentlyPlaying == i)
            {
                playlist[i].volume = playVolume;
            }
            else
            {
                playlist[i].volume = 0;
            }
        }

        if (Input.GetKeyDown(KeyCode.P))
        {
            if (currentlyPlaying < playlist.Length - 1)
            {
                currentlyPlaying++;
            }
            else
            {
                currentlyPlaying = 0;
            }
        }

        if (Input.GetKeyDown(KeyCode.O))
        {
            if (currentlyPlaying > 0)
            {
                currentlyPlaying--;
            }
            else
            {
                currentlyPlaying = playlist.Length - 1;
            }
        }
    }
}
