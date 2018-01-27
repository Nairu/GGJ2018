using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOptions
{
    public static GameOptions Instance()
    {
        if (option != null)
            option = new GameOptions();
        return option;
    }

    private static GameOptions option;

    public Color ButtonAColour;
    public Color ButtonBColour;
    public Color ButtonXColour;
    public Color ButtonYColour;
}
