using Godot;

public class UI : Position2D
{
    private Label _coinLabel;

    public int Coins
    {
        set => _coinLabel.Text = value.ToString();
    }

    public override void _Ready()
    {
        _coinLabel = (Label) FindNode("CoinLabel");
    }
}
