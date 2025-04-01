using TMPro;
using UnityEngine;

namespace Scripts.GameGeneral.ScoreGeneral.Score
{
    public class Score : MonoBehaviour
    {
        public int score;

        [SerializeField] private TMP_Text scoreText;
        [SerializeField] private TMP_Text scoreTwoText;
        void Update()
        {
            score = PlayerPrefs.GetInt("Score", score);
            scoreText.text = score.ToString();
            scoreTwoText.text = score.ToString();
        }
    }
}
