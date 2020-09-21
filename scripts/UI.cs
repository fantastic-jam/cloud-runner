using Godot;

public class UI : Node
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
