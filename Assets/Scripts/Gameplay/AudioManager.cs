using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    [SerializeField]
    private List<SoundScriptable> databaseManager;
    [SerializeField]
    private AudioSource musicAudioSource;
    [SerializeField]
    private List<AudioSource> soundAudioSourceList;

    private bool isPlayingMusic;
    private string playingMusicName;

    private void Awake()
    {
        instance = this;
        DontDestroyOnLoad(this);
        PlayerPrefs.SetFloat("MusicVolume", 0.6f);
        PlayerPrefs.SetFloat("SFXVolume", 1f);
    }

    private SoundScriptable GetSoundScriptable(string name)
    {
        for (int i = 0; i < databaseManager.Count; i++)
        {
            if (databaseManager[i].Name == name)
            {
                return databaseManager[i];
            }
        }

        return null;
    }

    public void PlayMusic(string name, bool isContinuing = false)
    {
        if (isContinuing && isPlayingMusic && playingMusicName == name)
            return;

        StopAllCoroutines();
        SoundScriptable audio = GetSoundScriptable(name);

        musicAudioSource.Stop();
        musicAudioSource.clip = audio.AudioClip;
        musicAudioSource.volume = PlayerPrefs.GetFloat("MusicVolume");
        musicAudioSource.loop = true;
        musicAudioSource.Play();
        isPlayingMusic = true;
        playingMusicName = name;
    }

    public void PlayMusicFade(string name, bool isContinuing = false)
    {
        if (isContinuing && isPlayingMusic && playingMusicName == name)
            return;

        StopAllCoroutines();
        SoundScriptable audio = GetSoundScriptable(name);

        musicAudioSource.Stop();
        musicAudioSource.clip = audio.AudioClip;
        VolumeUp(PlayerPrefs.GetFloat("MusicVolume"));
        musicAudioSource.loop = true;
        musicAudioSource.Play();
        isPlayingMusic = true;
        playingMusicName = name;
    }

    public void ChangeMusic(string name)
    {
        SoundScriptable audio = GetSoundScriptable(name);

        SongChange(musicAudioSource.volume, audio);

        musicAudioSource.loop = true;
    }

    private void SongChange(float volume, SoundScriptable audio)
    {
        VolumeChange(volume);
        musicAudioSource.clip = audio.AudioClip;
        musicAudioSource.Play();
    }

    private void VolumeUp(float volume)
    {
        musicAudioSource.volume = 0f;
        DOTween.To(() => musicAudioSource.volume, x => musicAudioSource.volume = x, volume, 0.5f);
    }

    private void VolumeDown(float volume)
    {
        musicAudioSource.volume = volume;
        DOTween.To(() => musicAudioSource.volume, x => musicAudioSource.volume = x, 0, 0.5f).OnComplete(() =>
        {
            musicAudioSource.Stop();
        });
    }

    private void VolumeChange(float volume)
    {
        musicAudioSource.volume = volume;
        DOTween.To(() => musicAudioSource.volume, x => musicAudioSource.volume = x, 0, volume).OnComplete(() =>
        {
            musicAudioSource.Stop();
            DOTween.To(() => musicAudioSource.volume, x => musicAudioSource.volume = x, volume, volume);
        });
    }

    public void ChangeMainVolume(float volume)
    {
        musicAudioSource.volume = volume;
    }

    public void ChangeSFXVolume(float volume)
    {
        for (int i = 0; i < soundAudioSourceList.Count; i++)
        {
            soundAudioSourceList[i].volume = volume;
        }
    }

    public void StopMusic()
    {
        StopAllCoroutines();
        musicAudioSource.Stop();
        isPlayingMusic = false;
        playingMusicName = "";
    }

    public void StopMusicFade()
    {
        StopAllCoroutines();
        VolumeDown(PlayerPrefs.GetFloat("MusicVolume"));
        isPlayingMusic = false;
        playingMusicName = "";
    }

    public void PlaySFX(string name)
    {
        bool played = false;
        SoundScriptable audio = GetSoundScriptable(name);

        for (int i = 0; i < soundAudioSourceList.Count; i++)
        {
            AudioSource soundAudioSource = soundAudioSourceList[i];
            if (!soundAudioSource.isPlaying)
            {
                soundAudioSource.Stop();
                soundAudioSource.clip = audio.AudioClip;
                soundAudioSource.volume = PlayerPrefs.GetFloat("SFXVolume");
                soundAudioSource.loop = false;
                soundAudioSource.Play();
                played = true;
                break;
            }
        }

        if (!played)
            throw new System.Exception($"AudioManager couldn't play SFX { name } because there were no free audio sources.");
    }
}