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

        private void Awake()
        {
            var tiles = new Tile[_tileSize, _tileSize];
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
                    tiles[x, y] = Instantiate(_tilePrefab, tileParent);
                    tiles[x, y].Configure(x, y);
                }
            }

            // add mines
            var mineAdded = 0;
            while (mineAdded < _mineCount)
            {
                var x = Random.Range(0, _tileSize);
                var y = Random.Range(0, _tileSize);

                if (tiles[x, y].HasMine)
                    continue;

                tiles[x, y].SetMine();
                ++mineAdded;
            }

            // configure nearby tiles
            for (var x = 0; x < _tileSize; ++x)
            {
                for (var y = 0; y < _tileSize; ++y)
                {
                    tiles[x, y].SetNearbyTiles(
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

            Tile GetTile(int x, int y) => IsValidTile(x, y) ? tiles[x, y] : null;
        }

        private bool IsValidTile(int x, int y) =>
            x >= 0 && x < _tileSize && y >= 0 && y < _tileSize;
    }
}
