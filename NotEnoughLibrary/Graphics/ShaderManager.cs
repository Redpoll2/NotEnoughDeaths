// Copyright (c) 2020 PshenoDev. See the MIT license for full information

using OpenTK.Graphics.OpenGL4;
using System;
using System.Collections.Generic;
using System.IO;

using NotEnoughLibrary.IO;
using OpenTK;

namespace NotEnoughLibrary.Graphics
{
    public class ShaderManager : IDisposable
    {
        private readonly List<Shader> shaders;
        private readonly Dictionary<string, int> uniforms;

        public int Handle { get; private set; }

        public ShaderManager(params string[] filenames)
        {
            shaders = new List<Shader>();
            uniforms = new Dictionary<string, int>();

            foreach (string filename in filenames)
            {
                Add(filename);
            }
        }

        public void Add(string filename)
        {
            var shader = new Shader(Storage.ReadToEnd(filename), filename.EndsWith(".vert") ? ShaderType.VertexShader : ShaderType.FragmentShader);

            if (!shader.TryCompile(out string log))
            {
                throw new Exception();
            }

            shaders.Add(shader);
        }

        public void Initialize()
        {
            Handle = GL.CreateProgram();

            foreach (var shader in shaders)
            {
                GL.AttachShader(Handle, shader.Handle);
            }

            GL.LinkProgram(Handle);

            foreach (var shader in shaders)
            {
                GL.DetachShader(Handle, shader.Handle);
                GL.DeleteShader(shader.Handle);
            }

            GL.GetProgram(Handle, GetProgramParameterName.ActiveUniforms, out int uniformCount);

            for (int i = 0; i < uniformCount; i++)
            {
                string key = GL.GetActiveUniform(Handle, i, out _, out _);

                int location = GL.GetUniformLocation(Handle, key);

                uniforms.Add(key, location);
            }
        }

        public void Use()
        {
            GL.UseProgram(Handle);
        }

        public void SetMatrix4(string name, Matrix4 data)
        {
            GL.UseProgram(Handle);

            if (uniforms.TryGetValue(name, out int location))
            {
                GL.UniformMatrix4(location, true, ref data);
            }
        }

        public void Dispose()
        {
            GL.DeleteProgram(Handle);
        }
    }
}
