using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class TimerText : MonoBehaviour
{

    BGM bgm;
    TextMeshProUGUI text;
    [SerializeField] float time_to_beat = 0f;
    [SerializeField] GameObject star1;
    [SerializeField] GameObject star2;
    [SerializeField] GameObject star3;
    [SerializeField] GameObject empty_star2;
    [SerializeField] GameObject empty_star3;
    [SerializeField] GameObject text1;
    [SerializeField] GameObject text2;
    [SerializeField] GameObject text3;
    [SerializeField] GameObject time_text;
    [SerializeField] GameObject background;
    public bool finished_level = false;


    // Start is called before the first frame update
    void Start()
    {
        bgm = FindObjectOfType<BGM>();
        text = GetComponent<TextMeshProUGUI>();


        // deativate level end stuff
        star1.SetActive(false);
        star2.SetActive(false);
        star3.SetActive(false);
        empty_star2.SetActive(false);
        empty_star3.SetActive(false);
        text1.SetActive(false);
        text2.SetActive(false);
        text3.SetActive(false);
        time_text.SetActive(false);
        background.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        text.text = bgm.current_time.ToString();

        if (bgm.current_time > time_to_beat)
        {
            text.color = Color.red;
        }

        if (finished_level && Input.GetButtonDown("Start"))
        {
            bgm.NextScene();
        }

    }

    public void LevelComplete()
    {
        text.enabled = false;
        text1.SetActive(true);
        text2.SetActive(true);
        text3.SetActive(true);
        background.SetActive(true);
        finished_level = true;
        time_text.SetActive(true);
        time_text.GetComponent<TextMeshProUGUI>().text = bgm.current_time.ToString("0.000");

        star1.SetActive(true);
        bgm.score++;
        if (bgm.has_collected_coin)
        {
            star2.SetActive(true);
            bgm.score++;

        }
        else
            empty_star2.SetActive(true);
        if (bgm.current_time <= time_to_beat)
        {
            star3.SetActive(true);
            bgm.score++;

        }
        else
            empty_star3.SetActive(true);
    }

    

}
