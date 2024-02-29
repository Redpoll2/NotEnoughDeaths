// Copyright (c) 2020 PshenoDev. See the MIT license for full information

using OpenTK.Graphics.OpenGL4;

namespace NotEnoughLibrary.Graphics
{
    public struct Shader
    {
        public int Handle { get; }

        public Shader(string code, ShaderType type)
        {
            Handle = GL.CreateShader(type);
            GL.ShaderSource(Handle, code);
        }

        public void Compile()
        {
            GL.CompileShader(Handle);
        }

        public bool TryCompile(out string log)
        {
            Compile();

            log = GL.GetShaderInfoLog(Handle);

            if (string.IsNullOrEmpty(log))
            {
                return true;
            }

            return false;
        }
    }
}
