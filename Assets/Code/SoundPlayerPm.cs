using Configs;
using Framework;
using Framework.Reactive;
using UniRx;
using UnityEngine;

public class SoundPlayerPm : BaseDisposable
{
    private readonly Ctx _ctx;

    public struct Ctx
    {
        public ReactiveTrigger onGameRun;
        public AudioConfigs configs;
        public AudioSource audioSource;
    }

    public SoundPlayerPm(Ctx ctx)
    {
        _ctx = ctx;

        AddUnsafe(_ctx.onGameRun.Subscribe(StartPlayingMusicProcess));
    }

    private void StartPlayingMusicProcess()
    {
        AddUnsafe(Observable.EveryUpdate().Subscribe(PlayingMusicProcess));
    }

    private void PlayingMusicProcess(long _)
    {
        if (!_ctx.audioSource.isPlaying)
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