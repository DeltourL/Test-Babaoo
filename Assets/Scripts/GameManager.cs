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

        // Start is called before the first frame update
        void Start()
        {

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
                GoToHomeScene();
            }

        }

        public void GoToHomeScene()
        {
            STSSceneManager.LoadScene(SceneForButton.ToString());
        }
    }
}
