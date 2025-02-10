using Godot;
using System;

public partial class DiceRollutton : Control
{
	[Export] private Node3D playScene;

	private Button _button;
	private DiceToss diceToss;
	private int result;
	private RichTextLabel resultText;
	private string centerText = "[center] ";
	
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		diceToss = playScene as DiceToss;
		_button = GetNode<Button>("Button");
		resultText = GetNode<RichTextLabel>("GridContainer/ResultText");
		GD.Print(resultText.Name);
	}

	public void printResults(int result)
	{
		if (result <= 1)
		{
			resultText.Text = centerText;
			return;
		}
		this.result = result;
		resultText.Text = centerText + result;
	}
	public void OnButtonPressed()
	{
		resultText.Text = centerText;
		diceToss.ThrowDice();
		_button.ReleaseFocus();
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		
	}
}
