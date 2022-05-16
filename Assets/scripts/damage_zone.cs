using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class damage_zone : MonoBehaviour
{
    public float damage = 0;
    public float fadedur = 0.8f;
    [ColorUsage(true, true)]
    public Color fadecol;
    private Renderer faderend;
    public int a = 0;
    public BoxCollider col;

    public void Start()
    {
        faderend = GetComponent<Renderer>();
    }

    public void updamage(float newd)
    {
        damage = newd;
    }


    public IEnumerator faderoutine(float ai, float ao)
    {
        float timer = 0;
        while (timer < fadedur)
        {
            Color newcolor = fadecol;
            newcolor.r = Mathf.Lerp(ai, ao, timer / fadedur);
            faderend.material.SetColor("_Emission", newcolor);
            timer += Time.deltaTime;
            yield return null;
        }
        if (a == 1)
        {
            a = 2;
            gameObject.SetActive(false);
        }
        if (a == 0)
        {
            a = 1;
            col.enabled = true;
            StartCoroutine(droutine());
            fadein();
        }
        if (a == 2)
            a = 0;
    }

    public IEnumerator droutine()
    {
        float timer = 0;
        while (timer < 0.3)
        {
            timer += Time.deltaTime;
            yield return null;
        }
        col.enabled = false;
    }

    public void fade(float ai, float ao)
    {
        StartCoroutine(faderoutine(ai, ao));
    }

    public void fadein()
    {
        fade(1, 0);
    }

    public void fadeout()
    {
        fade(0, 1);
    }

    public void attack(float timing)
    {
        fadedur = timing/2;
        faderend = GetComponent<Renderer>();
        fadeout();
    }
}
