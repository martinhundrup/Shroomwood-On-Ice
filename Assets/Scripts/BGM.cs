using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BGM : MonoBehaviour
{
    AudioSource audio_source;
    [SerializeField] AudioClip main_menu;
    [SerializeField] AudioClip stage1;
    [SerializeField] AudioClip stage2;
    [SerializeField] AudioClip stage3;
    [SerializeField] AudioClip stage4;
    [SerializeField] AudioClip game_over;
    float start_time;
    public float current_time;
    public float game_start_time;
    public int score;
    PlayerController shroomie;
    public Vector2 save_point;
    [SerializeField] GameObject coin;
    public bool has_collected_coin = false;
    [SerializeField] int coin_count = 0;

    private void Awake()
    {
        shroomie = FindObjectOfType<PlayerController>();
        


        int BGM_count = FindObjectsOfType<BGM>().Length;
        if (BGM_count > 1)
        {
            gameObject.SetActive(false);
            Destroy(gameObject);
        }

        else
        {
            DontDestroyOnLoad(gameObject);
        }
    }

    public void CoinCollected()
    {
        coin_count++;
        has_collected_coin = true;
        Destroy(coin);
    }

    public void OnReload()
    {
        shroomie = FindObjectOfType<PlayerController>();


        if (GameObject.FindGameObjectWithTag("Coin") != null)
            coin = GameObject.FindGameObjectWithTag("Coin");
        if (has_collected_coin && coin != null)
        {
            Destroy(coin);
        }

        
    }

    public void NextScene()
    {
        start_time = Time.time;
        current_time = 0f;
        save_point.x = save_point.y = 1;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        ResetMusic(1);
        has_collected_coin = false;
    }

    public void ResetMusic(int offset)
    {
        // start bgm
        if (SceneManager.GetActiveScene().buildIndex + offset == 0)
        {
            audio_source.clip = main_menu;
            audio_source.PlayDelayed(0f);
        }
        else if (SceneManager.GetActiveScene().buildIndex + offset == 1)
        {
            audio_source.clip = stage1;
            audio_source.PlayDelayed(0f);
        }
        else if (SceneManager.GetActiveScene().buildIndex + offset == 2)
        {
            audio_source.clip = stage2;
            audio_source.PlayDelayed(0f);
        }
        else if (SceneManager.GetActiveScene().buildIndex + offset == 3)
        {
            audio_source.clip = stage3;
            audio_source.PlayDelayed(0f);
        }
        else if (SceneManager.GetActiveScene().buildIndex + offset == 4)
        {
            audio_source.clip = stage4;
            audio_source.PlayDelayed(0f);
        }
    }

    private void Start()
    {
        audio_source = GetComponent<AudioSource>();
        shroomie = FindObjectOfType<PlayerController>();
        


        ResetMusic(0);
        OnReload();
    }

    private void Update()
    {
        current_time = Time.time - start_time;

        if (Input.GetKeyDown(KeyCode.R) && !FindObjectOfType<TimerText>().finished_level)
        {
            start_time = Time.time;
            has_collected_coin = false;
            current_time = 0f;
            save_point.x = save_point.y = 1;
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }

        if (Input.GetButtonDown("Select"))
        {
            Application.Quit();
        }

        if (SceneManager.GetActiveScene().buildIndex == 0)
        {
            if (Input.GetButtonDown("Start"))
            {
                game_start_time = Time.time;
                start_time = Time.time;
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
            }
            if (Input.GetButtonDown("Select"))
            {
                Application.Quit();
            }
        }
        if (SceneManager.GetActiveScene().buildIndex == 5)
        {
            if (Input.GetButtonDown("Select"))
            {
                Application.Quit();
            }
        }
    }





}
