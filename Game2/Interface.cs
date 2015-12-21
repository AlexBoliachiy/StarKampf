
﻿using System; using System.Collections.Generic; using System.Linq; using System.Text; using Microsoft.Xna.Framework; using Microsoft.Xna.Framework.Graphics; using Microsoft.Xna.Framework.Input; using System.Diagnostics;   namespace Game2 {     class Interface     {         private Vector2 firstLeftClickCoord; //         private Vector2 currentMousePos;     //         private MouseState mouseState;       // this is all for drawing  selecting rectangle         private KeyboardState keyboardState; //         private bool isClicded;              //         private bool isDrawable;             //         private SpriteBatch sprite;          //         public Texture2D Pixel;              //         public Camera2D camera;              //         public GraphicsDevice graphics;      //         public Map map;          private int screenWight = 1280;         private int screenHeight = 720;
        private bool Attack;          private int side;         private List<BaseUnit> selectedUnits;         private SpriteFont font;         private Texture2D[] allTextures;
        private Rectangle panel;
        private List<Rectangle> ClickableCreatUnits;
        private List<int> IdOfCreatUnits;



        private const int HealthInSquare = 200; //Count of health in one square          private const int WidthOfSquare = 10;         private const int HeightOfSquare = 10;
        private const int PanelHeight = 150 * 2 + 50;
        private const int MarginY = 20;
        private const int MarginX = 20;

        int screenLeftDownX;
        int screenLeftDownY;
        int fat = 4;
        private Stopwatch ClickTimer;

        private bool SelectOnlyBuilding = false;
        private bool SelectOnlyBuilder = false;

        private int BuilderID;
        private bool IsDrawBuilderId;


        public Interface(GraphicsDevice graphics, int side, Map map, SpriteFont font,Texture2D[] allTextures)         {             mouseState = Mouse.GetState();// ini mouse state             keyboardState = Keyboard.GetState();             sprite = new SpriteBatch(graphics);             this.map = map;              Pixel = new Texture2D(graphics, 1, 1); // ini pixel for drawing line              Pixel.SetData( new[] { Color.White } ) ;             this.graphics = graphics;             camera = new Camera2D();                          this.side = side;             selectedUnits = new List<BaseUnit>();              this.font = font;             ClickableCreatUnits = new List<Rectangle>();             IdOfCreatUnits = new List<int>();              screenLeftDownX = (int)camera.Pos.X - screenWight / 2;             screenLeftDownY = (int)camera.Pos.Y + screenHeight;             panel = new Rectangle(screenLeftDownX, screenLeftDownY - PanelHeight, screenWight, PanelHeight);             ClickTimer = new Stopwatch();             ClickTimer.Start();           }          public void Initialize(Texture2D[] allTextures)
        {
            this.allTextures = allTextures;
        }          public void Load()
        {
            
        }          public string Update(List<BaseUnit> VecUnits)         {
            mouseState = Mouse.GetState();
            ClickedOnBuildUnit();
            //Updating for drawing selectedunits on panel
            screenLeftDownX = (int)camera.Pos.X - screenWight / 2;             screenLeftDownY = (int)camera.Pos.Y + screenHeight;             panel = new Rectangle(screenLeftDownX, screenLeftDownY - PanelHeight, screenWight, PanelHeight);                           //TODO вынести в отдельный метод             keyboardState = Keyboard.GetState();             if (mouseState.Position.X < 50 && camera._pos.X > screenWight / 2)    //camera movement             {                                       //camera movement                 camera._pos += new Vector2(-10, 0); //camera movement             }                                       //camera movement             if (mouseState.Position.Y < 50 && camera._pos.Y > screenHeight / 2)    //camera movement             {                                       //camera movement                 camera._pos += new Vector2(0, -10); //camera movement             }                                       //camera movement             if (mouseState.Position.Y > 680 && camera._pos.Y < map.tileHeight * map.height - screenHeight / 2) //camera movement             {                                       //camera movement                 camera._pos += new Vector2(0, +10); //camera movement             }                                       //camera movement             if (mouseState.Position.X > 1230 && camera._pos.X < map.tileWidth * map.width - screenWight / 2)    //camera movement             {                                       //camera movement                 camera._pos += new Vector2(10, 0);  //camera movement             }                                       //camera movement              // I thought thic code is extremely simple                           if (isClicded == false && mouseState.LeftButton == ButtonState.Pressed &&
               !panel.Contains( Vector2.Transform(mouseState.Position.ToVector2(), Matrix.Invert(camera.GetTransformation(graphics))))) // If clicked fisrt time             {                 //Clean list of selecting unit becouse now will select new                 selectedUnits.Clear();                  //Transform pixel coords to window coords                 firstLeftClickCoord = Vector2.Transform(mouseState.Position.ToVector2(), Matrix.Invert(camera.GetTransformation(graphics)));                 isClicded = true;             }             else if (isClicded == true && mouseState.LeftButton == ButtonState.Pressed) // if moving mouse while selecting units              {                 currentMousePos = Vector2.Transform(mouseState.Position.ToVector2(), Matrix.Invert(camera.GetTransformation(graphics)));                 isDrawable = true;             }             else if (isClicded == true && mouseState.LeftButton == ButtonState.Released) // if have selected yet             {                 isClicded = false;                 isDrawable = false;                 SelectUnits(VecUnits);              }             else if (mouseState.RightButton == ButtonState.Pressed)             {
               currentMousePos = Vector2.Transform(mouseState.Position.ToVector2(), Matrix.Invert(camera.GetTransformation(graphics)));
                List<BaseUnit> fighters = new List<BaseUnit>();
                foreach (BaseUnit unit in VecUnits)
                {
                    if (unit is Fighter)
                    {
                        fighters.Add(unit);
                    }
                }
                foreach (Fighter unit in fighters)
                {
                    if (unit.unitBound.Contains(currentMousePos.X, currentMousePos.Y) && selectedUnits.Capacity != 0)
                    {
                        string msg = string.Empty;
                        List<BaseUnit> tmp = new List<BaseUnit>();
                        Attack = true;
                        foreach (BaseUnit fighter in selectedUnits)
                        {
                            if (fighter is Fighter)
                            {
                                tmp.Add(fighter);

                            }
                        }
                        foreach (Fighter fighter in tmp)
                        {
                            if (fighter != unit)
                            {
                                //fighter.setTarget(unit);
                                msg += "2 " + fighter.GN + " " + unit.GN;
                            }
                        }
                        return msg;

                    }
                }
                if (!Attack)
                {
                    if (VecUnits.Count == 0)
                        return null;

                    /* for (int j=0; j < VecUnits.Count; i++)                          {                            Вот тут надо сделать такое.                            1. Метод BaseUnit который возвращает его rect.                            2. Проверку на то, не перекает ли currentMousePos rect каждого юнита в Vecunits                             3 Если есть хоть какой-то юнит, и он вражеский, формируем запрос на атаку, возвращаем строку, и там уже она отошлется                          } */
                    // Если никакого юнита там нет, формируем запрос на перемещение
                    foreach (Fighter unit in fighters)
                    {
                        unit.isAttacking = false;
                    }
                    return PrepareRequestMoveUnit();
                }
                Attack = false;
            }
            return null;         }         private  string PrepareRequestMoveUnit()         {             string commands = String.Empty;              for (int i = 0; i < selectedUnits.Count(); i++)             {                  commands += "1" + " " + selectedUnits[i].GN.ToString() + " " +                            ((int)currentMousePos.X).ToString() + " " + ((int)currentMousePos.Y).ToString() + " " + side.ToString() + "\n";             }             if (commands == string.Empty) return null;             else return commands;         }          public void Draw()         {
           sprite.Begin(SpriteSortMode.Deferred ,
           BlendState.AlphaBlend,
           null,
           null,
           null,
           null,
           camera.GetTransformation(graphics));

            if (isDrawable == true)             {               // draw selecting rectangle; 4 line =  1 rectangle                 DrawLine(new Vector2(firstLeftClickCoord.X, firstLeftClickCoord.Y), new Vector2(firstLeftClickCoord.X, currentMousePos.Y), Color.Red);                 DrawLine(new Vector2(firstLeftClickCoord.X, firstLeftClickCoord.Y), new Vector2(currentMousePos.X, firstLeftClickCoord.Y), Color.Red);                 DrawLine(new Vector2(currentMousePos.X, currentMousePos.Y), new Vector2(currentMousePos.X, firstLeftClickCoord.Y), Color.Red);
                DrawLine(new Vector2(currentMousePos.X, currentMousePos.Y), new Vector2(firstLeftClickCoord.X, currentMousePos.Y), Color.Red);             }              if (IsDrawBuilderId)
            {
                Vector2 Pos = Vector2.Transform(mouseState.Position.ToVector2(), Matrix.
                        Invert(camera.GetTransformation(graphics)));
                sprite.Draw(allTextures[BuilderID], Pos, null);


            }              DrawPanel();
            DrawSelectedUnitsOnPanel();             sprite.End();         }          // Just drawing line          public void DrawLine(Vector2 begin, Vector2 end, Color color, int width = 2)         {             Rectangle r = new Rectangle((int)begin.X, (int)begin.Y, (int)(end - begin).Length() + width, width);             Vector2 v = Vector2.Normalize(begin - end);             float angle = (float)Math.Acos(Vector2.Dot(v, -Vector2.UnitX));             if (begin.Y > end.Y)                 angle = MathHelper.TwoPi - angle;             sprite.Draw(Pixel, r, null, color, angle, Vector2.Zero, SpriteEffects.None, 0);         }           private  void SelectUnits(List<BaseUnit> VecUnits)         {             /*Creating rectangle, with coord that we get from first click mouse and current his location             * Then we check intersection and if it's true --              * Add this unit to selecting rects;             */             Rectangle selectingRect = new Rectangle((int)firstLeftClickCoord.X,                                                     (int)firstLeftClickCoord.Y,                                                     (int)(currentMousePos.X - firstLeftClickCoord.X),                                                     (int)(currentMousePos.Y - firstLeftClickCoord.Y));             if ( selectingRect.Height < 0)             {                 selectingRect.Height = selectingRect.Height * (-1);                 selectingRect.Y -= selectingRect.Height;             }             if (selectingRect.Width < 0)             {                 selectingRect.Width = selectingRect.Width * (-1);                 selectingRect.X -= selectingRect.Width;             }              for (int i=0; i < VecUnits.Count; i++)             {                 if (VecUnits[i].side == side && selectingRect.Contains((int)VecUnits[i].X, (int)VecUnits[i].Y))                 {                     selectedUnits.Add(VecUnits[i]);                 }             }          }          public void DrawHealthUnderAllUnit(List<BaseUnit> VecUnits, Texture2D[] AllTextures)         {             foreach (BaseUnit A in VecUnits)             {                 int DistanceToHealthLine = Math.Max(AllTextures[A.id].Width, AllTextures[A.id].Height);                 int CountOfHealthSquare =  A.MaxHealth / HealthInSquare;                 int widthOfLine = CountOfHealthSquare * WidthOfSquare;                 int leftX = (int)A.X - widthOfLine / 2; // Считаем координаты, откуда будет начинать рисоваться линия хп.                 int leftY = (int)A.Y + DistanceToHealthLine/2;                 sprite.Begin(SpriteSortMode.Deferred,                        BlendState.AlphaBlend,                        null,                        null,                        null,                        null,                        camera.GetTransformation(graphics));                 sprite.Draw(Pixel, new Rectangle(leftX, leftY, (int) ( (((float)A.Health / (float)A.MaxHealth) * CountOfHealthSquare * WidthOfSquare)), HeightOfSquare),                                                 Color.Tomato); //Рисуем заполнение линии хп                 DrawLine(new Vector2(leftX, leftY), new Vector2(leftX + widthOfLine, leftY), Color.Black);                 DrawLine(new Vector2(leftX, leftY + HeightOfSquare), new Vector2(leftX + widthOfLine, leftY + HeightOfSquare), Color.Black);                  for (int i = 0; i < CountOfHealthSquare + 1; i++)                 {                     DrawLine(new Vector2(leftX+i*WidthOfSquare,leftY),new Vector2(leftX + i * WidthOfSquare, leftY+HeightOfSquare),Color.Black);                 }                   sprite.End();                }         }         private void DrawPanel()
        {
            
            DrawRectangle(panel, Color.AliceBlue);
            //Рамку рисуем
            DrawLine(new Vector2(screenLeftDownX, screenLeftDownY), new Vector2(screenLeftDownX + screenWight, screenLeftDownY), Color.Blue, fat);
            DrawLine(new Vector2(screenLeftDownX + screenWight, screenLeftDownY - PanelHeight), new Vector2(screenLeftDownX + screenWight, screenLeftDownY), Color.Blue,fat);
            DrawLine(new Vector2(screenLeftDownX, screenLeftDownY - PanelHeight), new Vector2(screenLeftDownX + screenWight, screenLeftDownY - PanelHeight), Color.Blue,fat);
            DrawLine(new Vector2(screenLeftDownX, screenLeftDownY), new Vector2(screenLeftDownX, screenLeftDownY - PanelHeight), Color.Blue,fat);
        }         private void DrawRectangle(Rectangle coords, Color color)
        {
            var rect = new Texture2D(graphics, 1, 1);
            rect.SetData(new[] { color });
            sprite.Draw(rect, coords, color);
        }          private void DrawSelectedUnitsOnPanel()
        {
            //Подсчитываем количество выделенных юнитов всех типов 
            ClickableCreatUnits.Clear();
            IdOfCreatUnits.Clear();
            Dictionary<int, int> count = new Dictionary<int, int>();
            for (int i=0; i < selectedUnits.Count; i++)
            {
                if (!count.ContainsKey(selectedUnits[i].id))
                {
                    count.Add(selectedUnits[i].id, 1);
                }
                else
                {
                    count[selectedUnits[i].id]++;
                }

            }
            if (count.Count == 1 && count.First().Key >= 10 && count.First().Key < 20) /// если выделено одно здание
            {
                DrawMenuBuild(count.First().Key);
                SelectOnlyBuilding = true;
                return;
            }
            else if (count.Count == 1 && count.First().Key == 100)
            {
                DrawMenuBuilder(count.First().Key);
                SelectOnlyBuilder = true;
                return;
            }
            else
            {
                SelectOnlyBuilding = false;
                SelectOnlyBuilder = false;
            }
            // выводим их на экран
            if (count.Count == 0) // Тафтология в коде
                return;

           
            int Margin = 0;
            foreach (var A in count)
            {



                IdOfCreatUnits.Add(A.Key);

                sprite.Draw(allTextures[A.Key], new Vector2(screenLeftDownX + Margin + MarginX,MarginY +  screenLeftDownY - PanelHeight));
                sprite.DrawString(font, A.Value.ToString(), new Vector2(screenLeftDownX + Margin + MarginX + allTextures[A.Key].Width/2, 
                                  MarginY- PanelHeight + 10 + screenLeftDownY+allTextures[A.Key].Height),
                                  Color.Black);

                Margin += allTextures[A.Key].Width + 20;

            }
        
            
        }         private void DrawMenuBuild(int id)
        {
            int Margin = 0;
            foreach ( var A in Building.CreatingUnits[id])
            {
                IdOfCreatUnits.Add(A.Key);
                ClickableCreatUnits.Add(new Rectangle(new Point(screenLeftDownX + Margin + MarginX,
                                 MarginY + screenLeftDownY - PanelHeight), new Point(allTextures[A.Key].Width,
                                  allTextures[A.Key].Height))); // Добавляем все юниты которые могут быть созданы тем зданием или билдером
                sprite.Draw(allTextures[A.Key], new Vector2(screenLeftDownX + Margin + MarginX, MarginY + screenLeftDownY - PanelHeight));
                Margin += allTextures[A.Key].Width + 20;
            }
        }         private void DrawMenuBuilder(int id)
        {

            int Margin = 0;
            foreach (var A in Support.CreatingUnits[id])
            {
                IdOfCreatUnits.Add(A.Key);
                ClickableCreatUnits.Add(new Rectangle(new Point(screenLeftDownX + Margin + MarginX,
                                 MarginY + screenLeftDownY - PanelHeight), new Point(allTextures[A.Key].Width,
                                  allTextures[A.Key].Height))); // Добавляем все юниты которые могут быть созданы тем зданием или билдером
                sprite.Draw(allTextures[A.Key], new Vector2(screenLeftDownX + Margin + MarginX, MarginY + screenLeftDownY - PanelHeight));
                Margin += allTextures[A.Key].Width + 20;
            }
        }                  private void ClickedOnBuildUnit()
        {
            if (SelectOnlyBuilding)
            {
                int i = 0; 
                foreach (var A in ClickableCreatUnits)
                {
                    if (ClickTimer.ElapsedMilliseconds > 200 && mouseState.LeftButton == ButtonState.Pressed &&
                        A.Contains(Vector2.Transform(mouseState.Position.ToVector2(), Matrix.
                        Invert(camera.GetTransformation(graphics)))))
                    {
                        ClickTimer.Restart();
                        Building building = selectedUnits.First() as Building;
                        building.SetBuild(IdOfCreatUnits[i]);
                        Console.WriteLine("ini unit with id {0}", IdOfCreatUnits[i]);
                        return;
                        
                    }
                    i++;

                }
            }
            else if (SelectOnlyBuilder)
            {
                int i = 0;
                foreach (var A in ClickableCreatUnits)
                {
                    if (ClickTimer.ElapsedMilliseconds > 200 && mouseState.LeftButton == ButtonState.Pressed &&
                        A.Contains(Vector2.Transform(mouseState.Position.ToVector2(), Matrix.
                        Invert(camera.GetTransformation(graphics)))))
                    {
                        ClickTimer.Restart();
                        Support building = selectedUnits.First() as Support;
                        //  building.SetBuild(IdOfCreatUnits[i]);
                        BuilderID = IdOfCreatUnits[i];
                        IsDrawBuilderId = true;
                        Console.WriteLine("ini unit with id {0}", IdOfCreatUnits[i]);
                        return;

                    }
                    i++;

                }

                if (ClickTimer.ElapsedMilliseconds > 200 && mouseState.LeftButton == ButtonState.Pressed && IsDrawBuilderId)
                       
                {
                    var pos = Vector2.Transform(mouseState.Position.ToVector2(), Matrix.
                        Invert(camera.GetTransformation(graphics)));
                    ClickTimer.Restart();
                    Support building = selectedUnits.First() as Support;
                    building.SetBuild(BuilderID,(int) pos.X, (int)pos.Y);
                    IsDrawBuilderId = false;
                    Console.WriteLine("ini unit with id {0}", BuilderID);
                    return;

                }
            }
        }     }       } 
