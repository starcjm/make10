using System.Text;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : Singleton<SoundManager>
{
    private const int SFX_SOURCE_COUNT = 3;

    private const string BGM_PATH = "sound/bgm/";
    private const string SE_PATH = "sound/se/";

    private Dictionary<string, AudioClip> _bgms = new Dictionary<string, AudioClip>();
    private Dictionary<string, AudioClip> _sfxs = new Dictionary<string, AudioClip>();
        
    private AudioSource _bgmSource;
    private AudioSource[] _sfxSource = new AudioSource[SFX_SOURCE_COUNT];

    private GameObject _soundSourceManager = null;

    public void Init()
    {
        _soundSourceManager = new GameObject("soundSourceManager");
        UnityEngine.Object.DontDestroyOnLoad(_soundSourceManager);
            
        float volume = PlayerPrefs.GetFloat("_volumeBGM", 1);

        var _bgmAudioSource = new GameObject("bgmSource");
        _bgmAudioSource.transform.parent = _soundSourceManager.transform;

        _bgmSource = _bgmAudioSource.AddComponent<AudioSource>();
        _bgmSource.volume = volume;
        _bgmSource.playOnAwake = false;
        _bgmSource.loop = true;

        volume = PlayerPrefs.GetFloat("_volumeSFX", 1);

        for (int i = 0; i < _sfxSource.Length; i++)
        {
            var _sfxAudioSource = new GameObject(string.Format("sfxAudioSource{0}", i + 1));
            _sfxAudioSource.transform.parent = _soundSourceManager.transform;
            _sfxSource[i] = _sfxAudioSource.AddComponent<AudioSource>();
            _sfxSource[i].playOnAwake = false;
            _sfxSource[i].volume = volume;
            _sfxSource[i].loop = false;
        }
        
        //LoadBGM();
        //LoadSFX();
    }

    //private void LoadBGM()
    //{
    //    var bgmTable = DataManager.instance.soundBGMTable.datas;
    //    foreach (var bgmData in bgmTable.Values)
    //    {
    //        StringBuilder stringBuilder = new StringBuilder();
    //        stringBuilder.Length = 0;
    //        stringBuilder.Append(BGM_PATH);
    //        stringBuilder.Append(bgmData.filename);
    //        var bgmName = stringBuilder.ToString();
    //        var bgm = ResourcesManager.instance.InstantiateObject<AudioClip>(bgmName);
    //        _bgms.Add(bgmData.filename, bgm);
    //    }

    //    var seTable = DataManager.instance.soundSETable.datas;
    //    foreach (var seData in seTable.Values)
    //    {
    //        StringBuilder stringBuilder = new StringBuilder();
    //        stringBuilder.Length = 0;
    //        stringBuilder.Append(SE_PATH);
    //        stringBuilder.Append(seData.filename);
    //        var seName = stringBuilder.ToString();
    //        var se = ResourcesManager.instance.InstantiateObject<AudioClip>(seName);
    //        _sfxs.Add(seData.filename, se);
    //    }
    //}


    public void PlaySFX(string name)
    {
        PlaySFX(name, false, 1);
    }

    public void PlaySFX(string name, bool loop, float pitch)
    {
        if (_sfxs.ContainsKey(name))
        {
            AudioSource a = GetEmptySource();
            a.loop = loop;
            a.pitch = pitch;
            a.clip = _sfxs[name];
            a.Play();
        }
    }

    private AudioSource GetEmptySource()
    {
        int lageindex = 0;
        float lageProgress = 0;
        for (int i = 0; i < _sfxSource.Length; i++)
        {
            if (!_sfxSource[i].isPlaying)
            {
                return _sfxSource[i];
            }

            float progress = _sfxSource[i].time / _sfxSource[i].clip.length;
            if (progress > lageProgress)
            {
                lageindex = i;
                lageProgress = progress;
            }
        }
        return _sfxSource[lageindex];
    }

    public void PlayBGM(string name)
    {
        if (_bgmSource == _bgms[name])
        {
            return;
        }
        AudioClip _changeClip;
        _changeClip = _bgms[name];
        if (_changeClip == null)
        {
            return;
        }

        _bgmSource.clip = _changeClip;
        _bgmSource.Play();
    }

    public void StopBGM()
    {
        _bgmSource.Stop();
    }

    public void SetPitch(float pitch)
    {
        _bgmSource.pitch = pitch;
    }

    public void ChangeBGMVolume(float volume)
    {
        PlayerPrefs.SetFloat("_volumeBGM", volume);
        _bgmSource.volume = volume;
    }

    public void ChangeSFXVolume(float volume)
    {
        PlayerPrefs.SetFloat("_volumeSFX", volume);
        for (int i = 0; i < _sfxSource.Length; i++)
        {
            _sfxSource[i].volume = volume;
        }
    }
}