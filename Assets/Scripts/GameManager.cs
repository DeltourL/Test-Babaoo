using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using SceneTransitionSystem;
using TMPro;

namespace TeasingGame
{

    public class GameManager : MonoBehaviour
    {
        public TeasingGameScene SceneForButton;

        private float timer = 180f;
        [SerializeField]
        private TextMeshProUGUI timerDisplay;

        [SerializeField]
        private Board board;

        // Start is called before the first frame update
        void Start()
        {
            board.CreateBoard();
            board.OnGameWon += Board_OnGameWon;
        }

        private void Board_OnGameWon(Board obj)
        {
            GoToHomeScene();
        }
        private void GameLost()
        {
            GoToHomeScene();
        }

        // Update is called once per frame
        void Update()
        {
            if (timer > 0f)
            {
                // update timer
                timer -= Time.deltaTime;
                int seconds = ((int)timer % 60);
                int minutes = ((int)timer / 60);
                timerDisplay.text = "Time left :\n" + string.Format("{0:00}:{1:00}", minutes, seconds);
            }
            else
            {
                // end game
                GameLost();
            }

        }

        public void GoToHomeScene()
        {
            STSSceneManager.LoadScene(SceneForButton.ToString());
        }
    }
}
