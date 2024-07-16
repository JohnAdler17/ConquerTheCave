using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class RecordsScreen : MonoBehaviour
{
    [SerializeField] private List<TextMeshProUGUI> levelTimesText;

    [SerializeField] private TextMeshProUGUI totalGameTimeText;

    [SerializeField] private TextMeshProUGUI highScoreText;

    Records records;

    private int numLevels;
    private double gameTimeTotalSeconds;
    private int gameTimeMinutes;
    private double gameTimeSeconds;

    void Start() {
      records = GameObject.FindWithTag("Records").GetComponent<Records>();
      numLevels = records.ReturnNumLevels();
    }

    public void UpdateBestTimes() {
      for (int i = 0; i < numLevels; i++) {
        if (records.ReturnLevelTime(i) == -1f) {
          levelTimesText[i].text = "- - : - -";
        }
        else {
          levelTimesText[i].text = records.ReturnLevelTime(i).ToString("F2") + "s";
        }
      }
      if (records.ReturnGameTime() == -1f) {
        totalGameTimeText.text = "- - : - -";
        highScoreText.text = "0";
      }
      else {
        gameTimeTotalSeconds = records.ReturnGameTime();
        gameTimeMinutes = Convert.ToInt32(Math.Floor(gameTimeTotalSeconds / 60f));
        gameTimeSeconds = Convert.ToInt32(Math.Floor(gameTimeTotalSeconds - (gameTimeMinutes * 60)));
        highScoreText.text = records.ReturnHighScore().ToString();
        totalGameTimeText.text = gameTimeMinutes.ToString("F0") + "m " + gameTimeSeconds.ToString("F0") + "s " + ((gameTimeTotalSeconds - Math.Truncate(gameTimeTotalSeconds)) * 100).ToString("F0") + "ms";
      }
    }

}
