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
    public string backgroudStr;
    // Start is called before the first frame update
    void Start()
    {
        AudioClip clip = Resources.Load<AudioClip>("Voices/" + backgroudStr);
        musicPlayer.clip = clip;
        musicPlayer.loop = true;
        musicPlayer.Play();
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
            PlayWithNamePlayer(musicPlayer, clipName, 0.0f, false);
        }
    }

    public void PlaySound(string clipName)
    {

        if (clipName.Length > 0)
        {

            AudioClip clip = Resources.Load<AudioClip>(clipName);
            PlayWithNamePlayer(soundPlayer, clipName, 0.0f, false);
        }
    }

    public void PlaySoundWithTime(string clipName,float time)
    {
        if (clipName.Length > 0)
        {
            AudioClip clip = Resources.Load<AudioClip>(clipName);
            PlayWithNamePlayer(soundPlayer, clipName, time, false);
        }
    }

    public void PlaySoundWithTimeLoop(string clipName, float time)
    {
        if (clipName.Length > 0)
        {
            AudioClip clip = Resources.Load<AudioClip>(clipName);
            PlayWithNamePlayer(soundPlayer, clipName, time, true);
            soundPlayer.loop = true;
        }
    }

    public void PlaySoundStop()
    {
        soundPlayer.Stop();
    }

    public void PlaySoundStopByLoop()
    {
        soundPlayer.loop = false;
    }

    public void PlaySoundBulletWithTime(string clipName, float time)
    {
        if (clipName.Length > 0)
        {
            AudioClip clip = Resources.Load<AudioClip>(clipName);
            if (!soundBulletPlayer.isPlaying) {
                PlayWithNamePlayer(soundBulletPlayer, clipName, time, false);
            }
            else if(!soundBulletPlayer2.isPlaying) {
                PlayWithNamePlayer(soundBulletPlayer2, clipName, time, false);
            }
        }
    }
    private void PlayWithNamePlayer(AudioSource player,string clipName,float time,bool loop)
    {
        AudioClip clip = Resources.Load<AudioClip>(clipName);
        player.clip = clip;
        player.time = time;
        soundPlayer.loop = loop;
        player.Play();
    }

    public void PlaySound3WithTime(string clipName, float time)
    {
        if (clipName.Length > 0)
        {
            AudioClip clip = Resources.Load<AudioClip>(clipName);
            PlayWithNamePlayer(soundBulletPlayer3, clipName, time, false);
        }
    }
}
