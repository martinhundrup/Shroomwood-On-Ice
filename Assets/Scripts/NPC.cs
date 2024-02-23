using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class NPC : MonoBehaviour
{
    TextMeshPro text_box;

    // Start is called before the first frame update
    void Start()
    {
        // find and disable npc text box
        text_box = GetComponentInChildren<TextMeshPro>();
        text_box.enabled = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            text_box.enabled = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            text_box.enabled = false;
        }
    }
}
