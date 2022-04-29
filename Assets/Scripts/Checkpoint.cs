using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Checkpoint : MonoBehaviour
{
    Vector2 lastCheckpoint;
    GameObject Mario;
    
    private void Start()
    {
        lastCheckpoint =  FindObjectOfType<Player>().transform.position;
    }
    public void RespawnAtCheckpoint()
    {
        SceneManager.LoadScene(1);
        FindObjectOfType<Player>().transform.position = lastCheckpoint;
        
    }
    
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            Player.lastCheckpoint = this.transform.localPosition;
            print(Player.lastCheckpoint);
        }
    }
}
