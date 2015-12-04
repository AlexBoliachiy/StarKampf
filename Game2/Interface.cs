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
/  Хорошо бы также малевать миникарту, но у нас пока недостаточно наработано для этого.
/  
/  Также именно тут происходит выделение юнитов в подгрупы и отдача им приказов 
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
        public Camera2D camera;              //
        public GraphicsDevice graphics;      //

        private int side;
        private List<BaseUnit> selectedUnits;
        private KeyboardState keyboardState; // playing with camera


        private const int HealthInSquare = 200; //Count of health in one square 
        private const int WidthOfSquare = 15;
       // private const int DistanceToHealthLine = 60;
        private const int HeightOfSquare = 20;

        public Interface(GraphicsDevice graphics, int side)
        {
            mouseState = Mouse.GetState();// ini mouse state
            sprite = new SpriteBatch(graphics);

            Pixel = new Texture2D(graphics, 1, 1); // ini pixel for drawing line 
            Pixel.SetData( new[] { Color.White } ) ;
            this.graphics = graphics;
            camera = new Camera2D();

            this.side = side;
            selectedUnits = new List<BaseUnit>();
        }

        public string Update(List<BaseUnit> VecUnits)
        {
            mouseState = Mouse.GetState();

            keyboardState = Keyboard.GetState();

            if (keyboardState.IsKeyDown(Keys.A) && camera._pos.X > 640)    //camera movement
            {                                       //camera movement
                camera._pos += new Vector2(-10, 0); //camera movement
            }                                       //camera movement
            if (keyboardState.IsKeyDown(Keys.W) && camera._pos.Y > 370)    //camera movement
            {                                       //camera movement
                camera._pos += new Vector2(0, -10); //camera movement
            }                                       //camera movement
            if (keyboardState.IsKeyDown(Keys.S) && camera._pos.Y < 2408)    //camera movement
            {                                       //camera movement
                camera._pos += new Vector2(0, +10); //camera movement
            }                                       //camera movement
            if (keyboardState.IsKeyDown(Keys.D) && camera._pos.X < 4214)    //camera movement
            {                                       //camera movement
                camera._pos += new Vector2(10, 0);  //camera movement
            }                                       //camera movement

            // I thought thic code is extremely simple


            if (isClicded == false && mouseState.LeftButton == ButtonState.Pressed) // If clicked fisrt time
            {
                //Clean list of selecting unit becouse now will select new
                selectedUnits.Clear(); 
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
                SelectUnits(VecUnits);

            }
            else if (mouseState.RightButton == ButtonState.Pressed)
            {
                if (VecUnits.Count == 0)
                    return null;
                string commands = string.Empty;
                currentMousePos = Vector2.Transform(mouseState.Position.ToVector2(), Matrix.Invert(camera.GetTransformation(graphics)));
                /* for (int j=0; j < VecUnits.Count; i++)
                     {
                       Вот тут надо сделать такое.
                       1. Метод BaseUnit который возвращает его rect.
                       2. Проверку на то, не перекает ли currentMousePos rect каждого юнита в Vecunits 
                       3 Если есть хоть какой-то юнит, и он вражеский, формируем запрос на атаку, возвращаем строку, и там уже она отошлется
                     } */
                // Если никакого юнита там нет, формируем запрос на перемещение.

                for (int i=0; i < selectedUnits.Count(); i++)
                {
                    commands += "1" + " " + selectedUnits[i].GN.ToString() + " " +
                               ((int)currentMousePos.X).ToString() + " " + ((int)currentMousePos.Y).ToString() + " " + "\n";
                }
                if (commands == string.Empty)
                    return null;

                return commands;
            }
            return null;
        }

        public void Draw()
        {
            
            if (isDrawable == false)
                return;
            sprite.Begin(SpriteSortMode.BackToFront,
                       BlendState.AlphaBlend,
                       null,
                       null,
                       null,
                       null,
                       camera.GetTransformation(graphics));
            // draw selecting rectangle; 4 line =  1 rectangle
            DrawLine(new Vector2(firstLeftClickCoord.X, firstLeftClickCoord.Y), new Vector2(firstLeftClickCoord.X, currentMousePos.Y), Color.Red);
            DrawLine(new Vector2(firstLeftClickCoord.X, firstLeftClickCoord.Y), new Vector2(currentMousePos.X, firstLeftClickCoord.Y), Color.Red);
            DrawLine(new Vector2(currentMousePos.X, currentMousePos.Y), new Vector2(currentMousePos.X, firstLeftClickCoord.Y), Color.Red);
            DrawLine(new Vector2(currentMousePos.X, currentMousePos.Y), new Vector2( firstLeftClickCoord.X, currentMousePos.Y), Color.Red);
            sprite.End();
        }

        // Just drawing line 
        public void DrawLine(Vector2 begin, Vector2 end, Color color, int width = 2 )
        {
            Rectangle r = new Rectangle((int)begin.X, (int)begin.Y, (int)(end - begin).Length() + width, width);
            Vector2 v = Vector2.Normalize(begin - end);
            float angle = (float)Math.Acos(Vector2.Dot(v, -Vector2.UnitX));
            if (begin.Y > end.Y)
                angle = MathHelper.TwoPi - angle;
            sprite.Draw(Pixel, r, null, color, angle, Vector2.Zero, SpriteEffects.None, 0);
        }


        private  void SelectUnits(List<BaseUnit> VecUnits)
        {
            /*Creating rectangle, with coord that we get from first click mouse and current his location
            * Then we check intersection and if it's true -- 
            * Add this unit to selecting rects;
            */
            Rectangle selectingRect = new Rectangle((int)firstLeftClickCoord.X,
                                                    (int)firstLeftClickCoord.Y,
                                                    (int)(currentMousePos.X - firstLeftClickCoord.X),
                                                    (int)(currentMousePos.Y - firstLeftClickCoord.Y));
            if ( selectingRect.Height < 0)
            {
                selectingRect.Height = selectingRect.Height * (-1);
                selectingRect.Y -= selectingRect.Height;
            }
            if (selectingRect.Width < 0)
            {
                selectingRect.Width = selectingRect.Width * (-1);
                selectingRect.X -= selectingRect.Width;
            }

            for (int i=0; i < VecUnits.Count; i++)
            {
                if (VecUnits[i].side == side && selectingRect.Contains((int)VecUnits[i].X, (int)VecUnits[i].Y))
                {
                    selectedUnits.Add(VecUnits[i]);
                }

            }

        }

        public void DrawHealthUnderAllUnit(List<BaseUnit> VecUnits, Texture2D[] AllTextures)
        {
            foreach (BaseUnit A in VecUnits)
            {
                int DistanceToHealthLine = Math.Max(AllTextures[A.id].Width, AllTextures[A.id].Height);
                int CountOfHealthSquare =  A.MaxHealth / HealthInSquare;
                int widthOfLine = CountOfHealthSquare * WidthOfSquare;
                int leftX = (int)A.X - widthOfLine / 2; // Считаем координаты, откуда будет начинать рисоваться линия хп.
                int leftY = (int)A.Y - DistanceToHealthLine;
                sprite.Begin(SpriteSortMode.Deferred,
                       BlendState.AlphaBlend,
                       null,
                       null,
                       null,
                       null,
                       camera.GetTransformation(graphics));
                sprite.Draw(Pixel, new Rectangle(leftX, leftY, (int)((float)A.MaxHealth /(float)A.Health) * CountOfHealthSquare * WidthOfSquare, HeightOfSquare),
                                                Color.Tomato); //Рисуем заполнение линии хп
                DrawLine(new Vector2(leftX, leftY), new Vector2(leftX + widthOfLine, leftY), Color.Black);
                DrawLine(new Vector2(leftX, leftY + HeightOfSquare), new Vector2(leftX + widthOfLine, leftY + HeightOfSquare), Color.Black);

                for (int i = 0; i < CountOfHealthSquare + 1; i++)
                {

                    DrawLine(new Vector2(leftX+i*WidthOfSquare,leftY),new Vector2(leftX + i * WidthOfSquare, leftY+HeightOfSquare),Color.Black);

                }


                sprite.End();



            }
        }
    }
}
