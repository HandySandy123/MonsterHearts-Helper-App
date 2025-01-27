using Godot;
using System;

public partial class DiceRollutton : Control
{
	[Export] private Node3D playScene;

	private Button _button;
	private DiceToss diceToss;
	private int result;
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		diceToss = playScene as DiceToss;
		_button = GetNode<Button>("Button");
	}

	public void OnButtonPressed()
	{
		result = 0;
		diceToss.ThrowDice();
		_button.ReleaseFocus();
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		if (diceToss.diceRoll != 0)
		{
			result = diceToss.diceRoll;
		}
	}
}
