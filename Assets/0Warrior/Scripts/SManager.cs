using EazyTools.SoundManager;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SManager : MonoBehaviour {

    public static SManager Ins;

    public AudioClip[] audios;

    int bgmInt, bgmInt2;

    private void Awake() {
        Ins = this;
    }

    public void PlayAppear() {
        SoundManager.PlaySound(audios[0], 1);
    }

    public void PlayOpen() {
        SoundManager.PlaySound(audios[1], 1);
    }

    public void PlayShutter() {
        SoundManager.PlaySound(audios[2]);
    }

    public void PlayBGM() {
        bgmInt = SoundManager.PlayMusic(audios[3], .7f, true, false, 2, 2);
    }

    public void StopBGM() {
        Audio audio = SoundManager.GetAudio(bgmInt);
        if (audio != null)
            audio.SetVolume(0, 2f);
    }

    public void PlayBGM2() {
        bgmInt2 = SoundManager.PlayMusic(audios[4], .6f, true, false, 2, 2);
    }

    public void StopBGM2() {
        Audio audio = SoundManager.GetAudio(bgmInt2);
        if (audio != null)
            audio.SetVolume(0, 2f);
    }

    public void PlayTouch() {
        SoundManager.PlaySound(audios[5], .4f);
    }
}
