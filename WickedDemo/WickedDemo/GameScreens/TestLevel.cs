using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

using TiledLib;
using WickedEngine;
using Microsoft.Xna.Framework.Input;
using WickedEngine.Input;
using WickedEngine.Components;

namespace WickedDemo.GameScreens
{
    public class TestLevel : GameState
    {
        private Character aCharacter;

        private Map map;
        private FPSCounter FpsCounter;
        private SpriteFont fontArial;

        //TODO: move these statics out of the level and into a static Game class
        public static SceneGraph SceneGraph { get; private set; }

        private static Texture2D CharacterTexture;
        private static Texture2D NPCTexture;
        private static Texture2D FireBallTexture;
        private static Texture2D SunTexture;
        private static Texture2D ExplosionTexture;

        //TODO: Add logic to use these lists to save memory
        private List<GameObject> FireBallList = new List<GameObject>();
        private List<GameObject> ExplosionList = new List<GameObject>();

        #region Constructor region

        public TestLevel(Game game, GameStateManager manager, SpriteBatch spriteBatch, Rectangle screenRectangle)
            : base(game, manager, spriteBatch, screenRectangle)
        {
            FpsCounter = new FPSCounter(game, manager, spriteBatch, screenRectangle, Vector2.Zero, Color.White, Color.Red, Color.Gray);
            ChildComponents.Add(FpsCounter);
        }

        #endregion

        #region XNA Method region

        protected override void LoadContent()
        {
            ContentManager Content = GameRef.Content;
            CharacterTexture = Content.Load<Texture2D>(@"Sprites\malepriest");
            NPCTexture = Content.Load<Texture2D>(@"Sprites\malefighter");
            SunTexture = Content.Load<Texture2D>(@"Sprites\sun_32");
            FireBallTexture = Content.Load<Texture2D>(@"Sprites\FireBall");
            ExplosionTexture = Content.Load<Texture2D>(@"Sprites\explosions");
            fontArial = Content.Load<SpriteFont>(@"Fonts\Arial");

            FpsCounter.Font = fontArial;

            aCharacter = GetCharacter();
            aCharacter.Position = new Vector2(70f, 50f);

            Sprite sprite = GetSun(); 
            aCharacter.AddChild(sprite);

            map = Content.Load<Map>("Map");

            Camera camera = new Camera(ScreenRectangle);
            camera.CameraMode = CameraMode.Follow;
            camera.FollowTarget = aCharacter;

            SceneGraph = new SceneGraph(camera, map);

            SceneGraph.Map = map;
            SceneGraph.Add(aCharacter);

            Character npc = GetNPC();
            npc.Position = new Vector2(512f, 512f);
            SceneGraph.Add(npc);

            base.LoadContent();
        }

