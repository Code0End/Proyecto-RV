using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class player : MonoBehaviour
{
    public float maxhp = 100;
    public float hp;
    public hb player_healthbar;
    public GameObject p_c;
    public AudioSource ass;
    public AudioSource ass2;
    public AudioClip mmsong;
    public fadescreen fs1;
    public fadescreen fs2;
    public int mn = 0;

    void Start()
    {
        
        hp = 100;
    }

    void Update()
    {

    }

    public void taked(float d)
    {
        hp = hp - d;
        player_healthbar.UpdateHealth(hp / maxhp);
        if (hp <= 0)
        {
            ass.pitch = UnityEngine.Random.Range(0.2f, 0.6f);
            ass.Play();
            death_action();
}
        else
        {
            ass.pitch = UnityEngine.Random.Range(0.8f, 1.2f);
            ass.Play();
        }
    }

    public void death_action()
    {
        fs1.fadeout();
        fade(0, 1);
    }

    public void fade(float ai, float ao)
    {
        StartCoroutine(faderoutine(ai, ao));
    }

    public IEnumerator faderoutine(float ai, float ao)
    {
        float timer = 0;
        while (timer < 3.8)
        {
            timer += Time.deltaTime;
            yield return null;
        }
        if (mn == 0)
        {
            p_c.SetActive(false);
            player_healthbar.UpdateHealth(hp / maxhp);
            ass2.Stop();
            ass2.clip = mmsong;
            ass2.Play();
            fs1.fadein();
            hp = 100;
            SceneManager.LoadScene(1);
        }
       else
        {
            p_c.SetActive(false);
            player_healthbar.UpdateHealth(hp / maxhp);
            ass2.Stop();
            ass2.clip = mmsong;
            ass2.Play();
            fs1.fadein();
            hp = 100;
            Destroy(GameObject.FindGameObjectWithTag("penemy"));
            
        }
    }

    void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.tag == "damage_zone")
        {
            taked(collider.gameObject.GetComponent<damage_zone>().damage);
        }
        if (collider.gameObject.tag == "damage_proyectile")
        {
            taked(collider.gameObject.GetComponent<proyectil>().damage);
        }
        if (collider.gameObject.tag == "damage_box")
        {
            taked(collider.gameObject.GetComponent<boxs>().damage);
        }
    }
}

