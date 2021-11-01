using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    List<Component> components = new List<Component>();

    GameState state;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
    }

    private void Start()
    {
        components.Add(new UIComponent());
        components.Add(new FloorComponent());
        components.Add(new InputComponent());
        components.Add(new CameraComponent());

        UpdateState(GameState.INIT);
    }

    public void UpdateState(GameState state)
    {
        for (int i = 0; i < components.Count; i++)
            components[i].UpdateState(state);

        this.state = state;

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

    public void UpdateState(int state)
    {
        UpdateState((GameState)state);
    }

}
