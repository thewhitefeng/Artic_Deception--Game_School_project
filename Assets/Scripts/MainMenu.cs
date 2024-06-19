using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class MainMenu : MonoBehaviour
{
    public GameObject OptionsPanel;
    private void Awake()
    {
       OptionsPanel.SetActive(false);
    }
    public void OpenOptions()
    {
        OptionsPanel.SetActive(true);
    }
    public void CloseOption()
    {
        OptionsPanel.SetActive(false);
    }
    public void PlayGame()
    {
        SceneManager.LoadScene("TestLevel");
    }
    public void QuitGame()
    {
            Application.Quit();
    }
}
