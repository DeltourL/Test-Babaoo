using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using SceneTransitionSystem;
using TMPro;
using System;

namespace TeasingGame
{

    public class GameManager : MonoBehaviour
    {
        public TeasingGameScene SceneForButton;

        private float timer;
        private readonly float startingTime = 180f; // time allowed to complete the puzzle, in seconds
        private bool isGameActive;
        int resolutionX = Screen.width; // saving screen width to check for changes during the game
        int resolutionY = Screen.height; // saving screen height to check for changes during the game

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

            isGameActive = true;
            // set the camera so that the board is fully visible
            SetCamera();
            // reset the camera if resolution changes
            StartCoroutine(CheckForScreenSizeChange());
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
            isGameActive = false;

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
            isGameActive = false;
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

        private IEnumerator CheckForScreenSizeChange()
        {
            while (isGameActive)
            {
                // if screen size changes
                if (resolutionX != Screen.width || resolutionY != Screen.height)
                {
                    // reset camera
                    SetCamera();
                    // save new screen size
                    resolutionX = Screen.width;
                    resolutionY = Screen.height;
                }
                yield return null;
            }
            yield return null;
        }
    }
}
