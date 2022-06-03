using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Ape.Minesweeper
{
    public enum TileState
    {
        Hidden,
        Open,
        Flag
    }

    public class Tile : MonoBehaviour, IPointerClickHandler
    {
        [SerializeField]
        private TMP_Text _nearbyMinesCountText;

        [SerializeField]
        private GameObject _mineDebugObject;

        [SerializeField]
        private GameObject _flagObject;

        private Tile[] _nearbyTiles;
        private TileState _tileState;
        private bool _hasMine;
        private int _nearbyMinesCount;
        private int _x;
        private int _y;

        public bool HasMine => _hasMine;
        public int X => _x;
        public int Y => _y;

        private void Awake()
        {
            _tileState = TileState.Hidden;
            _nearbyMinesCountText.gameObject.SetActive(false);
            _mineDebugObject.SetActive(false);
            _flagObject.SetActive(false);
        }

        internal void Configure(int x, int y)
        {
            _x = x;
            _y = y;
        }

        internal void SetNearbyTiles(Tile[] nearbyTiles)
        {
            var tiles = new List<Tile>();
            _nearbyMinesCount = 0;

            foreach (var tile in nearbyTiles)
            {
                if (tile == null)
                    continue;

                tiles.Add(tile);
                if (tile.HasMine)
                    ++_nearbyMinesCount;
            }

            _nearbyTiles = tiles.ToArray();
            _nearbyMinesCountText.text = _nearbyMinesCount.ToString();
        }

        internal void SetMine()
        {
            _hasMine = true;
            _mineDebugObject.SetActive(true);
        }

        void IPointerClickHandler.OnPointerClick(PointerEventData eventData)
        {
            switch (eventData.button)
            {
                case PointerEventData.InputButton.Left:
                    Open();
                    break;

                case PointerEventData.InputButton.Right:
                    Flag();
                    break;
            }
        }

        private void Open()
        {
            if (_tileState != TileState.Hidden)
                return;

            _nearbyMinesCountText.gameObject.SetActive(true);
            _tileState = TileState.Open;

            if (_nearbyMinesCount == 0)
            {
                foreach (var tile in _nearbyTiles)
                    tile.Open();
            }
        }

        private void Flag()
        {
            if (_tileState == TileState.Flag || _tileState == TileState.Hidden)
            {
                _tileState = _tileState == TileState.Flag ? TileState.Hidden : TileState.Flag;
                _flagObject.SetActive(_tileState == TileState.Flag);
            }
        }
    }
}
