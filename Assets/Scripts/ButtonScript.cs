using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class ButtonScript : MonoBehaviour
{
public int LevelIndex;
public enum ButtonUsage
{
    Changelevel,
    ReloadLevel
}
public ButtonUsage buttonType;

void Update()
{
    if(Input.GetKeyDown(KeyCode.N))
    {
    ReloadLevel();
    }
}

public void ButtonLevel(int index)
{
    if(index==-1)
    {
        GameController.Instance.RestartGame();
    }
    else
    {
        GameController.Instance.LevelChange(index);

    }
}

public void ReloadLevel()
{
    SceneManager.LoadScene(SceneManager.GetActiveScene().ToString());
}

}
