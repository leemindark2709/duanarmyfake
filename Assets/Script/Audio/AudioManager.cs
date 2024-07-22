using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public AudioSource musicAudioSource;
    public AudioSource vfxAudioSourse;
    // Start is called before the first frame update
    public AudioClip ButtonClick;
    public AudioClip SpawBulletSound;
    public AudioClip DameSender; 
    public AudioClip HealSkill;
    public AudioClip NextTurn; 
    public AudioClip Ready;
    public AudioClip Go;
    public AudioClip BackgroundMusic;   
    public AudioClip Win; 
    public AudioClip Pow;
    public AudioClip Moving; 
    
    public AudioClip Flash;            


    
    public void PlaySFX(AudioClip sfxClip)
    {
        this.vfxAudioSourse.clip = sfxClip;
        vfxAudioSourse.PlayOneShot(sfxClip);
    }
    public void PlayBackgroundMusic()
    {
        musicAudioSource.clip = BackgroundMusic;
        musicAudioSource.loop = true; // Ensure the music loops
        musicAudioSource.Play();
    } 
    public void MovingSound()
    {
        musicAudioSource.clip = Moving;
        musicAudioSource.loop = true; // Ensure the music loops
        musicAudioSource.Play();
    }

    public void StopBackgroundMusic()
    {
        musicAudioSource.Stop();
    }
}
