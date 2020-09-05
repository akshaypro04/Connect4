using Connect4.Comman.Game;
using Connect4.Player.DotMove;
using Connect4.Player.UIManage;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace Connect4.Player.Controller
{
    public class Player : MonoBehaviour
    {
        Transform target;
        int human = 1, opponent = -1, otherplayer = -2;
        Vector3 ScreenPos;
        Vector3 touchArea;
        Vector2 spawnPos;
        int col;
        bool findSpawnPos;
        int rowValue;
        bool System_Ready;
        bool gameOver;
        bool find;
        bool enemyFirst;
        bool board;
        bool playerOne;
        bool playerTwo;
        float DotXPose;
        float DotYPose;
        Vector2 pixels;

        // poiner
        float countDown;
        float value = 335f;
        float speed = 30f;

        [Header("Background")]
        [SerializeField] GameObject boardImage;

        [SerializeField]
        GameObject HumanDot;

        [SerializeField]
        GameObject AiDot;

        [SerializeField]
        Transform cam;

        [SerializeField]
        Transform dotsParents;

        [SerializeField]
        GameObject Winning_Panel;

        [SerializeField]
        GameObject lighting;

        [SerializeField]
        Text WinnerName;

        [SerializeField]
        RectTransform pointer;

        [SerializeField]
        GameObject rightPointer;

        [SerializeField]
        GameObject leftPointer;

        [SerializeField]
        GameObject partical;

        public GameObject dot;

        int[,] dotArray = new int[6, 7] {   { 0, 0, 0, 0, 0, 0, 0 },
                                        { 0, 0, 0, 0, 0, 0, 0 },
                                        { 0, 0, 0, 0, 0, 0, 0 },
                                        { 0, 0, 0, 0, 0, 0, 0 },
                                        { 0, 0, 0, 0, 0, 0, 0 },
                                        { 0, 0, 0, 0, 0, 0, 0 } };

        void Start()
        {
            playerOne = true;
            pixels.x = Camera.main.pixelWidth;
            pixels.y = Camera.main.pixelHeight;
        }

        void MakeBoard()
        {
            boardImage.SetActive(true);
            gameOver = false;
            System_Ready = true;

            ScreenPos = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, 0));
            cam.position = ScreenPos / 2;

            DotXPose = -0.2f;
            DotYPose = 1.4f;


            for (int i = 1; i <= dotArray.GetLength(0); i++)                                                         // create dots holes
            {
                for (int j = 1; j <= dotArray.GetLength(1); j++)
                {
                    GameObject d = Instantiate(dot, new Vector3((ScreenPos.x * 2 / 15f * j) + DotXPose, (ScreenPos.y * 2 / 24f * i) + DotYPose, 0), Quaternion.identity);
                    d.name = (i - 1).ToString() + "," + (j - 1).ToString();
                    d.transform.SetParent(dotsParents);
                }
            }
        }


        void SetBoard()
        {
            if (pixels.y == 2160)
            {
                dotsParents.position = new Vector3(0.15f, 0.25f, 1);
                dotsParents.localScale = new Vector3(0.95f, 0.95f, 1);

            }
            else if (pixels.y == 2960)
            {
                dotsParents.position = new Vector3(0.15f, 0.2f, 1);
                dotsParents.localScale = new Vector3(0.95f, 0.95f, 1);
            }
        }

        void Update()
        {
            if (gameOver)
                return;

            if (!GameManager.Instances.getGameReady())
                return;

            if (board == false)
            {
                MakeBoard();
                SetBoard();
                board = true;
            }

            touchArea = Camera.main.ScreenToWorldPoint(GameManager.Instances.playerController.touch);

            if (GameManager.Instances.getTurn())
            {
                var pos = pointer.localPosition;
                if (countDown >= -value)
                {
                    pos.x = countDown;
                    pointer.localPosition = pos;
                    countDown -= speed;
                    rightPointer.SetActive(false);
                    leftPointer.SetActive(true);
                }
            }
            else
            {
                var pos = pointer.localPosition;
                if (countDown <= value)
                {
                    pos.x = countDown;
                    pointer.localPosition = pos;
                    countDown += speed;
                    leftPointer.SetActive(false);
                    rightPointer.SetActive(true);
                }
            }


            if (!System_Ready)
                return;

            if (GameManager.Instances.playerController.press && GameManager.Instances.getTurn() && playerOne == true)                                               // first player
            {
                playerOne = false;
                playerTwo = true;
                HumanTurn(dotArray, human, HumanDot, false);
            }


            if (GameManager.Instances.playerController.press && GameManager.Instances.getFriendPlayer() && !GameManager.Instances.getTurn() && playerTwo == true)    // friend
            {
                playerOne = true;
                playerTwo = false;
                HumanTurn(dotArray, otherplayer, AiDot, true);
            }


            if (!GameManager.Instances.getTurn() && GameManager.Instances.getAIPlayer() && playerTwo == true)                                    // AI
            {
                playerOne = true;
                playerTwo = false;
                StartCoroutine(CallAI());
                System_Ready = false;
            }
        }

        IEnumerator CallAI()
        {
            yield return new WaitForSeconds(1f);
            AITurn(dotArray);
        }

        void HumanTurn(int[,] board, int playerID, GameObject DotID, bool turn)
        {
            System_Ready = false;
            for (int i = 0; i < board.GetLength(1); i++)                                            // 6
            {
                if (touchArea.x > (ScreenPos.x / 7) * i && touchArea.x < (ScreenPos.x / 7) * (i + 1))
                {
                    spawnPos.x = (ScreenPos.x * 2 / 15f * (i + 1)) + -0.2f;
                    spawnPos.y = ScreenPos.y * 0.8f;
                    col = i;
                }
            }

            rowValue = CheckAvailablity(board, col, playerID);

            switch (rowValue)
            {
                case -10:
                    gameOver = true;
                    print("table is full");
                    break;

                case -1:
                    System_Ready = true;
                    playerTwo = !playerTwo;
                    playerOne = !playerOne;
                    print("player turn again...");
                    break;

                default:
                    move(DotID, spawnPos, rowValue, col, playerID, board);
                    break;
            }

        }

        int CheckAvailablity(int[,] board, int col, int playervalue)                // check availability and set dot
        {
            if (!IsMoveLeft(board))                                                 // check for space in board
                return -10;

            findSpawnPos = false;
            for (int x = 0; x < board.GetLength(0); x++)                            // check in particuler coloume
            {

                if (board[x, col] == 0 && findSpawnPos == false)
                {
                    board[x, col] = playervalue;                                    // set the dot
                    findSpawnPos = true;
                    return x;
                }
            }
            return -1;
        }

        bool IsMoveLeft(int[,] board)
        {
            find = false;
            for (int i = 0; i < board.GetLength(0); i++)
            {
                for (int j = 0; j < board.GetLength(1); j++)
                {
                    if (board[i, j] == 0 && find == false)
                    {
                        find = true;
                        return true;
                    }
                }
            }
            print("board is full");
            return false;
        }

        void move(GameObject dot, Vector3 spawnPos, int row, int col, int playertype, int[,] board)
        {
            target = GameObject.Find("dotsParents/" + row + "," + col + "").GetComponent<Transform>();

            if (target == null)
                return;

            GameObject go = Instantiate(dot, spawnPos, Quaternion.identity);
            go.name = "SpawnDot" + row.ToString() + "," + col.ToString();
            go.GetComponent<Rigidbody2D>().AddForce(new Vector2(0, 50f));
            go.GetComponent<dotsMovement>().row = row;
            go.GetComponent<dotsMovement>().col = col;

            if (checkWinner(board, row, col, playertype, 4) > 0)
            {
                string name;

                if (playertype == 1)
                {
                    name = PlayerPrefs.GetString("nickname", "Player 1");
                    GameManager.Instances.setPlayerwin(GameManager.Instances.getPlayerwin() + 1);
                }
                else if (playertype == -1)
                {
                    name = "Robot";
                    GameManager.Instances.setAIwin(GameManager.Instances.getAIwin() + 1);
                }
                else
                {
                    name = "Friend ";
                    GameManager.Instances.setOtherPlayerwin(GameManager.Instances.getOtherPlayerwin() + 1);
                }
                CollectionWinnerDotes(board, row, col, playertype, 4);
                lighting.SetActive(true);
                Winning_Panel.SetActive(true);
                WinnerName.text = name + " Win Match";
                gameOver = true;
                partical.SetActive(true);
                GameObject.Find("Canvas/UiManager").GetComponent<UIManager>().PlayersData();
                GameManager.Instances.SetGameReady(false);
                return;
            }

            if (!IsMoveLeft(board))
            {
                Winning_Panel.SetActive(true);
                WinnerName.text = "Match tie";
                GameManager.Instances.SetGameReady(false);
            }

            System_Ready = true;
            findSpawnPos = false;
        }

        void AITurn(int[,] board)
        {
            int col = AKminimax1(board, false, 4);                                                                         // UnityEngine.Random.Range(0, 7);
            spawnPos.x = (ScreenPos.x * 2 / 15f * (col + 1)) + -0.2f;
            spawnPos.y = ScreenPos.y * 0.8f;

            rowValue = CheckAvailablity(board, col, opponent);                  //  set the value

            switch (rowValue)
            {
                case -10:
                    print("table is full");
                    gameOver = true;
                    break;

                case -1:
                    AITurn(board);
                    break;

                default:
                    move(AiDot, spawnPos, rowValue, col, -1, board);
                    break;
            }
        }

        //int FindBestMove(int[,] board)
        //{
        //    int bestVal = -1000;                                                              // we want positive value as high as possible
        //    int targetPos = -1;                                    

        //    for (int j = 0; j < board.GetLength(1); j++)                                     // col
        //    {
        //        for (int i = 0; i < board.GetLength(0); i++)                                 // row
        //        {
        //            if (board[i, j] == 0)
        //            {
        //                board[i, j] = -1;                                                     // ai dot

        //                int minimaxVlaue = AKminimax(board, i, j, 0, false);                 

        //                board[i, j] = 0;                                                     // undone ai dot

        //                if (minimaxVlaue > bestVal)
        //                {
        //                    targetPos = j;
        //                    bestVal = minimaxVlaue;
        //                }
        //                break;                                                                 // break the inner loop
        //            }
        //        }
        //    }
        //    return targetPos;
        //}


        //int minimax(int[,] board, int i, int j, int depth, bool isMax)
        //{

        //    if (checkWinner(board, i, j, -1, 4) > 0)                // for enemy wins
        //    {
        //        print("Computer win");
        //        return 10;
        //    }

        //    if (checkWinner(board, i, j, 1, 4) > 0)                // for player wins
        //    {
        //        print("Human win");
        //        return -10;
        //    }

        //    if (!IsMoveLeft(board))
        //        return  0;                                          // match is tie


        //    if (isMax)
        //    {
        //        int maxValue;
        //        int best = -1000;
        //        for (int o = 0; o < board.GetLength(0); o++)                    // col
        //        {
        //            for (int p = 0; p < board.GetLength(1); p++)                // row
        //            {
        //                if (board[o, p] == 0)
        //                {
        //                    board[o, p] = -1;

        //                    maxValue = Mathf.Max(best, minimax(board, o, p, depth + 1, !isMax));
        //                    board[o, p] = 0;

        //                    if (best > maxValue)
        //                    {
        //                        best = maxValue;
        //                        break;
        //                    }
        //                }
        //            }
        //        }
        //        return best;
        //    }
        //    else
        //    {   
        //        int minvalue;
        //        int best = 1000;
        //        for (int k = 0; k < board.GetLength(0); k++)            // row
        //        {
        //            for (int l = 0; l < board.GetLength(1); l++)        // col
        //            {
        //                if (board[k, l] == 0)
        //                {
        //                    board[k, l] = 1;

        //                    minvalue = Mathf.Min(best, minimax(board, k, l, depth + 1, !isMax));
        //                    board[k, l] = 0;

        //                    if(best > minvalue)
        //                    {
        //                        best = minvalue;
        //                        break;
        //                    }

        //                }
        //            }
        //        }
        //        return best;
        //    }
        //    //return UnityEngine.Random.Range(-100, 101);
        //}

        int AKminimax1(int[,] board, bool isMax, int enemyPair)
        {

            if (isMax)
            {
                int seen = 0;

                if (enemyFirst == false)
                {
                    return enemyRandom(board);
                }

                for (int x = 0; x < board.GetLength(0); x++)                            // stop player to win
                {
                    for (int y = 0; y < board.GetLength(1); y++)
                    {
                        if (board[x, y] == 0 && seen < 8)                                // if any blank found
                        {
                            seen++;
                            if (checkfornbd(board, x, y, -1))
                            {
                                board[x, y] = -1;
                                if (checkWinner(board, x, y, -1, enemyPair) > 0)
                                {
                                    board[x, y] = 0;
                                    return y;
                                }
                                board[x, y] = 0;
                            }
                        }
                        else
                        {
                            seen = 0;
                        }
                    }
                }

                if (enemyPair == 4)
                {
                    return AKminimax1(board, true, 3);
                }
            }
            else                                                                         // stop player to win
            {
                int seen = 0;

                for (int x = 0; x < board.GetLength(0); x++)
                {
                    for (int y = 0; y < board.GetLength(1); y++)
                    {
                        if (board[x, y] == 0 && seen < 8)                              // continuos run 7 times only in blank space
                        {
                            seen++;

                            if (checkfornbd(board, x, y, 1))
                            {
                                board[x, y] = 1;
                                if (checkWinner(board, x, y, 1, 4) > 0)
                                {
                                    board[x, y] = 0;
                                    return y;
                                }
                                board[x, y] = 0;
                            }
                        }
                        else
                        {
                            seen = 0;
                        }
                    }
                }

                // under last else
                if (GameManager.Instances.getLevel2())                                        // user select level2 medium
                {
                    int i;
                    if ((i = CheckForNextStepPlayer(board, 4, -1)) >= 0)                      // checking is AI making four pair
                    {
                        return i;
                    }

                    int m = UnityEngine.Random.Range(0, 2);
                    if (m > 0)
                    {
                        int j;
                        if ((j = CheckForNextStepPlayer(board, 3, 1)) >= 0)                 // checking is player can make 3 pair ??
                        {
                            return j;
                        }
                    }
                }

                if (GameManager.Instances.getLevel3())                                       // user select level3 hard
                {
                    int i;
                    if ((i = CheckForNextStepPlayer(board, 4, -1)) >= 0)                     // checking is AI making four pair
                    {
                        return i;
                    }
                    int j;
                    if ((j = CheckForNextStepPlayer(board, 3, 1)) >= 0)                     //checking is player can make 3 pair ??
                    {
                        return j;
                    }
                }

                if (GameManager.Instances.getLevel2() || GameManager.Instances.getLevel3())
                {
                    return AKminimax1(board, true, 3);                      // AI alreday for 4 pair
                }
                else
                {
                    return AKminimax1(board, true, 4);                      // ai dont check 4 pair
                }

            }
            print("random");
            return enemyRandom(board);
        }

        int CheckForNextStepPlayer(int[,] board, int pair, int playertype)
        {
            int seen = 0;

            for (int x = 0; x < board.GetLength(0); x++)
            {
                for (int y = 0; y < board.GetLength(1); y++)
                {
                    if (board[x, y] == 0 && seen < 8)                              // continuos run 7 times only in blank space
                    {
                        seen++;

                        if (checkfornbd(board, x, y, playertype))
                        {
                            board[x, y] = playertype;
                            if (checkWinner(board, x, y, playertype, pair) > 0)
                            {
                                board[x, y] = 0;
                                return y;
                            }
                            board[x, y] = 0;
                        }
                    }
                    else
                    {
                        seen = 0;
                    }
                }
            }
            return -1;
        }

        int enemyRandom(int[,] board)
        {
            enemyFirst = true;
            return UnityEngine.Random.Range(0, board.GetLength(1));
        }

        bool checkfornbd(int[,] board, int i, int j, int playertype)
        {

            if (check(board, i + 1, j - 1, playertype))
            {
                return true;
            }

            if (i > 0)
            {
                if (check(board, i, j - 1, playertype) && (check(board, i - 1, j, playertype) || check(board, i - 1, j, -1 * playertype)))          //  left
                {
                    return true;
                }

                if (check(board, i, j + 1, playertype) && (check(board, i - 1, j, playertype) || check(board, i - 1, j, -1 * playertype)))              //  right
                {
                    return true;
                }
            }
            else
            {
                if (check(board, 0, j - 1, playertype))          //  left
                {
                    return true;
                }
                if (check(board, 0, j + 1, playertype))          //  right
                {
                    return true;
                }
            }


            if (check(board, i - 1, j - 1, playertype) && (check(board, i - 1, j, playertype) || check(board, i - 1, j, -1 * playertype)))
            {
                return true;
            }
            if (check(board, i - 1, j, playertype))                                                                                         // down
            {
                return true;
            }
            if (check(board, i - 1, j + 1, playertype) && (check(board, i - 1, j, playertype) || check(board, i - 1, j, -1 * playertype)))
            {
                return true;
            }

            if (check(board, i + 1, j + 1, playertype))
            {
                return true;
            }

            return false;
        }

        bool check(int[,] board, int i, int j, int playertype)
        {
            if (i >= 0 && j >= 0)
            {
                if (i < board.GetLength(0) && j < board.GetLength(1))
                {
                    if (board[i, j] == playertype)
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        #region check winner return 10 or -10

        int checkWinner(int[,] board, int i, int j, int player, int pair)
        {

            int score;

            if ((score = verticalCkeck(board, i, j, player, pair)) > 0)
            {
                return score;
            }
            else if ((score = horizontalCheck(board, i, j, player, pair)) > 0)
            {
                return score;
            }
            else if ((score = DiogonalLeft2rightCheck(board, i, j, player, pair)) > 0)
            {
                return score;
            }
            else if ((score = DiogonalRight2leftCheck(board, i, j, player, pair)) > 0)
            {
                return score;
            }
            return -10;
        }

        int verticalCkeck(int[,] board, int i, int j, int player, int pair)
        {
            int seq = 0;

            for (int y = 0; y < board.GetLength(1); y++)
            {
                if (board[i, y] == player)
                {
                    seq++;

                    if (seq == pair)
                    {
                        return 10;
                    }
                }
                else
                {
                    seq = 0;
                }
            }
            return -10;
        }

        int horizontalCheck(int[,] board, int i, int j, int player, int pair)
        {
            int seq = 0;

            for (int x = 0; x < board.GetLength(0); x++)
            {
                if (board[x, j] == player)
                {
                    seq++;

                    if (seq == pair)
                    {
                        return 10;
                    }
                }
                else
                {
                    seq = 0;
                }
            }
            return -10;
        }

        int DiogonalLeft2rightCheck(int[,] board, int i, int j, int player, int pair)
        {

            if (i >= j)
            {
                int x = i - j;
                int y = 0;
                int seq = 0;

                for (int k = x; k < board.GetLength(0); k++)
                {
                    if (board[k, y] == player)
                    {
                        seq++;

                        if (seq == pair)
                        {
                            return 10;
                        }
                    }
                    else
                    {
                        seq = 0;
                    }
                    y++;
                }
            }
            else if (j > i)                                                                     // take left t right bottom to top
            {
                int x = 0;
                int y = j - i;
                int seq = 0;

                for (int k = y; k < board.GetLength(1); k++)
                {
                    if (board[x, k] == player)
                    {
                        seq++;

                        if (seq == pair)
                        {
                            return 10;
                        }
                    }
                    else
                    {
                        seq = 0;
                    }
                    x++;
                }
            }
            return -10;
        }

        int DiogonalRight2leftCheck(int[,] board, int i, int j, int player, int pair)
        {

            if (i + j > 5)
            {
                int seq = 0;
                int k = (i + j) - 6;                                   // know to value of colume
                int m = 6;

                for (int x = k; x < board.GetLength(0); x++)
                {
                    if (board[x, m] == player)
                    {
                        seq++;

                        if (seq == pair)
                        {
                            return 10;
                        }
                    }
                    else
                    {
                        seq = 0;
                    }
                    m--;
                }
            }
            else
            {
                int n = i + j;
                int m = n;
                int seq = 0;
                for (int x = 0; x <= n; x++)
                {
                    if (board[x, m] == player)
                    {
                        seq++;

                        if (seq == pair)
                        {
                            return 10;
                        }
                    }
                    else
                    {
                        seq = 0;
                    }
                    m--;
                }
            }

            return -10;
        }


        void CollectionWinnerDotes(int[,] board, int i, int j, int player, int pair)
        {


            if (true)
            {
                int seq = 0;

                for (int y = 0; y < board.GetLength(1); y++)                                // vertical
                {
                    if (board[i, y] == player)
                    {
                        seq++;

                        if (seq == pair)
                        {
                            GameObject.Find("SpawnDot" + i.ToString() + "," + y.ToString()).GetComponent<SpriteRenderer>().sortingOrder = 10;
                            GameObject.Find("SpawnDot" + i.ToString() + "," + (y - 1).ToString()).GetComponent<SpriteRenderer>().sortingOrder = 10;
                            GameObject.Find("SpawnDot" + i.ToString() + "," + (y - 2).ToString()).GetComponent<SpriteRenderer>().sortingOrder = 10;
                            GameObject.Find("SpawnDot" + i.ToString() + "," + (y - 3).ToString()).GetComponent<SpriteRenderer>().sortingOrder = 10;
                            return;
                        }
                    }
                    else
                    {
                        seq = 0;
                    }
                }


                for (int x = 0; x < board.GetLength(0); x++)                        // horizontal
                {
                    if (board[x, j] == player)
                    {
                        seq++;

                        if (seq == pair)
                        {
                            GameObject.Find("SpawnDot" + (x).ToString() + "," + j.ToString()).GetComponent<SpriteRenderer>().sortingOrder = 10;
                            GameObject.Find("SpawnDot" + (x - 1).ToString() + "," + j.ToString()).GetComponent<SpriteRenderer>().sortingOrder = 10;
                            GameObject.Find("SpawnDot" + (x - 2).ToString() + "," + j.ToString()).GetComponent<SpriteRenderer>().sortingOrder = 10;
                            GameObject.Find("SpawnDot" + (x - 3).ToString() + "," + j.ToString()).GetComponent<SpriteRenderer>().sortingOrder = 10;
                            return;
                        }
                    }
                    else
                    {
                        seq = 0;
                    }
                }
            }


            if (i >= j)
            {
                int x = i - j;
                int y = 0;
                int seq = 0;

                for (int k = x; k < board.GetLength(0); k++)
                {
                    if (board[k, y] == player)
                    {
                        seq++;

                        if (seq == pair)
                        {
                            GameObject.Find("SpawnDot" + (k).ToString() + "," + (y).ToString()).GetComponent<SpriteRenderer>().sortingOrder = 10;
                            GameObject.Find("SpawnDot" + (k - 1).ToString() + "," + (y - 1).ToString()).GetComponent<SpriteRenderer>().sortingOrder = 10;
                            GameObject.Find("SpawnDot" + (k - 2).ToString() + "," + (y - 2).ToString()).GetComponent<SpriteRenderer>().sortingOrder = 10;
                            GameObject.Find("SpawnDot" + (k - 3).ToString() + "," + (y - 3).ToString()).GetComponent<SpriteRenderer>().sortingOrder = 10;
                            return;
                        }
                    }
                    else
                    {
                        seq = 0;
                    }
                    y++;
                }
            }
            else if (j > i)                                                                     // take left t right bottom to top
            {
                int x = 0;
                int y = j - i;
                int seq = 0;

                for (int k = y; k < board.GetLength(1); k++)
                {
                    if (board[x, k] == player)
                    {
                        seq++;

                        if (seq == pair)
                        {
                            GameObject.Find("SpawnDot" + (x).ToString() + "," + (k).ToString()).GetComponent<SpriteRenderer>().sortingOrder = 10;
                            GameObject.Find("SpawnDot" + (x - 1).ToString() + "," + (k - 1).ToString()).GetComponent<SpriteRenderer>().sortingOrder = 10;
                            GameObject.Find("SpawnDot" + (x - 2).ToString() + "," + (k - 2).ToString()).GetComponent<SpriteRenderer>().sortingOrder = 10;
                            GameObject.Find("SpawnDot" + (x - 3).ToString() + "," + (k - 3).ToString()).GetComponent<SpriteRenderer>().sortingOrder = 10;
                            return;
                        }
                    }
                    else
                    {
                        seq = 0;
                    }
                    x++;
                }
            }

            if (i + j > 5)
            {
                int seq = 0;
                int k = (i + j) - 6;                                   // know to value of colume
                int m = 6;

                for (int x = k; x < board.GetLength(0); x++)
                {
                    if (board[x, m] == player)
                    {
                        seq++;

                        if (seq == pair)
                        {
                            GameObject.Find("SpawnDot" + (x).ToString() + "," + (m).ToString()).GetComponent<SpriteRenderer>().sortingOrder = 10;
                            GameObject.Find("SpawnDot" + (x - 1).ToString() + "," + (m + 1).ToString()).GetComponent<SpriteRenderer>().sortingOrder = 10;
                            GameObject.Find("SpawnDot" + (x - 2).ToString() + "," + (m + 2).ToString()).GetComponent<SpriteRenderer>().sortingOrder = 10;
                            GameObject.Find("SpawnDot" + (x - 3).ToString() + "," + (m + 3).ToString()).GetComponent<SpriteRenderer>().sortingOrder = 10;
                            return;
                        }
                    }
                    else
                    {
                        seq = 0;
                    }
                    m--;
                }
            }
            else
            {
                int n = i + j;
                int m = n;
                int seq = 0;
                for (int x = 0; x <= n; x++)
                {
                    if (board[x, m] == player)
                    {
                        seq++;

                        if (seq == pair)
                        {
                            GameObject.Find("SpawnDot" + (x).ToString() + "," + (m).ToString()).GetComponent<SpriteRenderer>().sortingOrder = 10;
                            GameObject.Find("SpawnDot" + (x - 1).ToString() + "," + (m + 1).ToString()).GetComponent<SpriteRenderer>().sortingOrder = 10;
                            GameObject.Find("SpawnDot" + (x - 2).ToString() + "," + (m + 2).ToString()).GetComponent<SpriteRenderer>().sortingOrder = 10;
                            GameObject.Find("SpawnDot" + (x - 3).ToString() + "," + (m + 3).ToString()).GetComponent<SpriteRenderer>().sortingOrder = 10;
                            return;
                        }
                    }
                    else
                    {
                        seq = 0;
                    }
                    m--;
                }
            }

        }

        #endregion
    }
}