using Godot;
using System;

public partial class OmniController : Node2D
{
    CanvasLayer mainMenu;
    GameStateController gameState;

    public override void _Ready()
    {
        mainMenu = GetNode<CanvasLayer>("MainMenu");
        gameState = GetNode<GameStateController>("GameStateController");
    }

    private void OnMainMenuStartButtonPressed()
    {
        GD.Print("onmainmenustartbuttonpressed entered");
        mainMenu.Hide();
        gameState.StartLevel();
    }
}

