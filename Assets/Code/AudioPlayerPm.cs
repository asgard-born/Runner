using System;
using Configs;
using Framework;
using Framework.Reactive;
using UniRx;
using UnityEngine;
using Random = UnityEngine.Random;

public class AudioPlayerPm : BaseDisposable
{
    private readonly Ctx _ctx;
    private IDisposable _musicPlayingProcess;
    
    public struct Ctx
    {
        public ReactiveTrigger onGameRun;
        public ReactiveTrigger onGameWin;
        public ReactiveTrigger onGameOver;
        public AudioConfigs configs;
        public AudioSource audioSource;
    }

    public AudioPlayerPm(Ctx ctx)
    {
        _ctx = ctx;

        AddUnsafe(_ctx.onGameRun.Subscribe(StartPlayingMusicProcess));
        AddUnsafe(_ctx.onGameWin.Subscribe(OnGameStopped));
        AddUnsafe(_ctx.onGameOver.Subscribe(OnGameStopped));
    }

    private void OnGameStopped()
    {
        _musicPlayingProcess.Dispose();
        _ctx.audioSource.Stop();
    }

    private void StartPlayingMusicProcess()
    {
        _musicPlayingProcess = Observable.EveryUpdate().Subscribe(PlayingMusicProcess);
    }

    private void PlayingMusicProcess(long _)
    {
        if (_ctx.audioSource != null && !_ctx.audioSource.isPlaying)
        {
            PlayRandomSound();
        }
    }

    private void PlayRandomSound()
    {
        var clips = _ctx.configs.musicList;
        var randomIndex = Random.Range(0, clips.Count);
        _ctx.audioSource.PlayOneShot(clips[randomIndex]);
    }
}