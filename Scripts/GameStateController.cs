using Godot;
using System;
using System.Collections.Generic;
using System.Data.Common;

public partial class GameStateController : Node2D
{
	const int PLAYER_SELECTION = 1;
	const int PLAYER_LAUNCH = 2;
	const int ENEMY_SELECTION = 3;
	const int ENEMY_LAUNCH = 4;
	const int LEVEL_COMPLETE = 5;
	const int LEVEL_FAIL = 6;
	int gameState = PLAYER_SELECTION;

	List<Disc> Discs = new List<Disc>();

	FellaMaker fellaMaker;
	Node2D level;
	HUD hud;
	AudioStreamPlayer sfx;
	int discType = 0;
	bool isPlayersDisc = true;
	Shooter shooter;
	Disc shootee;
	Vector2 forceToApply;
	double stateChangeTimer = 0;
	int stateToBe = 0;
	bool shouldChangeState = false;

	public override void _Ready()
	{
		//fellaMaker = GetNode<FellaMaker>("FellaMaker");  Removed TODO: Delete?
		shooter = GetNode<Shooter>("Shooter");
		level = GetNode<Node2D>("Level");
		hud = GetNode<HUD>("HUD");
		sfx = GetNode<AudioStreamPlayer>("AudioStreamPlayer");
	}

	
	public override void _Process(double delta)
	{
		// Changes the state after a short delay
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

		// Temporary things for testing purposes, used for spawning in new Discs
		/*if (Input.IsActionJustPressed("mouse_2"))
        {
			Disc newDisc = fellaMaker.Spawn(GetGlobalMousePosition(), discType);
			newDisc.player = isPlayersDisc;
			Discs.Add(newDisc);
			isPlayersDisc = !isPlayersDisc;
        }
		if (Input.IsActionJustPressed("scroll_down"))
		{
			discType = (discType + 1) % 2;
		}*/// Removed TODO: Delete?

		// temp tests for collisions
		foreach (Disc disc in Discs)
		{
			List<Disc> collisons = disc.HandleCollisons();
			foreach (Disc other in collisons)
			{
				sfx.Play();
				if (gameState == PLAYER_LAUNCH && !other.player)
				{
					GD.Print(other.health);
					other.health -= disc.damage;
				}
				else if (gameState == ENEMY_LAUNCH && other.player)
				{
					other.health -= disc.damage;
				}
			}
		}
		for (int i = Discs.Count - 1; i >= 0; i--)
		{
			if (Discs[i].health <= 0)
				{
					Discs[i].QueueFree();
					Discs.RemoveAt(i);
				}
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
    			EnemySelection();
    			break;
  			case ENEMY_LAUNCH:
    			EnemyLaunch();
    			break;
			case LEVEL_COMPLETE:
				GD.Print("well done!!!");
				hud.ShowMessage("Well Done!!!");
    			break;
			case LEVEL_FAIL:
				GD.Print("You let vegetable-kind down!");
				hud.ShowMessage("You Let Veggie-Kind Down!");
				break;
  			default:
    			GD.Print("oh no the gamestate has an invalid value");
				hud.ShowMessage("ERROR: Invalid Gamestate");
    			break;
		}
	}

	public void StartLevel()
	{
		hud.ShowMessage("Shoot Your Shot");
		Node2D disks = level.GetNode<Node2D>("Disks");
		for (int i = 0; i < disks.GetChildCount(); i++)
		{
			Discs.Add(disks.GetChild<Disc>(i));
		}
	}

	/**
	* Parameters: state - an int representing the current stage in a turn. Should use the consts
	*				defined above.
	* Called when the game's state should change. A short delay is added to allow for any end of
	*	state actions to take place before the next state starts.
	*/
	private void ChangeState(int state)
	{
		shouldChangeState = true;
		stateToBe = state;
		stateChangeTimer = .1;
	}

	/**
	* Called during the PLAYER_SELECTION state. Allows the user to select and flick their desired
	*	Disc for the turn.
	*/
	private void PlayerSelection()
	{
		if (Input.IsActionJustPressed("mouse_1"))
		{
			shootee = shooter.FindShootee();
		}
		if (Input.IsActionPressed("mouse_1") && shootee != null)
		{
			forceToApply = shooter.AimShot(shootee);
		}
		if (Input.IsActionJustReleased("mouse_1") && shootee != null)
		{
			shooter.ShootShootee(shootee);
			shootee.OnFlickEffects();
			shootee = null;
			forceToApply = Vector2.Zero;
			ChangeState(PLAYER_LAUNCH);
		}
	}

	/**
	* Called during the PLAYER_LAUNCH state. Checks for all Discs to have stopped moving. When they
	*	have then it calls the EndTurn function on each Disc.
	*/
	private void PlayerLaunch()
	{
		bool doneMoving = true;
		foreach (Disc d in Discs)
		{
			if (d.LinearVelocity.LengthSquared() > 0.1)
			{
				doneMoving = false;
			}
		}
		
		if (doneMoving)
		{
			foreach (Disc d in Discs)
			{
				d.EndTurn();
			}
			ChangeState(ENEMY_SELECTION);
		} 
	}

	private void EnemySelection()
	{
		List<Disc> enemyDiscs = new List<Disc>();
		List<Disc> playerDiscs = new List<Disc>();
		foreach (Disc d in Discs)
		{
			if (d.player) {playerDiscs.Add(d);}
			else {enemyDiscs.Add(d);}
		}
		if (enemyDiscs.Count < 1)
		{
			ChangeState(LEVEL_COMPLETE);
			return;
		}
		if (playerDiscs.Count < 1)
		{
			ChangeState(LEVEL_FAIL);
			return;
		}

		enemyDiscs[0].ApplyCentralImpulse((playerDiscs[0].GlobalPosition - enemyDiscs[0].GlobalPosition).Normalized() * 3000);

		ChangeState(ENEMY_LAUNCH);
	}

	private void EnemyLaunch()
	{
		bool doneMoving = true;
		foreach (Disc d in Discs)
		{
			if (d.LinearVelocity.LengthSquared() > 0.1)
			{
				doneMoving = false;
			}
		}
		
		if (doneMoving)
		{
			foreach (Disc d in Discs)
			{
				d.EndTurn();
			}
			ChangeState(PLAYER_SELECTION);
		} 
	}
}

		
