using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class fadescreen : MonoBehaviour
{
    public float fadedur = 2;
    public Color fadecol;
    private Renderer faderend;

    void Start()
    {
        faderend = GetComponent<Renderer>();
        fadein();
    }

    public void fadein()
    {
        fade(1,0);
    }

    public void fadeout()
    {
        fade(0,1);
    }

    public void fade(float ai, float ao)
    {
        StartCoroutine(faderoutine(ai,ao));
    }

    public IEnumerator faderoutine(float ai, float ao)
    {
        float timer = 0;
        while (timer < fadedur)
        {
            Color newcolor = fadecol;
            newcolor.a = Mathf.Lerp(ai, ao,timer/fadedur);
            faderend.material.SetColor("_BaseColor", newcolor);
            timer += Time.deltaTime;
            yield return null;
        }
        Color newcolor2 = fadecol;
        newcolor2.a = ao;
        faderend.material.SetColor("_BaseColor", newcolor2);
    }
}
