using Godot;
using System;

public partial class HUD : CanvasLayer
{
	Label message;
	Timer timer;

    public override void _Ready()
    {
		message = GetNode<Label>("Message");
		timer = GetNode<Timer>("MessageTimer");
    }

    public void ShowMessage(string text) {
		message.Text = text;
		message.Show();

		timer.Start();
	}

	private void OnMessageTimerTimeout() {
		GD.Print("timeout message");
		GetNode<Label>("Message").Hide();
	}
}
