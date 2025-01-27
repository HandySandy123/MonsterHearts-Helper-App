using Godot;
using System;

public partial class DiceToss : Node3D
{
	[Export] private PackedScene _dice;
	[Export] private float scale;
	[Export] private float rollFactor;
	
	private Vector3 diceScale;
	private Dice _diceScriptOne;
	private Dice _diceScriptTwo;
	private Vector3 forward = Vector3.Forward;
	private Node3D diceOne;
	private Node3D diceTwo;
	private Random random = new Random();
	private Timer _timer;

	public int diceRoll = 0;
	
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		rollFactor += random.Next(100);
		diceScale = new Vector3(scale, 0, scale);
		_timer = GetNode<Timer>("Timer");
		_timer.OneShot = false;
		//GD.Print(_dice != null);
	}

	public override void _Input(InputEvent @event)
	{
		// if (@event.IsActionPressed("ui_accept"))
		// {
		// 	_timer.Start();
		// 	if (diceOne != null || diceTwo != null)
		// 	{
		// 		diceRoll = 0;
		// 		diceOne.QueueFree();
		// 		diceTwo.QueueFree();
		// 	}
		// 	ThrowDice();
		// }
	}
	

	public void ThrowDice()
	{
		GD.Print("New Dice");
		_timer.Start();
		if (diceOne != null || diceTwo != null)
		{
			diceRoll = 0; 
			diceOne.QueueFree();
			diceTwo.QueueFree();
		}
		
		diceOne = (Node3D) _dice.Instantiate();
		diceTwo = (Node3D) _dice.Instantiate();
		_diceScriptOne = diceOne as Dice;
		_diceScriptTwo = diceTwo as Dice;

		AddChild(diceOne);
		AddChild(diceTwo);
		
		diceOne.Position = Position + diceScale;
		diceTwo.Position = Position - diceScale;
		
		diceOne.Rotation += Vector3.One;
		diceTwo.Rotation += Vector3.One;
		
		diceOne.Rotation *= (float) random.NextDouble() * rollFactor;
		diceTwo.Rotation *= (float) random.NextDouble() * rollFactor;
	
		diceOne.Name = "DiceOne";
		diceTwo.Name = "DiceTwo";
	}

	public override void _PhysicsProcess(double delta)
	{
	}

	public void GetDiceRoll()
	{
		if (!_diceScriptOne.isMoving && !_diceScriptTwo.isMoving)
		{
			diceRoll = _diceScriptOne.GetDiceRoll() + _diceScriptTwo.GetDiceRoll();
		}
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{

	}
}
