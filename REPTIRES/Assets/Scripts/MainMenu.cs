using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private GameObject titleScreen;
    [SerializeField] private GameObject levelSelect;

    [SerializeField] private Button[] levels;

    // Start is called before the first frame update
    private void Start()
    {
        for (int i = 0; i < levels.Length; i++)
        {
            if (i > LevelManagement.highestLevel)
                levels[i].interactable = false;
        }
    }

    public void EnableLevelSelect()
    {
        titleScreen.SetActive(false);
        levelSelect.SetActive(true);
    }

    public void EnableTitle()
    {
        levelSelect.SetActive(false);
        titleScreen.SetActive(true);
    }

    public void LoadLevel(int x)
    {
        SceneManager.LoadScene(x);
    }
}
