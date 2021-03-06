﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using SpaceDefender.Sprites;
using SpaceDefender.States;
using System.Runtime.InteropServices;
using SpaceDefender.Models;

namespace SpaceDefender
{
    public class Game1 : Game
    {
        public GraphicsDeviceManager Graphics { get; }
        SpriteBatch spriteBatch;

        public static Random Random;
        public int ScreenWidth { get; set; }
        public int ScreenHeight { get; set; }

        private State _currentState;
        private State _nextState;

        public static KeyboardState CurrentKey { get; set; }
        public static KeyboardState PreviousKey { get; set; }

        public Game1()
        {
            Graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

        }

        protected override void Initialize()
        {
            Random = new Random();

            ScreenWidth = 1280;
            ScreenHeight = 720;

            Graphics.PreferredBackBufferWidth = ScreenWidth;
            Graphics.PreferredBackBufferHeight = ScreenHeight;
            Graphics.ApplyChanges();

            IsMouseVisible = false;

            base.Initialize();
        }


        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);

            _currentState = new MenuState(this, Content);
            _currentState.LoadContent();
            _nextState = null;
        }


        protected override void UnloadContent() { }


        protected override void Update(GameTime gameTime)
        {
            PreviousKey = CurrentKey;
            CurrentKey = Keyboard.GetState();


            if (_currentState is MenuState)
            {
                if (Graphics.IsFullScreen)
                    Graphics.ToggleFullScreen();
                if (CurrentKey.IsKeyDown(Keys.Escape) && PreviousKey.IsKeyUp(Keys.Escape))
                    Exit();
            }

            if (CurrentKey.IsKeyDown(Keys.F11) && PreviousKey.IsKeyUp(Keys.F11) && !(_currentState is MenuState))
                Graphics.ToggleFullScreen();

            ScreenWidth = GraphicsDevice.Viewport.Bounds.Width;
            ScreenHeight = GraphicsDevice.Viewport.Bounds.Height;


            if (_nextState != null)
            {
                _currentState = _nextState;
                _currentState.LoadContent();

                _nextState = null;
            }

            _currentState.Update(gameTime);

            _currentState.PostUpdate(gameTime);

            base.Update(gameTime);
        }

        public void ChangeState(State state)
        {
            _nextState = state;
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(new Color(55, 55, 55));

            _currentState.Draw(gameTime, spriteBatch);

            if (!(_currentState is GameState))
            {
                spriteBatch.Begin();
                spriteBatch.Draw(Content.Load<Texture2D>("cursor"), new Vector2(Mouse.GetState().X, Mouse.GetState().Y), Color.White);
                spriteBatch.End();
            }

            base.Draw(gameTime);
        }
    }

}
