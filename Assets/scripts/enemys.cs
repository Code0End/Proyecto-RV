using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class enemys : MonoBehaviour
{
    private enum State
    {
        non_vulnerable,
        vulnerable,
    }
    private State state;

    private UnityEngine.Object enemyref;

    private string currentState;

    const string idle = "Idle";
    const string elbow_left = "elbow_left";
    const string elbow_right = "elbow_right";
    const string right_hook = "right_hook";

    public float hp;
    public float maxhp;
    public float posture,maxposture;
    public bool v = false;
    public int isatack = 0;
    public Animator anim;
    public Rigidbody RIGID_BODY;
    Collider[] colliders;
    Rigidbody[] limbsrbs;
    AudioSource ass;
    public AudioClip gothit;
    public AudioClip respawnso;

    public Rigidbody gc1;
    public SkinnedMeshRenderer skin;
    Animator anime;

    private Material[] dissm;
    hb hb;
    float dissolverate = 0.02f;
    float rr=0.04f; 

    Canvas cv;

    void Start()
    {
        state = State.non_vulnerable;
        this.anim.ResetTrigger("vulnerable");
        v = false;
        enemyref = Resources.Load("monk1");
        anime = GetComponent<Animator>();
        hp = 100.0f;
        maxhp = 100.0f;
        posture = 0.0f;
        maxposture = 100.0f;
        ass = this.gameObject.GetComponent<AudioSource>();
        colliders = this.gameObject.GetComponentsInChildren<Collider>();
        limbsrbs = this.gameObject.GetComponentsInChildren<Rigidbody>();
        hb = this.gameObject.GetComponent<hb>();
        SetRagdollParts();
        cv = this.gameObject.GetComponentInChildren<Canvas>();
        if(skin != null)
        {
            dissm = skin.materials;
        }
        ass.clip = respawnso;
        ass.pitch = UnityEngine.Random.Range(0.8f, 1.2f);
        ass.Play();
        StartCoroutine(Essolve());
    }

    void Update()
    {
        switch (state)
        {
            default:
            case State.non_vulnerable:
                if (isatack == 0)
                {
                    ChangeAnimationState(idle);
                    isatack = 1;
                    StartCoroutine(wind_attack());
                }
                if (isatack == 2)
                {
                    select_attack();
                    isatack = 3;
                }
                if (isatack == 3)
                {
                    isatack = 1;
                }
                break;
            case State.vulnerable:
                if (v == false)
                {
                    v = true;
                    go_non_vulnerable();
                }  
                break;

        }
    }

    public void taked(float d)
    {
        if (v == false) return;
        hp = hp - d;
        GotHit();
        hb.UpdateHealth(hp / maxhp);
        if (hp <= 0)
        {
            ass.clip = gothit;
            ass.pitch = UnityEngine.Random.Range(0.2f, 0.6f);
            ass.Play();
            Debug.Log("DEATH");
            cv.enabled = false;
            Invoke("Respawn",4);
            TurnOnRagdoll();
            StartCoroutine(Dissolve());

        }
    }

    private void GotHit()
    {
        this.anim.SetTrigger("hit");
    }

    private void TurnOnRagdoll()
    {        
        foreach (Collider c in colliders)
        {
            c.enabled = true;
        }
        foreach (Rigidbody c in limbsrbs)
        {
            c.isKinematic = false;
        }
        GetComponent<BoxCollider>().enabled = false;
        GetComponent<Rigidbody>().isKinematic = true;
        anim.enabled = false;
        anim.avatar = null;

        float x = UnityEngine.Random.Range(-1f,1f);
        gc1.AddForce(new Vector3(x,0f,1f)*100f,ForceMode.Impulse);
        gc1.AddForce(Vector3.up * UnityEngine.Random.Range(0f,100f), ForceMode.Impulse);



    }

    private void SetRagdollParts()
    {    
        foreach (Collider c in colliders)
        {
            c.enabled = false;
            
        }
        foreach (Rigidbody c in limbsrbs)
        {
            
                c.isKinematic = true;
            
        }

        this.gameObject.GetComponent<BoxCollider>().enabled = true;
        this.gameObject.GetComponent<Rigidbody>().isKinematic = false;
    }

    IEnumerator Dissolve()
    {
        yield return new WaitForSeconds(0.2f);

        float c= 0;

        while(dissm[0].GetFloat("_dissolveamount") < 1)
        {
            c += dissolverate;
            for (int i = 0; i < dissm.Length; i++)
                dissm[i].SetFloat("_dissolveamount",c);
            yield return new WaitForSeconds(rr);
        }
        Destroy(gameObject, 7);
    }

    IEnumerator Essolve()
    {
        yield return new WaitForSeconds(0.2f);

        float c = 1;

        while (dissm[0].GetFloat("_dissolveamount") > 0)
        {
            c -= dissolverate;
            for (int i = 0; i < dissm.Length; i++)
                dissm[i].SetFloat("_dissolveamount", c);
            yield return new WaitForSeconds(rr);
        }
    }

    IEnumerator recovery()
    {
        yield return new WaitForSeconds(5.0f);
        v = false;
        state = State.non_vulnerable;
        
    }

    IEnumerator wind_attack()
    {
        yield return new WaitForSeconds(3.0f);
        isatack = 2;
    }

    IEnumerator cooldown_attack()
    {
        yield return new WaitForSeconds(4.0f);
        isatack = 0;
    }
    IEnumerator attack_len(float a)
    {
        yield return new WaitForSeconds(a);
        isatack = 0;
    }

    void go_non_vulnerable()
    {
        StartCoroutine(recovery());
    }

    void Respawn()
    {
        GameObject clone1 = (GameObject)Instantiate(enemyref);
        clone1.transform.position = transform.position;
    }

    void ChangeAnimationState(string newState)
    {
        if (currentState == newState) return;
        //anime.Play(newState);
        anime.CrossFade(newState,0.1f);
        currentState = newState;
    }

    void select_attack()
    {
        int r = UnityEngine.Random.Range(1,4);
        switch (r){
            case 1:
                ChangeAnimationState(right_hook);
                StartCoroutine(attack_len(1.667f));
                break;
            case 2:
                ChangeAnimationState(elbow_right);
                StartCoroutine(attack_len(1.667f));
                break;
            case 3:
                ChangeAnimationState(elbow_left);
                StartCoroutine(attack_len(1.650f));
                break;
            default:
                break;

        }
    }
}
