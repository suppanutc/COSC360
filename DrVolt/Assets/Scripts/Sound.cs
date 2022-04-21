using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Sound : MonoBehaviour
{
    
    
    public AudioSource contLoop;
    public AudioSource winningMusic;
    static AudioSource soundTemp;
    bool winningMusicPlayed;
    // Start is called before
    private void Start()
    {
        DontDestroyOnLoad(this);
    }
    public void Update()
    {
        if (SceneManager.GetActiveScene().name == "MainMenu" )
        {
            Destroy(gameObject);
        }
        if (!winningMusic.isPlaying && winningMusicPlayed)
        {
            winningMusic.Stop();
            winningMusicPlayed = false;
        }
    }
    public static void PlaySound(AudioClip clip, float volume)
    {
        soundTemp.clip = clip;
        soundTemp.volume = volume;
    }
    

    public static IEnumerator StartFade(AudioSource audioSource, float duration, float targetVolume)
    {
        float currentTime = 0;
        float start = audioSource.volume;

        while (currentTime < duration)
        {
            currentTime += Time.deltaTime;
            audioSource.volume = Mathf.Lerp(start, targetVolume, currentTime / duration);
            yield return null;
        }
        yield break;

    }

    public void stopLoopMusic()
    {
        contLoop.Stop();

    }
    public void startGameMusic()
    {
            contLoop.Play();
        StartCoroutine(StartFade(contLoop, 2f, 1f));

    }

}
