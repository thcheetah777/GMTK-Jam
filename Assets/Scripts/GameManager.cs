using UnityEngine;

public class GameManager : MonoBehaviour
{

    private static bool s_pause;
    public static bool GlobalPause
    {
        get => s_pause || s_hitpause;
        set => s_pause = value;
    }
    
    private static float s_hitpauseTime;
    private static bool s_hitpause;
    public static bool Hitpause
    {
        get => s_hitpause;
        private set
        {
            Time.timeScale = value ? 0.001f : 1;
            s_hitpause = value;
        }
    }

    public static void ApplyHitpause(float time)
    {
        if (time == 0)
            return;
        s_hitpauseTime = Mathf.Max(s_hitpauseTime, time);
        Hitpause = true;
    }
    
    private void Awake()
    {
        Application.targetFrameRate = 60;
    }

    private void LateUpdate()
    {
        if (s_hitpauseTime > Time.unscaledDeltaTime)
            s_hitpauseTime -= Time.unscaledDeltaTime;
        else
            Hitpause = false;
    }
}
