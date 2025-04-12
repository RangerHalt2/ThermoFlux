using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellSounds : MonoBehaviour
{
    [SerializeField] private AudioClip iceSpell;
    [SerializeField] private AudioClip fireSpell;

    private AudioSource m_AudioSource;
    private static SpellSounds m_Instance;
    public static SpellSounds Instance
    {
        get
        {
            if (m_Instance == null)
            {
                m_Instance = FindObjectOfType<SpellSounds>();
            }
            return m_Instance;
        }
    }
    private AudioSource audioSource;
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
    }

    public void IceSound()
    {
        m_AudioSource.clip = iceSpell;
        m_AudioSource.loop = false;
        m_AudioSource.Play();
    }


    public void FireSound()
    {
        m_AudioSource.clip = fireSpell;
       
        m_AudioSource.loop = false;
        m_AudioSource.Play();
    }

    public void Stop()
    {
        m_AudioSource.Stop();
    }

}
