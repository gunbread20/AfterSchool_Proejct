using System.Collections.Generic;
using UnityEngine;

public class UIComponent : Component
{

    List<UIScreen> screens = new List<UIScreen>();

    public UIComponent()
    {
        screens.Add(GameObject.Find("StandbyScreen").GetComponent<UIScreen>());
        screens.Add(GameObject.Find("RunningScreen").GetComponent<UIScreen>());
        screens.Add(GameObject.Find("OverScreen").GetComponent<UIScreen>());
    }

    public void UpdateState(GameState state)
    {
        switch (state)
        {
            case GameState.INIT:
                CloseAllScreen();

                break;
            default:
                ActiveScreen(state);
                break;
        }
    }

    void ActiveScreen(GameState state)
    {
        CloseAllScreen();

        GetScreen(state).UpdateScreenState(true);
    }

    void CloseAllScreen()
    {
        foreach(var screen in screens)
        {
            screen.UpdateScreenState(false);
        }
    }

    UIScreen GetScreen(GameState state)
    {
        return screens.Find(screen => screen.screenState == state);
    }
}