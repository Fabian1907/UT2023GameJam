using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundCheck : MonoBehaviour
{
    public bool IsGrounded { get; private set; }
    private static int GROUNDLAYER = 3;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == GROUNDLAYER)
        {
            Debug.Log("GROUND");
            IsGrounded = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.layer == GROUNDLAYER)
        {
            IsGrounded = false;
        }
    }
}
