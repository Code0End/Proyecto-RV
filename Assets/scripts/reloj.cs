using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class reloj : MonoBehaviour
{
    public GameObject texto;
    public float segundos = 245;
    public GameObject denemies;
    public int numenemies = 0;
    GameObject P1;

    public void Start()
    {
        P1 = GameObject.FindGameObjectWithTag("MainCamera");
        if (P1.GetComponent<player>().mn == 0)
        {
            gameObject.SetActive(false);
        }
        else
        {
            P1.GetComponent<player>().mn = 0;
        }
    }

    public void updatenumenemies()
    {
        numenemies++;
        denemies.GetComponent<TMP_Text>().text = ""+numenemies;
    }

    void Update()
    {
        if (segundos > 0)
        {
            segundos -= Time.deltaTime;
            float minutos = Mathf.FloorToInt(segundos / 60);
            float displaysegundos = Mathf.FloorToInt(segundos % 60);
            texto.GetComponent<TMP_Text>().text = minutos + ":" + displaysegundos;
        }
        if (segundos <= 0)
        {
            
            texto.SetActive(false);
            P1.GetComponent<player>().mn = 1;
            P1.GetComponent<player>().death_action();
        }
    }
}
