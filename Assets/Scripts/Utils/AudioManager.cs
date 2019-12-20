using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using System;

public class AudioManager : MonoSingleton<AudioManager>
{
   

   

    private Dictionary<int, string> audioPathDict;      // 存放音频文件路径

    private AudioSource musicAudioSource;
    private AudioSource soundAudioSource;

   

    private Dictionary<int, AudioClip> audioClipDict ;       // 缓存音频文件

    private float musicVolume = 1;

    private float soundVolume = 1;

    private string musicVolumePrefs = "MusicVolume";

    private string soundVolumePrefs = "SoundVolume";

    

    void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
        

        audioPathDict = new Dictionary<int, string>()       // 这里设置音频文件路径。需要修改。 TODO
        {
            //{ 1, "Music/Background" },
            //{ 2, "Music/BattleScene" },
            { 11, "Music/Win"},
            { 12, "Music/Win"},
           
        };

        musicAudioSource = gameObject.AddComponent<AudioSource>();
        soundAudioSource = gameObject.AddComponent<AudioSource>();
        
        audioClipDict = new Dictionary<int, AudioClip>();
       
       
        // 先加载
        foreach (var item in audioPathDict)
        {

            if (!audioClipDict.ContainsKey(item.Key))
            {
                AudioClip ac = Resources.Load(audioPathDict[item.Key]) as AudioClip;
                if(ac)
                {
                    audioClipDict.Add(item.Key, ac);
                }
               
            }
        }

    }

    void Start()
    {
        // 从本地缓存读取声音音量
        if (PlayerPrefs.HasKey(musicVolumePrefs))
        {
            musicVolume = PlayerPrefs.GetFloat(musicVolumePrefs);
        }
        if (PlayerPrefs.HasKey(soundVolumePrefs))
        {
            soundVolume = PlayerPrefs.GetFloat(soundVolumePrefs);
        }
    }

    /// <summary>
    /// 播放背景音乐
    /// </summary>
    /// <param name="id"></param>
    /// <param name="loop"></param>
    public void PlayMusic(int id, bool loop = true)
    {
        // 通过Tween将声音淡入淡出
        DOTween.To(() => musicAudioSource.volume, value => musicAudioSource.volume = value, 0, 0.5f).OnComplete(() =>
        {
            musicAudioSource.clip = audioClipDict[id];
            musicAudioSource.clip.LoadAudioData();
            musicAudioSource.loop = loop;
            musicAudioSource.volume = musicVolume;
            musicAudioSource.Play();
            DOTween.To(() => musicAudioSource.volume, value => musicAudioSource.volume = value, musicVolume, 0.5f);
        });
    }

    /// <summary>
    /// 播放音效
    /// </summary>
    /// <param name="id"></param>
    public void PlaySound(int id, Action action = null)
    {
            soundAudioSource.clip = audioClipDict[id];
            soundAudioSource.clip.LoadAudioData();
            soundAudioSource.Play();
           // StartCoroutine(WaitPlayEnd(soundAudioSource, action));
        
        
    }

    /// <summary>
    /// 播放3d音效
    /// </summary>
    /// <param name="id"></param>
    /// <param name="position"></param>
    public void Play3dSound(int id, Vector3 position)
    {
        AudioClip ac = GetAudioClip(id);
        AudioSource.PlayClipAtPoint(ac, position);
    }

    /// <summary>
    /// 当播放音效结束后，将其移至未使用集合
    /// </summary>
    /// <param name="audioSource"></param>
    /// <returns></returns>
    IEnumerator WaitPlayEnd(AudioSource audioSource, Action action)
    {
        yield return new WaitUntil(() => { return !audioSource.isPlaying; });
       //xxx
        if (action != null)
        {
            action();
        }
    }

    /// <summary>
    /// 获取音频文件，获取后会缓存一份
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    private AudioClip GetAudioClip(int id)
    {
        if (!audioClipDict.ContainsKey(id))
        {
            if (!audioPathDict.ContainsKey(id))
                return null;
            AudioClip ac = Resources.Load(audioPathDict[id]) as AudioClip;
            audioClipDict.Add(id, ac);
        }
        return audioClipDict[id];
    }

   

  

   

    /// <summary>
    /// 修改背景音乐音量
    /// </summary>
    /// <param name="volume"></param>
    public void ChangeMusicVolume(float volume)
    {
        musicVolume = volume;
        musicAudioSource.volume = volume;

        PlayerPrefs.SetFloat(musicVolumePrefs, volume);
    }

    /// <summary>
    /// 修改音效音量
    /// </summary>
    /// <param name="volume"></param>
    public void ChangeSoundVolume(float volume)
    {
        soundVolume = volume;
        soundAudioSource.volume = volume;      
        PlayerPrefs.SetFloat(soundVolumePrefs, volume);
    }
}



/*
 
     使用
     using UnityEngine;
using System.Collections;
 
public class NewBehaviourScript : MonoBehaviour {
 
    // Use this for initialization
    void Start () {
        AudioManager.Instance.PlayMusic( 1 );
        AudioManager.Instance.PlaySound( 11, OnSoundPlayEnd );
 
        AudioManager.Instance.ChangeMusicVolume( 0.5f );
        AudioManager.Instance.ChangeSoundVolume( 0.5f );
    }
 
    void OnSoundPlayEnd()
    {
        Debug.Log( "音效播放完了" );
    }
}
     */
