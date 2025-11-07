using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class ArayuzKontrol : MonoBehaviour
{
    public Text mermiText;
    public Text saglikText;

    GameObject oyuncu;
    void Start()
    {
        oyuncu = GameObject.FindWithTag("SWAT");
    }

    
    void Update()
    {
        mermiText.text = "Mermi: " + oyuncu.GetComponent<AtesSistemi>().GetSarjor().ToString()+"/"+ oyuncu.GetComponent<AtesSistemi>().GetCephane().ToString();
        saglikText.text = "HP= " + oyuncu.GetComponent<KarakterKontrol>().GetSaglik();
    }
}
