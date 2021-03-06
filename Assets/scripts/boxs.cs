using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class boxs : MonoBehaviour
{
    public float hp;
    public float maxhp;
    public int mn;
    public GameObject version_destruida;
    public AudioSource as1;
    public float fadedur = 2;
    public fadescreen fs;
    GameObject fs1;
    GameObject mainc;

    public float damage;

    public AudioClip transsong;

    private Rigidbody rbb1;
    private SpringJoint rbb2;
    private BoxCollider rbb3;
    private MeshRenderer rbb4;

    void Start()
    {
        fadedur = 3.8f;
        hp = 100.0f;
        maxhp = 100.0f;
        fs1 = GameObject.FindGameObjectWithTag("sffder");
        mainc = GameObject.FindGameObjectWithTag("MainCamera");
        fs = fs1.GetComponent<fadescreen>();
    }

    public void box_action()
    {
        fs.fadeout();
        fade(0, 1);
    }

    public void taked(float d)
    {
        as1.pitch = Random.Range(0.5f,1.8f);
        as1.Play();
        hp = hp - d;
        if (hp <= 0)
        {
            Instantiate(version_destruida,transform.position,transform.rotation);
            Debug.Log("DEATH");
            box_action();
            rbb1 = gameObject.GetComponent<Rigidbody>();
            rbb2 = gameObject.GetComponent<SpringJoint>();
            rbb3 = gameObject.GetComponent<BoxCollider>();
            rbb4 = gameObject.GetComponent<MeshRenderer>();
            Destroy(rbb2);
            Destroy(rbb1);
            Destroy(rbb3);
            Destroy(rbb4);
        }
    }

    public void fade(float ai, float ao)
    {
        StartCoroutine(faderoutine(ai, ao));
    }

    public IEnumerator faderoutine(float ai, float ao)
    {
        float timer = 0;
        while (timer < fadedur)
        {
            timer += Time.deltaTime;
            yield return null;
        }
        if (mn == 0)
        {

            mainc.GetComponent<player>().p_c.SetActive(true);
            mainc.GetComponent<player>().player_healthbar.UpdateHealth(100 / 100);
            mainc.GetComponent<player>().ass2.clip = transsong;
            mainc.GetComponent<player>().ass2.Play();
            SceneManager.LoadScene(2);
            fs.fadein();
        }
        
        if (mn == 2)
        {
            fs.fadeout();
            mainc.GetComponent<player>().p_c.SetActive(false);
            mainc.GetComponent<player>().ass2.clip = transsong;
            mainc.GetComponent<player>().ass2.Play();
            SceneManager.LoadScene(1);
            fs.fadein();
        }
        if (mn == 3)
        {
            mainc.GetComponent<player>().p_c.SetActive(true);
            mainc.GetComponent<player>().player_healthbar.UpdateHealth(100 / 100);
            mainc.GetComponent<player>().ass2.clip = transsong;
            mainc.GetComponent<player>().ass2.Play();
            mainc.GetComponent<player>().mn = 1;
            SceneManager.LoadScene(2);
            fs.fadein();
        }
        if (mn == 1)
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
         Application.Quit();
#endif
        }


        Destroy(gameObject);
    }
}
