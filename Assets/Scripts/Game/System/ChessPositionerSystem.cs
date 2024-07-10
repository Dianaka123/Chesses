using Assets.Scripts.Game.Entity;
using Assets.Scripts.Game.System.Interfaces;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Game.System
{
    public class ChessPositionerSystem : IChessPositioner
    {
        private Dictionary<Figure, Vector2Int> _initionalFigureToPositionDictionary = new Dictionary<Figure, Vector2Int>(16);

        private Figure[,] _figureArray = new Figure[8, 8];
        private Dictionary<Figure, Vector2Int> _figureToPositionDictionary;

        public event Action<Figure, Vector2Int> ForceChangeChessPosition;
        
        public Dictionary<Figure, Vector2Int> FigureToPositionDictionary {
            get
            {
                _figureToPositionDictionary ??= new Dictionary<Figure, Vector2Int>(_initionalFigureToPositionDictionary);
                return _figureToPositionDictionary;
            }
        }
        public Figure[,] Figures => _figureArray;

        public void InitiChessPosition(Figure figure, Vector2Int vector)
        {
            _figureArray[vector.x, vector.y] = figure;
            _initionalFigureToPositionDictionary.Add(figure, vector);
        }

        public void ChangeChessPosition(Figure figure, Vector2Int newPosition)
        {
            if (_figureToPositionDictionary.TryGetValue(figure, out var oldPosition))
            {
                _figureArray[oldPosition.x, oldPosition.y] = null;

                var enemyFigure = _figureArray[newPosition.x, newPosition.y];
                if (enemyFigure != null)
                {
                    _figureToPositionDictionary.Remove(enemyFigure);
                }

                _figureArray[newPosition.x, newPosition.y] = figure;
                _figureToPositionDictionary[figure] = newPosition;
            }
        }

        public void ResetAllFiguresPositions()
        {
            foreach (var figureAndPos in _initionalFigureToPositionDictionary)
            {
                var figure = figureAndPos.Key;
                var initCoordinate = figureAndPos.Value;
                
                if(!_figureToPositionDictionary.TryGetValue(figure, out var lastCordinate))
                {
                    _figureToPositionDictionary.Add(figure, initCoordinate);
                    _figureArray[initCoordinate.x, initCoordinate.y] = figure;
                }
                else
                {
                    _figureToPositionDictionary[figure] = initCoordinate;
                    if (lastCordinate != initCoordinate)
                    {
                        _figureArray[lastCordinate.x, lastCordinate.y] = null;
                        _figureArray[initCoordinate.x, initCoordinate.y] = figure;
                    }
                }

                ForceChangeChessPosition.Invoke(figure, initCoordinate);
            }
        }
    }
}
