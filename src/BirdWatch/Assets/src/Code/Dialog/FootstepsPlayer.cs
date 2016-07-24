using UnityEngine;
using System.Collections;

public class FootstepsPlayer : MonoBehaviour
{
    private AudioSource _audioPlayer;

    private AudioSource AudioPlayer
    {
        get
        {
            if (_audioPlayer == null)
            {
                return _audioPlayer = GetComponent<AudioSource>();
            }
            return _audioPlayer;
        }
    }
    
    public void Play()
    {
        AudioPlayer.Play();
    }

    public void Stop()
    {
        AudioPlayer.Stop();
    }
}
