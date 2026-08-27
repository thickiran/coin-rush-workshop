using UnityEngine;
using UnityEngine.UI;

namespace Workshop.Ex09
{
    /// <summary>
    /// Tracks collected coins and updates the score label.
    /// </summary>
    public class ScoreManager : MonoBehaviour
    {
        public Text scoreText;

        public int CoinsCollected { get; private set; }
        public int TotalCoins { get; private set; }

        void Start()
        {
            TotalCoins = FindObjectsByType<CoinPickup>(FindObjectsSortMode.None).Length;
            UpdateLabel();
        }

        public void AddCoins(int amount)
        {
            CoinsCollected += amount;
            UpdateLabel();
            if (CoinsCollected >= TotalCoins)
            {
                var gm = FindFirstObjectByType<GameManager>();
                if (gm != null) gm.Win();
            }
        }

        void UpdateLabel()
        {
            if (scoreText != null) scoreText.text = "Coins: " + CoinsCollected + " / " + TotalCoins;
        }
    }
}
