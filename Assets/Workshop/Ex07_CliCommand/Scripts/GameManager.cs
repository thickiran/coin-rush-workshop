using UnityEngine;
using UnityEngine.SceneManagement;

namespace Workshop.Ex07
{
    /// <summary>
    /// Owns the win/lose flow and restarting.
    /// </summary>
    public class GameManager : MonoBehaviour
    {
        public GameObject winPanel;
        public GameObject losePanel;

        public bool IsGameOver { get; private set; }

        public void Win()  { End(winPanel); }
        public void Lose() { End(losePanel); }

        void End(GameObject panel)
        {
            if (IsGameOver) return;
            IsGameOver = true;
            if (panel != null) panel.SetActive(true);
            Time.timeScale = 0f;
        }

        public void Restart()
        {
            Time.timeScale = 1f;
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }
}
