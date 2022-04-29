using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block : MonoBehaviour
{
    public float bounce;
    public float bouncingSpeed;
    public bool canBounce;
    public bool canBreak;

    /*public bool mushroom;*/
    public bool coin;

    public int totalCoin;
    GameObject Mario;
    GameObject FireBall;

    Vector3 initialPosition;
    // Start is called before the first frame update
    void Start()
    {
        initialPosition = transform.localPosition;
        Mario = GameObject.FindGameObjectWithTag("Player");
        FireBall = GameObject.FindGameObjectWithTag("MarioFireBall");
    }

    // Update is called once per frame
    void Update()
    {

    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.tag == "Head" && collision.contacts[0].normal.y > 0)
        {
            totalCoin--;
            BouncingBlock();
            
        }
        if (collision.collider.tag == "MarioFireBall" && (collision.contacts[0].normal.y < 0|| collision.contacts[0].normal.x < 0|| collision.contacts[0].normal.x > 0))
        {
            Destroy(FireBall);
            
        }

        /*if(collision.collider.tag == "Player")
        {
            print("y: "+collision.contacts[0].normal.y);
            //cham duoi >0, cham tren < 0
            print("x: "+collision.contacts[0].normal.x);
            //cham ben trai >0, cham phai <0
        }*/
    }

    void BouncingBlock()
    {
        if (canBounce)
        {
            StartCoroutine(Bounce());
            if(totalCoin <= 0) canBounce = false;

            if (coin)
            {
                CoinUp();
            }
            else
            {
                ItemUp();
            }
        }
    }
    IEnumerator Bounce()
    {

        while (true)
        {
            transform.localPosition = new Vector2(transform.localPosition.x, transform.localPosition.y + bouncingSpeed * Time.deltaTime);
            if (transform.localPosition.y >= initialPosition.y + bounce) break;
            yield return null;
        }
        while (true)
        {
            transform.localPosition = new Vector2(transform.localPosition.x, transform.localPosition.y - bouncingSpeed * Time.deltaTime);
            if (transform.localPosition.y <= initialPosition.y)
            {

                if (totalCoin <= 0)
                {
                    Destroy(gameObject);
                    GameObject NullBlock = (GameObject)Instantiate(Resources.Load("Prefabs/NullBlock"));
                    NullBlock.transform.position = initialPosition;
                    break;
                }
                else break;

            }

            yield return null;
        }
    }

    void ItemUp()
    {
        if (Mario.GetComponent<Player>().countLV > 1)
        {
            GameObject Flower = (GameObject)Instantiate(Resources.Load("Prefabs/Flower"));
            Flower.transform.SetParent(this.transform.parent);
            Flower.transform.localPosition = new Vector2(initialPosition.x, initialPosition.y + 1);
        }
        else
        {
            GameObject Mushroom = (GameObject)Instantiate(Resources.Load("Prefabs/Mushroom"));
            Mushroom.transform.SetParent(this.transform.parent);
            Mushroom.transform.localPosition = new Vector2(initialPosition.x, initialPosition.y + 1);
        }
    }
    void CoinUp()
    {
        GameObject Coin = (GameObject)Instantiate(Resources.Load("Prefabs/Coin"));
        /*Coin.transform.SetParent(this.transform.parent);*/
        Coin.transform.localPosition = new Vector2(initialPosition.x, initialPosition.y + 1);
    }
}
