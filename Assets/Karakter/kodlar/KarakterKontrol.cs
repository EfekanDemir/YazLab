using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class KarakterKontrol : MonoBehaviour
{
    public float karakterHiz;
    public float hp = 100;
    bool isAlive;
    Animator anim;
    void Start()
    {
        anim = this.GetComponent<Animator>();
        isAlive = true;
    }

    void Update()
    {
        if (hp <= 0)
        {
            isAlive = false;
            anim.SetBool("isAlive", isAlive);
        }
        if(isAlive == true) { Hareket(); }
        
    }
    public bool isAliveControl()
    {
            return isAlive;
    }
    public void HasarAl()
    {
        hp -= Random.Range(5,10);
    }
    void Hareket()
    {
        float yatay = Input.GetAxis("Horizontal");
        float dikey = Input.GetAxis("Vertical");
        anim.SetFloat("Horizontal", yatay);
        anim.SetFloat("Vertical", dikey);
        this.gameObject.transform.Translate(yatay * karakterHiz*Time.deltaTime, 0, dikey * karakterHiz* Time.deltaTime);
    }
}
