using Assets.Scripts.Game.Entity;
using Assets.Scripts.Game.Managers;
using Cysharp.Threading.Tasks;
using System.Threading;
using UnityEngine;

namespace Assets.Scripts.StateMachines
{
    public class ChooseChessState : State
    {
        private readonly ChessSM _chessSM;
        private readonly Camera _camera;
        private readonly GameManager _gameManager;
        private readonly CameraManager _cameraManager;

        private Figure _highlightFigure;
        private Figure _selectedFigure;
        private PlayersColor _playerStep;

        public ChooseChessState(ChessSM sm)
        {
            _chessSM = sm;
            _camera = _cameraManager.CurrentCamera;
        }

        public override UniTask Enter(CancellationToken token)
        {
            _playerStep = _gameManager.PlayerStep;

            if (_selectedFigure != null)
            {
                _selectedFigure.Deselect().Forget();
            }

            return base.Enter(token);
        }

        public override async UniTask Run(CancellationToken token)
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
                    _selectedFigure = figure;
                    _selectedFigure.Select().Forget();
                    _chessSM.GoTo(new ChooseStepState(_chessSM, figure, _camera), token);
                }
            }
        }
    }
}