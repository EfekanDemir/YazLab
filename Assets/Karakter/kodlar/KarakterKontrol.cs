using UnityEngine;

public class KarakterKontrol : MonoBehaviour
{
    public float karakterHiz;
    Animator anim;
    void Start()
    {
        anim = this.GetComponent<Animator>();
    }

    void Update()
    {
        Hareket();
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
