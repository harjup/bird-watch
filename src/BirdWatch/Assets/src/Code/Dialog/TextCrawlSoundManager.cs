using UnityEngine;
using System.Collections;
using System.Linq;

public class TextCrawlSoundManager : MonoBehaviour
{

    private AudioSource _talkTickOld;
    private AudioSource _talkTickMid;
    private AudioSource _talkTickOther;
    private AudioSource _currentTalkTick;


    private TextCrawler _textCrawler;

    // Use this for initialization
    void Start()
    {
        var audioSources = GetComponentsInChildren<AudioSource>();

        _talkTickMid   = audioSources.First(n => n.name == "talk-tick-mid");
        _talkTickOld   = audioSources.First(n => n.name == "talk-tick-old");
        _talkTickOther = audioSources.First(n => n.name == "talk-tick-other");

        _currentTalkTick = _talkTickOther;

        _textCrawler = FindObjectOfType<TextCrawler>();

        var count = 0;
        _textCrawler.OnTextCrawlTick += () =>
        {
            if (count == 0)
            {
                _currentTalkTick.Play();
            }

            if (count >= 3)
            {
                count = 0;
            }
            else
            {
                count++;
            }
        };
    }
    
    public void SetTextCrawlVoice(string voice)
    {
        _currentTalkTick.Stop();
        if (voice.ToLower() == "greg")
        {
            _currentTalkTick = _talkTickMid;
            return;
        }

        if (voice.ToLower() == "judd")
        {
            _currentTalkTick = _talkTickOld;
            return;
        }

        _currentTalkTick = _talkTickOther;
    }


}
