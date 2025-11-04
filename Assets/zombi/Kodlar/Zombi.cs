using UnityEngine;
using System.Collections;

public class Zombi : MonoBehaviour
{
    public float zombiHP = 100;
    Animator zombiAnim;
    bool zombiOlu;
    void Start()
    {
        zombiAnim = this.GetComponent<Animator>();
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
            zombiAnim.SetBool("oldu", true);
            StartCoroutine(YokOl());
        }
        else
        {
            //ileride hareket kodunu buraya yazacaðýz.
        }
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
