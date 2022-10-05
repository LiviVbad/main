﻿using AppFramework.Vision.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppFramework.Vision.Services
{
    public class ShapeService : IVisionMatchSerivce
    {
        public void ApplySettings(Settings settings)
        {

        }

        public void FindResult()
        {

        }

        public Settings QueryDefaultSettings()
        {
            return new ShapeSettings()
            {
                angleStart = -0.39f,
                angleExtent = 0.79f,
                angleStep = "auto",
                optimization = "auto",
                metric = "use_polarity",
                contrast = "auto",
                minContrast = "auto",
            };
        }
    }
}