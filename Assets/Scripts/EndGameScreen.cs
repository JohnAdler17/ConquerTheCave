using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class EndGameScreen : MonoBehaviour
{
  private GameTimer TotalGameTimer;
  private Records records;
  private GameSession gameSession;

  [SerializeField] private TextMeshProUGUI gameTimeText;
  [SerializeField] private TextMeshProUGUI bestGameTimeText;

  [SerializeField] private TextMeshProUGUI finalScoreText;
  [SerializeField] private TextMeshProUGUI livesRemainingText;
  [SerializeField] private TextMeshProUGUI numDeathsText;

  void Awake() {
    TotalGameTimer = GameObject.FindWithTag("GameTimer").GetComponent<GameTimer>();
    records = GameObject.FindWithTag("Records").GetComponent<Records>();
    gameSession = GameObject.FindWithTag("GameSession").GetComponent<GameSession>();
  }

  void Start() {
    UpdateGameTime();
    UpdateBestGameTime();
    UpdateFinalScore();
    UpdateFinalLives();
    UpdateNumDeaths();
    TotalGameTimer.HideTimer();
  }

  public void BackToTitle() {
    Destroy(TotalGameTimer.gameObject);
    gameSession.ResetGameSession();
  }

  private void UpdateGameTime() {
    float finalTime = records.ReturnGameTime();
    int finalTimeMinutes = Convert.ToInt32(Math.Floor(finalTime / 60f));
    double finalTimeSeconds = Convert.ToInt32(Math.Floor(finalTime - (finalTimeMinutes * 60)));
    gameTimeText.text = "Final Game Time : " + finalTimeMinutes.ToString("F0") + "m " + finalTimeSeconds.ToString("F0") + "s " + ((finalTime - Math.Truncate(finalTime)) * 100).ToString("F0") + "ms";
  }

  private void UpdateBestGameTime() {
    float bestTime = TotalGameTimer.GetGameTime();
    int bestTimeMinutes = Convert.ToInt32(Math.Floor(bestTime / 60f));
    double bestTimeSeconds = Convert.ToInt32(Math.Floor(bestTime - (bestTimeMinutes * 60)));
    bestGameTimeText.text = "Best Game Time : " + bestTimeMinutes.ToString("F0") + "m " + bestTimeSeconds.ToString("F0") + "s " + ((bestTime - Math.Truncate(bestTime)) * 100).ToString("F0") + "ms";
  }

  private void UpdateFinalScore() {
    int scoreFromExtraLives = gameSession.ReturnNumLives() * 400;
    int finalScore = gameSession.ReturnScore() + scoreFromExtraLives;
    records.SetHighScore(finalScore);
    finalScoreText.text = "Final Score : \n" + gameSession.ReturnScore().ToString() + " + \n" + gameSession.ReturnNumLives().ToString() + " X 400\n = " + finalScore.ToString();
  }

  private void UpdateFinalLives() {
    livesRemainingText.text = "Remaining Lives : " + gameSession.ReturnNumLives().ToString();
  }

  private void UpdateNumDeaths() {
    numDeathsText.text = " : " + gameSession.ReturnNumDeaths().ToString();
  }
}
