using UnityEngine;
using System.Collections;
public class Example : MonoBehaviour
{
    public string url;
    void Start()
    {
        WWW www = new WWW(url);
        audio.clip = www.audioClip;
    }
    void Update()
    {
        if (!audio.isPlaying && audio.clip.isReadyToPlay)
            audio.Play();
       // Debug.Log("IsPlaying: " + audio.isPlaying + "  IsReady:" + audio.clip.isReadyToPlay);
    }
}