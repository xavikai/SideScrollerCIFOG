using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class SceneMusic
{
    public string sceneName;
    public AudioClip musicClip;
}

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    [Header("Configuració de Música per Escena")]
    public List<SceneMusic> sceneMusicList;

    [Header("Configuració d'àudio")]
    public AudioSource musicSource;
    public float fadeDuration = 1f;
    public float musicVolume = 0.5f;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        if (musicSource == null)
        {
            musicSource = gameObject.AddComponent<AudioSource>();
            musicSource.loop = true;
            musicSource.playOnAwake = false;
            musicSource.volume = musicVolume;
        }

        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        PlayMusicForScene(scene.name);
    }

    public void PlayMusicForScene(string sceneName)
    {
        AudioClip clipToPlay = null;

        foreach (var sceneMusic in sceneMusicList)
        {
            if (sceneMusic.sceneName == sceneName)
            {
                clipToPlay = sceneMusic.musicClip;
                break;
            }
        }

        if (clipToPlay != null && musicSource.clip != clipToPlay)
        {
            StartCoroutine(FadeInNewMusic(clipToPlay));
        }
    }

    private IEnumerator FadeInNewMusic(AudioClip newClip)
    {
        // Fade out actual
        float startVolume = musicSource.volume;
        for (float t = 0; t < fadeDuration; t += Time.deltaTime)
        {
            musicSource.volume = Mathf.Lerp(startVolume, 0f, t / fadeDuration);
            yield return null;
        }

        musicSource.Stop();
        musicSource.clip = newClip;
        musicSource.Play();

        // Fade in
        for (float t = 0; t < fadeDuration; t += Time.deltaTime)
        {
            musicSource.volume = Mathf.Lerp(0f, musicVolume, t / fadeDuration);
            yield return null;
        }

        musicSource.volume = musicVolume;
    }

    public void SetMusicVolume(float newVolume)
    {
        musicVolume = newVolume;
        musicSource.volume = newVolume;
    }
}
