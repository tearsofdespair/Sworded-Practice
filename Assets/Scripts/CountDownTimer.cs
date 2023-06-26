using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CountDownTimer : MonoBehaviour
{
    public float timeRemaining = 120;
    public bool timerIsRunning = false;
    public Text timeText;
    public Animation RedText;
    int tik=10;
    public AudioSource audio;
    GameUI gameUI;
    private void Start()
    {
        // Starts the timer automatically
        gameUI = GameUI.instance;
        timerIsRunning = true;
    }

    void Update()
    {
        if (timerIsRunning)
        {
            if (timeRemaining > 0)
            {
                timeRemaining -= Time.deltaTime;
                DisplayTime(timeRemaining);
                if (timeRemaining < 10)
                {
                    RedText.enabled = true;
                    transform.localScale = Vector3.Lerp(transform.localScale, Vector3.one * 1.2f, Time.deltaTime);
                    if (timeRemaining< tik)
                    {
                        tik--;
                        audio.Play();
                    }
                }
            }
            else
            {
                Debug.Log("Time has run out!");
                timeRemaining = 0;
                timerIsRunning = false;
                gameUI.EndGame();
                Destroy(this);
            }
        }
    }

    void DisplayTime(float timeToDisplay)
    {
        timeToDisplay += 1;

        float minutes = Mathf.FloorToInt(timeToDisplay / 60);
        float seconds = Mathf.FloorToInt(timeToDisplay % 60);

        timeText.text = string.Format("{0}:{1:00}", minutes, seconds);
    }
}
