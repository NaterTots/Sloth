using UnityEngine;
using System.Collections;

public class Projectile : MonoBehaviour
{
    public enum PlayerCollisionResults
    {
        Nothing,
        PlayerGetsHurt
    };



	// Use this for initialization
	void Start ()
    {
	
	}
	
    public void OnPassedPlayer()
    {
        Resolver.Instance.GetController<EventHandler>().Post(Events.Game.ScoredPoints, 1);
    }

    public void OnOutOfScope()
    {
        gameObject.SetActive(false);
    }

    public PlayerCollisionResults OnCollideWithPlayer()
    {
        //in here, the projectile does what it should be doing when it collides with player
        //blink?
        //die?
        gameObject.SetActive(false);

        //now suggest to the player what he should be doing
        return PlayerCollisionResults.PlayerGetsHurt;
    }
}
