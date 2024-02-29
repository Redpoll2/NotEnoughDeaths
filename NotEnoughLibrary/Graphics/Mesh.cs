// Copyright (c) 2020 PshenoDev. See the MIT license for full information

namespace NotEnoughLibrary.Graphics
{
    public class Mesh
    {
        public float[] Vertices { get; set; }
        public float[] TexCoords { get; set; }
        public float[] NorCoords { get; set; }
        public uint[] VertexIndices { get; set; }
        public uint[] TextureIndices { get; set; }
        public uint[] NormalIndices { get; set; }

        public Mesh(float[] vertices, float[] texcoord, float[] norcoord, uint[] vertexIndices, uint[] textureIndices, uint[] normalIndices)
        {
            Vertices = vertices;
            TexCoords = texcoord;
            NorCoords = norcoord;
            VertexIndices = vertexIndices;
            TextureIndices = textureIndices;
            NormalIndices = normalIndices;
        }
    }
}
