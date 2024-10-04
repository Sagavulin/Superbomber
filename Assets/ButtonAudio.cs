using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonAudio : MonoBehaviour
{
	[SerializeField] private AudioSource m_audioSource;
	[SerializeField] private AudioClip m_hoverAudio;
	[SerializeField] private AudioClip m_selectAudio;
	[SerializeField] private AudioClip m_startButtonAudio;
	
	public void PlayStartSound()
	{
		DontDestroyOnLoad(m_audioSource.gameObject); 
		m_audioSource.PlayOneShot(m_startButtonAudio);
	}

	public void PlaySelectSound()
	{
		m_audioSource.PlayOneShot(m_selectAudio);
	}
	
	public void OnPointerEnter()
	{
		m_audioSource.PlayOneShot(m_hoverAudio);
	}
}
