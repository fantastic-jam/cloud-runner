using Godot;
using System;

public class Coin : Area2D
{
    private GameManager _gameManager;

    public override void _Ready()
    {
        _gameManager = GetNode<GameManager>("/root/Level/GameManager");
    }

    private void OnCollision(Node other)
    {
        if (other is Player)
        {
            _gameManager.Coins++;
            QueueFree();
        }
    }
}
