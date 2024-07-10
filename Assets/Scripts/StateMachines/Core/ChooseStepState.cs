using Assets.Scripts.Game.Entity;
using Assets.Scripts.Game.Managers;
using Assets.Scripts.Game.System.Interfaces;
using Assets.Scripts.StateMachines.Base;
using Cysharp.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using UnityEngine;
using Zenject;

namespace Assets.Scripts.StateMachines.Core
{
    public class ChooseStepState : State
    {
        private readonly HighliterStep.Pool _pool;
        private readonly IPossibleStepsSystem _possibleStepsSystem;
        private readonly BoardManager _boardManager;
        private readonly GameManager _gameManager;
        private readonly CameraManager _camerManager;
        private readonly LazyInject<DoStepState> _doStepState;

        private List<HighliterStep> _steps = new List<HighliterStep>();
        private Camera _camera;
        private Figure _figure;

        public ChooseStepState(ChessSM chessSM, GameManager gameManager, HighliterStep.Pool pool, IPossibleStepsSystem possibleStepsSystem, BoardManager boardManager, CameraManager camerManager, LazyInject<DoStepState> doStepState) : base(chessSM)
        {
            _gameManager = gameManager;
            _pool = pool;
            _possibleStepsSystem = possibleStepsSystem;
            _boardManager = boardManager;
            _camerManager = camerManager;
            _doStepState = doStepState;
        }

        public override UniTask Enter(CancellationToken token)
        {
            _figure = _gameManager.SelectedFigure;
            _camera = _camerManager.CurrentCamera;

            var steps = _possibleStepsSystem.GetPossibleSteps(_figure, _boardManager.GetPositionByChess(_figure));
            var cellSize = _boardManager.GetCellSize();

            foreach (var step in steps)
            {
                var highlitedStep = _pool.Spawn(_boardManager.GetCellPositionByIndexes(step), _boardManager.GetCellSize());
                _steps.Add(highlitedStep);
            }
            return base.Enter(token);
        }

        public async override UniTask Run(CancellationToken token)
        {
            if (Input.GetMouseButtonDown(1))
            {
                Back(token);
            }

            if (Input.GetMouseButtonDown(0))
            {
                var ray = _camera.ScreenPointToRay(Input.mousePosition);
                RaycastHit raycastHit;
                if (Physics.Raycast(ray, out raycastHit))
                {
                    HighliterStep availableStep = _steps.FirstOrDefault(a => a.gameObject == raycastHit.collider.gameObject);
                    if(availableStep != null)
                    {
                        _gameManager.SelectedFigureStep = availableStep.transform.position;
                        await GoTo(_doStepState.Value, token);
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
                _pool.Despawn(step);
            }
            
            _steps.Clear();

            return base.Exit(token);
        }
    }
}