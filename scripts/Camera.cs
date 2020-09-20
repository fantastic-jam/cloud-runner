using Godot;

public class Camera : Camera2D
{
    [Export] private bool _scrolling = true;
    private float _speed = 220.0f;

    public override void _Process(float delta)
    {
        if (_scrolling)
            Translate(new Vector2(_speed * delta, 0));
    }

    public void SpeedUp()
    {
        _speed += 5f;
    }
}
