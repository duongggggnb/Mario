using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    private float RunSpeed = 7;
    private float Speed = 0;
    private float MaxSpeed = 13f;
    private bool IsLanding = true;
    private bool IsTurning = false;
    private bool TurnLeft = false;
    private float RealTime;
    private float JumpingHeigh = 450;
    private float LowJump = 5;
    private float HighJump = 5;
    private float TimeToDash = 1f;
    public int countLV = 0;
    public bool trans = false;
    private Vector2 DeadPosition;
    private bool CanShoot = false;
    private bool IsShooting = false;
    bool undead = false;
    private Animator animator;
    private Rigidbody2D rigidbody2D;
    private AudioSource audio;
    private int life = 3;
    public static Vector2 lastCheckpoint = new Vector2(-14.3f, -2.1f);
    public GameObject projectile;
    public Vector2 velocity;
    public Vector2 offset = new Vector2(0.4f, 0.1f);
    public float cooldown = 0.5f;
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        rigidbody2D = GetComponent<Rigidbody2D>();
        audio = GetComponent<AudioSource>();
        DontDestroyOnLoad(this.gameObject);
    }
    private void Awake()
    {
        GameObject.FindGameObjectWithTag("Player").transform.position = lastCheckpoint;
    }
    // Update is called once per frame
    void Update()
    {
        animator.SetFloat("Speed", Speed);
        animator.SetBool("IsLanding", IsLanding);
        animator.SetBool("IsTurning", IsTurning);
        animator.SetBool("IsShooting", IsShooting);
        Jump();
        ShootAndDash();

        if (trans == true)
        {
            switch (countLV)
            {
                case 1:
                    {
                        StartCoroutine(Smaller());
                        MakeSound("Pipe");
                        trans = false;
                        CanShoot = false;
                        break;
                    }
                case 2:
                    {
                        StartCoroutine(Bigger());
                        MakeSound("PowerUp");
                        trans = false;
                        CanShoot = false;
                        break;
                    }
                case 3:
                    {
                        StartCoroutine(Shootable());
                        MakeSound("PowerUp");
                        trans = false;
                        CanShoot = true;
                        break;
                    }
                default: trans = false; break;
            }
        }

        Falling();


    }

    private void FixedUpdate()
    {
        Movement();

    }

    public void Movement()
    {
        float inputKey = Input.GetAxis("Horizontal");
        rigidbody2D.velocity = new Vector2(RunSpeed * inputKey, rigidbody2D.velocity.y);
        Speed = Mathf.Abs(RunSpeed * inputKey);

        if (inputKey > 0 && TurnLeft == true)
        {
            Turn();
        }
        if (inputKey < 0 && TurnLeft == false)
        {
            Turn();

        }

    }


    public void Turn()
    {
        TurnLeft = !TurnLeft;
        Vector2 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;
        if (Speed >= 0)
        {
            StartCoroutine(MarioTurning());
        }
    }

    public void Jump()
    {
        if (Input.GetKeyDown(KeyCode.Space) && IsLanding == true)
        {
            rigidbody2D.AddForce(Vector2.up * JumpingHeigh);
            MakeSound("LowerJump");
            IsLanding = false;
        }
        if (rigidbody2D.velocity.y < 0)
        {
            rigidbody2D.velocity += Vector2.up * Physics2D.gravity.y * (HighJump - 1) * Time.deltaTime;
        }
        else
        {
            if (rigidbody2D.velocity.y > 0 && !Input.GetKey(KeyCode.Space))
            {
                rigidbody2D.velocity += Vector2.up * Physics2D.gravity.y * (LowJump - 1) * Time.deltaTime;
            }
        }

    }
    void Falling()
    {
        Vector2 pos = transform.localPosition;
        if (pos.y <= -6)
        {
            
            DeadAndRespawn();
        }
    }
    void DeadAndRespawn()
    {
        DeadPosition = transform.localPosition;
        GameObject DyingMario = (GameObject)Instantiate(Resources.Load("Prefabs/DyingMario"));
        DyingMario.transform.localPosition = DeadPosition;
        Destroy(this.gameObject);
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.tag == "Mushroom")
        {
            trans = true;
            countLV = 2;
        }
        if (collision.collider.tag == "Flower")
        {
            trans = true;
            countLV = 3;
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Land" || collision.tag == "Block" || collision.tag == "Tube" || collision.tag == "ItemBlock")
        {
            IsLanding = true;
        }

    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.tag == "Land" || collision.tag == "Block" || collision.tag == "Tube" || collision.tag == "ItemBlock")
        {
            IsLanding = true;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Land" || collision.tag == "Block" || collision.tag == "Tube" || collision.tag == "ItemBlock")
        {
            IsLanding = false;
        }
    }

    IEnumerator MarioTurning()
    {
        IsTurning = true;
        yield return new WaitForSeconds(0.2f);
        IsTurning = false;
    }
    public void ShootAndDash()
    {
        if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.LeftControl))
        {
            RealTime += Time.deltaTime;

            if (RealTime >= TimeToDash)
            {
                RunSpeed = RunSpeed * 1.2f;
                if (RunSpeed > MaxSpeed)
                {
                    RunSpeed = MaxSpeed;
                }
            }
            if (Input.GetKeyDown(KeyCode.LeftShift) || Input.GetKeyDown(KeyCode.LeftControl))
            {
                if (CanShoot)
                {
                    Vector2 pos = transform.position;
                    pos.y += 1;
                    pos.x += 1;
                    GameObject go = (GameObject)Instantiate(projectile, pos + offset * transform.localScale.x, Quaternion.identity);
                    go.GetComponent<Rigidbody2D>().velocity = new Vector2(velocity.x * transform.localScale.x, velocity.y);
                    StartCoroutine(DelayAnim());
                    StartCoroutine(Delay());
                }
            }
        }
        if (Input.GetKeyUp(KeyCode.LeftShift) || Input.GetKeyUp(KeyCode.LeftControl))
        {
            RealTime = 0;
            RunSpeed = 7f;
            IsShooting = false;
        }

    }
    IEnumerator Delay()
    {
        CanShoot = false;
        yield return new WaitForSeconds(cooldown);
        CanShoot = true;
    }
    IEnumerator DelayAnim()
    {
        IsShooting = true;
        yield return new WaitForSeconds(0.3f);
        IsShooting = false;
    }
    IEnumerator Bigger()
    {
        float delay = 0.1f;
        animator.SetLayerWeight(animator.GetLayerIndex("SmallMario"), 0);
        animator.SetLayerWeight(animator.GetLayerIndex("BigMario"), 1);
        animator.SetLayerWeight(animator.GetLayerIndex("ShootableMario"), 0);
        yield return new WaitForSeconds(delay);
        animator.SetLayerWeight(animator.GetLayerIndex("SmallMario"), 0);
        animator.SetLayerWeight(animator.GetLayerIndex("BigMario"), 0);
        animator.SetLayerWeight(animator.GetLayerIndex("ShootableMario"), 1);
        yield return new WaitForSeconds(delay);
        animator.SetLayerWeight(animator.GetLayerIndex("SmallMario"), 0);
        animator.SetLayerWeight(animator.GetLayerIndex("BigMario"), 1);
        animator.SetLayerWeight(animator.GetLayerIndex("ShootableMario"), 0);
        yield return new WaitForSeconds(delay);
        animator.SetLayerWeight(animator.GetLayerIndex("SmallMario"), 0);
        animator.SetLayerWeight(animator.GetLayerIndex("BigMario"), 0);
        animator.SetLayerWeight(animator.GetLayerIndex("ShootableMario"), 1);
        yield return new WaitForSeconds(delay);
        animator.SetLayerWeight(animator.GetLayerIndex("SmallMario"), 0);
        animator.SetLayerWeight(animator.GetLayerIndex("BigMario"), 1);
        animator.SetLayerWeight(animator.GetLayerIndex("ShootableMario"), 0);


    }

    IEnumerator Shootable()
    {
        float delay = 0.1f;
        animator.SetLayerWeight(animator.GetLayerIndex("SmallMario"), 0);
        animator.SetLayerWeight(animator.GetLayerIndex("BigMario"), 0);
        animator.SetLayerWeight(animator.GetLayerIndex("ShootableMario"), 1);
        yield return new WaitForSeconds(delay);
        animator.SetLayerWeight(animator.GetLayerIndex("SmallMario"), 0);
        animator.SetLayerWeight(animator.GetLayerIndex("BigMario"), 1);
        animator.SetLayerWeight(animator.GetLayerIndex("ShootableMario"), 0);
        yield return new WaitForSeconds(delay);
        animator.SetLayerWeight(animator.GetLayerIndex("SmallMario"), 0);
        animator.SetLayerWeight(animator.GetLayerIndex("BigMario"), 0);
        animator.SetLayerWeight(animator.GetLayerIndex("ShootableMario"), 1);
        yield return new WaitForSeconds(delay);
        animator.SetLayerWeight(animator.GetLayerIndex("SmallMario"), 0);
        animator.SetLayerWeight(animator.GetLayerIndex("BigMario"), 1);
        animator.SetLayerWeight(animator.GetLayerIndex("ShootableMario"), 0);
        yield return new WaitForSeconds(delay);
        animator.SetLayerWeight(animator.GetLayerIndex("SmallMario"), 0);
        animator.SetLayerWeight(animator.GetLayerIndex("BigMario"), 0);
        animator.SetLayerWeight(animator.GetLayerIndex("ShootableMario"), 1);

    }
    IEnumerator Smaller()
    {
        undead = true;
        float delay = 0.1f;
        animator.SetLayerWeight(animator.GetLayerIndex("SmallMario"), 0);
        animator.SetLayerWeight(animator.GetLayerIndex("BigMario"), 0);
        animator.SetLayerWeight(animator.GetLayerIndex("ShootableMario"), 1);
        yield return new WaitForSeconds(delay);
        animator.SetLayerWeight(animator.GetLayerIndex("SmallMario"), 0);
        animator.SetLayerWeight(animator.GetLayerIndex("BigMario"), 1);
        animator.SetLayerWeight(animator.GetLayerIndex("ShootableMario"), 0);
        yield return new WaitForSeconds(delay);
        animator.SetLayerWeight(animator.GetLayerIndex("SmallMario"), 1);
        animator.SetLayerWeight(animator.GetLayerIndex("BigMario"), 0);
        animator.SetLayerWeight(animator.GetLayerIndex("ShootableMario"), 0);
        yield return new WaitForSeconds(delay);
        animator.SetLayerWeight(animator.GetLayerIndex("SmallMario"), 0);
        animator.SetLayerWeight(animator.GetLayerIndex("BigMario"), 1);
        animator.SetLayerWeight(animator.GetLayerIndex("ShootableMario"), 0);
        yield return new WaitForSeconds(delay);
        animator.SetLayerWeight(animator.GetLayerIndex("SmallMario"), 1);
        animator.SetLayerWeight(animator.GetLayerIndex("BigMario"), 0);
        animator.SetLayerWeight(animator.GetLayerIndex("ShootableMario"), 0);
        yield return new WaitForSeconds(delay);
        animator.SetLayerWeight(animator.GetLayerIndex("SmallMario"), 0);
        animator.SetLayerWeight(animator.GetLayerIndex("BigMario"), 0);
        animator.SetLayerWeight(animator.GetLayerIndex("ShootableMario"), 1);
        yield return new WaitForSeconds(delay);
        animator.SetLayerWeight(animator.GetLayerIndex("SmallMario"), 1);
        animator.SetLayerWeight(animator.GetLayerIndex("BigMario"), 0);
        animator.SetLayerWeight(animator.GetLayerIndex("ShootableMario"), 0);
        yield return new WaitForSeconds(2);
        undead = false;
    }

    public void Dead()
    {
        if (!undead)
        {
            life--;
            if (life > 0)
            {
                DeadAndRespawn();
            }
            if (life <= 0)
            {

            }

        }

    }

    public void MakeSound(string fileName)
    {
        audio.PlayOneShot(Resources.Load<AudioClip>("Audios/" + fileName));
    }

}
