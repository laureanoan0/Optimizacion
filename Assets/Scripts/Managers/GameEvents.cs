using System;


public static class GameEvents
{
    public static event Action OnSceneExit;

    public static void ExitScene()
    {
        OnSceneExit?.Invoke();
        OnSceneExit = null;
    }
}
