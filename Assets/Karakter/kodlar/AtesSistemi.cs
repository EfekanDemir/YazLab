using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AtesSistemi : MonoBehaviour
{
    Camera kamera;
    public LayerMask zombiKatman;
    KarakterKontrol hpcontrol;
    public ParticleSystem muzzleFlash;
    Animator anim;

    private float sarjor = 50;
    private float cephane = 300;
    private float sarjorKapasitesi = 50;

    [Header("Ateþ Etme Ayarlarý")]
    public float atesEtmeAraligi = 0.1f;
    private float sonrakiAtesZamani = 0f;

    void Start()
    {
        kamera = Camera.main;
        hpcontrol = this.gameObject.GetComponent<KarakterKontrol>();
        anim = this.gameObject.GetComponent<Animator>();
    }

    void Update()
    {
        if (hpcontrol.isAliveControl() == true)
        {
            if (Input.GetMouseButton(0))
            {
                if (sarjor > 0 && Time.time >= sonrakiAtesZamani)
                {
                    sonrakiAtesZamani = Time.time + atesEtmeAraligi;

                    anim.SetBool("AtesEt", true);
                    AtesEtme();
                }
                else if (sarjor <= 0 && cephane > 0)
                {
                    anim.SetBool("AtesEt", false);
                    anim.SetBool("sarjorDegistirme", true);
                }
                else if (sarjor <= 0)
                {
                    anim.SetBool("AtesEt", false);
                }
            }
            else if (Input.GetMouseButtonUp(0))
            {
                anim.SetBool("AtesEt", false);
            }
        }
    }
    public void SarjorDegistirme()
    {
        cephane -= sarjorKapasitesi - sarjor;
        sarjor = sarjorKapasitesi;
        anim.SetBool("sarjorDegistirme", false);
    }
    public void AtesEtme()
    {
        sarjor--;
        muzzleFlash.Emit(10);
        Ray ray = kamera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, Mathf.Infinity, zombiKatman))
        {
            hit.collider.gameObject.GetComponent<Zombi>().HasarAl();
        }
    }

    public float GetSarjor()
    {
        return sarjor;
    }
    public float GetCephane()
    {
        return cephane;
    }
}