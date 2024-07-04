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

        private readonly GameConfiguration _config;
        
        private Dictionary<FigureType, Figure> _figureByType;

        public GameConfigurationFacade(GameConfiguration gameConfigs) 
        {
            _config = gameConfigs;
            FigureByType = _figureByType ??= gameConfigs.ChessConfiguration.Figures.ToDictionary(it => it.Type, it => it.Figure);
        }

        public Material GetMaterialByPlayerColor(PlayersColor color)
            => PlayersColor.White == color ? _config.ChessConfiguration.WhiteMaterial : _config.ChessConfiguration.BlackMaterial;
    }
}