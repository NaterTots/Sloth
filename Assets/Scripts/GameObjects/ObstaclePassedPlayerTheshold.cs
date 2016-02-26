using UnityEngine;
using System.Collections;

public class ObstaclePassedPlayerTheshold : MonoBehaviour
{
    public Player.PlayerRegion playerRegion;

	// Use this for initialization
	void Start ()
    {
        Resolver.Instance.GetController<EventHandler>().Register(Events.Game.PlayerDead, OnPlayerDead);
	}

    void OnDestroy()
    {
        Resolver.Instance.GetController<EventHandler>().UnRegister(Events.Game.PlayerDead, OnPlayerDead);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag.Equals("Projectile"))
        {
            other.gameObject.GetComponent<Projectile>().OnPassedPlayer();
        }
    }

    void OnPlayerDead(int id, object param)
    {
        Player.PlayerRegion deadPlayerRegion = (Player.PlayerRegion)param;

        //if the sloth fell off the tree, don't it score points anymore
        if (playerRegion == deadPlayerRegion)
        {
            GetComponent<BoxCollider2D>().enabled = false;
        }
    }
}
