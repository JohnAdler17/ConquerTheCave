using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    Vector2 moveInput;
    Rigidbody2D myRigidbody;
    Animator myAnimator;
    CapsuleCollider2D myCapsuleCollider;
    BoxCollider2D myFeetCollider;
    float gravityScaleAtStart;
    bool isAlive = true;
    private bool inControl = true;
    private bool levelOver = false;

    [SerializeField] AudioSource myAudioSource;

    [SerializeField] AudioSource footstepsAudio;

    [SerializeField] AudioClip bounceAudioClip;
    [SerializeField] AudioClip rollAudioClip;

    [SerializeField] AudioClip[] myJumpAudioClips;
    [SerializeField] AudioClip[] myDeathAudioClips;

    [SerializeField] float runSpeed = 10f;
    [SerializeField] float jumpSpeed = 5f;
    [SerializeField] float climbSpeed = 5f;
    [SerializeField] float jumpCooldownTime = 0.3f;
    [SerializeField] Vector2 deathKick = new Vector2 (10f, 10f);
    [SerializeField] PhysicsMaterial2D deathFrictionMaterial;
    [SerializeField] GameObject bullet;
    [SerializeField] Transform gun;

    [SerializeField] float maxFallingSpeed = 10f;

    [SerializeField] private GameObject deathParticleSystem;

    void Start()
    {
      myRigidbody = GetComponent<Rigidbody2D>();
      myAnimator = GetComponent<Animator>();
      myCapsuleCollider = GetComponent<CapsuleCollider2D>();
      myFeetCollider = GetComponent<BoxCollider2D>();
      gravityScaleAtStart = myRigidbody.gravityScale;
    }

    // Update is called once per frame
    void Update()
    {
      if (!isAlive) { return;}
      if (levelOver) {return;}
      Die();
      if (!inControl) { return;}
      TouchingGround();
      Run();
      FlipSprite();
      ClimbLadder();
      BounceSound();
    }

    private bool bounceSE = true;
    private float bounceCooldown = 0.2f;
    void BounceSound() {
      if ((myFeetCollider.IsTouchingLayers(LayerMask.GetMask("Bouncing")) || myCapsuleCollider.IsTouchingLayers(LayerMask.GetMask("Bouncing"))) && bounceSE) {
        myAudioSource.PlayOneShot(bounceAudioClip);
        StartCoroutine(BounceSECooldown());
      }
    }

    IEnumerator BounceSECooldown() {
      bounceSE = false;
      yield return new WaitForSeconds(bounceCooldown);
      bounceSE = true;
    }

    void FixedUpdate() {
      MaxFallSpeed();
    }

    void MaxFallSpeed() {
      myRigidbody.velocity = new Vector2 (myRigidbody.velocity.x, Mathf.Clamp(myRigidbody.velocity.y, -maxFallingSpeed, 50));
    }


    void OnMove(InputValue value) {
      if (!isAlive) { return;}
      if (levelOver) {return;}
      moveInput = value.Get<Vector2>();
      //Debug.Log(moveInput);
    }

    public float ReturnPlayerMove() {
      return moveInput.y;
    }

    private bool isGrounded = false;
    void TouchingGround() {
      if (myFeetCollider.IsTouchingLayers(LayerMask.GetMask("Ground"))) {
        isGrounded = true;
        canRoll = true;
      }
      else {
        isGrounded = false;
      }
      //Debug.Log(isGrounded);
    }

    public void DisablePlayerControls() {
      levelOver = true;
      myRigidbody.velocity = Vector2.zero;
      footstepsAudio.Stop();
      myAnimator.SetBool("isRunning", false);
    }

    private int previousJumpAudioPlayed;
    IEnumerator JumpCooldown() {
      isJumping = true;
      int randIndex = Random.Range(0, myJumpAudioClips.Length);
      if (randIndex == previousJumpAudioPlayed) {
        randIndex += 1;
      }
      if (randIndex >= myJumpAudioClips.Length) {
        randIndex = 0;
      }
      AudioClip randomClip = myJumpAudioClips[randIndex];
      myAudioSource.PlayOneShot(randomClip);
      previousJumpAudioPlayed = randIndex;
      yield return new WaitForSeconds(jumpCooldownTime);
      isJumping = false;
    }

    private bool isJumping = false;
    private bool canDoubleJump = true;
    void OnJump(InputValue value) {
      if (!isAlive) { return;}
      if (levelOver) {return;}
      //if (!inControl) { return;}

      if (value.Get<float>() >= 1f) {
        //Debug.Log("Jump Being Called");
        if(myFeetCollider.IsTouchingLayers(LayerMask.GetMask("Ground", "Climbing"))) {
          //Debug.Log("Feet Touching Ground");
          canDoubleJump = true;

          if (isClimbing) {
            myRigidbody.velocity += new Vector2 (0f, jumpSpeed / 3f);
            //Debug.Log("Ladder Jump");
          }
          else {
            myRigidbody.velocity += new Vector2 (0f, jumpSpeed);
            //Debug.Log("Normal Jump");
          }

          StartCoroutine(JumpCooldown());
        }
        else if (canDoubleJump) {
          canDoubleJump = false;

          myRigidbody.velocity = new Vector2(myRigidbody.velocity.x, 0);
          myRigidbody.velocity += new Vector2 (0f, jumpSpeed / 1.2f);

          StartCoroutine(JumpCooldown());
        }
      }
      else {
        if(!isGrounded) {
          myRigidbody.velocity = new Vector2 (myRigidbody.velocity.x, myRigidbody.velocity.y / 2f);
        }
      }
    }


    private bool isRolling = false;
    private bool canRoll = true;
    [SerializeField] private float rollSpeed = 10f;
    [SerializeField] private float rollCooldownTime = 0.3f;

    void OnRoll(InputValue value) {
      if (!isAlive) { return;}
      if (levelOver) {return;}

      if (value.isPressed && !isRolling && canRoll) {
        if (!isGrounded) {
          canRoll = false;
        }
        myAudioSource.PlayOneShot(rollAudioClip);
        isRolling = true;
        inControl = false;
        float rollDirection = Mathf.Sign(transform.localScale.x);
        myRigidbody.velocity = new Vector2 (rollDirection * rollSpeed, 0f);
        myAnimator.SetBool("isRolling", true);
        //play rolling sound
        StartCoroutine(RollCooldown());
      }
    }

    IEnumerator RollCooldown() {
      yield return new WaitForSeconds(rollCooldownTime);
      myAnimator.SetBool("isRolling", false);
      isRolling = false;
      inControl = true;
    }

    public bool IsRolling() {
      return isRolling;
    }

    void OnFire(InputValue value) {
      if (!isAlive) { return;}
      if (levelOver) {return;}
      if (!inControl) { return;}
      //Instantiate(bullet, gun.position, transform.rotation);
    }

    bool runningStarted = false;
    void Run() {
      Vector2 playerVelocity = new Vector2 (moveInput.x * runSpeed, myRigidbody.velocity.y);
      myRigidbody.velocity = playerVelocity;

      bool playerHasHorizontalSpeed = Mathf.Abs(myRigidbody.velocity.x) > Mathf.Epsilon;
      myAnimator.SetBool("isRunning", playerHasHorizontalSpeed);
      if (playerHasHorizontalSpeed && isGrounded && !runningStarted) {
        footstepsAudio.Play();
        runningStarted = true;
      }
      else if (runningStarted && (!playerHasHorizontalSpeed || !isGrounded || isClimbing)) {
        footstepsAudio.Pause();
        runningStarted = false;
      }
    }

    void FlipSprite() {
      bool playerHasHorizontalSpeed = Mathf.Abs(myRigidbody.velocity.x) > Mathf.Epsilon;

      if (playerHasHorizontalSpeed) {
        transform.localScale = new Vector2 (Mathf.Sign(myRigidbody.velocity.x), 1f);

      }
    }

    private bool isClimbing = false;
    void ClimbLadder() {
      bool playerIsClimbing = Mathf.Abs(myRigidbody.velocity.y) > Mathf.Epsilon;
      if (!myFeetCollider.IsTouchingLayers(LayerMask.GetMask("Climbing"))) {
        myRigidbody.gravityScale = gravityScaleAtStart;
        isClimbing = false;
        myAnimator.SetBool("isOnLadder", false);
        myAnimator.SetBool("isClimbing", false);
        return;
      }
      if(!isJumping) {
        Vector2 climbVelocity = new Vector2 (myRigidbody.velocity.x, moveInput.y * climbSpeed);
        myRigidbody.velocity = climbVelocity;
        myRigidbody.gravityScale = 0f;
        isClimbing = true;
        if (!playerIsClimbing) {
          myAnimator.SetBool("isOnLadder", true);
        }
        else {
          myAnimator.SetBool("isOnLadder", false);
          myAnimator.SetBool("isClimbing", playerIsClimbing);
        }
      }

    }

    public bool ReturnAliveState() {
      return isAlive;
    }

    void Die() {
      if (myCapsuleCollider.IsTouchingLayers(LayerMask.GetMask("Enemy", "Hazards"))) {
        isAlive = false;
        footstepsAudio.Pause();
        myAnimator.SetTrigger("Dying");
        Instantiate(deathParticleSystem, new Vector2 (transform.position.x, transform.position.y), Quaternion.identity);
        int randIndex = Random.Range(0, myDeathAudioClips.Length);
        AudioClip randomClip = myDeathAudioClips[randIndex];
        myAudioSource.PlayOneShot(randomClip);
        myRigidbody.velocity = deathKick;
        myCapsuleCollider.sharedMaterial = deathFrictionMaterial;
        myFeetCollider.sharedMaterial = deathFrictionMaterial;

        FindObjectOfType<GameSession>().ProcessPlayerDeath();
      }
    }
}
