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


    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void PlaySFX(AudioClip sfxClip)
    {
        this.vfxAudioSourse.clip = sfxClip;
        vfxAudioSourse.PlayOneShot(sfxClip);
    }
}
