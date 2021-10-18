using UnityEngine;
using UnityEngine.UI;

public class RunningScreen : UIScreen
{
    [SerializeField]
    Button startButton;
    [SerializeField]
    Text score;

    private void Awake()
    {
        startButton.onClick.AddListener(() => GameManager.Instance.UpdateState(GameState.OVER));
    }

    public override void Init()
    {
        GameManager.Instance.GetGameBaseComponent<ScoreComponent>().Subscribe(score => { this.score.text = score.ToString(); });
    }

    public override void UpdateScreenStatus(bool open)
    {
        base.UpdateScreenStatus(open);
    }
}