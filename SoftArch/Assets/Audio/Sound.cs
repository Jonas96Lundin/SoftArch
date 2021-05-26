﻿using UnityEngine;
using UnityEngine.Audio;
/// <summary>
/// Create a sound that can be played through AudioManager.
/// Created by: Jonas
/// </summary>
[System.Serializable]
public class Sound {

	public string name;

	public AudioClip clip;

	[Range(0f, 1f)]
	public float volume = .75f;
	[Range(0f, 1f)]
	public float volumeVariance = .1f;

	[Range(.1f, 3f)]
	public float pitch = 1f;
	[Range(0f, 1f)]
	public float pitchVariance = .1f;

	public bool loop = false;

	public AudioMixerGroup mixerGroup;

	public bool isSFX;

	[HideInInspector]
	public AudioSource source;

}
