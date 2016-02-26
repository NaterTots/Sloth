using UnityEngine;
using System.Collections;

/// <summary>
/// This class will be used to load resources into subclasses.  It allows the object pool to more easily initialize objects generically without needing prefabs for each subclassed object.
/// </summary>
public class ResourceController : MonoBehaviour, IController
{
	Sprite[] collectibleSprites; //TODO: should rename this since we just put all sprites in the same folder and same array, not just collectible sprites
	AudioClip[] soundEffects; //just putting all sound effects in one array for now

	// Use this for initialization
	void Start () 
	{
		collectibleSprites = Resources.LoadAll<Sprite>("Sprites");
		soundEffects = Resources.LoadAll<AudioClip>("Audio/SoundEffects");
		//DebugAllSprites();
	}

	#region Visuals

	//returns the entire array of sprites
	public Sprite[] GetAllSprites()
	{
		return collectibleSprites;
	}

	public bool TryGetCollectibleSprite(string name, out Sprite value)
	{
		bool found = false;
		value = null;

		for (int i = 0; i < collectibleSprites.Length; i++)
		{
			if (collectibleSprites[i].name == name)
			{
				value = collectibleSprites[i];
				found = true;
				break;
			}
		}

		//didn't find the sprite
		return found;
	}

	//simple method to loop through the loaded resources by name
	private void DebugAllSprites()
	{
		for (int i = 0; i < collectibleSprites.Length; i++)
		{
			Debug.Log(collectibleSprites[i].name);
		}
	}

	#endregion Visuals

	#region Audio
	
	//returns the entire array of sound effects
	public AudioClip[] GetAllSoundEffects()
	{
		return soundEffects;
	}

	public bool TryGetSoundEffect(string name, out AudioClip value)
	{
		bool found = false;
		value = null;

		for (int i = 0; i < soundEffects.Length; i++)
		{
			if (soundEffects[i].name == name)
			{
				value = soundEffects[i];
				found = true;
				break;
			}
		}

		//didn't find the sound effect
		return found;
	}

	#endregion Audio


	#region IController

	public void Cleanup()
	{

	}

	#endregion IController
	
}
