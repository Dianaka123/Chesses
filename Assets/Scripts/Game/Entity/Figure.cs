using Assets.Scripts.Game.Extensions;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using System;
using UnityEngine;
using UnityEngine.UIElements;

namespace Assets.Scripts.Game.Entity
{
    public class Figure : MonoBehaviour
    {
        public PlayersColor Color { get; private set; }
        public FigureType Type { get; private set; }

        [SerializeField] 
        private Renderer _renderer;
        [SerializeField]
        private float _yOffset = 0.05f;

        private float _defaultY;
        private Color _selectedColor;
        private Color _initColor;

        public void Init(Material material, Color selectedColor, PlayersColor playersType, FigureType type )
        {
            _renderer.material = material;
            _selectedColor = selectedColor;
            _initColor = material.color;
            Color = playersType;
            Type = type;
        }

        public void SetInitialPosition(Vector3 position)
        {
            transform.localPosition = position;

            _defaultY = transform.localPosition.y;
        }

        public UniTask MoveTo(Vector3 position)
        {
             transform.DOLocalMove(position, 1);
             return UniTask.Delay(1000);
        }

        public void ChangeColorBySelect(bool isSelected)
        {
            _renderer.material.color = isSelected ? UnityEngine.Color.Lerp(_initColor, _selectedColor, _selectedColor.a) : _initColor;
        }

        public async UniTask Deselect()
        {
            await transform.DOLocalMoveY(_defaultY, 1f);
        }

        public async UniTask Select()
        {
           await transform.DOLocalMoveY(_defaultY + _yOffset, 1f);
        }

        public async UniTask AttackAnimation()
        {
            await transform.DORotate(new Vector3(-30, 0, 0), 1f);
            await transform.DORotate(new Vector3(30, 0, 0), 1f);
            await transform.DORotate(new Vector3(0, 0, 0), 1f);
        }

        public async UniTask FallAnimation()
        {
            await transform.DORotate(new Vector3(90, 0, 0), 1f);
            await _renderer.DissolveAsync(1f);
        }

        public void ResetFigure(Vector3 resetPosition)
        {
            _renderer.ResetShaderToStandart();
            if(transform.rotation.x != 0)
            {
                transform.rotation = Quaternion.identity;
            }
            transform.localPosition = resetPosition;
        }
    }
}