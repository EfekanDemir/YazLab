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
    public NavMeshAgent zombiNav;
    public GameObject hedefOyuncu;

    public AudioSource audioSource_Ambient; 
    public AudioSource audioSource_Actions; 

    public AudioClip attackingSound;
    public AudioClip dyingSound;
 

    public Transform[] patrolPoints;
    private int currentPatrolIndex;

    void Start()
    {
        zombiAnim = this.GetComponent<Animator>();
        hedefOyuncu = GameObject.FindGameObjectWithTag("SWAT");
        zombiNav = this.GetComponent<NavMeshAgent>();

        if (patrolPoints != null && patrolPoints.Length > 0)
        {
            currentPatrolIndex = 0;
            zombiNav.SetDestination(patrolPoints[currentPatrolIndex].position);
            zombiAnim.SetBool("isWalking", true);
            if (audioSource_Actions != null && !audioSource_Actions.isPlaying)
                audioSource_Actions.Play();
        }
        else
        {
            zombiAnim.SetBool("isWalking", false);
            if (audioSource_Actions != null && audioSource_Actions.isPlaying)
                audioSource_Actions.Stop();
        }
    }

    void Update()
    {
        

        
        if (zombiHP <= 0 && zombiOlu == false)
        {
            zombiOlu = true; 

            if (audioSource_Ambient != null && audioSource_Ambient.isPlaying)
            {
                audioSource_Ambient.Stop(); 
            }
            if (audioSource_Actions != null && audioSource_Actions.isPlaying)
            {
                audioSource_Actions.Stop(); 
            }


            zombiAnim.SetBool("isDead", true);
            zombiAnim.SetBool("isAttacking", false);
            zombiAnim.SetBool("isRunning", false);
            zombiAnim.SetBool("isWalking", false);
            zombiNav.isStopped = true;
            StartCoroutine(YokOl());
        }

        if (zombiOlu == true)
        {
            return;
        }


        mesafe = Vector3.Distance(hedefOyuncu.transform.position, this.transform.position);

        if (mesafe <= kovalamaMesafesi)
        {
            if (mesafe < saldirmaMesafesi)
            {
                // DURUM: SALDIRI
                this.transform.LookAt(hedefOyuncu.transform.position);
                zombiNav.isStopped = true;
                zombiAnim.SetBool("isAttacking", true);
                zombiAnim.SetBool("isRunning", false);
                zombiAnim.SetBool("isWalking", false);
            }
            else
            {
                // DURUM: KOVALAMA (Running)
                zombiNav.isStopped = false;
                zombiNav.SetDestination(hedefOyuncu.transform.position);
                this.transform.LookAt(hedefOyuncu.transform.position);
                zombiAnim.SetBool("isRunning", true);
                zombiAnim.SetBool("isAttacking", false);
                zombiAnim.SetBool("isWalking", false);
            }
        }
        else
        {
            zombiAnim.SetBool("isRunning", false);
            zombiAnim.SetBool("isAttacking", false);

            if (patrolPoints != null && patrolPoints.Length > 0)
            {
                zombiNav.isStopped = false;
                zombiAnim.SetBool("isWalking", true);

                if (!zombiNav.pathPending && zombiNav.remainingDistance <= zombiNav.stoppingDistance)
                {
                    currentPatrolIndex = (currentPatrolIndex + 1) % patrolPoints.Length;
                    zombiNav.SetDestination(patrolPoints[currentPatrolIndex].position);
                }
            }
            else
            {
                zombiNav.isStopped = true;
                zombiAnim.SetBool("isWalking", false);
            }
        }

        if (audioSource_Actions != null)
        {
            if (zombiAnim.GetBool("isRunning") || zombiAnim.GetBool("isWalking"))
            {
                if (!audioSource_Actions.isPlaying)
                {
                    audioSource_Actions.Play();
                }
            }
            else
            {
                if (audioSource_Actions.isPlaying)
                {
                    audioSource_Actions.Stop();
                }
            }
        }
    }

    public void zombiOlduSes()
    {
 
            audioSource_Actions.PlayOneShot(dyingSound);
        
    }
    

    public void hasarVerSes()
    {
        if (!audioSource_Actions.isPlaying)
        {
            audioSource_Actions.Play();
        }
        audioSource_Actions.PlayOneShot(attackingSound);

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
        zombiHP -= Random.Range(15, 25);
    }
}