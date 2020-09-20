using Godot;

public class Camera : Camera2D
{
    [Export] private bool _scrolling = true;
    private float _speed = 190.0f;

    public override void _Process(float delta)
    {
        if (_scrolling)
            Translate(new Vector2(_speed * delta, 0));
    }

    public void SpeedUp()
    {
        _speed += 10f;
    }
}
