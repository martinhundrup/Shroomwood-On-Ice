using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class End : MonoBehaviour
{

    [SerializeField] TextMeshPro score_text;
    [SerializeField] TextMeshPro time_text;
    BGM bgm;


    // Start is called before the first frame update
    void Start()
    {
        bgm = FindObjectOfType<BGM>();

        score_text.text = "score: " + bgm.score + "/10";
        time_text.text = "time: " + (Time.time - bgm.game_start_time).ToString("0.000");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