        public override void Update(GameTime gameTime)
        {

            //if (InputHelper.KeyPressed(Keys.PageUp))
            //{
            //    SceneGraph.Camera.ZoomIn();
            //    if (SceneGraph.Camera.CameraMode == CameraMode.Follow)
            //    {
            //        SceneGraph.Camera.LockToObject(aCharacter);
            //    }
            //}
            //else if (InputHelper.KeyPressed(Keys.PageDown))
            //{
            //    SceneGraph.Camera.ZoomOut();
            //    if (SceneGraph.Camera.CameraMode == CameraMode.Follow)
            //    {
            //        SceneGraph.Camera.LockToObject(aCharacter);
            //    }
            //}

            if (InputHelper.KeyPressed(Keys.RightControl))
            {
                Fireball fireBall = GetFireBall();

                //Set direction and animation key based on where the player is facing
                fireBall.CurrentAnimation = aCharacter.CurrentAnimation;
                switch (aCharacter.CurrentAnimation)
                {
                    case AnimationKey.Down:
                        fireBall.Velocity = Constants.VelocityDown;
                        fireBall.Position = aCharacter.Position + new Vector2(0, 32);
                        break;
                    case AnimationKey.Left:
                        fireBall.Velocity = Constants.VelocityLeft;
                        fireBall.Position = aCharacter.Position + new Vector2(-32, 0);
                        break;
                    case AnimationKey.Right:
                        fireBall.Velocity = Constants.VelocityRight;
                        fireBall.Position = aCharacter.Position + new Vector2(32, 0);
                        break;
                    case AnimationKey.Up:
                        fireBall.Velocity = Constants.VelocityUp;
                        fireBall.Position = aCharacter.Position + new Vector2(0, -32);
                        break;
                }

                FireBallList.Add(fireBall);
                SceneGraph.Add(fireBall);
            }
            if (InputHelper.KeyPressed(Keys.E))
            {
                Explosion explosion = GetExplosion();
                explosion.Position = aCharacter.Position;
                ExplosionList.Add(explosion);
                SceneGraph.Add(explosion);
            }

            SceneGraph.Update();

            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
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

        public static Fireball GetFireBall()
        {
            Dictionary<AnimationKey, Animation> animations = new Dictionary<AnimationKey, Animation>();
            Animation animation = new Animation(3, 32, 32, 0, 0);
            animations.Add(AnimationKey.Down, animation);
            animation = new Animation(3, 32, 32, 0, 32);
            animations.Add(AnimationKey.Left, animation);
            animation = new Animation(3, 32, 32, 0, 64);
            animations.Add(AnimationKey.Right, animation);
            animation = new Animation(3, 32, 32, 0, 96);
            animations.Add(AnimationKey.Up, animation);
            Fireball fireBall = new Fireball(SceneGraph, FireBallTexture, animations);
            fireBall.SetCollisionSize(32, 32);

            return fireBall;
        }

        public static Sprite GetSun()
        {
            Sprite sprite = new Sprite(SunTexture);
            sprite.Position = new Vector2(-40f, -40f);
            sprite.SetDrawSize(32, 32);

            return sprite;
        }

        public static Character GetCharacter()
        {
            Dictionary<AnimationKey, Animation> animations = new Dictionary<AnimationKey, Animation>();
            Animation animation = new Animation(3, 32, 32, 0, 0);
            animations.Add(AnimationKey.Down, animation);
            animation = new Animation(3, 32, 32, 0, 32);
            animations.Add(AnimationKey.Left, animation);
            animation = new Animation(3, 32, 32, 0, 64);
            animations.Add(AnimationKey.Right, animation);
            animation = new Animation(3, 32, 32, 0, 96);
            animations.Add(AnimationKey.Up, animation);
            Character character = new Character(SceneGraph, CharacterTexture, animations);
            character.AI = new PlayerAI();
            character.SetCollisionSize(32, 32);

            return character;
        }

        public static Character GetNPC()
        {
            Dictionary<AnimationKey, Animation> animations = new Dictionary<AnimationKey, Animation>();
            Animation animation = new Animation(3, 32, 32, 0, 0);
            animations.Add(AnimationKey.Down, animation);
            animation = new Animation(3, 32, 32, 0, 32);
            animations.Add(AnimationKey.Left, animation);
            animation = new Animation(3, 32, 32, 0, 64);
            animations.Add(AnimationKey.Right, animation);
            animation = new Animation(3, 32, 32, 0, 96);
            animations.Add(AnimationKey.Up, animation);
            Character character = new Character(SceneGraph, NPCTexture, animations);
            character.AI = new IdleNpcAI();
            character.SetCollisionSize(32, 32);

            return character;
        }

        public static Explosion GetExplosion()
        {
            Dictionary<AnimationKey, Animation> animations = new Dictionary<AnimationKey, Animation>();
            Animation animation = new Animation(16, 64, 64, 0, 0);
            animation.FramesPerSecond = 20;
            animations.Add(AnimationKey.Down, animation);


            Explosion explosion = new Explosion(ExplosionTexture, animations);

            return explosion;
        }

        #endregion
    }
}