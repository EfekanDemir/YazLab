using UnityEngine.SceneManagement;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameObject oyunBittiEkrani;
    public bool oyunBittiMi = false;

    
    void Update()
    {
        // Test için p ye basınca bitir
        if (Input.GetKeyDown(KeyCode.P))
        {
            OyuncuOldu();
        }
    }

    public void OyuncuOldu()
    {
        if (oyunBittiMi == true)
        {
            return;
        }
        
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
}