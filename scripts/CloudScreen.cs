using System;
using System.Collections.Generic;
using Godot;

public class CloudScreen : Node2D
{
    private readonly RandomNumberGenerator _rand = new RandomNumberGenerator();
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
            lastCloud = cloud;
        }
    }

    private Vector2 GetCloudPosition(Cloud lastCloud)
    {
        if (lastCloud == null)
            return new Vector2(440, 400);
        return new Vector2
        (
            lastCloud.GlobalPosition.x + lastCloud.Size.x / 2f + _rand.RandfRange(180f, 360f),
            _rand.RandfRange(Math.Max(lastCloud.Position.y - 80f, 80f), Math.Min(lastCloud.Position.y + 80f, Size.y - 80f))
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
