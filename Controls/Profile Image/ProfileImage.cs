using Godot;
using System;

public partial class ProfileImage : TextureRect
{
	Texture2D _texture;
	private Image _profilePic = new Image();
	private Vector2 _currentSize;
	private FileDialog _dialog;
	private Button _button;
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		_button = GetNode<Button>("Button");
		_texture = Texture;
		_button.Pressed += OnClick;
		//GD.Print(_texture.ResourcePath);
		_currentSize = _texture.GetSize();
		_dialog = GetNode<FileDialog>("FileDialog");
		_dialog.CurrentDir = "/Users/";
		

	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		
	}

	public void OnClick()
	{
		_dialog.PopupCentered();
		_button.ReleaseFocus();
	}

	public void GetImagePath(string path)
	{
		_profilePic = Image.LoadFromFile(path);
		_profilePic.Resize((int)_currentSize.X, (int)_currentSize.Y);
		_texture = ImageTexture.CreateFromImage(_profilePic);
		Texture = _texture;
	}
}
