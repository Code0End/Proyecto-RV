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

    private Rigidbody rbb1;
    private SpringJoint rbb2;
    private BoxCollider rbb3;
    private MeshRenderer rbb4;

    void Start()
    {
        hp = 100.0f;
        maxhp = 100.0f;
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
            Destroy(rbb1);
            Destroy(rbb2);
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
        if (mn==0)
            SceneManager.LoadScene(1);
        if (mn == 1)
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
         Application.Quit();
#endif
        }
            if (mn == 2)
            SceneManager.LoadScene(0);

        Destroy(gameObject);
    }
}
