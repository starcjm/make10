using System.Text;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum E_BGM
{
    BGM_ONE,
}

public enum E_SFX
{
    BUTTON,
    //SHAPE_BLOCK_RESET,
    SHAPE_BLOCK_UP,
    SHAPE_BLOCK_ROT,
    BLOCK_MERGE,
    BLOCK_DROP,
    OPEN_POPUP,
}

public class SoundManager : Singleton<SoundManager>
{
    private const int SFX_SOURCE_COUNT = 3;

    public List<AudioClip> bgms = new List<AudioClip>();
    public List<AudioClip> sfxs = new List<AudioClip>();
        
    private AudioSource bgmSource;
    private AudioSource[] sfxSource = new AudioSource[SFX_SOURCE_COUNT];


    public void Init()
    {       
        float volume = PlayerPrefs.GetFloat("volumeBGM", 1);

        var _bgmAudioSource = new GameObject("bgmSource");
        _bgmAudioSource.transform.SetParent(Instance.transform);

        bgmSource = _bgmAudioSource.AddComponent<AudioSource>();
        bgmSource.volume = volume;
        bgmSource.playOnAwake = false;
        bgmSource.loop = true;

        volume = PlayerPrefs.GetFloat("volumeSFX", 1);

        for (int i = 0; i < sfxSource.Length; i++)
        {
            var _sfxAudioSource = new GameObject(string.Format("sfxAudioSource{0}", i + 1));
            _sfxAudioSource.transform.SetParent(Instance.transform);
            sfxSource[i] = _sfxAudioSource.AddComponent<AudioSource>();
            sfxSource[i].playOnAwake = false;
            sfxSource[i].volume = volume;
            sfxSource[i].loop = false;
        }
        if(UserInfo.Instance.IsSound())
        {
            //PlayBGM(E_BGM.BGM_ONE);
        }
        else
        {
            StopBGM();
        }
    }

    public void PlaySFX(E_SFX type)
    {
        PlaySFX(type, false, 1);
    }

    public void PlaySFX(E_SFX type, bool loop, float pitch)
    {
        if (!UserInfo.Instance.IsSound())
        {
            return;
        }
        AudioSource a = GetEmptySource();
        a.loop = loop;
        a.pitch = pitch;
        a.clip = sfxs[(int)type];
        a.Play();
    }

    private AudioSource GetEmptySource()
    {
        int lageindex = 0;
        float lageProgress = 0;
        for (int i = 0; i < sfxSource.Length; i++)
        {
            if (!sfxSource[i].isPlaying)
            {
                return sfxSource[i];
            }

            float progress = sfxSource[i].time / sfxSource[i].clip.length;
            if (progress > lageProgress)
            {
                lageindex = i;
                lageProgress = progress;
            }
        }
        return sfxSource[lageindex];
    }

    public void PlayBGM(E_BGM type)
    {
        if(!UserInfo.Instance.IsSound())
        { 
            return;
        }
        AudioClip _changeClip;
        _changeClip = bgms[(int)type];
        if (_changeClip == null)
        {
            return;
        }

        bgmSource.clip = _changeClip;
        bgmSource.Play();
    }

    public void StopBGM()
    {
        if(bgmSource)
        {
            bgmSource.Stop();
        }
    }

    public void SetPitch(float pitch)
    {
        bgmSource.pitch = pitch;
    }

    public void ChangeBGMVolume(float volume)
    {
        PlayerPrefs.SetFloat("volumeBGM", volume);
        bgmSource.volume = volume;
    }

    public void ChangeSFXVolume(float volume)
    {
        PlayerPrefs.SetFloat("volumeSFX", volume);
        for (int i = 0; i < sfxSource.Length; i++)
        {
            sfxSource[i].volume = volume;
        }
    }
}