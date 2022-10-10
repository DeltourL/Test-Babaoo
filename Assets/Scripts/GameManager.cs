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
        [SerializeField]
        private readonly float startingTime = 180f;
        [SerializeField]
        private TextMeshProUGUI timerDisplay;
        [SerializeField]
        private Board board;
        [SerializeField]
        Camera camera;

        // Start is called before the first frame update
        void Start()
        {
            board.CreateBoard();
            board.OnGameWon += Board_OnGameWon;
            timer = startingTime;
            SetCamera();
        }

        private void Board_OnGameWon(Board obj)
        {
            float timeElapsed = startingTime - timer;
            float bestTime = PlayerPrefs.GetFloat("Best time");
            if (bestTime > timeElapsed || bestTime == 0)
            {
                PlayerPrefs.SetFloat("Best time", timeElapsed);
            }
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

        // set the camera so that the board is fully visible
        void SetCamera()
        {
            if (Screen.width > Screen.height)
            {
                camera.orthographicSize = board.boardSize * .6f;
            }
            else
            {
                camera.orthographicSize = board.boardSize * .6f / Camera.main.aspect;
            }
        }
    }
}
