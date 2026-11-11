using UnityEngine;

public class LauncherManager : MonoBehaviour
{
    [Header("Panels / Pages")]
    public GameObject[] pages; // Przeciągnij tutaj Page_Home, Page_News, Page_Settings w inspektorze

    // Wywoływane z przycisków nawigacyjnych (np. kliknięcie "Aktualności" włącza indeks 1)
    public void SwitchPage(int pageIndex)
    {
        for (int i = 0; i < pages.Length; i++)
        {
            if (pages[i] != null)
            {
                pages[i].SetActive(i == pageIndex);
            }
        }
    }

    // Metoda wywoływana przez przycisk "GRAJ"
    public void LaunchGame()
    {
        Debug.Log("Uruchamianie gry...");
        // Tutaj docelowo wrzucisz: SceneManager.LoadScene("MainGameScene");
        // Lub odpalanie zewnętrznego pliku .exe: Process.Start("GameFolder/Game.exe");
    }
}