using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;


/* Тут Амир должен запилить такие вещи: отображение выбранных юнитов, это для начала 
/  далее надо будет замутить отображение меню юнитов для постройки (когда например выбраны одни заводы)
/  отображение меню постройки строительных юнитов ( тоже самое по сути ) 
/  Само собой все это должно красиво выглядеть , быть обведенным в какую-нибудь рамочку, а если пойдет то и логотипчик можно какой-либо 
/  отображать
/  Хорошо бы также малевать миникарту, но у нас пока недостаточно наработано для этого .
*/ 
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

        public string Update(string arrOfUnitProp)
        {
            mouseState = Mouse.GetState();
            //На этом месте я подумал что можно было бы запросто отобразить вместо курсора МПХ. Даша, нарисуешь текстурку?. Смешно, не так ли?
            // Прошли примерно одни сутки.Мне уже не так весело. Теперь я понял что нихуя не запросто, как и вообще все в этом блядском фреймворке. 
            // Казалось бы , ебанный шарп, тут все уже написанно, а нихуя , все приходится пилить самому. Просто блядский язык для ебанных пидаров
            // Вдоволь наебавшись, я решил этот вопрос и по чуть-чуть остываю.
            // Новый день, новые задачи, сегодня я настроен более-менее оптимистически

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
                return  SelectUnits(arrOfUnitProp);

            }
            return null;
        }

        public void Draw()
        {
            
            if (isDrawable == false)
                return;

            // draw selecting rectangle; 4 line =  1 rectangle
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

        // Just drawing line 
        public void DrawLine(Vector2 begin, Vector2 end, int width = 1)
        {
            Rectangle r = new Rectangle((int)begin.X, (int)begin.Y, (int)(end - begin).Length() + width, width);
            Vector2 v = Vector2.Normalize(begin - end);
            float angle = (float)Math.Acos(Vector2.Dot(v, -Vector2.UnitX));
            if (begin.Y > end.Y)
                angle = MathHelper.TwoPi - angle;
            sprite.Draw(Pixel, r, null, Color.Red, angle, Vector2.Zero, SpriteEffects.None, 0);
        }


        private string SelectUnits(string arrOfUnitProp)
        {
            string SelectedUnits = string.Empty;
            int[] arr;
            Rectangle selectingRect = new Rectangle((int)firstLeftClickCoord.X, 
                                                    (int)firstLeftClickCoord.Y,
                                                    (int)(currentMousePos.X - firstLeftClickCoord.X),
                                                    (int)(currentMousePos.Y - firstLeftClickCoord.Y));
            foreach(string A in arrOfUnitProp.Split('\n'))
            {

                arr = A.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries)
                .Select(n => int.Parse(n))
                .ToArray();

                if (selectingRect.Contains(arr[1], arr[2]))
                {
                    //Add unit to collection of selected units;
                    SelectedUnits += arr.Last().ToString() + " ";
                }

            }

            return SelectedUnits;
        }
    }
}
