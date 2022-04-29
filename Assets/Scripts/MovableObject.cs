using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovableObject : MonoBehaviour
{
    public float speed;
    public bool moveToTheLeft = true;
    GameObject Mario;
    // Start is called before the first frame update
    private void Awake()
    {
        Mario = GameObject.FindGameObjectWithTag("Player");
    }
    private void FixedUpdate()
    {
        Vector2 position = transform.localPosition;
        if (transform.localPosition.y > -6.5f)
        {
            if(Mario != null)
            {
                if (Mathf.Abs(Mario.transform.position.x - position.x) <= 20f)
                {
                    if (moveToTheLeft)
                    {
                        position.x -= speed * Time.deltaTime;
                    }
                    else position.x += speed * Time.deltaTime;
                    transform.localPosition = position;
                }
            }
            
        }
        

    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.contacts[0].normal.x > 0 && collision.collider.tag != "Player")
        {
            /*moveToTheLeft = true;*/
            TurnAround();
        }
        else if (collision.contacts[0].normal.x < 0 && collision.collider.tag != "Player")
        {
            /*moveToTheLeft = false; */
            TurnAround();
        }

    }
    void TurnAround()
    {
        moveToTheLeft = !moveToTheLeft;
        Vector2 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;
    }

}
