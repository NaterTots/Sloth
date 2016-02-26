using UnityEngine;
using System.Collections;

public class SoundController : MonoBehaviour, IController
 {
	public AudioSource musicSource;

    void Start()
    {
        PlayMusic();
    }

	//used to play background music
	public void PlayMusic(AudioClip clip = null)
	{
        if (clip != null)
        {
            musicSource.clip = clip;
        }
		musicSource.Play();
	}


	#region IController

	public void Cleanup()
	{

	}

	#endregion IController

}
