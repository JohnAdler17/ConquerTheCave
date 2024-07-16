using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LifePickup : MonoBehaviour
{
    [SerializeField] AudioSource lifePickupSFX;
    [SerializeField] int numLives;

    bool wasCollected = false;

    PlayerMovement player;

    void Start() {
      //player = GameObject.FindWithTag("Player").GetComponent<PlayerMovement>();
    }

    void OnTriggerEnter2D(Collider2D other) {
      player = GameObject.FindWithTag("Player").GetComponent<PlayerMovement>();
      //Debug.Log(player.ReturnAliveState());

      if (other.tag == "Player" && !wasCollected && player.ReturnAliveState()) {
        FindObjectOfType<GameSession>().AddToLives(numLives);
        lifePickupSFX.Play(); //can add in a third parameter to control volume, first parameter is the sound and second is the point of origin
        //you must use PlayClipAtPoint because the gameObject is being destroyed immediately after playing the sound, so the sound would be cut off
        wasCollected = true;
        //gameObject.SetActive(false);
        //Destroy(gameObject);
        StartCoroutine(WaitToDestroy());
      }
    }

    IEnumerator WaitToDestroy() {
      gameObject.GetComponent<SpriteRenderer>().enabled = false;
      yield return new WaitForSeconds(0.2f);
      Destroy(gameObject);
    }
}
