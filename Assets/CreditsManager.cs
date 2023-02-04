using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class CreditsManager : MonoBehaviour
{
    private void Start()
    {
        AudioManager.instance.PlayMusicFade("intro", true);
    }
    public void GoMainMenu()
    {
        AudioManager.instance.StopMusic();
        SceneManager.LoadScene("MainMenu");

    }
}
