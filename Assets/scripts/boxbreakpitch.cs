using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class boxbreakpitch : MonoBehaviour
{
    public AudioSource as1;
    void Awake()
    {
        as1 = gameObject.GetComponent<AudioSource>();
        as1.pitch = Random.Range(0.5f, 1.8f);
        as1.Play();
    }
}
