using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Game2
{
    class Interface
    {

        private Vector2 firstLeftClickCoord; //
        private Vector2 currentMousePos;     //
        private MouseState mouseState;       // this is all for drawing  selecting rectangle
        private bool isClicded;              //
        private bool isDrawable;             //
        private SpriteBatch sprite;          //
        public Texture2D Pixel;              //
        public Camera2D camera;
        public GraphicsDevice graphics;
        public Interface(GraphicsDevice graphics)
        {
            mouseState = Mouse.GetState();// ini mouse state
            sprite = new SpriteBatch(graphics);

            Pixel = new Texture2D(graphics, 1, 1); // ini pixel for drawing line 
            Pixel.SetData( new[] { Color.White } ) ;
            this.graphics = graphics;
            camera = new Camera2D(); 
        }

        public void Update()
        {
            mouseState = Mouse.GetState();
            //На этом месте я подумал что можно было бы запросто отобразить вместо курсора МПХ. Даша, нарисуешь текстурку?. Смешно, не так ли?
            // Прошли примерно одни сутки.Мне уже не так весело. Теперь я понял что нихуя не запросто, как и вообще все в этом блядском фреймворке. 
            // Казалось бы , ебанный шарп, тут все уже написанно, а нихуя , все приходится пилить самому. Просто блядский язык для ебанных пидаров
            // Вдоволь наебавшись, я решил этот вопрос и по чуть-чуть остываю.
            // I thought thic code is extremely simple

            if (isClicded == false && mouseState.LeftButton == ButtonState.Pressed) // If clicked fisrt time
            {
                //Transform pixel coords to window coords
                firstLeftClickCoord = Vector2.Transform(mouseState.Position.ToVector2(), Matrix.Invert(camera.GetTransformation(graphics)));
                isClicded = true;
            }
            else if (isClicded == true && mouseState.LeftButton == ButtonState.Pressed) // if moving mouse while selecting units 
            {
                currentMousePos = Vector2.Transform(mouseState.Position.ToVector2(), Matrix.Invert(camera.GetTransformation(graphics)));
                isDrawable = true;
            }
            else if (isClicded == true && mouseState.LeftButton == ButtonState.Released) // if have selected yet
            {
                isClicded = false;
                isDrawable = false;
            }


        }
        public void Draw()
        {
            
            if (isDrawable == false)
                return;

            // draw selecting rectangle; 
            sprite.Begin(SpriteSortMode.BackToFront,
                       BlendState.AlphaBlend,
                       null,
                       null,
                       null,
                       null,
                       this.camera.GetTransformation(graphics));
            DrawLine(new Vector2(firstLeftClickCoord.X, firstLeftClickCoord.Y), new Vector2(firstLeftClickCoord.X, currentMousePos.Y));
            DrawLine(new Vector2(firstLeftClickCoord.X, firstLeftClickCoord.Y), new Vector2(currentMousePos.X, firstLeftClickCoord.Y));
            DrawLine(new Vector2(currentMousePos.X, currentMousePos.Y), new Vector2(currentMousePos.X, firstLeftClickCoord.Y));
            DrawLine(new Vector2(currentMousePos.X, currentMousePos.Y), new Vector2( firstLeftClickCoord.X, currentMousePos.Y));
            sprite.End();

        }

        public void DrawLine(Vector2 begin, Vector2 end, int width = 1)
        {
            Rectangle r = new Rectangle((int)begin.X, (int)begin.Y, (int)(end - begin).Length() + width, width);
            Vector2 v = Vector2.Normalize(begin - end);
            float angle = (float)Math.Acos(Vector2.Dot(v, -Vector2.UnitX));
            if (begin.Y > end.Y)
                angle = MathHelper.TwoPi - angle;
            sprite.Draw(Pixel, r, null, Color.Red, angle, Vector2.Zero, SpriteEffects.None, 0);
        }

    }
}
