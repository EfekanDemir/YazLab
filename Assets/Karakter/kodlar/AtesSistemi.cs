using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class AtesSistemi : MonoBehaviour
{
    Camera kamera;
    public LayerMask zombiKatman;
    void Start()
    {
        kamera = Camera.main;

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButton(0))
        {
            AtesEtme();
        }
    }
    void AtesEtme()
    {
        Ray ray = kamera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, Mathf.Infinity, zombiKatman))
        {
            hit.collider.gameObject.GetComponent<Zombi>().HasarAl();
        }

    }
}