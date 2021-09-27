using UnityEngine;
using UnityEngine.UI;

public class RunningScreen : UIScreen
{
    [SerializeField]
    Button startButton;

    private void Awake()
    {
        startButton.onClick.AddListener(() => GameManager.Instance.UpdateState(GameState.OVER));
    }

    public override void UpdateScreenStatus(bool open)
    {
        base.UpdateScreenStatus(open);
    }
}