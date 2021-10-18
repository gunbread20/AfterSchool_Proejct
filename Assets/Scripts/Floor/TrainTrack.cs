using UnityEngine;
using DG.Tweening;

public class TrainTrack : Floor
{

    [SerializeField]
    PoolObjectType objectType;

    public override void Generate()
    {
        CreateTrainLoop();
    }

    [SerializeField]
    GameObject train;

    float speed = 5;

    [SerializeField]
    Light notifylight;

    [SerializeField]
    GameObject bells;

    void CreateTrainLoop()
    {
        int direction = Random.Range(0, 101) > 50 ? -1 : 1;

        train.transform.position = new Vector3(40 * direction, 0, transform.position.z);
        train.transform.GetChild(0).localEulerAngles = new Vector3(0, direction == 1 ? 0 : 180, 0);

        float delayTime = Random.Range(1, 3);

        bells.transform.DOScale(Vector3.one * 1.5f, .16f).SetLoops(6, LoopType.Yoyo).SetDelay(delayTime);

        notifylight.DOIntensity(10, .16f).SetLoops(6, LoopType.Yoyo).SetDelay(delayTime).OnComplete(() =>
        {
            train.transform.DOLocalMoveX(.4f * -direction, speed).SetEase(Ease.Linear)
                .OnComplete(() =>
                {
                    CreateTrainLoop();
                });
        });
    }


    public override void Reset()
    {
        bells.transform.DOKill();
        notifylight.DOKill();
        train.transform.DOKill();
        bells.transform.DOScale(Vector3.one, 0f);
        notifylight.DOIntensity(0, 0f);
        ObjectPool.Instance.ReturnObject(objectType, gameObject);
    }
}