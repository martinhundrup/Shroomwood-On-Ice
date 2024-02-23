using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float speed; // the top speed of the player
    [SerializeField] private int health; // the current health of the player
    [SerializeField] private bool is_moving = false; // whether or not the player is currently moving
    [SerializeField] private bool on_ice = false; // whether or not the player is currently on ice
    [SerializeField] private Vector2 movement_vect; // the current vector the player should move in
    [SerializeField] private Vector2 target_pos; // the current target poisition of the player's movement
    [SerializeField] private Vector2 last_position; // the previous position the player was stationed at
    [SerializeField] bool frostbit = false;
    [SerializeField] ParticleSystem frozen_particles; // the particle system to activate upon frostbit death
    [SerializeField] ParticleSystem fire_particles; // the particle system to activate upon torched death
    TimerText timer;
    int on_ice_counter = 0;
    SpriteRenderer sr;
    Animator animator;
    Rigidbody2D rb;
    CameraMovement cam;
    AudioSource audio_source;
    BGM bgm;
    [SerializeField] AudioClip death_sfx;
    [SerializeField] AudioClip coin_sfx;
    [SerializeField] AudioClip bump_sfx;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        cam = FindObjectOfType<CameraMovement>();
        audio_source = GetComponent<AudioSource>();
        bgm = FindObjectOfType<BGM>();
        timer = FindObjectOfType<TimerText>();

        bgm.OnReload();
        transform.position = bgm.save_point;
        last_position = target_pos = transform.position; // make sure the player is not moving upon start
    }

    // Update is called once per frame
    void Update()
    {
        UserInput();
        Movement();
        SetLastPosition();
        AnimationUpdate();
    }

    private void UserInput(){

        // movement input
        if (!is_moving)
        {
            if (Input.GetButton("Right")) // move right
            {
                movement_vect = new Vector2(1f, 0f);
                target_pos.x = Mathf.RoundToInt(transform.position.x + 1);
                last_position = transform.position;
                sr.flipX = false;
                is_moving = true;
            }
            else if (Input.GetButton("Left")) // move left
            {
                movement_vect = new Vector2(-1f, 0f);
                target_pos.x = Mathf.RoundToInt(transform.position.x - 1);
                sr.flipX = true;
                last_position = transform.position;

                is_moving = true;
            }
            else if (Input.GetButton("Up")) // move up
            {
                movement_vect = new Vector2(0f, 1f);
                target_pos.y = Mathf.RoundToInt(transform.position.y + 1);
                last_position = transform.position;

                is_moving = true;
            }
            else if (Input.GetButton("Down")) // move down
            {
                movement_vect = new Vector2(0f, -1f);
                target_pos.y = Mathf.RoundToInt(transform.position.y - 1);
                last_position = transform.position;

                is_moving = true;
            }
        }
    }


    private void Movement()
    {
        if (!is_moving)
        {
            last_position = transform.position;
        }
        
        if(is_moving)
        {
            rb.velocity = movement_vect * speed; // sets player speed

            // checks if the target position has been reached
            if ((movement_vect.x < 0 && transform.position.x <= target_pos.x) || (movement_vect.x > 0 && transform.position.x >= target_pos.x)
                || (movement_vect.y < 0 && transform.position.y <= target_pos.y) || (movement_vect.y > 0 && transform.position.y >= target_pos.y))


            //if (Vector2.Distance(target_pos, transform.position) < 0.01f || Vector2.Distance(target_pos, transform.position) > 1.01f) // use your tools lol this is much more succinct
            {
                if (!on_ice && !frostbit)
                {
                    is_moving = false;
                    rb.velocity = Vector2.zero;
                    // make sure player is exactly on target point
                    transform.position = target_pos;
                }
                else
                {
                    if (movement_vect.x > 0) // keep moving right
                    {
                        target_pos.x = Mathf.RoundToInt(transform.position.x + 1);
                        //last_position = transform.position;
                    }
                    else if (movement_vect.x < 0) // keep moving left
                    {
                        target_pos.x = Mathf.RoundToInt(transform.position.x - 1);
                        //last_position = transform.position;
                    }
                    else if (movement_vect.y > 0) // keep moving up
                    {
                        target_pos.y = Mathf.RoundToInt(transform.position.y + 1);
                        //last_position = transform.position;
                    }
                    else if (movement_vect.y < 0) // keep moving down
                    {
                        target_pos.y = Mathf.RoundToInt(transform.position.y - 1);
                        //last_position = transform.position;
                    }
                }
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Wall")) // did we collide with a wall?
        {
            //is_moving = false;
            //rb.velocity = Vector2.zero;
            //rb.MovePosition(last_position);
            //audio_source.PlayOneShot(bump_sfx);

            target_pos = last_position;
            if (on_ice || frostbit)
            {
                is_moving = false;
                rb.velocity = Vector2.zero;
                transform.position = last_position;

                if (frostbit) // player dies on collision with a wall when frozen
                {
                    StartCoroutine(OnDeath());
                    frozen_particles.gameObject.SetActive(true);
                }
            }
        }

        if (collision.gameObject.CompareTag("Spike"))
        {
            StartCoroutine(OnDeath());
            frozen_particles.gameObject.SetActive(true);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Ice")){
            on_ice = true;
            on_ice_counter++;
        }
        if (collision.CompareTag("Frost"))
        {
            frostbit = true;
        }
        if (collision.CompareTag("Fire"))
        {
            if (frostbit)
                frostbit = false;
            else
            {
                fire_particles.gameObject.SetActive(true);
                StartCoroutine(OnDeath());

            }
        }

        if (collision.CompareTag("CameraTarget"))
        {
            cam.UpdateTarget(collision.gameObject.transform);
        }
        if (collision.CompareTag("Save"))
        {
            bgm.save_point = collision.transform.position;
        }
        if (collision.CompareTag("Coin"))
        {
            bgm.CoinCollected();
            audio_source.PlayOneShot(death_sfx);
        }
        if (collision.CompareTag("Goal"))
        {
            //bgm.NextScene();
            timer.LevelComplete();
            sr.enabled = false;
            rb.velocity = Vector2.zero;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Ice"))
        {
            on_ice_counter--;
            if (on_ice_counter <= 0)
                on_ice = false;
        }
        
    }

    private void SetLastPosition()
    {
        int rounded_xpos = Mathf.RoundToInt(transform.position.x);
        int rounded_ypos = Mathf.RoundToInt(transform.position.y);
        Vector2 rounded_pos = new Vector2(rounded_xpos, rounded_ypos);

        if (Vector2.Distance(rounded_pos, transform.position) < 0.1)
            last_position = rounded_pos;
    }


    private void AnimationUpdate()
    {
        animator.SetBool("frostbit", frostbit);
    }

    IEnumerator OnDeath()
    {
        sr.enabled = false;
        rb.velocity = Vector2.zero;
        audio_source.PlayOneShot(death_sfx);

        yield return new WaitForSeconds(0.5f);

        string currentSceneName = SceneManager.GetActiveScene().name;
        SceneManager.LoadScene(currentSceneName);
    }



}
