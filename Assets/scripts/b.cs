using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class b : MonoBehaviour
{
    void Start()
    {
        fade(0,1);
    }

    public void fade(float ai, float ao)
    {
        StartCoroutine(faderoutine(ai, ao));
    }

    public IEnumerator faderoutine(float ai, float ao)
    {
        float timer = 0;
        while (timer < 1.2)
        {
            timer += Time.deltaTime;
            yield return null;
        }
        SceneManager.LoadScene(1);

    }
}
