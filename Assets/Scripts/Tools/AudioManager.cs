using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : Singleton<AudioManager>
{

    public AudioSource musicPlayer;
    public AudioSource soundPlayer;
    public AudioSource soundBulletPlayer;
    public AudioSource soundBulletPlayer2;
    public AudioSource soundBulletPlayer3;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PlayMusic(string clipName)
    {
        if (clipName.Length > 0)
        {
            AudioClip clip = Resources.Load<AudioClip>(clipName);
            musicPlayer.clip = clip;
            musicPlayer.Play();
        }
    }

    public void PlaySound(string clipName)
    {
        if (clipName.Length > 0)
        {
            AudioClip clip = Resources.Load<AudioClip>(clipName);
            soundPlayer.clip = clip;
            soundPlayer.Play();
        }
    }

    public void PlaySoundWithTime(string clipName,float time)
    {
        if (clipName.Length > 0)
        {
            AudioClip clip = Resources.Load<AudioClip>(clipName);
            soundPlayer.clip = clip;
            soundPlayer.PlayDelayed(time);
        }
    }

    public void PlaySoundBulletWithTime(string clipName, float time)
    {
        if (clipName.Length > 0)
        {
            AudioClip clip = Resources.Load<AudioClip>(clipName);
            if (!soundBulletPlayer.isPlaying) { 
                soundBulletPlayer.clip = clip;
                soundBulletPlayer.PlayDelayed(time);
            }
            else if(!soundBulletPlayer2.isPlaying) {
                soundBulletPlayer2.clip = clip;
                soundBulletPlayer2.PlayDelayed(time);
            }
        }
    }
}
