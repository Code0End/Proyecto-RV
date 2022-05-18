using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class plane : MonoBehaviour
{   
    public float damage = 0;
    public float fadedur = 0.8f;

    int a = 3;
    public BoxCollider col;

    float randomX;
    float randomY;
    float randomZ;

    Transform self;

    public GameObject ptil;

    public void Start()
    {
        //randomX = Random.Range(gameObject.transform.position.x - gameObject.transform.localScale.x / 2, gameObject.transform.position.x + gameObject.transform.localScale.x / 2);
        //randomY = Random.Range(gameObject.transform.position.y - gameObject.transform.localScale.y / 2, gameObject.transform.position.y + gameObject.transform.localScale.y / 2);
        //randomZ = Random.Range(gameObject.transform.position.y - gameObject.transform.localScale.z / 2, gameObject.transform.position.y + gameObject.transform.localScale.z / 2);
    }

    public IEnumerator faderoutine(float ai, float ao)
    {
        float timer = 0;
        while (timer < fadedur)
        {
            timer += Time.deltaTime;
            yield return null;
        }
        objectSpawn();
        if (a == 1)
        {
            a = 2;
            gameObject.SetActive(false);
        }
        if (a == 0)
        {
            a = 1;
            col.enabled = true;
            StartCoroutine(droutine());
            fadein();
        }
        if (a == 2)
            a = 0;
    }

    public IEnumerator droutine()
    {
        float timer = 0;
        while (timer < 0.3)
        {
            timer += Time.deltaTime;
            yield return null;
        }
        col.enabled = false;
    }

    public void fade(float ai, float ao)
    {
        StartCoroutine(faderoutine(ai, ao));
    }

    public void fadein()
    {
        fade(1, 0);
    }

    public void fadeout()
    {
        fade(0, 1);
    }

    public void attack(float timing)
    {
        fadedur = timing / 2;

        fadeout();
    }

    void objectSpawn()
    {

        Vector3 randomPosition = GetARandomPos(gameObject);

        Transform instance = Instantiate(ptil, this.transform).transform;
        instance.localPosition = randomPosition;
    }

    public Vector3 GetARandomPos(GameObject plane)
    {

        Mesh planeMesh = plane.GetComponent<MeshFilter>().mesh;
        float minx = planeMesh.bounds.size.x;
        float miny = planeMesh.bounds.size.z;
        Vector3 newVec = Vector3.zero;
        newVec.x = Random.Range(-minx / 2f, minx / 2f);
        newVec.y = plane.transform.position.y;
        newVec.z = Random.Range(-miny / 2f, miny / 2f);
        //Bounds bounds = planeMesh.bounds;
        //float minX = plane.transform.position.x - plane.transform.localScale.x * bounds.size.x * 0.5f;
        //float minZ = plane.transform.position.z - plane.transform.localScale.z * bounds.size.z * 0.5f;
        //Vector3 newVec = new Vector3(Random.Range(minX, -minX), plane.transform.position.y, Random.Range(minZ, -minZ));
        return newVec;
    }

}
