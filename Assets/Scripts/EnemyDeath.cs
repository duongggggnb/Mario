using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDeath : MonoBehaviour
{
    public bool IsTurtle;
    Vector2 DeadPosition;
    public GameObject Turtle;
    // Start is called before the first frame update
    void Start()
    {
        DeadPosition = transform.localPosition;
        if (!IsTurtle) StartCoroutine(Disappear());
        if (IsTurtle) StartCoroutine(WaitForRespawn());
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    IEnumerator Disappear()
    {
        yield return new WaitForSeconds(2f);
        Destroy(gameObject);
    }
    IEnumerator WaitForRespawn()
    {
        yield return new WaitForSeconds(2.5f);
        Destroy (gameObject);
        GameObject TurtleWaitForRespawn = (GameObject)Instantiate(Resources.Load("Prefabs/GreenTurtleWaitForRespawn"));
        TurtleWaitForRespawn.transform.localPosition = DeadPosition;
    }
}
