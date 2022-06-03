using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Ape.Minesweeper
{
    public class Tile : MonoBehaviour, IPointerClickHandler
    {
        [SerializeField]
        private TMP_Text _nearbyMinesCountText;

        [SerializeField]
        private GameObject _mineDebug;

        private Tile[] _nearbyTiles;
        private bool _hasMine;
        private int _x;
        private int _y;

        public bool HasMine => _hasMine;
        public int X => _x;
        public int Y => _y;

        private void Awake()
        {
            _nearbyMinesCountText.gameObject.SetActive(false);
            _mineDebug.SetActive(false);
        }

        internal void Configure(int x, int y)
        {
            _x = x;
            _y = y;
        }

        internal void SetNearbyTiles(Tile[] nearbyTiles)
        {
            var tiles = new List<Tile>();
            var nearbyMinesCount = 0;

            foreach (var tile in nearbyTiles)
            {
                if (tile == null)
                    continue;

                tiles.Add(tile);
                if (tile.HasMine)
                    ++nearbyMinesCount;
            }

            _nearbyTiles = tiles.ToArray();
            _nearbyMinesCountText.text = nearbyMinesCount.ToString();
        }

        internal void SetMine()
        {
            _hasMine = true;
            _mineDebug.SetActive(true);
        }

        void IPointerClickHandler.OnPointerClick(PointerEventData eventData)
        {
            switch (eventData.button)
            {
                case PointerEventData.InputButton.Left:
                    break;
                case PointerEventData.InputButton.Right:
                    break;
            }
        }
    }
}
