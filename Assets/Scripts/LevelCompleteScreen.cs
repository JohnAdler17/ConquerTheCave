using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class LevelCompleteScreen : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI levelTimeText;
    [SerializeField] private TextMeshProUGUI gameTimeText;
    [SerializeField] private TextMeshProUGUI bestTimeText;

    private int gameTimeMinutes;
    private double gameTimeSeconds;

    private int bestTimeMinutes;
    private double bestTimeSeconds;

    private int levelTimeMinutes;
    private double levelTimeSeconds;

    public void SetBestTime(float bestTime) {
      bestTimeMinutes = Convert.ToInt32(Math.Floor(bestTime / 60f));
      bestTimeSeconds = Convert.ToInt32(Math.Floor(bestTime - (bestTimeMinutes * 60)));
      bestTimeText.text = "Best Time : " + bestTimeMinutes.ToString("F0") + "m " + bestTimeSeconds.ToString("F0") + "s " + ((bestTime - Math.Truncate(bestTime)) * 100).ToString("F0") + "ms";
    }

    public void SetLevelTime(float levelTime) {
      levelTimeMinutes = Convert.ToInt32(Math.Floor(levelTime / 60f));
      levelTimeSeconds = Convert.ToInt32(Math.Floor(levelTime - (levelTimeMinutes * 60)));
      levelTimeText.text = "Level Time : " + levelTimeMinutes.ToString("F0") + "m " + levelTimeSeconds.ToString("F0") + "s " + ((levelTime - Math.Truncate(levelTime)) * 100).ToString("F0") + "ms";
    }

    public void SetGameTime(float gameTime) {
      gameTimeMinutes = Convert.ToInt32(Math.Floor(gameTime / 60f));
      gameTimeSeconds = Convert.ToInt32(Math.Floor(gameTime - (gameTimeMinutes * 60)));
      gameTimeText.text = "Total Game Time : " + gameTimeMinutes.ToString("F0") + "m " + gameTimeSeconds.ToString("F0") + "s " + ((gameTime - Math.Truncate(gameTime)) * 100).ToString("F0") + "ms";
    }
}
