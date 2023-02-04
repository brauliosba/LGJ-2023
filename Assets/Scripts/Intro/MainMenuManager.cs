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
    public void GoCredits()
    {
        AudioManager.instance.StopMusic();
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
