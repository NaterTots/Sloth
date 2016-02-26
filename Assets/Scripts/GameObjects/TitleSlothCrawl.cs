using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Rigidbody2D))]
public class TitleSlothCrawl : MonoBehaviour
{
	// Use this for initialization
	void Start ()
    {
        GetComponent<Rigidbody2D>().AddForce(Vector2.up * 6f);
    }
	
	void OnTriggerEnter2D(Collider2D coll)
    {
        GetComponent<Rigidbody2D>().velocity = new Vector2(0, 0);
    }
}
