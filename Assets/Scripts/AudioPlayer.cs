using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioPlayer : MonoBehaviour
{
    static AudioPlayer instance; //static variables persist through all instances of a class

    [SerializeField] private List<AudioSource> backgroundTracks;

    void Awake() {
      ManageSingleton();
    }

    void ManageSingleton() {

      if (instance != null)
      {
        gameObject.SetActive(false); //by disabling the audio player before destroying it, you can make sure nothing else in the scene will try to access the instance that will be destroyed
        Destroy(gameObject);
      }
      else {
        instance = this;
        DontDestroyOnLoad(gameObject);
      }

    }

    private void StopAllMusic() {
      foreach (AudioSource song in backgroundTracks) {
        song.Stop();
      }
    }

    public void ChangeMusic(int trackIndex) {
      StopAllMusic();
      backgroundTracks[trackIndex].Play();
    }

}
