using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Ape.Minesweeper
{
    public class MenuHandler : MonoBehaviour
    {
        public const string TileSizePrefs = "__tileSize__";
        public const string MineCountPrefs = "__mineCount__";

        [SerializeField]
        private Slider _tilesSlider;

        [SerializeField]
        private TMP_Text _tilesText;

        [SerializeField]
        private Slider _minesSlider;

        [SerializeField]
        private TMP_Text _minesText;

        private void Awake()
        {
            UpdateMinesSlider();
            UpdateTilesSlider();
        }

        public void UpdateMinesSlider()
        {
            _minesSlider.minValue = _tilesSlider.value;
            _minesSlider.maxValue = (_tilesSlider.value * _tilesSlider.value) - 1;
            _minesText.text = $"Mines: {_minesSlider.value}";
        }

        public void UpdateTilesSlider() =>
            _tilesText.text = $"Tiles: {_tilesSlider.value}x{_tilesSlider.value}";

        public void Play()
        {
            PlayerPrefs.SetInt(TileSizePrefs, (int)_tilesSlider.value);
            PlayerPrefs.SetInt(MineCountPrefs, (int)_minesSlider.value);

            SceneManager.LoadScene("Game");
        }

        public void Quit() => Application.Quit();
    }
}
