using UnityEngine;
    using UnityEngine.UI;

public class Pause : MonoBehaviour
{
    public GameObject pauseMenu;
    public Button resumeButton;
    
    void Awake()
    {
        pauseMenu.SetActive(false);
        resumeButton.onClick.AddListener(OnResumePressed);

        Time.timeScale = 1;
    }

   void OnResumePressed()
    {
        pauseMenu.SetActive(false);
        Time.timeScale = 1;
        
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            
            pauseMenu.SetActive(true);
            Time.timeScale = 0;
        }
    }
}



