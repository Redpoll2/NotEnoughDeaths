// Copyright (c) 2020 PshenoDev. See the MIT license for full information

using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Input;
using System;
using System.Windows;

using NotEnoughLibrary.Configuration;
using NotEnoughLibrary.Utils;

using WindowState = OpenTK.WindowState;

namespace NotEnoughLibrary.Graphics
{
    public class Window : GameWindow, IDisposable
    {
        private int vertexBufferID;
        private int vertexArrayID;

        private int elementBufferID;

        private Matrix4 view;
        private Matrix4 projection;

        private readonly Mesh devCube;

        public GraphicsConfigManager GraphicsConfiguration { get; private set; }
        public ShaderManager ShaderManager { get; private set; }

        public static double GetScreenWidth => SystemParameters.PrimaryScreenWidth;
        public static double GetScreenHeight => SystemParameters.PrimaryScreenHeight;

        public double Ticks { get; private set; }

        public Window(string title = "NotEnoughLibrary") : base(640, 720, GraphicsMode.Default, title, GameWindowFlags.FixedWindow)
        {
            InitializeDefaults();
            devCube = OBJ.Parse("dev_primitive_cube.obj");
            X = ((int)GetScreenWidth - Width) / 2;
            Y = ((int)GetScreenHeight - Height) / 2;
        }

        public void InitializeDefaults()
        {
            ShaderManager = new ShaderManager("shader.vert", "shader.frag");

            GraphicsConfiguration = new GraphicsConfigManager();
            GraphicsConfiguration.Load();

            Width = int.Parse(GraphicsConfiguration[GraphicsSettings.Width]);
            Height = int.Parse(GraphicsConfiguration[GraphicsSettings.Height]);
            WindowState = GraphicsConfiguration[GraphicsSettings.Fullscreen] == "True" ? WindowState.Fullscreen : WindowState.Normal;
            VSync = GraphicsConfiguration[GraphicsSettings.Fullscreen] == "True" ? VSyncMode.On : VSyncMode.Off;
        }

        protected override void OnLoad(EventArgs e)
        {
            GL.ClearColor(0.0f, 0.0f, 0.0f, 1.0f);
            GL.Enable(EnableCap.DepthTest);

            vertexBufferID = GL.GenBuffer();

            GL.BindBuffer(BufferTarget.ArrayBuffer, vertexBufferID);
            GL.BufferData(BufferTarget.ArrayBuffer, devCube.Vertices.Length * sizeof(float), devCube.Vertices, BufferUsageHint.StaticDraw);

            elementBufferID = GL.GenBuffer();

            GL.BindBuffer(BufferTarget.ElementArrayBuffer, elementBufferID);
            GL.BufferData(BufferTarget.ElementArrayBuffer, devCube.VertexIndices.Length * sizeof(uint), devCube.VertexIndices, BufferUsageHint.StaticDraw);

            ShaderManager.Initialize();
            ShaderManager.Use();

            vertexArrayID = GL.GenVertexArray();

            GL.BindVertexArray(vertexArrayID);

            GL.BindBuffer(BufferTarget.ArrayBuffer, vertexBufferID);
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, elementBufferID);

            GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 3 * sizeof(float), 0);
            GL.EnableVertexAttribArray(0);

            view = Matrix4.CreateTranslation(0.0f, 0.0f, -3.0f);

            projection = Matrix4.CreatePerspectiveFieldOfView(MathHelper.DegreesToRadians(45f), (float)Width / (float)Height, 0.1f, 100.0f);

            base.OnLoad(e);
        }

        protected override void OnResize(EventArgs e)
        {
            GL.Viewport(ClientRectangle.X, ClientRectangle.Y, Width, Height);

            base.OnResize(e);
        }

        protected override void OnRenderFrame(FrameEventArgs e)
        {
            Ticks += 8 * e.Time;

            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

            ShaderManager.Use();

            GL.BindVertexArray(vertexArrayID);

            Matrix4 model = Matrix4.Identity * Matrix4.CreateRotationX((float)MathHelper.DegreesToRadians(Ticks));

            ShaderManager.SetMatrix4("model", model);
            ShaderManager.SetMatrix4("view", view);
            ShaderManager.SetMatrix4("projection", projection);

            GL.DrawElements(PrimitiveType.Triangles, devCube.VertexIndices.Length, DrawElementsType.UnsignedInt, 0);   // рисовать свои модели

            SwapBuffers();
            base.OnRenderFrame(e);
        }

        protected override void OnUpdateFrame(FrameEventArgs e)
        {
            KeyboardState state = Keyboard.GetState();

            if (state.IsKeyDown(Key.Escape))
            {
                Exit();
            }

            if (state.IsKeyDown(Key.F8))
            {
                throw new Exception();
            }

            if (state.IsKeyDown(Key.L))
            {
                GL.PolygonMode(MaterialFace.FrontAndBack, PolygonMode.Line);
            }

            if (state.IsKeyDown(Key.F))
            {
                GL.PolygonMode(MaterialFace.FrontAndBack, PolygonMode.Fill);
            }

            base.OnUpdateFrame(e);
        }

        protected override void OnUnload(EventArgs e)
        {
            GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
            GL.BindVertexArray(0);
            GL.UseProgram(0);

            GL.DeleteBuffer(vertexBufferID);
            GL.DeleteBuffer(elementBufferID);
            GL.DeleteVertexArray(vertexArrayID);

            ShaderManager.Dispose();

            base.OnUnload(e);
        }

        protected override void Dispose(bool manual)
        {
            GraphicsConfiguration.Dispose();

            base.Dispose(manual);
        }
    }
}
