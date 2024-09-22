using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManager : MonoBehaviour
{
	private AudioSource m_audioSource;
	[SerializeField] private AudioClip[] trackArray;
	private int trackIndex = 0;
	
	private void Awake()
	{
		// Singleton for music manager
		int numberOfMusicManagers = FindObjectsOfType<MusicManager>().Length; // checks arrays lenght = objects in the array
		if (numberOfMusicManagers > 1) // if there's at least one object in the array --> destroys itself
		{
			Destroy(gameObject);
		}
		else
		{
			DontDestroyOnLoad(gameObject);
		}

		m_audioSource = GetComponent<AudioSource>();
	}

	private void Update()
	{
		if (!m_audioSource.isPlaying)
		{
			m_audioSource.clip = trackArray[trackIndex];
			m_audioSource.Play();
			if (trackIndex < trackArray.Length - 1)
			{
				trackIndex++;
			}
			else
			{
				trackIndex = 0;
			}
		}
	}
}
