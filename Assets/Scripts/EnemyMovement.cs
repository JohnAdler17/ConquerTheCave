using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    Rigidbody2D myRigidbody;
    BoxCollider2D myBoxCollider;

    [SerializeField] float moveSpeed = 1f;

    void Start()
    {
      myRigidbody = GetComponent<Rigidbody2D>();
      myBoxCollider = GetComponent<BoxCollider2D>();
    }

    void Update()
    {
      myRigidbody.velocity = new Vector2 (moveSpeed, 0f);
    }

    void OnTriggerExit2D(Collider2D other) {
      moveSpeed = -moveSpeed;
      FlipEnemyFacing();
    }

    void FlipEnemyFacing() {
      transform.localScale = new Vector2 (-(Mathf.Sign(myRigidbody.velocity.x)), 1f);
    }
}
