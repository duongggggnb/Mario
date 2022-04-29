using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CameraFollow : MonoBehaviour
{
    private float minX = -8, maxX = 193.5f;
    private Transform Player;
    AudioSource audio;
    
    public Transform rightBoundary;
    // Start is called before the first frame update
    void Start()
    {
        Player = GameObject.FindWithTag("Player").transform;
        audio = GetComponent<AudioSource>();
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void FollowPlayer()
    {
        if (Player != null)
        {
            Vector3 CamPos = transform.position;
            CamPos.x = Player.position.x;
            Vector3 PlayerPos = Player.transform.position;
            
            if (CamPos.x < minX) CamPos.x = minX;
            if (CamPos.x > maxX) CamPos.x = maxX;
            /*transform.position = Vector3.Lerp(CamPos, PlayerPos, Time.deltaTime * 3f);*/
            transform.position = CamPos;
        }
    }

    private void FixedUpdate()
    {
        GameObject mario = GameObject.FindGameObjectWithTag("Player");
        if (mario != null)
        {
            var PlayerViewportPos = Camera.main.WorldToViewportPoint(Player.position);
            if (PlayerViewportPos.x >= 0.5f)
            {
                FollowPlayer();
            }
        }
        
        
    }
    public void MakeSound(string fileName)
    {
        audio.PlayOneShot(Resources.Load<AudioClip>("Audios/" + fileName));
    }
    
}
