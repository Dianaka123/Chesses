using System.Threading;
using UnityEngine;

namespace Assets.Scripts.StateMachines
{
    public class ChessClient : MonoBehaviour
    {
        private ChessSM chess;
        private CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();

                                                           
        private void Start()
        {
            chess = GetComponent<ChessSM>();
        }

        private void Update()
        {
            chess.Run(cancellationTokenSource.Token);
        }
    }
}