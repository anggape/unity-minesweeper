using UnityEngine;
using UnityEngine.UI;

namespace Ape.Minesweeper
{
    public class GameHandler : MonoBehaviour
    {
        [SerializeField]
        private int _tileSize = 10;

        [SerializeField]
        private int _mineCount = 10;

        [SerializeField]
        private Tile _tilePrefab;

        [SerializeField]
        private GridLayoutGroup _gridLayout;

        [SerializeField]
        private GameObject _gameOverScreen;

        private Tile[,] _tiles;

        private void Awake()
        {
            _tiles = new Tile[_tileSize, _tileSize];
            _gameOverScreen.SetActive(false);

            var tileParent = (RectTransform)_gridLayout.transform;
            var cellSize = (tileParent.rect.width / _tileSize) - _gridLayout.spacing.x;

            foreach (Transform child in tileParent)
                Destroy(child.gameObject);

            _gridLayout.cellSize = Vector2.one * cellSize;

            // spawn tiles
            for (var x = 0; x < _tileSize; ++x)
            {
                for (var y = 0; y < _tileSize; ++y)
                {
                    _tiles[x, y] = Instantiate(_tilePrefab, tileParent);
                    _tiles[x, y].Configure(x, y);
                    _tiles[x, y].OnMineSelected += OnMineSelected;
                }
            }

            // add mines
            var mineAdded = 0;
            while (mineAdded < _mineCount)
            {
                var x = Random.Range(0, _tileSize);
                var y = Random.Range(0, _tileSize);

                if (_tiles[x, y].HasMine)
                    continue;

                _tiles[x, y].SetMine();
                ++mineAdded;
            }

            // configure nearby tiles
            for (var x = 0; x < _tileSize; ++x)
            {
                for (var y = 0; y < _tileSize; ++y)
                {
                    _tiles[x, y].SetNearbyTiles(
                        new Tile[]
                        {
                            GetTile(x + 1, y + 1),
                            GetTile(x - 1, y - 1),
                            GetTile(x + 1, y - 1),
                            GetTile(x - 1, y + 1),
                            GetTile(x + 1, y),
                            GetTile(x, y + 1),
                            GetTile(x - 1, y),
                            GetTile(x, y - 1),
                        }
                    );
                }
            }

            Tile GetTile(int x, int y) => IsValidTile(x, y) ? _tiles[x, y] : null;
        }

        private void OnMineSelected(int x, int y)
        {
            // reveal all tiles
            for (var row = 0; row < _tileSize; ++row)
            {
                for (var col = 0; col < _tileSize; ++col)
                {
                    _tiles[row, col].Reveal();
                }
            }

            _gameOverScreen.SetActive(true);
        }

        private bool IsValidTile(int x, int y) =>
            x >= 0 && x < _tileSize && y >= 0 && y < _tileSize;
    }
}
