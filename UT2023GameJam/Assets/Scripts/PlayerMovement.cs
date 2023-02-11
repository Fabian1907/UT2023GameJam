using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private float horizontalSpeed = 0f;
    [SerializeField] private GroundCheck groundCheck;
    private Rigidbody2D body;

    #region Input
    private float horizontalInput;
    private bool jumpUp, jumpDown;

    private void GatherInput()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");
        jumpDown = Input.GetButtonDown("Jump");
        jumpUp = Input.GetButtonUp("Jump");

        if (jumpDown) lastTimeJumpPressed = Time.time;
        if (jumpUp) lastTimeJumpReleased = Time.time;
        if (jumpUp && body.velocity.y > 0) releasedJumpEarly = true;
    }
    #endregion

    #region Gravity
    [Header("GRAVITY")]
    [SerializeField] private float upwardsGravityMultiplier = 1.7f;
    [SerializeField] private float downwardsGravityMultiplier = 3f;
    private float defaultGravityScale = 1f;

    private void ChangeGravity()
    {
        if (body.velocity.y > 0 && releasedJumpEarly)
            body.gravityScale = upwardsGravityMultiplier * earlyReleaseGravity;
        else if (body.velocity.y > 0)
            body.gravityScale = upwardsGravityMultiplier;
        else if (body.velocity.y < 0)
            body.gravityScale = downwardsGravityMultiplier;
        else if (body.velocity.y == 0)
            body.gravityScale = defaultGravityScale;
    }
    #endregion

    #region Grounded
    private bool isGrounded;
    private float timeLeftGround;
    private void CheckGrounded()
    {
        if (isGrounded && !groundCheck.IsGrounded)
        {
            timeLeftGround = Time.time;
        }
        else if (!isGrounded && groundCheck.IsGrounded)
        {
            coyoteUsable = true;
        }

        isGrounded = groundCheck.IsGrounded;
    }
    #endregion

    #region Flip
    private bool facingRight = true;
    private Vector3 currentScale;

    private void CheckFlip()
    {
        if (horizontalInput > 0 && !facingRight)
            Flip();
        else if (horizontalInput < 0 && facingRight)
            Flip();
    }

    private void Flip()
    {
        currentScale = gameObject.transform.localScale;
        currentScale.x *= -1;
        gameObject.transform.localScale = currentScale;

        facingRight = !facingRight;
    }
    #endregion

    #region Walk
    [Header("WALK")]
    [SerializeField] private float acceleration = 60f;
    [SerializeField] private float deceleration = 30f;
    [SerializeField] private float maxSpeed = 10f;

    private void Walk()
    {
        if (horizontalInput != 0)
        {
            horizontalSpeed += horizontalInput * acceleration * Time.deltaTime;
            horizontalSpeed = Mathf.Clamp(horizontalSpeed, -maxSpeed, maxSpeed);
        }
        else
            horizontalSpeed = Mathf.MoveTowards(horizontalSpeed, 0f, deceleration * Time.deltaTime);
    }
    #endregion

    #region Jump
    [Header("JUMP")]
    [SerializeField] private float jumpHeight = 5f;
    [SerializeField] private float coyoteTime = 0.1f;
    [SerializeField] private float bufferTime = 0.1f;
    [SerializeField] private float earlyReleaseGravity = 3f;
    private bool coyoteUsable;
    private float lastTimeJumpPressed;
    private float lastTimeJumpReleased;
    private bool releasedJumpEarly;

    private bool CanUseCoyote()
    {
        return coyoteUsable && !isGrounded && timeLeftGround + coyoteTime > Time.time;
    }

    private bool HasBufferedJump()
    {
        return isGrounded && lastTimeJumpPressed + bufferTime > Time.time;
    }

    private void Jump()
    {
        if ((jumpDown && CanUseCoyote()) || HasBufferedJump())
        {
            Debug.Log(lastTimeJumpReleased + " > " + lastTimeJumpPressed);
            Debug.Log(HasBufferedJump());
            if (lastTimeJumpReleased > lastTimeJumpPressed - bufferTime && HasBufferedJump())
            {
                releasedJumpEarly = true;
            }
            else
                releasedJumpEarly = false;

            body.velocity = new Vector2(body.velocity.x, 0f);
            body.AddForce(new Vector2(0f, jumpHeight));
            lastTimeJumpPressed = float.MinValue;

            coyoteUsable = false;
        }
    }
    #endregion

    #region Move
    private void Move()
    {
        body.velocity = new Vector2(horizontalSpeed, body.velocity.y);
    }
    #endregion

    private void Awake()
    {
        body = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        GatherInput();
        ChangeGravity();
        CheckGrounded();
        CheckFlip();
        Walk();
        Jump();
        Move();
    }
}
