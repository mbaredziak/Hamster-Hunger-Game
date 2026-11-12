using UnityEngine;

public class LauncherManager : MonoBehaviour
{
    [Header("Panels / Pages")]
    public GameObject[] pages; 

    
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

   
    public void LaunchGame()
    {
        Debug.Log("Uruchamianie gry...");
        
    }
}
