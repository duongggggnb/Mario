using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DyingMario : MonoBehaviour
{
    Vector2 DeadPosition;
    public float bounce = 20f;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        StartCoroutine(Disappear());
    }
    IEnumerator Disappear()
    {
        while (true)
        {

            transform.localPosition = new Vector2(transform.localPosition.x, transform.localPosition.y + bounce * Time.deltaTime);
            if (transform.localPosition.y >= DeadPosition.y + 50f)
                break;
            yield return null;
            transform.localPosition = new Vector2(transform.localPosition.x, transform.localPosition.y - bounce * Time.deltaTime);
            if (transform.localPosition.y <= -10)
            {
                transform.localPosition = new Vector2(transform.localPosition.x, -10);
                yield return new WaitForSeconds(0.2f);

                FindObjectOfType<Checkpoint>().RespawnAtCheckpoint();
                Destroy(gameObject);
                break;
            }
            yield return null;

        }
        
    }
}
