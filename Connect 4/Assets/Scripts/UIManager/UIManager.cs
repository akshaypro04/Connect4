
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Connect4.Comman.Game;

namespace Connect4.Player.UIManage
{
    public class UIManager : MonoBehaviour
    {
        [Header("Switching Panels")]
        [SerializeField] GameObject GameScene;
        [SerializeField] GameObject MainMenu;
        [SerializeField] GameObject SelectDifficulty;

        [Header("Panel")]
        [SerializeField] GameObject PausePanel;
        [SerializeField] GameObject gameBoard;

        [Header("Title")]
        [SerializeField] InputField playerNickName;
        [SerializeField] GameObject offbutn;
        [SerializeField] GameObject offbutnPause;

        [Header("Game")]
        [SerializeField] Text playerWin;
        [SerializeField] Text playerName;
        [SerializeField] Text oppoentName;
        [SerializeField] Text otherplayerWin;
        [SerializeField] GameObject Winning_panel;
        [SerializeField] GameObject lighting;
        [SerializeField] GameObject particale;

        int i = 0;
        void Start()
        {
            GameManager.Instances.SetGameReady(false);
            Winning_panel.SetActive(false);
            ActivatePanel(MainMenu.name);
            GameManager.Instances.SetTurn(true);
            PausePanel.SetActive(false);
            gameBoard.SetActive(false);
            lighting.SetActive(false);
            playerNickName.text = PlayerPrefs.GetString("nickname", "");
            MusicBtnStatus();
            particale.SetActive(false);
        }

        public void MusicBtnStatus()
        {
            if (GameManager.Instances.getAudioBool())
            {
                offbutn.SetActive(false);
                offbutnPause.SetActive(false);
            }
            else
            {
                offbutn.SetActive(true);
                offbutnPause.SetActive(true);
            }
        }

        public void OnClickFiendBtn()
        {
            GameManager.Instances.setAiPlayer(false);
            GameManager.Instances.setFriendPlayer(true);
            ActivatePanel(GameScene.name);
            GameManager.Instances.SetGameReady(true);
        }

        public void OnClickAIBtn()
        {
            GameManager.Instances.setAiPlayer(true);
            GameManager.Instances.setFriendPlayer(false);
            ActivatePanel(SelectDifficulty.name);
        }

        public void OnClickLevel1()
        {
            GameManager.Instances.setLevel2(false);
            GameManager.Instances.setLevel3(false);
            ActivatePanel(GameScene.name);
            GameManager.Instances.SetGameReady(true);
        }

        public void OnClickLevel2()
        {
            GameManager.Instances.setLevel2(true);
            GameManager.Instances.setLevel3(false);
            ActivatePanel(GameScene.name);
            GameManager.Instances.SetGameReady(true);
        }

        public void OnClickLevel3()
        {
            GameManager.Instances.setLevel2(false);
            GameManager.Instances.setLevel3(true);
            ActivatePanel(GameScene.name);
            GameManager.Instances.SetGameReady(true);
        }

        public void OnClickPause()
        {
            PausePanel.SetActive(true);
            MusicBtnStatus();
            Time.timeScale = 0;
            GameManager.Instances.SetGameReady(false);
        }

        public void OnClickResume()
        {
            Time.timeScale = 1;
            GameManager.Instances.SetGameReady(true);
            PausePanel.SetActive(false);
        }

        public void OnMainMenu()
        {
            Time.timeScale = 1;
            SceneManager.LoadScene(0);
        }

        public void OnQuitGame()
        {
            Time.timeScale = 1;
            print("application quitting...");
            Application.Quit();
        }

        public void onEndEdit()
        {
            string name = playerNickName.text;
            print("player name set : " + name);
            if (!string.IsNullOrEmpty(name))
            {
                PlayerPrefs.SetString("nickname", name);
            }
        }

        public void AudioManage()
        {
            print("manager called");

            if (GameManager.Instances.getAudioBool())
            {
                print("audio stop");
                GameManager.Instances.SetAudio(0);
                offbutn.SetActive(true);
                offbutnPause.SetActive(true);
                GameManager.Instances.stop("music");
            }
            else
            {
                print("audio start");
                GameManager.Instances.SetAudio(1);
                offbutn.SetActive(false);
                offbutnPause.SetActive(false);
                GameManager.Instances.play("music");
            }
        }

        public void PlayersData()
        {

            if (GameManager.Instances.getAIPlayer())
            {
                oppoentName.text = "Robot";
                otherplayerWin.text = GameManager.Instances.getAIwin().ToString();
            }
            else
            {
                oppoentName.text = "Friend";
                otherplayerWin.text = GameManager.Instances.getOtherPlayerwin().ToString();
            }

            playerName.text = PlayerPrefs.GetString("nickname", "Player1");
            playerWin.text = GameManager.Instances.getPlayerwin().ToString();

        }

        public void ActivatePanel(string matchesPanelName)
        {
            PlayersData();
            GameScene.SetActive(matchesPanelName.Equals(GameScene.name));
            MainMenu.SetActive(matchesPanelName.Equals(MainMenu.name));
            SelectDifficulty.SetActive(matchesPanelName.Equals(SelectDifficulty.name));
            GameManager.Instances.play("click");
        }
    }
}