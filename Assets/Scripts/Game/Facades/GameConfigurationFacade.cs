using Assets.Scripts.Game.Configuration;
using Assets.Scripts.Game.Entity;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.Scripts.Game.System
{
    public class GameConfigurationFacade
    {
        public IReadOnlyDictionary<FigureType, Figure> FigureByType { get; private set; }
        public BoardConfiguration BoardConfiguration => _config.BoardConfiguration;
        public Color HighlighterChessColor => _config.ChessConfiguration.HighliterColor;
        public FigureType[] FirstRowInitSequence => secondRowInitSequence ??= new FigureType[] { FigureType.Rook, FigureType.Knight, FigureType.Bishop, FigureType.King, FigureType.Queen, FigureType.Bishop, FigureType.Knight, FigureType.Rook };
        public FigureType SecondRowFigure => FigureType.Pawn;

        private readonly GameConfiguration _config;

        private Dictionary<FigureType, Figure> _figureByType;
        private FigureType[] secondRowInitSequence;

        public GameConfigurationFacade(GameConfiguration gameConfigs)
        {
            _config = gameConfigs;
            FigureByType = _figureByType ??= gameConfigs.ChessConfiguration.Figures.ToDictionary(it => it.Type, it => it.Figure);
        }

        public Material GetMaterialByPlayerColor(PlayersColor color)
            => PlayersColor.White == color ? _config.ChessConfiguration.WhiteMaterial : _config.ChessConfiguration.BlackMaterial;
    }
}