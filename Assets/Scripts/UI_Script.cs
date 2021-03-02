using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UI_Script : MonoBehaviour
{
    public Slider slider;
    public GameObject pauseMenu;
    public Button restartB;
    public Button resumeB;
    private bool pause = false;
    private bool restart = false;
    private Vector2 StartPause;
    private Vector2 EndPause;
    private Vector2 StartResume;
    private Vector2 EndResume;
    private Vector2 StartMenu;
    private Vector2 EndMenu;
    

    void Start()
    {
        Vector2 PausePos = GameObject.Find("PauseButton").transform.position;
        float PauseHeight = GameObject.Find("PauseButton").GetComponent<RectTransform>().rect.height;
        float PauseWidth = GameObject.Find("PauseButton").GetComponent<RectTransform>().rect.width;
        StartPause = new Vector2(PausePos.x - (PauseWidth / 2), PausePos.y - (PauseHeight / 2));
        EndPause = new Vector2(PausePos.x + (PauseWidth / 2), PausePos.y + (PauseHeight / 2));
    }

    void Update()
    {
        if (!pause && Input.touchCount > 0 && between(StartPause, EndPause, Input.GetTouch(0).position))
            PuaseGame();
        else if (pause && Input.touchCount > 0 && between(StartResume, EndResume, Input.GetTouch(0).position))
            ResumeGame();
        else if (pause && Input.touchCount > 0 && between(StartMenu, EndMenu, Input.GetTouch(0).position))
            GoMenu();
        else if (restart && Input.touchCount > 0 && between(StartResume, EndResume, Input.GetTouch(0).position))
            RestartGame();
        else if (restart && Input.touchCount > 0 && between(StartMenu, EndMenu, Input.GetTouch(0).position))
            GoMenu();
    }

    IEnumerator delay()
    {
        yield return new WaitForSeconds(0.2f);
        //Time.timeScale = 1;
    }

    public void SetMaxFuel(int fuel)
    {
        slider.maxValue = fuel;
        slider.value = fuel;
    }



    public void SetFuel(int fuel)
    {
        slider.value = fuel;
    }

    public void PuaseGame()
    {
        Time.timeScale = 0;
        pauseMenu.SetActive(true);
        // animation
        Vector2 ResumePos = GameObject.Find("ResumeButton").transform.position;
        float ResumeHeight = GameObject.Find("ResumeButton").GetComponent<RectTransform>().rect.height;
        float ResumeWidth = GameObject.Find("ResumeButton").GetComponent<RectTransform>().rect.width;
        StartResume = new Vector2(ResumePos.x - (ResumeWidth / 2), ResumePos.y - (ResumeHeight / 2));
        EndResume = new Vector2(ResumePos.x + (ResumeWidth / 2), ResumePos.y + (ResumeHeight / 2));

        Vector2 ManuPos = GameObject.Find("Menu").transform.position;
        float ManuHeight = GameObject.Find("Menu").GetComponent<RectTransform>().rect.height;
        float ManuWidth = GameObject.Find("Menu").GetComponent<RectTransform>().rect.width;
        StartMenu = new Vector2(ManuPos.x - (ManuWidth / 2), ManuPos.y - (ManuHeight / 2));
        EndMenu = new Vector2(ManuPos.x + (ManuWidth / 2), ManuPos.y + (ManuHeight / 2));

        pause = true;
    }

    public void ResumeGame()
    {
        Time.timeScale = 1;
        pauseMenu.SetActive(false);
        pause = false;
    }

    public void Restart()
    {
        pauseMenu.SetActive(true);
        restart = true;
        restartB.gameObject.SetActive(true);
        resumeB.gameObject.SetActive(true);
        Vector2 ResumePos = GameObject.Find("ResumeButton").transform.position;
        float ResumeHeight = GameObject.Find("ResumeButton").GetComponent<RectTransform>().rect.height;
        float ResumeWidth = GameObject.Find("ResumeButton").GetComponent<RectTransform>().rect.width;
        StartResume = new Vector2(ResumePos.x - (ResumeWidth / 2), ResumePos.y - (ResumeHeight / 2));
        EndResume = new Vector2(ResumePos.x + (ResumeWidth / 2), ResumePos.y + (ResumeHeight / 2));
        resumeB.gameObject.SetActive(false);

        Vector2 ManuPos = GameObject.Find("Menu").transform.position;
        float ManuHeight = GameObject.Find("Menu").GetComponent<RectTransform>().rect.height;
        float ManuWidth = GameObject.Find("Menu").GetComponent<RectTransform>().rect.width;
        StartMenu = new Vector2(ManuPos.x - (ManuWidth / 2), ManuPos.y - (ManuHeight / 2));
        EndMenu = new Vector2(ManuPos.x + (ManuWidth / 2), ManuPos.y + (ManuHeight / 2));
    }

    public void RestartGame()
    {
        SceneManager.LoadScene("Game");
    }

    public void GoMenu()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene("Opening");
    }

    private bool between(Vector2 Start, Vector2 End, Vector2 pos)
    {
        return ((pos.x <= End.x && pos.x >= Start.x) && (pos.y <= End.y && pos.y >= Start.y));
    }



}
