using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading.Tasks;
using System.Diagnostics;
using SFML.Window;
using SFML.Graphics;
using System.Numerics;
using SFML.System;
using System.Reflection;
using System.ComponentModel.Design;
using System.Net.Mail;

public class KeysState
        {

            static public bool Shift_Pressed, Q_Pressed, D_Pressed, S_Pressed, Z_Pressed;
        }
        public class Game
        {
            public Game()
            {
                string path = "D:\\GitHub\\test-movement-sfml\\wargames.ttf";
        font = new SFML.Graphics.Font(path);
        System.Console.WriteLine( font.GetInfo().Family.ToString() );
                        CreateSFMLWindow();
                         mouse.FillColor = SFML.Graphics.Color.White;
                    cat.FillColor = SFML.Graphics.Color.Red;
                cat.Position = new SFML.System.Vector2f(renderWindow.GetView().Size.X / 2, renderWindow.GetView().Size.Y / 2);
            }
        SFML.Graphics.CircleShape mouse = new CircleShape(10);
        SFML.Graphics.CircleShape cat = new CircleShape(10);

        SFML.Graphics.CircleShape zone = new CircleShape(10);    

        SFML.System.Clock clock = new Clock();
        static Random random = new Random();
        static SFML.Graphics.Text catPosition = new SFML.Graphics.Text("",font);
        public static float getRandom(int N)
        {
            return   2*(float)N * random.NextSingle() - (float)N;
        }

        SFML.System.Vector2f catVelocity = new Vector2f( getRandom(5), getRandom(5));
        float Distance(Vector2f u,Vector2f v)
        {
            return (float) (Math.Pow( (double)(u.X - v.X), 2.0) + Math.Pow( (double)(u.Y - v.Y), 2.0) );
        }
        public void Loop()
        {
            catPosition.Position = new Vector2f(10, 10);
        cat.Origin = new Vector2f(-cat.Radius,-cat.Radius);
        zone.Origin = new Vector2f(-zone.Radius, -zone.Radius);
        mouse.Origin = new Vector2f(-mouse.Radius, -mouse.Radius);
        float maxD = 200;

        zone.Radius = maxD;
        zone.OutlineColor = new SFML.Graphics.Color(20, 20, 20);
        zone.FillColor =new SFML.Graphics.Color(0,0,0,0);
        zone.OutlineThickness = 5;
        float maxDistance =maxD*maxD;
        clock.Restart();
        while (renderWindow.IsOpen)
            {
            float deltaTime = 0.001f; // clock.ElapsedTime.AsMilliseconds();
                

                renderWindow.DispatchEvents();
                renderWindow.Clear();
                SFML.Graphics.View v = renderWindow.GetView();
                renderWindow.SetView(v);

                if (Distance(mouse.Position, cat.Position) < maxDistance)
                {
                    catVelocity = 0.4f * (mouse.Position - cat.Position);
                }
                else
                {
                    if ((cat.Position.X + 2*cat.Radius > renderWindow.GetView().Size.X) || (cat.Position.X * cat.Radius <= 0))
                    {
                        catVelocity.X = -catVelocity.X;
                    }
                    else if ((cat.Position.Y + 2 * cat.Radius > renderWindow.GetView().Size.Y) || (cat.Position.Y  <= 0))
                    {
                        catVelocity.Y = -catVelocity.Y;
                    }
                    else // if (deltaTime > 10)
                    {
                   
                            catVelocity.X += deltaTime * getRandom(100);
                            catVelocity.Y += deltaTime *  getRandom(100); 
                   
                    }
                }



            cat.Position += catVelocity * deltaTime;


            zone.Position = cat.Position - new Vector2f(zone.Radius,zone.Radius); 
              //  catPosition.DisplayedString = "cat velocity "+catVelocity.ToString();

                renderWindow.Draw(mouse);
                renderWindow.Draw(cat);
                renderWindow.Draw(catPosition);
            renderWindow.Draw(zone);
                renderWindow.Display();


            clock.Restart();

        }
        }

            
            public static SFML.Graphics.Font font;
            SFML.Graphics.RenderWindow renderWindow;
            SFML.Window.ContextSettings contextSettings;


            
            void DisplayText(string txt, int x, int y)
            {
                SFML.Graphics.Text text = new SFML.Graphics.Text(txt, font);
                text.Position = new SFML.System.Vector2f(x, y);
                renderWindow.Draw(text);
            }

            public void CreateSFMLWindow()
            {
                contextSettings = new ContextSettings(32, 32, 16);

                renderWindow = new SFML.Graphics.RenderWindow(new VideoMode(800, 600), "test", Styles.Titlebar | Styles.Resize | Styles.Close, contextSettings);
                renderWindow.SetVisible(true);
                renderWindow.SetVerticalSyncEnabled(true);
                renderWindow.SetTitle("GAME TEST");
                renderWindow.SetMouseCursorVisible(true);
                renderWindow.KeyPressed += OnKeyPressed;
                renderWindow.KeyReleased += OnKeyReleased;
                renderWindow.MouseButtonPressed += OnMouseButtonPressed;
                renderWindow.MouseMoved += OnMouseMove;
                renderWindow.MouseEntered += OnMouseEnters;
                renderWindow.MouseLeft += OnMouseLeaves;
                renderWindow.MouseWheelScrolled += onMouseWheelScrolled;


            }

            public void onMouseWheelScrolled(object sender, MouseWheelScrollEventArgs e)
            {
                SFML.Graphics.View v = renderWindow.GetView();

                if (e.Delta > 0)
                {
                    v.Zoom(1.01f);
                }
                else
                {
                    v.Zoom(0.99f);
                }
                renderWindow.SetView(v);
            }

            Stopwatch stopwatch = Stopwatch.StartNew();
            public void OnMouseEnters(object sender, EventArgs e)
            {
                mouseIn = true;
            }

            bool mouseIn = false;
            bool mouseIsPressed = false;

            public void OnMouseLeaves(object sender, EventArgs e)
            {
                mouseIn = false;
            }


            SFML.System.Vector2i mousePressedPosition;

            public void OnMouseMove(object sender, SFML.Window.MouseMoveEventArgs e)
            {
                    mouse.Position = new SFML.System.Vector2f(e.X , e.Y );
            }

            public void OnMouseButtonPressed(object sender, SFML.Window.MouseButtonEventArgs e)
            {
                if (!mouseIn)
                {
                    return;
                }
                mouseIsPressed = true;
                mousePressedPosition = new SFML.System.Vector2i(e.X, e.Y);

            }

            public bool ShiftPressed = false;
            public bool DPressed = false;
            public bool QPressed = false;
            public void OnKeyPressed(object sender, SFML.Window.KeyEventArgs e)
            {
                switch ((byte)e.Code)
                {
                    case (byte)Keyboard.Key.LShift:
                    case (byte)Keyboard.Key.RShift:
                        {
                            KeysState.Shift_Pressed = true;
                            return;
                        }
                    case (byte)Keyboard.Key.D:
                        {
                            KeysState.D_Pressed = true;
                            return;
                        }
                    case (byte)Keyboard.Key.Q:
                        {
                            KeysState.Q_Pressed = true;
                            return;
                        }
                    case (byte)Keyboard.Key.Z:
                        {
                            KeysState.Z_Pressed = true;
                            return;
                        }
                    case (byte)Keyboard.Key.S:
                        {
                            KeysState.S_Pressed = true;
                            return;
                        }
                }

            }

            public void OnKeyReleased(object sender, SFML.Window.KeyEventArgs e)
            {
                switch ((byte)e.Code)
                {
                    case (byte)Keyboard.Key.LShift:
                    case (byte)Keyboard.Key.RShift:
                        {
                            KeysState.Shift_Pressed = false;
                            return;
                        }
                    case (byte)Keyboard.Key.D:
                        {
                            KeysState.D_Pressed = false;
                            return;
                        }
                    case (byte)Keyboard.Key.Q:
                        {
                            KeysState.Q_Pressed = false;
                            return;
                        }
                    case (byte)Keyboard.Key.Z:
                        {
                            KeysState.Z_Pressed = false;
                            return;
                        }
                    case (byte)Keyboard.Key.S:
                        {
                            KeysState.S_Pressed = false;
                            return;
                        }
                }
            }

         

}

