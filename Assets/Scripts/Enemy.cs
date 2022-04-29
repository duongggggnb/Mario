using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    GameObject Mario;
    Vector2 DeadPosition;
    public bool isPoisonMushroom;
    private void Awake()
    {
        Mario = GameObject.FindGameObjectWithTag("Player");

    }
    private void Update()
    {
        DeadPosition = transform.localPosition;
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if ((collision.contacts[0].normal.x > 0 || collision.contacts[0].normal.x < 0) && collision.collider.tag == "Player")
        {
            if (Mario.GetComponent<Player>().countLV > 1)
            {
                Mario.GetComponent<Player>().countLV = 1;
                Mario.GetComponent<Player>().trans = true;
            }
            else
            {
                Mario.GetComponent<Player>().Dead();
            }
        }

        if (collision.contacts[0].normal.y < 0 && collision.collider.tag == "Player")
        {
            Destroy(gameObject);
            if (isPoisonMushroom)
            {
                GameObject mushroom = (GameObject)Instantiate(Resources.Load("Prefabs/PoisonousMushroomDead"));
                mushroom.transform.localPosition = DeadPosition;
            }
            else 
            {
                GameObject turtle = (GameObject)Instantiate(Resources.Load("Prefabs/GreenTurtleDead"));
                turtle.transform.localPosition = DeadPosition;
            } 

        }
        if (collision.collider.tag == "MarioFireBall")
        {
            Destroy(gameObject);
            if (isPoisonMushroom)
            {
                GameObject mushroom = (GameObject)Instantiate(Resources.Load("Prefabs/PoisonousMushroomDead"));
                mushroom.transform.localPosition = DeadPosition;
            }
            else
            {
                GameObject mushroom = (GameObject)Instantiate(Resources.Load("Prefabs/GreenTurtleDead"));
                mushroom.transform.localPosition = DeadPosition;
            }
        }

    }

}
