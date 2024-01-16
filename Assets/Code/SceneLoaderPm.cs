using Framework;
using Framework.Reactive;
using UnityEngine.SceneManagement;

/// <summary>
/// Работает с загрузкой и перезагрузкой сцен
/// </summary>
public class SceneLoaderPm : BaseDisposable
{
    public struct Ctx
    {
        public ReactiveTrigger onRestartLevel;
        public ReactiveTrigger onNextLevel;
    }

    public SceneLoaderPm(Ctx ctx)
    {
        AddUnsafe(ctx.onRestartLevel.Subscribe(ReloadScene));
        AddUnsafe(ctx.onNextLevel.Subscribe(LoadNextScene));
    }

    private void LoadNextScene()
    {
        // Зацикленная реализация в тестовом проекте 
        ReloadScene();
    }

    private void ReloadScene()
    {
        SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().buildIndex);
    }
}