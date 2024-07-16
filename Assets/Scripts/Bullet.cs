using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    Rigidbody2D myRigidbody;
    PlayerMovement player; //references the player so you can determine the direction of the shooting
    float xSpeed;

    [SerializeField] float bulletSpeed = 15f;

    void Start()
    {
        myRigidbody = GetComponent<Rigidbody2D>();
        player = FindObjectOfType<PlayerMovement>(); //do not use FindObjectOfType in update (bad performance)
        xSpeed = player.transform.localScale.x * bulletSpeed; //player.transform.localScale.x gives you the direction of the player (1 for right, -1 for left)
        if (xSpeed < 0) {
          transform.localScale = new Vector2 (-(Mathf.Sign(myRigidbody.velocity.x)), 1f);
        }
    }

    void Update()
    {
        myRigidbody.velocity = new Vector2 (xSpeed, 0f);
    }

    void OnTriggerEnter2D(Collider2D other) {
      if (other.tag == "Enemy") {
        Destroy(other.gameObject); //destroys the enemy if the bullet makes contact
      }
      Destroy(gameObject); //destroys the bullet (self)
    }

    void OnCollisionEnter2D(Collision2D other) {
      Destroy(gameObject);
    }
}
