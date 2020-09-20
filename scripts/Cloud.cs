using Godot;

public class Cloud : KinematicBody2D
{
    public Vector2 Size => GetNode<Sprite>("Sprite").RegionRect.Size;
}
