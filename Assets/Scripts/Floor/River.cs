using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class River : Floor
{ 
    [SerializeField]
    PoolObjectType objectType;

    List<Log> logs = new List<Log>();

    int direction = 1;

    float speed = 5;

    public override void Generate()
    {
        direction = Random.Range(0, 101) > 50 ? -1 : 1;

        speed = Random.Range(70, 150) * .1f;

        StartLogsLoop();
    }

    void StartLogsLoop()
    {
        int type = Random.Range(0, 2);

        PoolObjectType log = PoolObjectType.Log_Type0;

        switch (type)
        {
            case 0:
                log = PoolObjectType.Log_Type0;
                break;

            case 1:
                log = PoolObjectType.Log_Type1;
                break;

            case 2:
                log = PoolObjectType.Log_Type2;
                break;

            default:
                log = PoolObjectType.Log_Type0;
                break;
        }

        logs.Add(ObjectPool.Instance.GetObject(log).GetComponent<Log>());

        logs[logs.Count - 1].transform.position = new Vector3(15 * direction, 0, transform.position.z);
        logs[logs.Count - 1].transform.localEulerAngles = Vector3.zero;

        logs[logs.Count - 1].transform.DOLocalMoveX(-15 * direction, speed).SetDelay(Random.Range(2, 4)).SetEase(Ease.Linear)
            .OnPlay(() =>
            {
                StartLogsLoop();
            })
            .OnComplete(() =>
            {
                logs[0].ReturnObject();

                logs.RemoveAt(0);
            });
    }

    public override void Reset()
    {
        AllReturnLogs();

        ObjectPool.Instance.ReturnObject(objectType, gameObject);
    }

    void AllReturnLogs()
    {
        for (int i = 0; i < logs.Count; i++)
            logs[i].ReturnObject();

        logs.Clear();
    }
}
