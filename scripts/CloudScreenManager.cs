using Godot;
using System.Collections.Generic;

public class CloudScreenManager : Node2D
{
    private readonly LinkedList<CloudScreen> _screens = new LinkedList<CloudScreen>();

    [Export] private PackedScene _cloudScreenPrefab;

    private Camera _camera;
    private Player _player;

    public override void _Ready()
    {
        _camera = GetParent().GetNode<Camera>("Camera2D");
        _player = GetParent().GetNode<Player>("Player");
        AddCloudScreen();
        AddCloudScreen();
        AddCloudScreen();
    }

    private void AddCloudScreen()
    {
        Cloud lastCloud = _screens.Last?.Value?.Clouds?.Last?.Value;
        float x = _screens.Count > 0 ? _screens.Last.Value.Position.x + _camera.ScreenSize.x : 0f;
        var screen = (CloudScreen) _cloudScreenPrefab.Instance();
        screen.Position = new Vector2(x, 0);
        screen.Camera = _camera;
        screen.OutOfScreen += OnScreenOutOfScreen;
        _screens.AddLast(screen);
        AddChild(screen);
        screen.InitClouds(lastCloud);
    }

    private void OnScreenOutOfScreen(CloudScreen screen)
    {
        _camera.SpeedUp();
        _player.SpeedUp();
        _screens.Remove(screen);
        AddCloudScreen();
    }
}
