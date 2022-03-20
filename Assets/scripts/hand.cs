using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

[RequireComponent(typeof(Animator))]

public class hand : MonoBehaviour
{

    Animator animator;
    private float gripTarget;
    private float triggerTarget;
    private float gripCurrent;
    private float triggerCurrent;
    public float speed;
    public GameObject fistpos;
    Vector3 start;
    Vector3 end;
    bool punchstarted = false;

    float dist;
    public float damage;
    public Collider p;

    LineRenderer lr;
    Transform[] points;
    TrailRenderer Tr;
    VisualEffect ve;
    public GameObject e1;



    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        lr = GetComponent<LineRenderer>();
        Tr = GetComponent<TrailRenderer>();
        ve = this.gameObject.GetComponentInChildren<VisualEffect>();
        lr.enabled = false;
        Tr.enabled = false;
        
    }

    // Update is called once per frame
    void Update()
    {

        AnimateHand(); 

    }

    internal void SetGrip(float v)
    {
        gripTarget = v;
    }

    internal void SetTrigger(float v)
    {
        triggerTarget = v;
    }

    void OnCollisionEnter(Collision collision)
    {
        if (punchstarted == true)
        {
            if (collision.gameObject.tag == "eb1")
            {
                punchstarted = false;
                end = fistpos.transform.position;
                lr.SetPosition(0, start);
                lr.SetPosition(1, end);
                //lr.enabled = true;
                dist = Vector3.Distance(start, end);
                damage = (dist * 100) / 2;
                Debug.Log(damage);
                Tr.enabled = false;
                ve.Play();
                //enemys r = collision.gameObject.GetComponent<enemys>();
                collision.gameObject.GetComponent<enemys>().taked(damage); 
              
            }
        }
    }




    void AnimateHand() {

        if (gripCurrent != gripTarget)
        {           
            gripCurrent = Mathf.MoveTowards(gripCurrent, gripTarget, Time.deltaTime * speed);
            animator.SetFloat("grip", gripCurrent);


        }if (triggerCurrent != triggerTarget)
        {        
            triggerCurrent = Mathf.MoveTowards(triggerCurrent, triggerTarget, Time.deltaTime * speed);
            animator.SetFloat("trigger", triggerCurrent);

        }

        if (triggerCurrent != triggerTarget && gripCurrent != gripTarget)
        {
            
            if (gripCurrent < gripTarget && triggerCurrent < triggerTarget)
            {
                Tr.enabled = true;
                if (punchstarted == false)
                {
                    lr.enabled = false;
                    start = fistpos.transform.position;
                    punchstarted = true;
                    

                }


            }
            else
            {
                Tr.enabled = false; 
                //punchstarted = false;
                //end = fistpos.transform.position;
                //lr.SetPosition(0, start);
                //lr.SetPosition(1, end);
                //lr.enabled = true;
                //dist = Vector3.Distance(start, end);
                //damage = (dist * 100)/2;
                //Debug.Log(damage);

            }
        }
        
    }

}
