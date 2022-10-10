using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using SceneTransitionSystem;
using TMPro;


namespace TeasingGame
{
    public enum TeasingGameScene : int
    {
        Home,
        Game,
    }
    public class TeasingGameHomeSceneController : MonoBehaviour
    {
        public TeasingGameScene SceneForButton;


        [SerializeField]
        private TextMeshProUGUI bestTimeDisplay;
        // Start is called before the first frame update
        void Start()
        {
            float bestTime = PlayerPrefs.GetFloat("Best time");
            if (bestTime > 0f)
            {
                int seconds = ((int)bestTime % 60);
                int minutes = ((int)bestTime / 60);
                bestTimeDisplay.text = "Best time :\n" + string.Format("{0:00}:{1:00}", minutes, seconds);
            }
        }

        // Update is called once per frame
        void Update()
        {

        }

        public void GoToGameScene()
        {
            STSSceneManager.LoadScene(SceneForButton.ToString());
        }
    }
}