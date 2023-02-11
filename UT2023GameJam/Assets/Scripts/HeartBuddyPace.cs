using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// No animations yet! Follow the tutorial: Npc Animation FOllowing Player in Unity 2D to get there

public class HeartBuddyPace : MonoBehaviour
{

    [SerializeField] float speed;
    [SerializeField] float targetPosition;
    [SerializeField] Transform playerTransform;
    Rigidbody2D myRigidbody2D;

    // Animator myAnimator;
    int yOffset = 2;

    Transform target;

    // Start is called before the first frame update
    void Awake()
    {
        // myAnimator = GetComponent<Animator>();
        myRigidbody2D = GetComponent<Rigidbody2D>();
        target = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
    }

    // Update is called once per frame
    void Update()
    {
        TargetFollow();
    }

    void TargetFollow()
    {
        
        if(Vector2.Distance(transform.position, target.position) > targetPosition)
        {     
            // float yOffset = Random.Range(1f, 2f); // Generate a random value between 1 and 2
            Vector2 targetPosition = new Vector2(target.position.x, target.position.y + yOffset);
            // myAnimator.SetBool("Running", true);
            transform.position = Vector2.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);
        }
        else
        {
            // Calculate the new y position of the HeartBuddyCharacter based on a sine wave
            float newY = Mathf.Sin(Time.time * 2f) * 0.5f + target.position.y + 2f;
        
            // Move the HeartBuddyCharacter to the new position
            transform.position = Vector2.MoveTowards(transform.position, new Vector2(transform.position.x, newY), speed * Time.deltaTime);

            
            // myAnimator.SetBool("Running", false);
        }
    }
}
