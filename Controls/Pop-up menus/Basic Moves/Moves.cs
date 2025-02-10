using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

public partial class Moves : Control
{
	[Export] float _menuSize = 0.70f;
	[Export] private float _lerpSpeed =  0.2f;
	[Export] private Color textColor;
	[Export] private RichTextLabel[] _labels;
	[Export] private bool basicMoves;
	[Export] private DiceToss _diceTosser;
	
	private bool _poppedUp = false;
	private Vector2 _leftAnchor;
	private Vector2 _rightAnchor;
	private Vector2 _targetAnchor;
	
	
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
        _rightAnchor  = new Vector2(1, 1 + _menuSize);
        _leftAnchor  = new Vector2(1 - _menuSize, 1);
        _targetAnchor = _rightAnchor;
        SetTextColor();
	}

	

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		AnchorLeft = (float) Mathf.Lerp(AnchorLeft, _targetAnchor.X, _lerpSpeed);
		AnchorRight = (float) Mathf.Lerp(AnchorRight, _targetAnchor.Y, _lerpSpeed);
	}
	
	private void SetTextColor()
	{
		foreach (RichTextLabel label in _labels)
		{
			string labelText = label.Text;
			label.Text = "[color=" + textColor.ToHtml() + "]" + labelText; 
		}
	}
	public void OnTextureButtonPressed()
	{
		if (!_poppedUp)
		{
			_targetAnchor = _leftAnchor;
		}
		else
		{
			_targetAnchor = _rightAnchor;
		}
		_poppedUp = !_poppedUp;
	}
}

struct Basicmoves
{
	enum MoveType
	{
		TurnSomeoneOn = 0,
		ShutSomeoneDown = 1,
		KeepYourCool = 1,
		LashOutPhysically = 2,
		RunAway = 2,
		GazeIntoTheAbyss = 3,
		PullingStrings = 4
	}
}
