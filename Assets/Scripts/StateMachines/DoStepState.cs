using Assets.Scripts.Game;
using Assets.Scripts.Game.Entity;
using Assets.Scripts.Game.Managers;
using Assets.Scripts.StateMachines;
using Cysharp.Threading.Tasks;
using System.Threading;
using UnityEngine;

public class DoStepState : State
{
    private readonly ChessSM _sm;
    private readonly Figure _figure;
    private readonly GameObject _step;
    private readonly BoardManager _boardManager;
    private readonly GameManager _gameManager;

    public DoStepState(ChessSM sm, Figure figure, GameObject step)
    {
        _sm = sm;   
        _figure = figure;
        _step = step;
    }

    public async override UniTask Run(CancellationToken token)
    {
        var newPosition = _boardManager.GetIndxesByCellPosition(_step.transform.position);
        var enemyChess = _boardManager.GetChessByPosition(newPosition);

        _boardManager.ChangeChessPosition(_figure, newPosition);
        await _figure.MoveTo(_step.transform.position);

        if (enemyChess != null)
        {
            await enemyChess.FallAnimation();
        }

        if(enemyChess != null &&  enemyChess.Type == FigureType.King)
        {
            _sm.GoTo(new WinState(), token);
        }
        else
        {
            _figure.ChangeColorBySelect(false);

            _boardManager.IsShah(_figure.Color);

            _gameManager.StepComplited();

            _sm.GoTo(new ChooseChessState(_sm), token);
        }
    }
}
