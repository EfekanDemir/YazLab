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

    // --- YENÝ SES SÝSTEMÝ (ÝKÝ KAYNAKLI) ---
    // BU ALANLARI UNITY INSPECTOR'DAN ATAMAN GEREKÝYOR!
    public AudioSource audioSource_Ambient; // Hýrýltý (Growl) sesini çalan kaynak
    public AudioSource audioSource_Actions; // Adým, Saldýrý, Ölüm seslerini çalan kaynak

    // Bu sesler Animasyon Olaylarý (Animation Events) için
    public AudioClip attackingSound;
    public AudioClip dyingSound;
    // Not: Yürüme ve hýrýltý sesleri için deðiþkene gerek yok,
    // çünkü onlarý doðrudan Inspector'daki AudioSource'lara atamýþsýn (görselde görünüyor).
    // --- BÝTTÝ ---

    public Transform[] patrolPoints;
    private int currentPatrolIndex;

    void Start()
    {
        zombiAnim = this.GetComponent<Animator>();
        hedefOyuncu = GameObject.FindGameObjectWithTag("SWAT");
        zombiNav = this.GetComponent<NavMeshAgent>();

        // --- DEÐÝÞTÝ ---
        // Eski 'audioSource = this.GetComponent<AudioSource>();' satýrýný sildik.
        // Artýk iki kaynaðý da Inspector'dan atadýðýný varsayýyoruz.

        // Görseldeki 'Play On Awake' ayarýna göre:
        // Hýrýltý (Ambient) zaten kendi çalmaya baþlar.
        // Adým sesleri (Actions) de çalmaya baþlar, ama duruma göre durdurmamýz gerekebilir.

        if (patrolPoints != null && patrolPoints.Length > 0)
        {
            currentPatrolIndex = 0;
            zombiNav.SetDestination(patrolPoints[currentPatrolIndex].position);
            zombiAnim.SetBool("isWalking", true);
            // Yürüyerek baþladýðý için adým sesleri çalmalý
            if (audioSource_Actions != null && !audioSource_Actions.isPlaying)
                audioSource_Actions.Play();
        }
        else
        {
            // Devriye yoksa, boþta durur. Adým sesleri susmalý.
            zombiAnim.SetBool("isWalking", false);
            if (audioSource_Actions != null && audioSource_Actions.isPlaying)
                audioSource_Actions.Stop();
        }
    }

    void Update()
    {
        // --- 1. DURUM: ÖLÜM (En Yüksek Öncelik) ---

        // --- GÜNCELLENDÝ: ÝSTEDÝÐÝN ÖLÜM MANTIÐI ---
        // Bu blok, ölümün *sadece ilk karesinde* çalýþýr.
        if (zombiHP <= 0 && zombiOlu == false)
        {
            zombiOlu = true; // Durumu 'ölü' olarak ayarla

            // --- ÝSTEÐÝN: BÜTÜN SESLERÝN SUSMASI ---
            if (audioSource_Ambient != null && audioSource_Ambient.isPlaying)
            {
                audioSource_Ambient.Stop(); // Hýrýltýyý (loop) durdur
            }
            if (audioSource_Actions != null && audioSource_Actions.isPlaying)
            {
                audioSource_Actions.Stop(); // Adým seslerini (loop) durdur
            }
            // -----------------------------------------

            // Ölüm sesi Animasyon Olayý'ndan (zombiOlduSes) çalýnacak,
            // o yüzden burada ÇALMIYORUZ. Sadece animasyonu tetikliyoruz.

            zombiAnim.SetBool("isDead", true);
            zombiAnim.SetBool("isAttacking", false);
            zombiAnim.SetBool("isRunning", false);
            zombiAnim.SetBool("isWalking", false);
            zombiNav.isStopped = true;
            StartCoroutine(YokOl());
        }

        // Zombi öldüyse, Update'in geri kalanýný çalýþtýrma.
        if (zombiOlu == true)
        {
            return;
        }
        // --- ÖLÜM MANTIÐI BÝTTÝ ---


        // --- Zombi hayattaysa ---
        mesafe = Vector3.Distance(hedefOyuncu.transform.position, this.transform.position);

        // --- BÖLÜM 1: DURUM (STATE) BELÝRLEME (Önceki cevaptaki gibi) ---
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
            // Oyuncu menzil dýþýnda
            zombiAnim.SetBool("isRunning", false);
            zombiAnim.SetBool("isAttacking", false);

            if (patrolPoints != null && patrolPoints.Length > 0)
            {
                // DURUM: DEVRÝYE (Walking)
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
                // DURUM: IDLE (Boþta Durma)
                zombiNav.isStopped = true;
                zombiAnim.SetBool("isWalking", false);
            }
        }

        // --- BÖLÜM 2: SES (AUDIO) YÖNETÝMÝ (Önceki cevaptaki gibi) ---
        // 'audioSource_Actions' (Adým Sesi) döngüsünü yönetir.
        if (audioSource_Actions != null)
        {
            // Zombi YÜRÜYOR veya KOÞUYORSA
            if (zombiAnim.GetBool("isRunning") || zombiAnim.GetBool("isWalking"))
            {
                // Adým sesi çalmýyorsa, baþlat
                if (!audioSource_Actions.isPlaying)
                {
                    audioSource_Actions.Play();
                }
            }
            // Zombi SALDIRIYOR veya DURUYORSA (hareket etmiyorsa)
            else
            {
                // Adým sesi çalýyorsa, durdur
                if (audioSource_Actions.isPlaying)
                {
                    audioSource_Actions.Stop();
                }
            }
        }
    }

    // --- GÜNCELLENDÝ: ÖLÜM SESÝ OLAYI ---
    public void zombiOlduSes()
    {
 
            audioSource_Actions.PlayOneShot(dyingSound);
        
    }
    // --- BÝTTÝ ---

    // --- GÜNCELLENDÝ: SALDIRI SESÝ OLAYI ---
    public void hasarVerSes()
    {
        if (!audioSource_Actions.isPlaying)
        {
            audioSource_Actions.Play();
        }
        audioSource_Actions.PlayOneShot(attackingSound);

    }
    // --- BÝTTÝ ---

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