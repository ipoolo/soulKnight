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
            PlayWithNamePlayer(musicPlayer, clipName, 0.0f);
        }
    }

    public void PlaySound(string clipName)
    {
        if (clipName.Length > 0)
        {
            AudioClip clip = Resources.Load<AudioClip>(clipName);
            PlayWithNamePlayer(soundPlayer, clipName, 0.0f);
        }
    }

    public void PlaySoundWithTime(string clipName,float time)
    {
        if (clipName.Length > 0)
        {
            AudioClip clip = Resources.Load<AudioClip>(clipName);
            PlayWithNamePlayer(soundPlayer, clipName, time);
        }
    }

    public void PlaySoundBulletWithTime(string clipName, float time)
    {
        if (clipName.Length > 0)
        {
            AudioClip clip = Resources.Load<AudioClip>(clipName);
            if (!soundBulletPlayer.isPlaying) {
                PlayWithNamePlayer(soundBulletPlayer, clipName, time);
            }
            else if(!soundBulletPlayer2.isPlaying) {
                PlayWithNamePlayer(soundBulletPlayer2, clipName, time);
            }
        }
    }
    private void PlayWithNamePlayer(AudioSource player,string clipName,float time)
    {
        AudioClip clip = Resources.Load<AudioClip>(clipName);
        player.clip = clip;
        player.time = time;
        player.Play();
    }
}
