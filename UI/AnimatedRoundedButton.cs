using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using Timer = System.Windows.Forms.Timer;

namespace HolosMigratorUI.UI;

public class AnimatedRoundedButton : Button
{
    private int _borderRadius = 10;
    private Color _originalBackColor;
    private Color _hoverBackColor;
    private Color _currentBackColor;
    private Timer _animationTimer;
    private bool _isHovering;
    private int _animationStep = 15; // Velocidad de la animación

    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public int BorderRadius
    {
        get => _borderRadius;
        set { _borderRadius = value; Invalidate(); }
    }

    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public Color HoverBackColor
    {
        get => _hoverBackColor;
        set => _hoverBackColor = value;
    }

    public AnimatedRoundedButton()
    {
        FlatStyle = FlatStyle.Flat;
        FlatAppearance.BorderSize = 0;
        BackColor = Color.FromArgb(30, 30, 40);
        ForeColor = Color.White;
        _hoverBackColor = Color.FromArgb(50, 50, 65);
        _originalBackColor = BackColor;
        _currentBackColor = BackColor;
        Cursor = Cursors.Hand;

        _animationTimer = new Timer();
        _animationTimer.Interval = 15; // ~60fps
        _animationTimer.Tick += AnimationTimer_Tick;
    }

    protected override void OnBackColorChanged(EventArgs e)
    {
        base.OnBackColorChanged(e);
        if (_animationTimer != null && !_animationTimer.Enabled && BackColor != _currentBackColor)
        {
            _originalBackColor = BackColor;
            _currentBackColor = BackColor;
        }
    }

    protected override void OnMouseEnter(EventArgs e)
    {
        base.OnMouseEnter(e);
        _isHovering = true;
        _animationTimer.Start();
    }

    protected override void OnMouseLeave(EventArgs e)
    {
        base.OnMouseLeave(e);
        _isHovering = false;
        _animationTimer.Start();
    }

    private void AnimationTimer_Tick(object? sender, EventArgs e)
    {
        Color targetColor = _isHovering ? _hoverBackColor : _originalBackColor;

        int currentR = _currentBackColor.R;
        int currentG = _currentBackColor.G;
        int currentB = _currentBackColor.B;

        int targetR = targetColor.R;
        int targetG = targetColor.G;
        int targetB = targetColor.B;

        int newR = StepColor(currentR, targetR);
        int newG = StepColor(currentG, targetG);
        int newB = StepColor(currentB, targetB);

        _currentBackColor = Color.FromArgb(newR, newG, newB);
        Invalidate();

        if (newR == targetR && newG == targetG && newB == targetB)
        {
            _animationTimer.Stop();
        }
    }

    private int StepColor(int current, int target)
    {
        if (current < target) return Math.Min(current + _animationStep, target);
        if (current > target) return Math.Max(current - _animationStep, target);
        return current;
    }

    protected override void OnPaint(PaintEventArgs pevent)
    {
        base.OnPaint(pevent);

        Graphics g = pevent.Graphics;
        g.SmoothingMode = SmoothingMode.AntiAlias;

        Rectangle rect = new Rectangle(0, 0, Width - 1, Height - 1);
        using (GraphicsPath path = GetRoundedPath(rect, _borderRadius))
        {
            // Dibujar fondo
            using (SolidBrush brush = new SolidBrush(_currentBackColor))
            {
                g.FillPath(brush, path);
            }

            // Dibujar borde según el estado
            Color borderColor = FlatAppearance.BorderColor;
            if (borderColor != Color.Empty && borderColor != Color.Transparent && FlatAppearance.BorderSize > 0)
            {
                using (Pen pen = new Pen(borderColor, FlatAppearance.BorderSize))
                {
                    g.DrawPath(pen, path);
                }
            }
            
            // Re-dibujar texto e imagen
            TextRenderer.DrawText(g, Text, Font, rect, ForeColor, TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter | TextFormatFlags.PreserveGraphicsClipping);
        }
    }

    private GraphicsPath GetRoundedPath(Rectangle rect, int radius)
    {
        GraphicsPath path = new GraphicsPath();
        if (radius <= 0)
        {
            path.AddRectangle(rect);
            return path;
        }

        int diameter = radius * 2;
        path.AddArc(rect.X, rect.Y, diameter, diameter, 180, 90);
        path.AddArc(rect.Right - diameter, rect.Y, diameter, diameter, 270, 90);
        path.AddArc(rect.Right - diameter, rect.Bottom - diameter, diameter, diameter, 0, 90);
        path.AddArc(rect.X, rect.Bottom - diameter, diameter, diameter, 90, 90);
        path.CloseFigure();
        return path;
    }
}
