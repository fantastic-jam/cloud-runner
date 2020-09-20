using Godot;

public class GameManager : Node
{
    private UI _ui;

    private int _coins = 0;

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
        _ui = (UI) GetParent().FindNode("UI");
    }
}
