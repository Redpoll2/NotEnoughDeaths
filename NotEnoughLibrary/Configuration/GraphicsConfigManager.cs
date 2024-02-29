// Copyright (c) 2020 PshenoDev. See the MIT license for full information

using NotEnoughLibrary.Graphics;

namespace NotEnoughLibrary.Configuration
{
    public class GraphicsConfigManager : IniConfigManager<GraphicsSettings>
    {
        public override string Filename => @"graphics.ini";

        public override void InitializeDefaults()
        {
            Set(GraphicsSettings.Width, Window.GetScreenWidth);
            Set(GraphicsSettings.Height, Window.GetScreenHeight);
            Set(GraphicsSettings.Fullscreen, true);
            Set(GraphicsSettings.VSync, true);
        }
    }

    public enum GraphicsSettings
    {
        Width,
        Height,
        Fullscreen,
        VSync,
    }
}
