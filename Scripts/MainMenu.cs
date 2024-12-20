using Godot;
using System;

public partial class MainMenu : CanvasLayer
{
    [Signal]
    public delegate void StartGameEventHandler();

    private void OnStartButtonPressed()
    {
        GD.Print("Emitting StartGame signal");
        //GetNode<Button>("StartButton").Hide();
        EmitSignal(SignalName.StartGame);
    }
}
