using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;
using UnityEngine.XR.Interaction.Toolkit;

[RequireComponent(typeof(Animator))]

public class hand : MonoBehaviour
{
    public XRBaseController currc;
    public float defampl = 0.2f;
    public float defdur = 0.08f;

    Animator animator;
    private float gripTarget;
    private float triggerTarget;
    private float gripCurrent;
    private float triggerCurrent;
    public float speed;
    public GameObject fistpos;
    Vector3 start;
    Vector3 end;
    bool punchstarted = true;
    AudioSource ass;

    float dist;
    public float damage;
    public Collider p;

    LineRenderer lr;
    Transform[] points;
    public TrailRenderer Tr;
    VisualEffect ve;
    public GameObject e1;

    void Start()
    {
        animator = GetComponent<Animator>();
        lr = GetComponent<LineRenderer>();
        ve = this.gameObject.GetComponentInChildren<VisualEffect>();
        ass = GetComponent<AudioSource>();
        lr.enabled = false;
        Tr.enabled = true;
    }

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
                dist = Vector3.Distance(start, end);
                damage = (dist * 100) / 2;
                Tr.emitting = false;
                ve.Play();
                collision.gameObject.GetComponent<enemys>().taked(damage);
                sendhap();
                sends();
            }
            if (collision.gameObject.tag == "eb2")
            {
                punchstarted = false;
                end = fistpos.transform.position;
                lr.SetPosition(0, start);
                lr.SetPosition(1, end);
                dist = Vector3.Distance(start, end);
                damage = (dist * 100) / 2;
                Tr.emitting = false;
                ve.Play();
                collision.gameObject.GetComponent<boxs>().taked(damage);
                sendhap();
            }
        }
    }

    public void sendhap()
    {
        currc.SendHapticImpulse(defampl, defdur);
    }

    void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.tag == "damage_proyectile")
        {

            Destroy(collider.gameObject);
            ve.Play();
            ass.pitch = UnityEngine.Random.Range(0.3f, 0.5f);
            ass.volume = ass.volume / 3;
            ass.Play();
            ass.volume = ass.volume * 3;
        }
    }

    public void sends()
    {
        ass.pitch = UnityEngine.Random.Range(0.5f, 0.8f);
        ass.Play();
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
                Tr.emitting = true;
                if (punchstarted == false)
                {
                    lr.enabled = false;
                    start = fistpos.transform.position;
                    punchstarted = true;             
                }
            }
            else
            {
                Tr.emitting = false; 
            }
        }
        
    }

}
