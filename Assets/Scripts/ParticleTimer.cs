using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleTimer : MonoBehaviour
{
    BoxCollider2D bc;
    ParticleSystem particles;
    [SerializeField] float offset_time;
    [SerializeField] float on_time;
    [SerializeField] float off_time;

    private void Start()
    {
        bc = GetComponentInChildren<BoxCollider2D>();
        particles = GetComponentInChildren<ParticleSystem>();

        StartCoroutine(Timer());
    }


    IEnumerator Timer()
    {
        yield return new WaitForSeconds(offset_time);

        while (true)
        {
            //bc.enabled = true;
            
            particles.Play();

            yield return new WaitForSeconds(.7f);

            bc.gameObject.SetActive(true);


            yield return new WaitForSeconds(on_time);

            //bc.enabled = false;
            particles.Stop();

            yield return new WaitForSeconds(.7f);
            
            bc.gameObject.SetActive(false);

            yield return new WaitForSeconds(off_time);
        }
    }



}
