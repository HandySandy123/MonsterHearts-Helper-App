using Godot;
using System;

public partial class DiceToss : Node3D
{
	[Export] private PackedScene _dice;
	[Export] private float scale;
	[Export] private float rollFactor;
	
	[Export] public DiceRollButton rollButton;
	
	private Vector3 diceScale;
	private Dice _diceScriptOne, _diceScriptTwo;
	private Vector3 forward = Vector3.Forward;
	private Random random = new Random();
	private int stoppedDice = 0;
	private bool readyToPrint;
	private Action _diceRolling;

	public int diceRoll = 0;
	public Node3D diceOne, diceTwo;
	
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		rollFactor += random.Next(100);
		diceScale = new Vector3(scale, 0, scale);
	}
	
	public void ThrowDice()
	{
		GD.Print("New Dice");
		diceRoll = 0;
		stoppedDice = 0;
		
		//_timer.Start();
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
		
		_diceRolling += CheckForRoll;
		
		GetTree().CurrentScene.AddChild(diceOne);
		GetTree().CurrentScene.AddChild(diceTwo);
		
		diceOne.Position = Position + diceScale;
		diceTwo.Position = Position - diceScale;
		
		diceOne.Rotation += Vector3.One;
		diceTwo.Rotation += Vector3.One;
		
		diceOne.Rotation *= (float) random.NextDouble() * rollFactor;
		diceTwo.Rotation *= (float) random.NextDouble() * rollFactor;
	
		diceOne.Name = "DiceOne";
		diceTwo.Name = "DiceTwo";
	}

	public void UpdateRoll(int roll)
	{
		diceRoll = roll;
		rollButton.printResults(diceRoll);
	}
	
	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		_diceRolling?.Invoke();
	}

	private void CheckForRoll()
	{
		if (_diceScriptOne.IsMoving == false && _diceScriptTwo.IsMoving == false)
		{
			readyToPrint = true;
		}

		if (readyToPrint)
		{
			UpdateRoll(_diceScriptOne.GetDiceRoll() + _diceScriptTwo.GetDiceRoll());
			readyToPrint = false;
		}
	}
}
