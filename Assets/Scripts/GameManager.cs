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

        private float timer;
        private readonly float startingTime = 180f; // time allowed to complete the puzzle, in seconds

        [SerializeField]
        private TextMeshProUGUI timerDisplay;
        [SerializeField]
        private Board board;
        [SerializeField]
        private new Camera camera; // the camera used to display the board

        // Start is called before the first frame update
        void Start()
        {
            // create and shuffle the board
            board.CreateBoard();
            // listen to winning condition
            board.OnGameWon += Board_OnGameWon;

            // start the timer
            timer = startingTime;

            // set the camera so that the board is fully visible
            SetCamera();
        }

        // Update is called once per frame
        void Update()
        {
            if (timer > 0f)
            {
                // update timer
                timer -= Time.deltaTime;

                // update timer display
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

        // game won
        private void Board_OnGameWon(Board obj)
        {
            float timeElapsed = startingTime - timer;
            float bestTime = PlayerPrefs.GetFloat("Best time");
            if (bestTime > timeElapsed || bestTime == 0)
            {
                // update best time if beat or save a time if none were saved previsouly
                PlayerPrefs.SetFloat("Best time", timeElapsed);
            }
            GoToHomeScene();
        }

        // game lost
        private void GameLost()
        {
            GoToHomeScene();
        }

        public void GoToHomeScene()
        {
            STSSceneManager.LoadScene(SceneForButton.ToString());
        }

        void SetCamera()
        {
            if (Screen.width > Screen.height)
            {
                // fit camera to screen height
                camera.orthographicSize = board.boardSize * .6f;
            }
            else
            {
                // fit camera to screen width
                camera.orthographicSize = board.boardSize * .6f / camera.aspect;
            }
        }
    }
}
