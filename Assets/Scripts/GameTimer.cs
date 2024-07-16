using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class GameTimer : MonoBehaviour
{
  static GameTimer instance; //static variables persist through all instances of a class

  private float gameTimer = 0f;
  private bool timerStarted = false;
  [SerializeField] private TextMeshProUGUI timerText;
  [SerializeField] private Image timerBackground;
  [SerializeField] private bool startImmediately = false;
  [SerializeField] private bool hideTimerAtStart = false;
  [SerializeField] private bool isLevelTimer = true;

  void Awake() {
    if (!isLevelTimer) {
      ManageSingleton();
    }
  }

  void ManageSingleton() {
    if (instance != null)
    {
      gameObject.SetActive(false);
      Destroy(gameObject);
    }
    else {
      instance = this;
      DontDestroyOnLoad(gameObject);
    }
  }

    void Start()
    {
      if (startImmediately) {
        StartGameTimer();
      }
      if (hideTimerAtStart) {
        HideTimer();
      }
    }

    int gameTimerMinutes;
    double gameTimerSeconds;
    void Update()
    {
      if (timerStarted) {
        gameTimer += Time.deltaTime;
        //Debug.Log(gameTimer);
      }
      gameTimerMinutes = Convert.ToInt32(Math.Floor(gameTimer / 60f));
      gameTimerSeconds = Convert.ToInt32(Math.Floor(gameTimer - (gameTimerMinutes * 60)));
      if (gameTimerMinutes == 0) {
        timerText.text = gameTimerSeconds.ToString("F0") + "s " + ((gameTimer - Math.Truncate(gameTimer)) * 100).ToString("F0") + "ms";
      }
      else {
        timerText.text = gameTimerMinutes.ToString() + "m " + gameTimerSeconds.ToString("F0") + "s " + ((gameTimer - Math.Truncate(gameTimer)) * 100).ToString("F0") + "ms";
      }
    }

    public void StartGameTimer() {
      //Start Timer
      timerStarted = true;
    }

    public void StartGameTimerWithDelay(float delay) {
      StartCoroutine(DelayStartTimer(delay));
    }

    private IEnumerator DelayStartTimer(float delay) {
      //Debug.Log("DelayStartTimer called");
      yield return new WaitForSeconds(delay);
      timerStarted = true;
    }

    public void StopGameTimer() {
      timerStarted = false;
    }

    public void ShowTimer() {
      timerText.gameObject.SetActive(true);
      timerBackground.gameObject.SetActive(true);
    }

    public void HideTimer() {
      timerText.gameObject.SetActive(false);
      timerBackground.gameObject.SetActive(false);
    }

    public float GetGameTime() {
      return gameTimer;
    }

    public void ResetGameTimer() {
      //Debug.Log("ResetGameTimer called");
      gameTimer = 0f;
    }
}
