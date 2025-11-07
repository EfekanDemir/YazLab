using UnityEngine.SceneManagement;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameObject oyunBittiEkrani;
    public GameObject pausePaneli;

    public bool oyunBittiMi = false;
    public bool oyunDurduMu = false;

    
    void Update()
    {
        //oyun bittiyse pause çalışmasın
        if (oyunBittiMi == true)
        {
            return;
        }

        // P ile pause
        if (Input.GetKeyDown(KeyCode.P))
        {
            if (oyunDurduMu == false)
            {
                OyunuDurdur(); 
            }
            else
            {
                OyunaDevamEt();
            }
        }
        if (Input.GetKeyDown(KeyCode.O))
        {
            OyuncuOldu(); 
        }
    }

    public void OyuncuOldu()
    {
        if (oyunBittiMi == true) return;
        
        oyunBittiMi = true;
        Time.timeScale = 0f;
        
        oyunBittiEkrani.SetActive(true); 
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
    public void YenidenBasla()
    {
        Time.timeScale = 1f; 
        SceneManager.LoadScene("AnaOyun");
    }
    public void OyunuDurdur()
    {
        oyunDurduMu = true;
        Time.timeScale = 0f;
        pausePaneli.SetActive(true);
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
    public void OyunaDevamEt()
    {
        oyunDurduMu = false;
        Time.timeScale = 1f;
        pausePaneli.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    // quit için
    public void OyundanCik()
    {
        Application.Quit();
    }
}