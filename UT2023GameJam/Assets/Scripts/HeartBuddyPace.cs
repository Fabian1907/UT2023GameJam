using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// No animations yet! Follow the tutorial: Npc Animation FOllowing Player in Unity 2D to get there

public class HeartBuddyPace : MonoBehaviour
{
    [SerializeField] float moveSpeed = 10f;
    [SerializeField] float sineSpeed = 2f;
    [SerializeField] float sineAmplitude = 1f;
    [SerializeField] float targetDistance;
    [SerializeField] Transform playerTransform;
    [SerializeField] private float yOffset = 2f;
    private Vector2 heartPosition, playerPosition, target;
    void Awake()
    {
        //playerTransform = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
    }

    void Update()
    {
        heartPosition = transform.position;
        playerPosition = playerTransform.position;
        TargetFollow();
    }

    void TargetFollow()
    {
        
        if(Mathf.Abs(heartPosition.x - playerPosition.x) > targetDistance)
        {
            target.x = playerPosition.x;
        }
        else
        {
            target.x = heartPosition.x;
        }

        target.y = playerPosition.y + yOffset + sineAmplitude * Mathf.Sin(Time.time * sineSpeed);
        transform.position = Vector2.MoveTowards(heartPosition, target, moveSpeed * Time.deltaTime);
        //else
        //{
         //   float newY = Mathf.Sin(Time.time * 2f) * 0.5f + target.position.y + 2f;
         //   transform.position = Vector2.MoveTowards(transform.position, new Vector2(transform.position.x, newY), speed * Time.deltaTime);
        //}
    }
    /*
    void FlipSprite()
    {
        if(playerTransform.position.x > transform.position.x)
        {
            transform.localScale = new Vector3(1,1,1);
        } 
        else if(playerTransform.position.x < transform.position.x)
        {
            // faceleft
            transform.localScale = new Vector3(-1,1,1);
        }
    }
    */
}
