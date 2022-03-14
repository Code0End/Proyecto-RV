using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hitdata 
{
    public int damage;
    public Vector3 hitpoint;
    public Vector3 hitnormal;
    public Ihurtbox hurtbox;
    public IHitDetector hitdetector;

    public bool Validate()
    {
        if (hurtbox != null)
        {
            if (hurtbox.CheckHit(this))
            {
                if (hurtbox.HurtResponder == null || hurtbox.HurtResponder.CheckHit(this))
                {
                    if (hitdetector.HitResponder == null || hitdetector.HitResponder.CheckHit(this))
                    {
                        return true;
                    }
                }
            }

        }
        return false;
    }


    public interface IHitResponder
    {

        int damage { get; }

        bool CheckHit(Hitdata data);
        void Response(Hitdata data);

    }
    
    public interface IHitDetector
    {

        IHitResponder HitResponder { get; set; }
        void CheckHit();

    }

    public interface Ihurtresponder
    {

        bool CheckHit(Hitdata data);
        void Response(Hitdata data);


    }

    public interface Ihurtbox
    {

        bool Active { get; }
        GameObject Owner { get; }
        Transform Transform { get; }
        Ihurtresponder HurtResponder { get; set; }
        bool CheckHit(Hitdata hitdata);

    }
}
