using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using UnityEngine.InputSystem;

public class OneWayPlat : MonoBehaviour
{
    private BoxCollider2D platCollider;

    PlayerMovement player;

    float moveInput;

    void Awake() {
      platCollider = GetComponent<BoxCollider2D>();
      player = GameObject.FindWithTag("Player").GetComponent<PlayerMovement>();
    }

    void Update()
    {
      FlipPlatform();
    }

    void FlipPlatform() {
      moveInput = player.ReturnPlayerMove();
      if (moveInput == -1) {
        platCollider.enabled = false;
      }
      else {
        platCollider.enabled = true;
      }
    }
}
