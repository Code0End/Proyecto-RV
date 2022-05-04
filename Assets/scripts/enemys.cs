using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class enemys : MonoBehaviour
{

    private UnityEngine.Object enemyref;

    public float hp;
    public float maxhp;
    public float posture,maxposture;
    public bool v = true;
    public Animator anim;
    public Rigidbody RIGID_BODY;
    Collider[] colliders;
    Rigidbody[] limbsrbs;

    public Rigidbody gc1;
    public SkinnedMeshRenderer skin;
    private Material[] dissm;
    hb hb;
    float dissolverate = 0.02f;
    float rr=0.04f; 

    Canvas cv;

    void Start()
    {
        enemyref = Resources.Load("monk1");
        hp = 100.0f;
        maxhp = 100.0f;
        posture = 0.0f;
        maxposture = 100.0f;
        colliders = this.gameObject.GetComponentsInChildren<Collider>();
        limbsrbs = this.gameObject.GetComponentsInChildren<Rigidbody>();
        hb = this.gameObject.GetComponent<hb>();
        SetRagdollParts();
        cv = this.gameObject.GetComponentInChildren<Canvas>();
        if(skin != null)
        {
            dissm = skin.materials;
        }
        StartCoroutine(Essolve());
    }



    public void taked(float d)
    {
        hp = hp - d;
        GotHit();
        hb.UpdateHealth(hp / maxhp);
        if (hp <= 0)
        {
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

    void Respawn()
    {
        GameObject clone1 = (GameObject)Instantiate(enemyref);
        clone1.transform.position = transform.position;
    }
}
