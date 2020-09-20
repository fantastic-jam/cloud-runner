using System;
using Godot;

public class Player : KinematicBody2D
{
    private const float DragX = 0.5f;
    private const float AirDragX = 0.95f;
    private const float AirDragY = 0.88f;
    private const float Gravity = 800f;
    private const float Speed = 250f;
    private const float AirSpeed = Speed * 0.05f;
    private float MaxSpeed = 275f;
    private const float JumpSpeed = 70f;
    private const float JumpMaxSpeed = 500f;

    private Vector2 _velocity = Vector2.Zero;
    private Vector2 _acceleration = Vector2.Zero;
    private bool _isJumping = false;

    private AnimatedSprite _sprite;

    public override void _Ready()
    {
        _sprite = GetNode<AnimatedSprite>("AnimatedSprite");
    }

    public override void _Process(float delta)
    {
        // region DEBUG
        if (Input.IsActionJustPressed("ui_cancel"))
        {
            GetTree().ReloadCurrentScene();
            return;
        }
        // endregion

        float speed = IsOnFloor() ? Speed : AirSpeed;

        float left = Input.GetActionStrength("left");
        float right = Input.GetActionStrength("right");

        var up = 0f;
        if (IsOnFloor() && Input.IsActionJustPressed("jump"))
        {
            _isJumping = true;
            up = -JumpSpeed;
        }
        else if (_isJumping && Input.IsActionJustReleased("jump"))
        {
            _isJumping = false;
        }
        else if (_isJumping)
        {
            up = _acceleration.y * AirDragY;
        }

        if (left > float.Epsilon)
            _sprite.FlipH = true;
        else if (right > float.Epsilon)
            _sprite.FlipH = false;

        _acceleration = new Vector2((right - left) * speed, up);
    }

    public override void _PhysicsProcess(float delta)
    {
        _velocity = new Vector2
        (
            _acceleration.x < -float.Epsilon
                ? Math.Max(_velocity.x + _acceleration.x, -MaxSpeed)
                : _acceleration.x > float.Epsilon
                    ? Math.Min(_velocity.x + _acceleration.x, MaxSpeed)
                    : _velocity.x * (IsOnFloor() ? DragX : AirDragX),
            Math.Max(_velocity.y + _acceleration.y, -JumpMaxSpeed) + Gravity * delta
        );
        _velocity = MoveAndSlide(_velocity, Vector2.Up);

        var animation = "idle";
        if (!IsOnFloor())
        {
            animation = _velocity.y <= 0f ? "jump" : "fall";
        }
        else if (_velocity.x < -0.1f || _velocity.x > 0.1f)
        {
            animation = "walk";
        }

        if (_sprite.Animation != animation)
        {
            _sprite.Play(animation);
        }
    }

    public void SpeedUp()
    {
        MaxSpeed += 5f;
    }
}
