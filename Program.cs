﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace vk_youtube_kyrsach
{
    static class Program
    {
        /// <summary>
        /// Главная точка входа для приложения.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form2(new List<info>()));
            Application.Run(new Form1());
        }
    }
}
