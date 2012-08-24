using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using SpaceCowboy.Objects;
using SpaceCowboy.Objects.Weapons;
using WickedEngine;
using WickedEngine.Components;
using WickedEngine.Input;

namespace SpaceCowboy.GameScreens
{
    public class TestLevel : GameState
    {
        private FPSCounter FpsCounter;
        private SpriteFont fontArial;
        public static Texture2D ShipTexture;
        public static Texture2D LaserTexture;
        public static Texture2D Asteriod64;
        public static Texture2D Asteriod32;
        public static Texture2D StarTexture;
        public static Texture2D PlanetBlueTexture;
        public static Texture2D PlanetPurpleTexture;
        public static Texture2D CreatureTexture;
        public static Texture2D SpaceTexture;
        public static Texture2D LaserTurretTexture;
        public static Texture2D LaserFireTexture;
        private SpaceFighter Ship;
        public Random RNG;

        //TODO: move these statics out of the level and into a static Game class
        public static SceneGraph SceneGraph { get; private set; }

        public TestLevel(Game game, GameStateManager manager, SpriteBatch spriteBatch, Rectangle screenRectangle)
            : base(game, manager, spriteBatch, screenRectangle)
        {
            FpsCounter = new FPSCounter(game, manager, spriteBatch, screenRectangle, Vector2.Zero, Color.White, Color.Red, Color.Gray);
            ChildComponents.Add(FpsCounter);
        }

        protected override void LoadContent()
        {
            ContentManager Content = GameRef.Content;
            ShipTexture = Content.Load<Texture2D>(@"Sprites\block_64");
            LaserTexture = Content.Load<Texture2D>(@"Sprites\laser");
            Asteriod64 = Content.Load<Texture2D>(@"Sprites\asteriod_64");
            Asteriod32 = Content.Load<Texture2D>(@"Sprites\asteriod_32");
            StarTexture = Content.Load<Texture2D>(@"Sprites\Star_256");
            PlanetBlueTexture = Content.Load<Texture2D>(@"Sprites\Planet_Blue_128");
            PlanetPurpleTexture = Content.Load<Texture2D>(@"Sprites\Planet_Purple_128");
            CreatureTexture = Content.Load<Texture2D>(@"Sprites\Creature_32");
            LaserTurretTexture = Content.Load<Texture2D>(@"Sprites\Weapon_16");
            SpaceTexture = Content.Load<Texture2D>(@"Backgrounds\Nebula_1920X1200");
            fontArial = Content.Load<SpriteFont>(@"Fonts\Arial");

            LaserFireTexture = new Texture2D(GraphicsDevice, 1, 1, false, SurfaceFormat.Color);
            LaserFireTexture.SetData(new[] { Color.Yellow });

            FpsCounter.Font = fontArial;
            RNG = new Random(3); //TODO: Randomize seed

            SolarSystem solarSystem = new SolarSystem();
            solarSystem.Generate(RNG, 1000000);
            
            Camera2D camera = new Camera2D(ScreenRectangle);
            SceneGraph = new SceneGraph(camera, null);
            SceneGraph.Sky = SpaceTexture;

            foreach (GameObject obj in solarSystem.Stars)
            {
                SceneGraph.Add(obj);
            }
            foreach (GameObject obj in solarSystem.Planets)
            {
                SceneGraph.Add(obj);
            }
            foreach (GameObject obj in solarSystem.Asteroids)
            {
                SceneGraph.Add(obj);
            }

            Ship = GetShip();
            Ship.Position = Vector2.Zero;
            

            SceneGraph.Add(Ship);
            camera.CameraMode = CameraMode.Follow;
            camera.FollowTarget = Ship;

            SpaceCreature creature = new SpaceCreature();
            creature.Position = new Vector2(600f, 600f);
            creature.AI = new SimpleFollowAI();
            creature.Target = Ship;
            SceneGraph.Add(creature);

            //SpaceCreature creature2 = new SpaceCreature();
            //creature2.Position = new Vector2(400f, 300f);
            //creature2.AI = new SimpleFollowAI();
            //creature2.Target = Ship;
            //SceneGraph.Add(creature2);

            base.LoadContent();
        }

        public override void Update(GameTime gameTime)
        {
            Camera2D camera = SceneGraph.Camera as Camera2D;
            if (InputHelper.KeyDown(Keys.Q))
            {
                camera.Rotation += .1f;
            }
            else if (InputHelper.KeyDown(Keys.E))
            {
                camera.Rotation -= .1f;
            }
            else if (InputHelper.KeyPressed(Keys.PageUp))
            {
                camera.Zoom += .1f;
            }
            else if (InputHelper.KeyPressed(Keys.PageDown))
            {
                camera.Zoom -= .1f;
            }

            SceneGraph.Update();

            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            //SpriteBatch.Begin(
            //    SpriteSortMode.Deferred,
            //    BlendState.AlphaBlend,
            //    SamplerState.PointClamp,
            //    null,
            //    null,
            //    null,
            //    Matrix.Identity);

            //SceneGraph.DrawSky(SpriteBatch);

            //SpriteBatch.End();

            SpriteBatch.Begin(
                SpriteSortMode.Deferred,
                BlendState.AlphaBlend,
                SamplerState.PointClamp,
                null,
                null,
                null,
                SceneGraph.Camera.CreateTransformation());

            SceneGraph.Draw(SpriteBatch);

            SpriteBatch.End();

            base.Draw(gameTime);
        }

        private static SpaceFighter GetShip()
        {
            SpaceFighter character = new SpaceFighter();
            character.SceneGraph = SceneGraph;
            character.AI = new PlayerController();

            return character;
        }
    }
}
