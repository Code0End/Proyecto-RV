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
    const string dizzy = "dizzy";
    const string elbow_left = "elbow_left";
    const string elbow_right = "elbow_right";
    const string right_hook = "right_hook";
    const string mid_slam = "mid_slam";
    const string left_punch = "left_punch";
    const string right_punch = "right_punch";

    public GameObject left_elbow1;
    public GameObject left_elbow2;
    public GameObject right_elbow1;
    public GameObject right_elbow2;
    public GameObject overhead_1;
    public GameObject midslam_1;

    public GameObject left_plane;
    public GameObject right_plane;

    public GameObject dparent;

    public float[] attacktimes = { 1.667f, 1.650f, 4.783f,1.2f };
    public float attackt;
    public float attackm = 0.5f;

    public GameObject player;
    public float shp;

    public float hp;
    public float maxhp;
    public float posture =0f,maxposture =100f;
    public bool v = false;
    public int isatack = 0;
    public Animator anim;
    public Rigidbody RIGID_BODY;
    Collider[] colliders;
    Rigidbody[] limbsrbs;
    AudioSource ass;
    public AudioClip gothit;
    public AudioClip respawnso;
    public AudioClip pbreak;

    public Rigidbody gc1;
    public SkinnedMeshRenderer skin;
    Animator anime;

    private Material[] dissm;
    public hb hb;
    public hb pst;
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
            isatack = 4;
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
        dparent.GetComponent<dedestroy>().dedestroys();
        //Destroy(gameObject, 7);
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
        yield return new WaitForSeconds(8.0f);
        v = false;
        state = State.non_vulnerable;
        posture = 0f;
        pst.UpdateHealth(posture / maxposture);
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
        check_posture();
    }

    void go_non_vulnerable()
    {
        StartCoroutine(recovery());
    }

    void Respawn()
    {
        GameObject clone1 = (GameObject)Instantiate(enemyref);
        clone1.transform.position = new Vector3(-0.03f, 1.785f, -1.04f);
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
        anime.speed = attackm;
        shp = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<player>().hp;
        int r = UnityEngine.Random.Range(1,7);
        switch (r){
            case 1:
                attackt = attacktimes[1] * (attackm*2-attackm);
                ChangeAnimationState(right_hook);
                StartCoroutine(attack_len(attackt));
                overhead_1.SetActive(true);
                overhead_1.GetComponent<damage_zone>().attack(attackt);
                break;
            case 2:
                attackt = attacktimes[1] * (attackm * 2 - attackm);
                ChangeAnimationState(elbow_right);
                StartCoroutine(attack_len(attackt));
                if (UnityEngine.Random.Range(0, 2) == 1)
                {
                    right_elbow1.SetActive(true);
                    right_elbow1.GetComponent<damage_zone>().attack(attackt);
                }
                else
                {
                    right_elbow2.SetActive(true);
                    right_elbow2.GetComponent<damage_zone>().attack(attackt);
                }
                break;
            case 3:
                attackt = attacktimes[0] * (attackm * 2 - attackm);
                ChangeAnimationState(elbow_left);
                StartCoroutine(attack_len(attackt));
                if (UnityEngine.Random.Range(0, 2) == 1)
                {
                    left_elbow1.SetActive(true);
                    left_elbow1.GetComponent<damage_zone>().attack(attackt);
                }
                else
                {
                    left_elbow2.SetActive(true);
                    left_elbow2.GetComponent<damage_zone>().attack(attackt);
                }
                break;
            case 4:
                attackt = attacktimes[2] * (attackm*0.6f);
                anime.speed += 0.4f;
                ChangeAnimationState(mid_slam);
                StartCoroutine(attack_len(attackt));
                midslam_1.SetActive(true);
                midslam_1.GetComponent<damage_zone>().attack(attackt);
                break;
            case 5:
                attackt = attacktimes[3] * (attackm*3);
                anime.speed -= 0.4f;
                ChangeAnimationState(left_punch);
                StartCoroutine(attack_len(attackt));
                int rng = UnityEngine.Random.Range(0, 3);
                if (rng == 0)
                {
                    left_plane.GetComponent<plane>().attack(attackt);
                }
                if (rng == 1)
                {
                    left_plane.GetComponent<plane>().attack(attackt);
                    left_plane.GetComponent<plane>().attack(attackt);
                }
                if (rng== 2)
                {
                    left_plane.GetComponent<plane>().attack(attackt);
                    left_plane.GetComponent<plane>().attack(attackt);
                    left_plane.GetComponent<plane>().attack(attackt);
                }
                break;
            case 6:
                attackt = attacktimes[3] * (attackm * 3);
                anime.speed -= 0.4f;
                ChangeAnimationState(right_punch);
                StartCoroutine(attack_len(attackt));
                int rng2 = UnityEngine.Random.Range(0, 3);
                if(rng2 == 0)
                {
                    right_plane.GetComponent<plane>().attack(attackt);
                }
                if (rng2 == 1)
                {
                    right_plane.GetComponent<plane>().attack(attackt);
                    right_plane.GetComponent<plane>().attack(attackt);
                }
                if (rng2 == 2)
                {
                    right_plane.GetComponent<plane>().attack(attackt);
                    right_plane.GetComponent<plane>().attack(attackt);
                    right_plane.GetComponent<plane>().attack(attackt);
                }
                break;
            default:
                break;

        }
    }

    public void check_posture()
    {
        if (shp == GameObject.FindGameObjectWithTag("MainCamera").GetComponent<player>().hp)
        {
            posture += 20f;
            pst.UpdateHealth(posture / maxposture);
            if (posture >= 100)
            {
                ChangeAnimationState(dizzy);
                state = State.vulnerable;
                ass.clip = pbreak;
                ass.pitch = UnityEngine.Random.Range(0.7f, 1.1f);
                ass.Play();
            }
        }
    }
}
