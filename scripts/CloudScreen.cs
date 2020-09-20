using System;
using System.Collections.Generic;
using Godot;

public class CloudScreen : Node2D
{
    private const float Margin = 80f;

    private readonly RandomNumberGenerator _rand = new RandomNumberGenerator();

    [Export] private PackedScene _coinPrefab;
    [Export] private PackedScene[] _cloudPrefabs;

    public Camera2D Camera { private get; set; }
    public LinkedList<Cloud> Clouds { get; } = new LinkedList<Cloud>();
    public Vector2 Size { get; set; }

    public event Action<CloudScreen> OutOfScreen;

    public override void _Ready()
    {
        _rand.Randomize();
    }

    public void InitClouds(Cloud lastCloud)
    {
        while (true)
        {
            Vector2 cloudPosition = GetCloudPosition(lastCloud);
            var cloud = (Cloud) _cloudPrefabs[_rand.RandiRange(0, _cloudPrefabs.Length - 1)].Instance();
            if (cloudPosition.x + cloud.Size.x / 2f > GlobalPosition.x + Size.x)
                break;
            Clouds.AddLast(cloud);
            AddChild(cloud);
            cloud.GlobalPosition = cloudPosition;
            if (_rand.RandiRange(0, 1) == 0)
            {
                var coin = (Coin) _coinPrefab.Instance();
                coin.Position = new Vector2(-7.5f, -35f);
                cloud.AddChild(coin);
            }
            lastCloud = cloud;
        }
    }

    private Vector2 GetCloudPosition(Cloud lastCloud)
    {
        if (lastCloud == null)
            return new Vector2(440, 350);
        return new Vector2
        (
            lastCloud.GlobalPosition.x + lastCloud.Size.x / 2f + _rand.RandfRange(180f, 360f),
            _rand.RandfRange(Math.Max(lastCloud.Position.y - Margin, Margin), Math.Min(lastCloud.Position.y + Margin, Size.y - Margin))
        );
    }

    public override void _Process(float delta)
    {
        if (Position.x + Size.x < Camera.Position.x)
        {
            OutOfScreen?.Invoke(this);
            QueueFree();
        }
    }
}
