using Framework;
using Framework.Reactive;
using UnityEngine.SceneManagement;

public class SceneManagerPm : BaseDisposable
{
    public struct Ctx
    {
        public ReactiveTrigger onRestartLevel;
    }

    public SceneManagerPm(Ctx ctx)
    {
        AddUnsafe(ctx.onRestartLevel.Subscribe(RestartGame));
    }

    private void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}