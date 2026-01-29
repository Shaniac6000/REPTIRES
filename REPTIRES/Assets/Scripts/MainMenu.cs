using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private GameObject titleScreen;
    [SerializeField] private GameObject levelSelect;

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
