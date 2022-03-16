using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemys : MonoBehaviour
{

    public float hp;
    public bool v = true;
    public Animator anim;
    public Rigidbody RIGID_BODY;

    public List<Collider> RagdollParts = new List<Collider>();

    // Start is called before the first frame update
    void Start()
    {
        SetRagdollParts();
        SetColliderSpheres();
        hp = 100.0f;
    }

    private void SetColliderSpheres()
    {
        
    }



    // Update is called once per frame
    void Update()
    {

    }

    public void taked(float d)
    {
        hp = hp - d;
        GotHit();
        if (hp <= 0)
        {
            Debug.Log("DEATH");
            die();
        }
    }

    //private void FixedUpdate()
    //{
      //  GotHit();
    //}

    private void GotHit()
    {
        this.anim.SetTrigger("hit");
    }

    private void die()
    {
        TurnOnRagdoll();
    }

    private void TurnOnRagdoll()
    {
        RIGID_BODY.useGravity = false;
        this.gameObject.GetComponent<CapsuleCollider>().enabled = false;
        anim.enabled = false;
        anim.avatar = null;
        foreach (Collider c in RagdollParts)
        {
            c.isTrigger = false;
        }
    }

    private void SetRagdollParts()
    {
        Collider[] colliders = this.gameObject.GetComponentsInChildren<Collider>();

        foreach(Collider c in colliders)
        {
            if(c.gameObject != this.gameObject)
            {
                c.isTrigger = true;
                RagdollParts.Add(c);
            }
        }
    }
}
