using Godot;
using System;
using MonsterHeartsHelper.Multiplayer;

public partial class MultiplayerController : Control
{
	[Export] private int port = 8910;
	[Export] private String address = "127.0.0.1";

	private ENetMultiplayerPeer peer;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		Multiplayer.PeerConnected += PeerConnected;
		Multiplayer.PeerDisconnected += PeerDisconnected;
		Multiplayer.ConnectedToServer += ConnectToServer;
		Multiplayer.ConnectionFailed += ConnectionFailed;
	}

	

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
	/*<summary>
	 * Runs when the connection fails, and it only runs on the clients
	 * </summary>
	 * <exception cref="NotImplementedException"></exception> 
	 */
	private void ConnectionFailed()
	{
		GD.Print("Connection Failed");
	}
	
	/*<summary>
	 * Runs when the connection is successful, and it only runs on the clients
	 * </summary>
	 * <exception cref="NotImplementedException"></exception>
	 */
	private void ConnectToServer()
	{
		GD.Print("Connecting to server");
		RpcId(1, "SendPlayerInformation", GetNode<LineEdit>("LineEdit").Text, Multiplayer.GetUniqueId());
	}
	/*<summary>
	 * Runs when a player is disconnected and runs on all peers
	 * </summary>
	 * <param name="id">id of the player that disconnected</param>
	 * <exception cref="NotImplementedException"></exception>
	 */
	private void PeerDisconnected(long id)
	{
		GD.Print("Player Disconnected: " + id);
	}

	
	/*<summary>
	 * Runs when a player is connected and runs on all peers
	 * </summary>
	 * <param name="id">id of the player that disconnected</param>
	 * <exception cref="NotImplementedException"></exception>
	 */
	public void PeerConnected(long id)
	{
		GD.Print("Player Connected: " + id);
	}
	public void OnHostDown()
	{
		peer = new ENetMultiplayerPeer();
		var error = peer.CreateServer(port, 8);
		if (error != Error.Ok)
		{
			GD.Print("Error Cannot host: " + error.ToString());
			return;
		}
		peer.Host.Compress(ENetConnection.CompressionMode.RangeCoder);
		Multiplayer.MultiplayerPeer = peer;
		GD.Print("Waiting For Players");
		SendPlayerInformation(GetNode<LineEdit>("LineEdit").Text, 1);
	}
	public void OnJoinDown()
	{
		peer = new ENetMultiplayerPeer();
		peer.CreateClient(address, port);
		
		peer.Host.Compress(ENetConnection.CompressionMode.RangeCoder);
		Multiplayer.MultiplayerPeer = peer;
		GD.Print("Joining game");
	}
	public void OnStartGameDown()
	{
		Rpc("StartGame");
	}

	[Rpc(MultiplayerApi.RpcMode.AnyPeer, CallLocal = true, TransferMode = MultiplayerPeer.TransferModeEnum.Reliable)]
	private void StartGame()
	{
		foreach (var playerInfo in GameManager.Players)
		{
			GD.Print(playerInfo.Name + " is playing");
		}
		var scene = ResourceLoader.Load<PackedScene>("res://3D Scenes/PlayScene.tscn").Instantiate<Node3D>();
		GetTree().Root.AddChild(scene);
		this.Hide();
	}
	[Rpc(MultiplayerApi.RpcMode.AnyPeer)]
	private void SendPlayerInformation(string name, int id)
	{
		PlayerInfo playerInfo = new PlayerInfo()
		{
			Name = name,
			Id = id
		};
		if (!GameManager.Players.Contains(playerInfo))
		{
			GameManager.Players.Add(playerInfo);
		}

		if (Multiplayer.IsServer())
		{
			foreach (var item in GameManager.Players)
			{
				Rpc("SendPlayerInformation", name, id);
			}
		}
	}
}
