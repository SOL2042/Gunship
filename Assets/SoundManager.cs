using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Sound
{
    public string name; // ���� �̸�

    public AudioClip clip; // ��
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

    public AudioSource[] audioSourceEffects;    //����Ʈ ����
    public AudioSource audioSourceBgm;          //BGM

    public string[] playSoundName;              //�÷������� ���� �̸�

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
                    Debug.Log("��� ���� AudioSource�� ������Դϴ�.");
                    return;
                }
            }
        }
        Debug.Log(name + "���尡 SoundManager�� ��ϵ��� �ʾҽ��ϴ�.");
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
        Debug.Log("��� ����" + name + "���尡 �����ϴ�.");
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
        Debug.Log(name + "���尡 SoundManager�� ��ϵ��� �ʾҽ��ϴ�.");
    }

    public void StopAllbgm()
    {
        audioSourceBgm.Stop();
    }
    
}
