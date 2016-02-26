using UnityEngine;
using System.Collections;

public class ObstacleOutOfScope : MonoBehaviour
{
	// Use this for initialization
	void Start () {
	
	}

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag.Equals("Projectile"))
        {
            other.gameObject.GetComponent<Projectile>().OnOutOfScope();
        }
    }
}
