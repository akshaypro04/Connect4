
using UnityEngine;

namespace Connect4.Player.Imput
{
    public class PlayerController : MonoBehaviour
    {
        [HideInInspector]
        public bool press;
        [HideInInspector]
        public Vector2 touch;


        void Update()
        {
            press = Input.GetButtonUp("Fire1");
            touch = Input.mousePosition;
        }
    }
}