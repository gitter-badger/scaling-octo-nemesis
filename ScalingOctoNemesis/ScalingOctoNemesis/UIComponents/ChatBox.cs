﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ScalingOctoNemesis.UI;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ScalingOctoNemesis.Util;

namespace ScalingOctoNemesis.UIComponents
{
    public class ChatBox : UIContainer
    {
        int _index = 0; // Index from which to draw the messages
        int _maxLines; // Max nb of lines to be shown concurently 
        float _lineHeight; // Height of each message lines
        SpriteFont _font;

        public int Index { get { return _index; } }
        public int InnerLength { get { return Messages * (int)_lineHeight; } }
        public bool Full { get { return Messages >= _maxLines; } }
        public int Messages     { get { return _messages.Count; } }
        public int MaxMsg { get { return _maxLines; } }
        // Replaced by vector be cause we need direct access
        // Or any collection that allows the behaviours
        // of both Vector and Queue (Direct access + FIFO)
        List<UIComponent> _messages = new List<UIComponent>();
        Timer _timer = new Timer();
        public ChatBox(string id, Vector2 pos, Vector2 size, Vector2 padding, SpriteFont font)
            : base(id, pos, size, padding)
        {
            _font = font;
            _lineHeight = _font.MeasureString(" ").Y;
            _maxLines = (int)((Size.Y - Padding.Y * 2) / _lineHeight);
        }

        public void UpIndex()
        {
            if (_index != 0)
                --_index;
        }

        public void DownIndex()
        {
            if (_index != Messages - 1 && _index + 1 < _messages.Count && _messages.Count >= _maxLines)
                ++_index;
        }

        public void AddMessage(string message, string from)
        {
            Label label = new Label(ComponentsCount.ToString(), from + ": " + message, 0, 0, 0, 0, 0, 0, _font);
            //label.Position = new Vector2(Position.X + Padding.X, Position.Y + Padding.Y + ComponentsCount * 20);
            _components.Add(label);
            _messages.Add(label);
            //Delay.AddOps(new DelayOps(RemoveMessage, new Timer(), 3000));
        }

        private void RemoveMessage()
        {
            if (_messages.Count != 0)
            {
                _components.Remove(_messages[0]);
                _messages.RemoveAt(0);
            }
            if (_components.Count != 0)
                foreach (UIComponent c in _components)
                    c.Position -= new Vector2(0, 20);
        }

        public override void Update(GameTime elapsedTime)
        {
            base.Update(elapsedTime);
        }

        public virtual void DrawInner(SpriteBatch sb)
        {
            DrawingTools.DrawRectangle(sb, Position, Size + Padding * 2, new Color(0, 0, 0, 0.3f), LayerDepths.D1);
        }

        public virtual void DrawBorder(SpriteBatch sb)
        {
            DrawingTools.DrawEmptyRectangle(sb, Position, Size + Padding * 2, Color.Black, LayerDepths.D2);
        }

        public virtual void DrawMessages(SpriteBatch sb)
        {
            if (_messages.Count == 0)
                return;

            for (int i = _index, j = 0; i != _messages.Count && j != _maxLines; ++i, ++j)
            {
                _messages[i].Visible = true;
                _messages[i].Position = Position + Padding + new Vector2(0, j * _lineHeight);
                _messages[i].Draw(sb);
            }
        }

        public override void Draw(SpriteBatch sb)
        {
            DrawInner(sb);
            DrawBorder(sb);
            DrawMessages(sb);
        }
    }
}
