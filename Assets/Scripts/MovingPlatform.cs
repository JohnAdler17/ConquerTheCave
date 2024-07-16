using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    [SerializeField] private Transform positionA;
    [SerializeField] private Transform positionB;
    [SerializeField] private float moveSpeed;
    Vector3 targetPosition;
    Rigidbody2D myRigidbody;

    [SerializeField] private bool isHazard = false;

    // Start is called before the first frame update
    void Start()
    {
      myRigidbody = GetComponent<Rigidbody2D>();
      targetPosition = positionB.position;
      if (isHazard) {
        StartCoroutine(FlipCD());
      }
    }

    // Update is called once per frame
    void Update()
    {
      if (Vector2.Distance(transform.position, positionA.position) < 0.1f) {
        targetPosition = positionB.position;
        if (isHazard) {
          FlipEnemyFacing();
        }
      }
      if (Vector2.Distance(transform.position, positionB.position) < 0.1f) {
        targetPosition = positionA.position;
        if (isHazard) {
          FlipEnemyFacing();
        }
      }

      transform.position = Vector2.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);
    }

    private float flipCooldown = 0.2f;
    private bool canFlip = false;

    void FlipEnemyFacing() {
      //Debug.Log("Flip");
      if (canFlip) {
        transform.localScale = new Vector2 (transform.localScale.x * -1, 1f);
        StartCoroutine(FlipCD());
      }
    }

    IEnumerator FlipCD() {
      canFlip = false;
      yield return new WaitForSeconds(flipCooldown);
      canFlip = true;
    }

    void OnTriggerEnter2D(Collider2D other) {
      if (other.CompareTag("Player") && !isHazard) {
        other.transform.SetParent(this.transform);
      }
    }
    void OnTriggerExit2D(Collider2D other) {
      if (other.CompareTag("Player") && !isHazard) {
        other.transform.SetParent(null);
      }
    }

    void OnDrawGizmos() {
      Gizmos.color = Color.yellow;
      Gizmos.DrawLine(positionA.position, positionB.position);
    }
}
