using Godot;
using System;
using System.Collections.Generic;

public partial class GameStateController : Node2D
{
	const int PLAYER_SELECTION = 1;
	const int PLAYER_LAUNCH = 2;
	const int ENEMY_SELECTION = 3;
	const int ENEMY_LAUNCH = 4;
	int gameState = PLAYER_SELECTION;

	List<Disc> playerDiscs = new List<Disc>();
	List<Disc> enemyDiscs = new List<Disc>();

	FellaMaker fellaMaker;
	Shooter shooter;
	double stateChangeTimer = 0;
	int stateToBe = 0;
	bool shouldChangeState = false;

	public override void _Ready()
	{
		fellaMaker = GetNode<FellaMaker>("FellaMaker");
		shooter = GetNode<Shooter>("Shooter");
	}

	
	public override void _Process(double delta)
	{
		if (shouldChangeState)
		{
			stateChangeTimer -= delta;
			if (stateChangeTimer <= 0)
			{
				gameState = stateToBe;
				shouldChangeState = false;
			}
			return;
		}

		if (Input.IsActionJustPressed("mouse_2"))
        {
            playerDiscs.Add(fellaMaker.Spawn(GetGlobalMousePosition()));
        }

		switch(gameState) 
		{
  			case PLAYER_SELECTION:
    			PlayerSelection();
    			break;
  			case PLAYER_LAUNCH:
    			PlayerLaunch();
    			break;
			case ENEMY_SELECTION:
    			ChangeState(ENEMY_LAUNCH);
    			break;
  			case ENEMY_LAUNCH:
    			ChangeState(PLAYER_SELECTION);
    			break;
  			default:
    			GD.Print("oh no the gamestate has an invalid value");
    			break;
		}
	}

	private void ChangeState(int state)
	{
		shouldChangeState = true;
		stateToBe = state;
		stateChangeTimer = .1;
	}

	private void PlayerSelection()
	{
		if (shooter.HandleMouseInput())
		{
			ChangeState(PLAYER_LAUNCH);
		}
	}

	private void PlayerLaunch()
	{
		bool doneMoving = true;
		foreach (Disc d in playerDiscs)
		{
			if (d.LinearVelocity.LengthSquared() > 0.1)
			{
				doneMoving = false;
			}
		}
		foreach (Disc d in enemyDiscs)
		{
			if (d.LinearVelocity.LengthSquared() > 0.1)
			{
				doneMoving = false;
			}
		}
		
		if (doneMoving)
		{
			ChangeState(ENEMY_SELECTION);
		} 
	}
}

		
