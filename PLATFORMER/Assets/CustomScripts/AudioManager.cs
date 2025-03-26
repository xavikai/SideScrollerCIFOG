using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

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
    public float fadeDuration = 0.5f; // fade ràpid
    public float musicVolume = 0.5f;

    [Header("Efectes de so globals")]
    public AudioClip platformMoveSound;
    public AudioClip platformFallSound;
    public AudioClip platformDestroySound;
    public AudioClip damageImpactSound;
    public AudioClip deadZoneFallSound;
    public AudioClip coinPickupSound;
    public AudioClip regeneratorPickupSound;

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
        float startVolume = musicSource.volume;
        for (float t = 0; t < fadeDuration; t += Time.deltaTime)
        {
            musicSource.volume = Mathf.Lerp(startVolume, 0f, t / fadeDuration);
            yield return null;
        }

        musicSource.Stop();
        musicSource.clip = newClip;
        musicSource.Play();

        for (float t = 0; t < fadeDuration; t += Time.deltaTime)
        {
            musicSource.volume = Mathf.Lerp(0f, musicVolume, t / fadeDuration);
            yield return null;
        }

        musicSource.volume = musicVolume;
    }

    public void PlaySound(AudioClip clip, Vector3 position, float volume = 1f)
    {
        if (clip != null)
        {
            AudioSource.PlayClipAtPoint(clip, position, volume);
        }
    }
}
