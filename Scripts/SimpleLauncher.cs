using UnityEngine;

public class SimpleLauncher : MonoBehaviour
{
    // Ta funkcja wykona się po kliknięciu przycisku GRAJ
    public void OnPlayButtonClicked()
    {
        Debug.Log("Kliknięto GRAJ! Gra wystartowała!");
        
        // Tutaj w przyszłości dopiszemy kod włączający właściwą scenę gry:
        // UnityEngine.SceneManagement.SceneManager.LoadScene("NazwaScenyZGrą");
    }
}