using Assets.Scripts.Game.Entity;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Game.Configuration
{
    [Serializable]
    public class GameConfiguration
    {
        public BoardConfiguration BoardConfiguration;
        public ChessConfiguration ChessConfiguration;
    }

    [Serializable]
    public class BoardConfiguration
    {
        public Board Board;
    }

    [Serializable]
    public class ChessConfiguration
    {
        public List<FigureDesc> Figures;

        public Material WhiteMaterial;
        public Material BlackMaterial;
        public Color HighliterColor;
    }

    [Serializable]
    public class FigureDesc
    {
        public Figure Figure;
        public FigureType Type;
    }
}
