using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FadeCanvas : MonoBehaviour
{
    [SerializeField] private Image fadePanel;

    [SerializeField] private float fadeTime = 0.5f;

    void Start() {
      fadePanel.enabled = true;
      StartCoroutine(FadeOutImage(fadePanel));
    }

    public void FadeOut() {
      StartCoroutine(FadeOutImage(fadePanel));
    }

    public void FadeIn() {
      StartCoroutine(FadeInImage(fadePanel));
    }

    IEnumerator FadeOutImage(Image image) {
      //Debug.Log("coroutine started");
      Color tempColor = image.color;

      //Debug.Log(tempColor.a);

      while (tempColor.a > 0f) {
        tempColor.a -= fadeTime * Time.deltaTime;
        image.color = tempColor;
        yield return null;
      }
      //hasFaded = true;
    }

    IEnumerator FadeInImage(Image image) {
      //Debug.Log("coroutine started");
      Color tempColor = image.color;

      //Debug.Log(tempColor.a);

      while (tempColor.a < 1f) {
        tempColor.a += fadeTime * Time.deltaTime;
        image.color = tempColor;
        yield return null;
      }
      //hasFaded = false;
    }
}
