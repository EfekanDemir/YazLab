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

    // --- YENÝ DEVRÝYE DEÐÝÞKENLERÝ ---
    public Transform[] patrolPoints; // Devriye noktalarýný buraya atayacaðýz
    private int currentPatrolIndex; // Hangi noktaya gittiðimizi tutar

    void Start()
    {
        zombiAnim = this.GetComponent<Animator>();
        hedefOyuncu = GameObject.FindGameObjectWithTag("SWAT");
        zombiNav = this.GetComponent<NavMeshAgent>();

        // Devriyeye ilk noktadan baþla
        if (patrolPoints != null && patrolPoints.Length > 0)
        {
            currentPatrolIndex = 0;
            zombiNav.SetDestination(patrolPoints[currentPatrolIndex].position);
            zombiAnim.SetBool("isWalking", true); // Yürümeye baþla
        }
    }

    // Update'i daha net bir Durum Makinesi (State Machine) gibi yeniden düzenledim
    void Update()
    {
        // --- 1. DURUM: ÖLÜM (En Yüksek Öncelik) ---
        if (zombiHP <= 0)
        {
            zombiOlu = true;
        }

        if (zombiOlu == true)
        {
            zombiAnim.SetBool("isDead", true);
            zombiAnim.SetBool("isAttacking", false);
            zombiAnim.SetBool("isRunning", false);
            zombiAnim.SetBool("isWalking", false); // Yürümeyi de durdur
            zombiNav.isStopped = true;
            StartCoroutine(YokOl());
            return; // Ölüyse baþka bir kod çalýþtýrma
        }

        // --- Zombi hayattaysa ---
        mesafe = Vector3.Distance(hedefOyuncu.transform.position, this.transform.position);

        // --- 2. DURUM: SALDIRI (Yüksek Öncelik) ---
        // Oyuncu saldýrý mesafesindeyse, baþka bir þey yapma, saldýr.
        if (mesafe <= saldirmaMesafesi)
        {
            zombiNav.isStopped = true; // Dur ve saldýr
            this.transform.LookAt(hedefOyuncu.transform.position); // Yüzün oyuncuya dönsün

            zombiAnim.SetBool("isAttacking", true);
            zombiAnim.SetBool("isRunning", false);
            zombiAnim.SetBool("isWalking", false);
        }
        // --- 3. DURUM: KOVALAMA (Orta Öncelik) ---
        // Oyuncu saldýrý mesafesinde deðil AMA kovalama mesafesindeyse, kovala.
        else if (mesafe <= kovalamaMesafesi)
        {
            zombiNav.isStopped = false; // Kovalamak için hareket et
            zombiNav.SetDestination(hedefOyuncu.transform.position);
            this.transform.LookAt(hedefOyuncu.transform.position); // Kovalamada da baksýn

            zombiAnim.SetBool("isRunning", true);
            zombiAnim.SetBool("isAttacking", false);
            zombiAnim.SetBool("isWalking", false);
        }
        // --- 4. DURUM: DEVRÝYE (Düþük Öncelik) ---
        // Oyuncu menzilde deðilse, devriye gez.
        else
        {
            // Kovalamayý ve saldýrmayý býrak
            zombiAnim.SetBool("isRunning", false);
            zombiAnim.SetBool("isAttacking", false);

            // Devriye noktalarý atanmýþ mý kontrol et
            if (patrolPoints != null && patrolPoints.Length > 0)
            {
                zombiNav.isStopped = false;
                zombiAnim.SetBool("isWalking", true); // Yürüme animasyonu

                // NavMeshAgent'ýn hedefe ulaþýp ulaþmadýðýný kontrol et
                // pathPending: Agent'ýn hala bir yol hesaplamaya çalýþmadýðýndan emin ol
                // remainingDistance: Hedefe kalan mesafe
                // stoppingDistance: Hedefe ne kadar yaklaþýnca duracaðý (Inspector'dan ayarlanýr)
                if (!zombiNav.pathPending && zombiNav.remainingDistance <= zombiNav.stoppingDistance)
                {
                    // Hedefe ulaþtýk, bir sonraki devriye noktasýna geç
                    currentPatrolIndex = (currentPatrolIndex + 1) % patrolPoints.Length;
                    zombiNav.SetDestination(patrolPoints[currentPatrolIndex].position);
                }
            }
            else
            {
                // Devriye noktasý yoksa, dur (senin orijinal kodundaki gibi)
                zombiNav.isStopped = true;
                zombiAnim.SetBool("isWalking", false);
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
        zombiHP -= Random.Range(15, 25);
    }
}