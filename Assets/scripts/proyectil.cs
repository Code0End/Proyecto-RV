using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class proyectil : MonoBehaviour
{

    public float damage = 5;
    public float speed = .1f;

    GameObject mainc;
    public Vector3 directr;

    public AudioSource ass;
    public AudioSource ass2;

    public void updamage(float newd)
    {
        damage = newd;
    }

    public void upspeed(float news)
    {
        speed = news;
    }

    void Start()
    {
        ass.pitch = UnityEngine.Random.Range(0.8f, 1.2f);
        ass2.pitch = UnityEngine.Random.Range(0.8f, 1.2f);
        ass.Play();
        ass2.Play();
        mainc = GameObject.FindGameObjectWithTag("MainCamera");
        directr = mainc.transform.position - transform.position;
    }


    void Update()
    {
        if (mainc == null) return;
        float framedistance = speed * Time.deltaTime;
        transform.Translate(directr.normalized*framedistance,Space.World);
    }

    void OnTriggerEnter(Collider collider)
    {
        Destroy(gameObject);
    }
}
