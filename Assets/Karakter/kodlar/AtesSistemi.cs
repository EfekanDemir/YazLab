using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class AtesSistemi : MonoBehaviour
{
    Camera kamera;
    public LayerMask zombiKatman;
    KarakterKontrol hpcontrol;
    Animator anim;
    void Start()
    {
        kamera = Camera.main;
        hpcontrol = this.gameObject.GetComponent<KarakterKontrol>();
        anim = this.gameObject.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (hpcontrol.isAliveControl() == true)
        {
            if (Input.GetMouseButton(0))
            {
                anim.SetBool("AtesEt", true);
                AtesEtme();
            }
            else if (Input.GetMouseButtonUp(0))
            {
                anim.SetBool("AtesEt", false);
            }
        }
    }
    public void AtesEtme()
    {
        
        Ray ray = kamera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, Mathf.Infinity, zombiKatman))
        {
            hit.collider.gameObject.GetComponent<Zombi>().HasarAl();
        }
        
    }
}