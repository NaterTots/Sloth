using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour
{
    public GameObject positionLeft;
    public GameObject positionRight;

    public enum PlayerRegion
    {
        Left,
        Right,
        Both
    };

    public PlayerRegion playerRegion;

    public AudioSource hitSoundEffect;

    bool isInPositionLeft = true;
    bool dead = false;

    private Animator anim;
    private InputController inputController;

	// Use this for initialization
	void Start ()
    {
        anim = GetComponent<Animator>();
        inputController = Resolver.Instance.GetController<InputController>();

        PositionAndOrient();
    }
	
	// Update is called once per frame
	void Update ()
    {
	    if (!dead)
        {
            InputController.ClickRegion clickRegion = inputController.GetRegionClicked();

            if (clickRegion != InputController.ClickRegion.None)
            {
                if (playerRegion == PlayerRegion.Both ||
                    playerRegion == PlayerRegion.Left && clickRegion == InputController.ClickRegion.Left ||
                    playerRegion == PlayerRegion.Right && clickRegion == InputController.ClickRegion.Right)
                {
                    isInPositionLeft = !isInPositionLeft;
                    PositionAndOrient();
                }
            }
        }
	}

    void PositionAndOrient()
    {
        gameObject.transform.position = (isInPositionLeft) ? positionLeft.transform.position : positionRight.transform.position;

        gameObject.transform.rotation = (isInPositionLeft) ?
                                                Quaternion.identity :
                                                Quaternion.Euler(0, 180, 0);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (!dead && other.tag.Equals("Projectile"))
        {
            switch (other.gameObject.GetComponent<Projectile>().OnCollideWithPlayer())
            {
                case Projectile.PlayerCollisionResults.PlayerGetsHurt:
                    anim.SetTrigger("Hit");
                    hitSoundEffect.Play();
                    dead = true;
                    Resolver.Instance.GetController<EventHandler>().Post(Events.Game.PlayerDead, playerRegion);
                    Invoke("OnDead", 2f); //wait until animation is done before calling OnDead
                    
                    break;
            }
        }
    }

    void OnDead()
    {
        Rigidbody2D rigidbody2D = GetComponent<Rigidbody2D>();
        rigidbody2D.isKinematic = false;
        rigidbody2D.AddForce(new Vector2(isInPositionLeft ? -75 : 75, 0));

    }

}
