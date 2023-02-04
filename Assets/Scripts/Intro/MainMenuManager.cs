using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class MainMenuManager : MonoBehaviour
{
    private void Start()
    {
        
        AudioManager.instance.PlayMusicFade("intro",true);
    }
    public void GoMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
        AudioManager.instance.StopMusicFade();
    }
    public void GoCredits()
    {
        SceneManager.LoadScene("Credits");
    }
    public void GoToIntroScene()
    {
        SceneManager.LoadScene("IntroScene");
    }
    public void Close()
    {
        Application.Quit();
    }
}
