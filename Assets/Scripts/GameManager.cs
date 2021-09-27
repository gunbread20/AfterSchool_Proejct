﻿using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UniRx;
using UniRx.Triggers;

public class GameManager : MonoBehaviour
{

    public static GameManager Instance;

    public GameState STATE
    {
        get
        {
            return state;
        }
    }

    private List<Component> components = new List<Component>();

    private GameState state;

    private void Start()
    {
        Instance = this;

        components.Add (new UiComponent());
        components.Add(new FloorComponent());
        components.Add(new WindowInputComponent());
        components.Add(new PlayerComponent());
        components.Add(new CameraComponent());

        UpdateState(GameState.INIT);
    }

    public void UpdateState(GameState state)
    {
        this.state = state;

        for (int i = 0; i < components.Count; i++)
            components[i].UpdateState(state);

        if (state == GameState.INIT)
            UpdateState(GameState.STANDBY);
    }

    public T GetGameBaseComponent<T>() where T : Component
    {
        T value = default(T);

        for (int i = 0; i < components.Count; i++)
            if (components[i] is T)
                value = (T)components[i];

        return value;
    }

}