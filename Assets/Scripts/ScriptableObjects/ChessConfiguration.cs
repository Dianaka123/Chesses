using Assets.Scripts.Game;
using Assets.Scripts.Game.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.Scripts.ScriptableObjects
{
    [Serializable]
    public class FigureDesc
    {
        public Figure Figure;
        public FigureType Type;
    }

    [CreateAssetMenu(fileName = "ChessConfiguration", menuName = "SO/ChessConfiguration")]
    public class ChessConfiguration : ScriptableObject
    {
        public Board Board;
        public List<FigureDesc> Figures;

        public Material whiteMaterial;
        public Material blackMaterial;
        public Color selectedColor;

        private Dictionary<FigureType, Figure> _figureByType;
        public IReadOnlyDictionary<FigureType, Figure> FigureByType => _figureByType ??= Figures.ToDictionary(it => it.Type, it => it.Figure);

        public Material GetMaterialByColor(PlayersColor color)
        {
            return PlayersColor.White == color ? whiteMaterial : blackMaterial;
        }
    }
}
