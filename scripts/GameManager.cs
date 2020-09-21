using Godot;

public class GameManager : Node
{
    [Export] private PackedScene _gameOver = null;

    private UI _ui;

    private int _coins = 0;
    private bool _isGameOver = false;

    public int Coins
    {
        get => _coins;
        set
        {
            _coins = value;
            _ui.Coins = value;
        }
    }

    public override void _Ready()
    {
        _ui = GetNode<UI>("/root/Level/UI");
    }

    public override void _Input(InputEvent @event)
    {
        if (_isGameOver && @event.IsPressed() && !@event.IsEcho())
        {
            switch (@event)
            {
                case InputEventKey _:
                case InputEventMouseButton _:
                case InputEventJoypadButton _:
                    GetTree().ReloadCurrentScene();
                    break;
            }
        }
    }

    public void OnPlayerDeath()
    {
        var timer = new Timer
        {
            OneShot = true,
            Autostart = true,
            WaitTime = 1
        };
        timer.Connect("timeout", this, nameof(GameOver));
        AddChild(timer);
    }

    private void GameOver()
    {
        _isGameOver = true;
        _ui.AddChild(_gameOver.Instance());
    }
}
