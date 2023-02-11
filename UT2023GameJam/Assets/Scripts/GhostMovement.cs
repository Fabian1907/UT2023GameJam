using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostMovement : MonoBehaviour
{
    [SerializeField] private Transform playerTransform;
    [SerializeField] private PlayerMovement playerMovement;
    private Vector2 ghostPosition, playerPosition;

    void Awake()
    {
        
    }

    #region Flip
    private bool facingLeft = true;
    private Vector3 currentScale;
    private void CheckFlip()
    {
        if (playerPosition.x < ghostPosition.x && !facingLeft)
            Flip();
        else if (playerPosition.x > ghostPosition.x && facingLeft)
            Flip();
    }

    private void Flip()
    {
        currentScale = gameObject.transform.localScale;
        currentScale.x *= -1;
        gameObject.transform.localScale = currentScale;

        facingLeft = !facingLeft;
    }
    #endregion

    #region Collision
    private int PLAYERLAYER = 9;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == PLAYERLAYER)
        {
            playerMovement.Die();
            Debug.Log("BOOOO");
        }
    }
    #endregion

    #region Teleport
    [SerializeField] private float distanceToTeleport = 8f;
    [SerializeField] private float teleportLowerBoundX = 4f;
    [SerializeField] private float teleportUpperBoundX = 6f;
    [SerializeField] private float teleportLowerBoundY = -4f;
    [SerializeField] private float teleportUpperBoundY = 4f;
    private bool IsTeleporting = false;

    private void CheckTeleport()
    {
        if(Mathf.Abs(ghostPosition.x - playerPosition.x) > distanceToTeleport && !IsTeleporting)
        {
            StartCoroutine(Teleport());
        }
    }

    IEnumerator Teleport()
    {
        IsTeleporting = true;
        Disappear();
        yield return new WaitForSeconds(0.4f);
        float xOffset = Random.Range(teleportLowerBoundX, teleportUpperBoundX) * ((playerPosition.x > ghostPosition.x) ? 1 : -1);
        float yOffset = Random.Range(teleportLowerBoundY, teleportUpperBoundY);
        transform.position = new Vector2(playerPosition.x + xOffset, playerPosition.y + yOffset);
        Appear();
        yield return new WaitForSeconds(0.4f);
        IsTeleporting = false;
    }
    #endregion

    #region Animation
    [SerializeField] private Animator animator;

    private void Appear()
    {
        animator.Play("Ghost_Appear");
    }

    private void Disappear()
    {
        animator.Play("Ghost_Disappear");
    }
    #endregion

    #region Move
    [SerializeField] private float moveSpeed = 2f;
    [SerializeField] private float sineSpeed = 1f;
    [SerializeField] private float sineAmplitude = 2f;
    private void Move()
    {
        ghostPosition = transform.position;
        playerPosition = playerTransform.position;
        Vector2 target = new Vector2(playerPosition.x, playerPosition.y + sineAmplitude * Mathf.Sin(Time.time * sineSpeed));
        transform.position = Vector2.MoveTowards(ghostPosition, target, moveSpeed * Time.deltaTime);
    }
    #endregion

    void Update()
    {
        Move();
        CheckFlip();
        CheckTeleport();
    }
}
