using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RespawnTurtle : MonoBehaviour
{
    Vector2 position;
    // Start is called before the first frame update
    void Start()
    {
        position = transform.localPosition;
        StartCoroutine(Respawn());
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if ((collision.contacts[0].normal.y < 0 && collision.collider.tag == "Player") || collision.collider.tag == "MarioFireBall")
        {
            Destroy(gameObject);
            GameObject turtle = (GameObject)Instantiate(Resources.Load("Prefabs/GreenTurtleDead"));
            turtle.transform.localPosition = position;
        }
    }
    /*private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "MarioFireBall")
        {
            Destroy(gameObject);
            GameObject turtle = (GameObject)Instantiate(Resources.Load("Prefabs/GreenTurtleDead"));
            turtle.transform.localPosition = position; 
        }
    }*/
    IEnumerator Respawn()
    {
        yield return new WaitForSeconds(1.5f);
        Destroy(gameObject);
        GameObject Turtle = (GameObject)Instantiate(Resources.Load("Prefabs/GreenTurtle"));
        Turtle.transform.localPosition = position;
    }
}
