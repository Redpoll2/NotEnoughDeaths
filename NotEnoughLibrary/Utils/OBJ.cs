// Copyright (c) 2020 PshenoDev. See the MIT license for full information

using OpenTK;
using System.Collections.Generic;
using System.IO;

using NotEnoughLibrary.Graphics;
using NotEnoughLibrary.IO;

namespace NotEnoughLibrary.Utils
{
    public class OBJ
    {
        public static Mesh Parse(string filename)
        {
            List<float> vertices = new List<float>();
            List<float> texcoord = new List<float>();
            List<float> norcoord = new List<float>();
            List<uint> vertexIndices = new List<uint>();
            List<uint> texcoordIndices = new List<uint>();
            List<uint> norcoordIndices = new List<uint>();

            using (var reader = new StreamReader(Storage.GetStream(filename)))
            {
                string line = null;

                while ((line = reader.ReadLine()) != null)
                {
                    if (string.IsNullOrEmpty(line) || line.StartsWith("#"))
                    {
                        continue;
                    }

                    string[] parts = line.Split(' ');

                    switch (parts[0])
                    {
                        case "v":
                            foreach (string part in parts)
                            {
                                if (part != "v")
                                {
                                    vertices.Add(float.Parse(part.Replace('.', ',')));
                                }
                            }
                            break;

                        case "vt":
                            foreach (string part in parts)
                            {
                                if (part != "vt")
                                {
                                    texcoord.Add(float.Parse(part.Replace('.', ',')));
                                }
                            }
                            break;

                        case "vn":
                            foreach (string part in parts)
                            {
                                if (part != "vn")
                                {
                                    norcoord.Add(float.Parse(part.Replace('.', ',')));
                                }
                            }
                            break;

                        case "f":
                            int count = parts.Length - 1;
                            uint[] vertexIndex = new uint[count];
                            uint[] textureIndex = new uint[count];
                            uint[] normalIndex = new uint[count];

                            for (int i = 0; i < count; i++)
                            {
                                string[] elements = parts[i + 1].Split('/');

                                vertexIndex[i] = uint.Parse(elements[0]) - 1;
                                textureIndex[i] = uint.Parse(elements[0]) - 1;
                                normalIndex[i] = uint.Parse(elements[0]) - 1;
                            }

                            vertexIndices.AddRange(vertexIndex);
                            texcoordIndices.AddRange(textureIndex);
                            norcoordIndices.AddRange(normalIndex);
                            break;
                    }
                }

                return new Mesh(vertices.ToArray(), texcoord.ToArray(), norcoord.ToArray(), vertexIndices.ToArray(), texcoordIndices.ToArray(), norcoordIndices.ToArray());
            }
        }
    }
}
