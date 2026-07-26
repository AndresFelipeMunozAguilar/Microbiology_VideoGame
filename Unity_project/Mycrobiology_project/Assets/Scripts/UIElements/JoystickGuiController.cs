using UnityEngine;

public class JoystickGuiController : MonoBehaviour,IPuzzlePausable
{
    [SerializeField] GameObject joystick;
    public void PuzzlePauseMe()
    {
        joystick.SetActive(false);
    }

    public void PuzzleResumeMe()
    {
        joystick.SetActive(true);
    }
    GameManager gameManager;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        gameManager = GameManager.GetInstance();
        gameManager.SubscribePuzzlePausable(this);
    }

    private void OnDestroy() {
        gameManager.UnsubscribePuzzlePausable(this);
    }
}
