using Godot;
using System;

public partial class OmniController : Node2D
{
    private void OnMainMenuStartButtonPressed()
    {
        GD.Print("onmainmenustartbuttonpressed entered");
        GetNode<CanvasLayer>("MainMenu").Hide();
    }
}

