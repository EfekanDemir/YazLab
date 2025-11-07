using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class KarakterKontrol : MonoBehaviour
{
    public float karakterHiz;
    public float hp = 100;
    bool isAlive;
    Animator anim;
    public float zýplamaGucu = 5f;
    Rigidbody rb;

    private Camera mainKamera;
    public float normalFOV = 60f;
    public float zoomFOV = 30f;

    void Start()
    {
        anim = this.GetComponent<Animator>();
        rb = this.GetComponent<Rigidbody>();
        isAlive = true;

        mainKamera = Camera.main;

        if (mainKamera != null)
        {
            mainKamera.fieldOfView = normalFOV;
        }
    }

    void Update()
    {
        if (hp <= 0)
        {
            isAlive = false;
            anim.SetBool("isAlive", isAlive);
        }

        if (isAlive == true)
        {
            Hareket();

            if (Input.GetButtonDown("Jump"))
            {
                anim.SetTrigger("isJumping");
                rb.AddForce(Vector3.up * zýplamaGucu, ForceMode.Impulse);
            }

            if (Input.GetKeyDown(KeyCode.C))
            {
                bool suAnComeliyorMu = anim.GetBool("isCrouching");
                anim.SetBool("isCrouching", !suAnComeliyorMu);
            }

            if (Input.GetMouseButton(1))
            {
                anim.SetBool("isAiming", true);

                mainKamera.fieldOfView = zoomFOV;
            }
            else
            {
                anim.SetBool("isAiming", false);

                mainKamera.fieldOfView = normalFOV;
            }
        }
    }


    public float GetSaglik()
    {
        return hp;
    }

    public bool isAliveControl()
    {
        return isAlive;
    }

    public void HasarAl()
    {
        hp -= Random.Range(5, 10);
    }

    void Hareket()
    {
        float yatay = Input.GetAxis("Horizontal");
        float dikey = Input.GetAxis("Vertical");
        anim.SetFloat("Horizontal", yatay);
        anim.SetFloat("Vertical", dikey);
        this.gameObject.transform.Translate(yatay * karakterHiz * Time.deltaTime, 0, dikey * karakterHiz * Time.deltaTime);
    }
}