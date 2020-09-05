using Connect4.Comman.Audio;
using Connect4.Player.Imput;
using UnityEngine;

namespace Connect4.Comman.Game
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager Instances { get; private set; }

        bool level2;
        bool level3;
        bool aiplayer;
        bool friendplayer;
        bool gameReady;
        bool nextTurn;

        void Awake()
        {
            if (Instances == null)
            {
                Instances = this;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
                return;
            }
        }


        PlayerController m_playerController;
        public PlayerController playerController
        {
            get
            {
                if (m_playerController == null)
                    m_playerController = gameObject.GetComponent<PlayerController>();
                return m_playerController;
            }
        }

        AudioManager m_audioManager;
        public AudioManager audioManager
        {
            get
            {
                if (m_audioManager == null)
                    m_audioManager = gameObject.GetComponent<AudioManager>();
                return m_audioManager;
            }
        }

        public void play(string play)
        {
            audioManager.Play(play);
        }

        public void stop(string play)
        {
            audioManager.Stop(play);
        }


        public void SetAudio(int status)
        {
            PlayerPrefs.SetInt("audio", status);
        }

        public int getAudio()
        {
            return PlayerPrefs.GetInt("audio", 1);
        }

        public bool getAudioBool()
        {
            int i = getAudio();
            if (i == 0)
            {
                return false;
            }
            return true;
        }


        public void setLevel3(bool level)
        {
            level3 = level;
        }

        public bool getLevel3()
        {
            return level3;
        }

        public void setLevel2(bool level)
        {
            level2 = level;
        }

        public bool getLevel2()
        {
            return level2;
        }


        public void setAiPlayer(bool aiplay)
        {
            aiplayer = aiplay;
        }

        public bool getAIPlayer()
        {
            return aiplayer;
        }

        public void setFriendPlayer(bool friend)
        {
            friendplayer = friend;
        }

        public bool getFriendPlayer()
        {
            return friendplayer;
        }

        public void SetGameReady(bool gameStatus)
        {
            gameReady = gameStatus;
        }

        public bool getGameReady()
        {
            return gameReady;
        }

        public void SetTurn(bool turn)
        {
            nextTurn = turn;
        }

        public bool getTurn()
        {
            return nextTurn;
        }

        public int getPlayerwin()
        {
            return PlayerPrefs.GetInt("playerWin", 0);
        }

        public void setPlayerwin(int win)
        {
            PlayerPrefs.SetInt("playerWin", win);
        }

        public int getAIwin()
        {
            return PlayerPrefs.GetInt("AIWin", 0);
        }

        public void setAIwin(int win)
        {
            PlayerPrefs.SetInt("AIWin", win);
        }

        public int getOtherPlayerwin()
        {
            return PlayerPrefs.GetInt("otherplayerWin", 0);
        }

        public void setOtherPlayerwin(int win)
        {
            PlayerPrefs.SetInt("otherplayerWin", win);
        }

    }
}
