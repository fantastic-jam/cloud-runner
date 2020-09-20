using System.Collections.Generic;
using Godot;

public class Parallax : Node2D
{
    [Export] private float _speed;

    private Camera _camera;
    private LinkedList<Sprite> _sprites = new LinkedList<Sprite>();

    public override void _Ready()
    {
        _camera = (Camera) GetParent();
        _sprites.AddLast(GetNode<Sprite>("Sprite"));
    }

    public override void _Process(float delta)
    {
        if (_sprites.First.Value.Position.x + _sprites.First.Value.Texture.GetWidth() < 0)
        {
            _sprites.First.Value.QueueFree();
            _sprites.RemoveFirst();
        }
        if (_sprites.Last.Value.Position.x < _camera.Position.x + _camera.GetViewport().Size.x)
        {
                var sprite = (Sprite) _sprites.Last.Value.Duplicate();
                var position = new Vector2(sprite.Position.x + sprite.Texture.GetWidth(), sprite.Position.y);
                AddChild(sprite);
                sprite.Position = position;
                _sprites.AddLast(sprite);
                if (sprite.Position.x < _sprites.Last.Value.Position.x || sprite.Position.x < 0)
                {
                    GD.Print("?");
                }

                GD.Print($"camera: {_camera.Position}");
                GD.Print($"add: {sprite.Position}");
        }
    }

    public override void _PhysicsProcess(float delta)
    {
        Translate(new Vector2(-_speed * delta, 0));
    }
}
