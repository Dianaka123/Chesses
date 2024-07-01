using Assets.Scripts.Game.Entity;
using Assets.Scripts.Game.Managers;
using Assets.Scripts.Game.Pool;
using Assets.Scripts.Game.System.Interfaces;
using Cysharp.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using UnityEngine;

namespace Assets.Scripts.StateMachines
{
    public class ChooseStepState : State
    {
        private readonly ChessSM _chessSM;
        private readonly Figure _selectedFigure;
        private readonly Camera _camera;

        private readonly IPossibleStepsSystem _possibleStepsSystem;
        private readonly GameManager _gameManager;
        private readonly BoardManager _boardManager;

        private List<GameObject> _steps = new List<GameObject>();

        public ChooseStepState(ChessSM chessSM, Figure figure, Camera camera)
        {
            _chessSM = chessSM;
            _selectedFigure = figure;
            _camera = camera;
        }

        public override UniTask Enter(CancellationToken token)
        {
            var steps = _possibleStepsSystem.GetPossibleSteps(_selectedFigure, _boardManager.GetPositionByChess(_selectedFigure));
            var cellSize = _boardManager.GetCellSize();

            foreach (var step in steps)
            {
                HighliterPool.Instance.Pool.Get(out var quad);
                quad.transform.position = _boardManager.GetCellPositionByIndexes(step);
                _steps.Add(quad);
            }
            return base.Enter(token);
        }

        public async override UniTask Run(CancellationToken token)
        {
            if (Input.GetMouseButtonDown(1))
            {
                _chessSM.Back(token);
            }

            if (Input.GetMouseButtonDown(0))
            {
                var ray = _camera.ScreenPointToRay(Input.mousePosition);
                RaycastHit raycastHit;
                if (Physics.Raycast(ray, out raycastHit))
                {
                    GameObject availableStep = _steps.FirstOrDefault(a => a == raycastHit.collider.gameObject);
                    if(availableStep != null)
                    {
                        _chessSM.GoTo(new DoStepState(_chessSM, _selectedFigure, availableStep), token);
                    }
                    else
                    {
                        // TODO: Play wrong sound?
                    }
                }
            }
        }

        public override UniTask Exit(CancellationToken token)
        {
            foreach (var step in _steps)
            {
                HighliterPool.Instance.Pool.Release(step);
            }

            return base.Exit(token);
        }
    }
}