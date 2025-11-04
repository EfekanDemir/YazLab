using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using static UnityEngine.Random;
using UnityEngine.AI;

public class Zombi : MonoBehaviour
{
    public float zombiHP = 100;
    Animator zombiAnim;
    bool zombiOlu;
    public float kovalamaMesafesi;
    public float saldirmaMesafesi;
    float mesafe;
    NavMeshAgent zombiNav;

    GameObject hedefOyuncu;

    void Start()
    {
        zombiAnim = this.GetComponent<Animator>();
        hedefOyuncu = GameObject.FindGameObjectWithTag("SWAT");
        zombiNav = this.GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {
       
        if (zombiHP <= 0)
        {
            zombiOlu = true;
        }
        if (zombiOlu == true)
        {
            zombiAnim.SetBool("isDead", true);
            zombiAnim.SetBool("isAttacking", false);
            zombiAnim.SetBool("isRunning", false);
            zombiNav.isStopped = true;
            StartCoroutine(YokOl());
        }
        else
        {
            mesafe = Vector3.Distance(hedefOyuncu.transform.position, this.transform.position);
            if(mesafe <= kovalamaMesafesi)
            {
                zombiNav.isStopped = false;
                zombiNav.SetDestination(hedefOyuncu.transform.position);
                zombiAnim.SetBool("isRunning", true);
                this.transform.LookAt(hedefOyuncu.transform.position);
            }
            else
            {
                zombiNav.isStopped = true;
                zombiAnim.SetBool("isRunning", false);
                zombiAnim.SetBool("isAttacking", false);
            }
            if (mesafe <= saldirmaMesafesi)
            {
                zombiNav.isStopped = true;
                zombiAnim.SetBool("isAttacking", true);
                zombiAnim.SetBool("isRunning", false);
                this.transform.LookAt(hedefOyuncu.transform.position);
            }
            else
            {
                
            }
        }
    }
    public void hasarVer()
    {
        hedefOyuncu.GetComponent<KarakterKontrol>().HasarAl();
    }
    IEnumerator YokOl()
    {
        yield return new WaitForSeconds(5);
        Destroy(this.gameObject);
    }
    public void HasarAl()
    {
        zombiHP -= Random.Range(15,25);
       
    }
}
