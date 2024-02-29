// Copyright (c) 2020 PshenoDev. See the MIT license for full information

using System;
using System.Diagnostics;
using System.Windows;

using NotEnoughLibrary.Logging;

using Window = NotEnoughLibrary.Graphics.Window;

namespace NotEnoughLibrary
{
    /// <summary>
    /// Абстрактный класс игры, работающий с движком.
    /// </summary>
    public abstract class Game : IDisposable
    {
        public Window Window { get; private set; }
        public Logger Logger { get; private set; }

        public virtual string Title { get; } = "NotEnoughLibrary";
        public virtual string Author { get; } = "PshenoDev";

        /// <summary>
        /// Является ли данный экземляр первостепенным?
        /// </summary>
        public bool IsPrimaryInstance => Process.GetProcessesByName(Title).Length == 1;

        public Game()
        {
            InitializeClasses();
        }

        public void ShowWarning(string text)
        {
            MessageBox.Show(text, Title, MessageBoxButton.OK, MessageBoxImage.Warning);
        }

        /// <summary>
        /// Инициализирует все поля.
        /// </summary>
        public void InitializeClasses()
        {
            Window = new Window(Title);
            Logger = new Logger();
        }

        public void Run()
        {
            Window.Run(60.0);
        }

        public void Dispose()
        {
            Window.Dispose();
            Logger.Dispose();
        }
    }
}
