﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using States;
using ScalingOctoNemesis.UI;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using ScalingOctoNemesis.UIComponents;
using Microsoft.Xna.Framework;

namespace ScalingOctoNemesis.States
{
    class InnerGame : GameState
    {
        InputField input;
        Button up;
        Button down;
        Button populate;
        ChatBox chat;
        ScrollBar scroll;
        SpriteFont _font;
        GameSlot[] _slots = new GameSlot[8];
        public InnerGame(StateManager manager)
            : base(manager)
        {
            for (int i = 0; i != _slots.Length; ++i)
                _slots[i] = new GameSlot(_font, new Vector2(10, i * 30 + 10), new Vector2(600, 25), new Vector2(5, 5));

            populate = new Button("Populate", "populate", new Vector2(100, 25), new Vector2(500, 50), new Vector2(5, 5), _font);
            populate.Action = delegate { Populate("Player"); };
            chat = new ChatBox("ChatBox", new Vector2(50, 450), new Vector2(600, 300), Vector2.Zero, _font);
            up = new Button("U", "U", 25, 25, 350, 350, 5, 5, _font);
            up.Action = chat.UpIndex;
            down = new Button("D", "D", 25, 25, 350, 400, 5, 5, _font);
            down.Action = chat.DownIndex;
            scroll = new ScrollBar("Scroll", 12, (int)(chat.Size.Y),
                                    (int)(chat.Position.X + chat.size.X + chat.Padding.X * 2),
                                    chat.Position.Y,
                                    chat.Padding.X, chat.Padding.Y, 
                                    up, down);
            input = new InputField("Test", _font, "input1", 12, 75, 375, 100, 30, 5, 5);
            input.AddKeyHandler(delegate(object o, KeyEventArgs args) {
                if (args.KeyCode == Keys.Enter)
                {
                    string val = input.Value;
                    input.Clear();
                    chat.AddMessage(val, "Prismik");
                    scroll.InnerLength = chat.InnerLength;
                }
            });

            input.OnFocus();
        }

        private void Populate(string name)
        {
            Player p = new Player(name, Color.Blue, "Yamato");
            foreach (GameSlot g in _slots)
                if (g.Available && g.Open)
                {
                    g.Populate(p);
                    break;
                }
        }

        public override void LoadContent()
        {
            base.LoadContent();
            _font = Manager.Game.Content.Load<SpriteFont>("monospace");
        }

        public override void Update(Microsoft.Xna.Framework.GameTime gameTime)
        {
            base.Update(gameTime);
            up.Update(gameTime);
            down.Update(gameTime);
            input.Update(gameTime);
            chat.Update(gameTime);
            populate.Update(gameTime);
        }

        public override void Draw()
        {
            Manager.SpriteBatch.Begin(SpriteSortMode.BackToFront, null);
            up.Draw(Manager.SpriteBatch);
            down.Draw(Manager.SpriteBatch);
            input.Draw(Manager.SpriteBatch);
            chat.Draw(Manager.SpriteBatch);
            populate.Draw(Manager.SpriteBatch);
            
            foreach (GameSlot g in _slots)
                g.Draw(Manager.SpriteBatch);
            Manager.SpriteBatch.End();
        }
    }
}
