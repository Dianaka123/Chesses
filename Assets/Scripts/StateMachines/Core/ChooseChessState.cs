using Assets.Scripts.Game.Entity;
using Assets.Scripts.Game.Managers;
using Assets.Scripts.StateMachines.Base;
using Cysharp.Threading.Tasks;
using System.Threading;
using UnityEngine;
using Zenject;

namespace Assets.Scripts.StateMachines.Core
{
    public class ChooseChessState : State
    {
        private readonly GameManager _gameManager;
        private readonly LazyInject<ChooseStepState> _chooseStepState;

        private Figure _highlightFigure;
        private PlayersColor _playerStep;
        private Camera _camera;

        public ChooseChessState(ChessSM chessSM, GameManager gameManager, CameraManager cameraManager, LazyInject<ChooseStepState> chooseStepState) : base(chessSM)
        {
            _gameManager = gameManager;
            _camera = cameraManager.CurrentCamera;
            _chooseStepState = chooseStepState;
        }

        public override UniTask Enter(CancellationToken token)
        {
            _playerStep = _gameManager.PlayerStep;

            if (_gameManager.SelectedFigure != null)
            {
                _gameManager.SelectedFigure.Deselect().Forget();
            }

            return base.Enter(token);
        }

        public override UniTask Run(CancellationToken token)
        {
            Ray ray = _camera.ScreenPointToRay(Input.mousePosition);
            RaycastHit raycastHit;
            if (Physics.Raycast(ray, out raycastHit))
            {
                Figure figure = raycastHit.collider.gameObject.GetComponent<Figure>();

                if (figure != null && figure.Color == _playerStep)
                {
                    if (figure != _highlightFigure && _highlightFigure != null)
                    {
                        _highlightFigure.ChangeColorBySelect(false);

                    }
                    figure.ChangeColorBySelect(true);
                    _highlightFigure = figure;
                }
            }
            else
            {
                _highlightFigure?.ChangeColorBySelect(false);
                _highlightFigure = null;
            }

            if (Input.GetMouseButtonDown(0))
            {
                var figure = raycastHit.collider.gameObject?.GetComponent<Figure>();
                if (figure != null && figure.Color == _playerStep)
                {
                    _gameManager.SelectedFigure = figure;
                    _gameManager.SelectedFigure.Select().Forget();

                    GoTo(_chooseStepState.Value, token);
                }
            }

            return UniTask.CompletedTask;
        }
    }
}