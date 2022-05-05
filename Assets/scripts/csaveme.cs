using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class csaveme : MonoBehaviour
{
    public string objectID;
    void Awake()
    {
        objectID = name + transform.position.ToString();
        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        DontDestroyOnLoad(gameObject);
    }

    /*
    for (int i = 0; i < FindObjectsOfType<csaveme>().Length; i++)
    {
        if (FindObjectsOfType<csaveme>()[i] != this)
        {
            if (FindObjectsOfType<csaveme>()[i].name == gameObject.name)
            {
                Destroy(gameObject);
            }
        }
    }*/


}
