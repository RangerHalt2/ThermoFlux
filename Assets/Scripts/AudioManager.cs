using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [SerializeField] private AudioClip a_BGM;
    [SerializeField] private AudioClip a_Victory;
    [SerializeField] private AudioClip a_Loss;


    private AudioSource m_AudioSource;
    private static AudioManager m_Instance;
    public static AudioManager Instance
    {
        get
        {
            if (m_Instance == null)
            {
                m_Instance = FindObjectOfType<AudioManager>();
            }
            return m_Instance;
        }
    }

    private void Awake()
    {
        if (m_Instance == null)
        {
            m_Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            //Destroy(gameObject);
        }
        m_AudioSource = GetComponent<AudioSource>();
        PlayBGM();
    }

    public void StopBGM()
    {
        m_AudioSource.Stop();
    }
    private void PlayBGM()
    {
        m_AudioSource.clip = a_BGM;
        m_AudioSource.loop = true;
        m_AudioSource.Play();
    }
    public void PlayVictory()
    {
        m_AudioSource.clip = a_Victory;
        m_AudioSource.loop = false;
        m_AudioSource.Play();
    }
    public void PlayLoss()
    {
        m_AudioSource.clip = a_Loss;
        m_AudioSource.loop = false;
        m_AudioSource.Play();
    }

    public void Stop()
    {
        m_AudioSource.Stop();
    }

}