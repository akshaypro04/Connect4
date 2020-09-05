using Connect4.Comman.Game;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Connect4.Player.DotMove
{
    public class dotsMovement : MonoBehaviour
    {

        private int m_row;
        public int row
        {
            set
            {
                m_row = value;
            }
            get
            {
                return m_row;
            }
        }

        private int m_col;
        public int col
        {
            set
            {
                m_col = value;
            }
            get
            {
                return m_col;
            }
        }

        bool found;
        Transform target;
        Rigidbody2D rd;
        float posY;

        void Start()
        {
            rd = GetComponent<Rigidbody2D>();
        }


        void Update()
        {
            if (row != null && col != null && found == false)
            {
                target = GameObject.Find("dotsParents/" + row + "," + col + "").GetComponent<Transform>();
                found = true;
            }

            if (target != null)
            {
                if (transform.position.y <= target.position.y)
                {
                    GameManager.Instances.play("drop");
                    posY = target.position.y;
                    transform.position = target.position;
                    rd.constraints = RigidbodyConstraints2D.FreezeAll;
                    GameManager.Instances.SetTurn(!GameManager.Instances.getTurn());
                    target = null;
                }
            }

        }
    }
}