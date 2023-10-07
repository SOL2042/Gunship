using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Sound
{
    public string name; // 곡의 이름

    public AudioClip clip; // 곡
}
public class SoundManager : MonoBehaviour
{
    private static SoundManager _instance;
    public static SoundManager instance
    {
        get
        {
            if (_instance == null) _instance = FindObjectOfType<SoundManager>();
            return _instance;
        }
    }

    public AudioSource[] audioSourceEffects;    //이펙트 사운드
    public AudioSource audioSourceBgm;          //BGM

    public string[] playSoundName;              //플레이중인 사운드 이름

    public Sound[] effectsSound;                
    public Sound[] BgmSound;

    public void PlaySE(string name)
    {
        for (int i = 0; i < effectsSound.Length; i++)
        {
            if(name == effectsSound[i].name)
            {
                for (int j = 0; j < audioSourceEffects.Length; j++)
                {
                    if(!audioSourceEffects[j].isPlaying)
                    {
                        playSoundName[j] = effectsSound[i].name;
                        audioSourceEffects[j].clip = effectsSound[i].clip;
                        audioSourceEffects[j].Play();
                        return;
                    }
                    Debug.Log("모든 가용 AudioSource가 사용중입니다.");
                    return;
                }
            }
        }
        Debug.Log(name + "사운드가 SoundManager에 등록되지 않았습니다.");
    }

    public void StopAllSE()
    {
        for (int i = 0; i < audioSourceEffects.Length; i++)
        {
            audioSourceEffects[i].Stop();
        }
    }
    public void StopSE(string name)
    {
        for (int i = 0; i < audioSourceEffects.Length; i++)
        {
            if(playSoundName[i] == name)
            {
                audioSourceEffects[i].Stop();
                return;
            }
        }
        Debug.Log("재생 중인" + name + "사운드가 없습니다.");
    }

    public void PlayBgm(string name)
    {
        for (int i = 0; i < BgmSound.Length; i++)
        {
            if (name == BgmSound[i].name)
            {
                audioSourceBgm.clip = BgmSound[i].clip;
                audioSourceBgm.Play();
                return;
            }
        }
        Debug.Log(name + "사운드가 SoundManager에 등록되지 않았습니다.");
    }

    public void StopAllbgm()
    {
        audioSourceBgm.Stop();
    }
    
}
