using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using LifeIn2D;
using System;

public class TextDisplayAction : ITimedAction
{
    private double _duration;
    public double Duration => _duration;

    private double _currentTime;
    public double CurrentTime { get => _currentTime; set => _currentTime = value; }

    private SpriteFont _spriteFont;
    private bool _canDisplay;
    private string _text;
    private Vector2 _position;
    private Color _color;

    public event Action OnBegin;
    public event Action OnComplete;

    public TextDisplayAction(float duration, string text, SpriteFont spriteFont, Vector2 position, Color color)
    {
        _duration = duration;
        _currentTime = 0;
        _spriteFont = spriteFont;
        _canDisplay = false;
        _color = color;
        _position = position;
        _text = text;
    }

    public void Finish()
    {
        _canDisplay = false;
        OnComplete?.Invoke();
    }

    public void Start()
    {
        _canDisplay = true;
        OnBegin?.Invoke();
    }

    public void Update()
    {
    }

    public void Draw(Sprites sprites)
    {
        if (_canDisplay)
        {
            sprites.DrawString(_spriteFont, _text,_position,_color);
        }
    }
}