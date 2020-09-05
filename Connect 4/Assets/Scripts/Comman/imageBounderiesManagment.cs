using UnityEngine;
using UnityEngine.UIElements;

namespace Connect4.Comman.Bounderies
{
    public class imageBounderiesManagment : MonoBehaviour
    {

        Vector3 Boundries;
        Vector3 pixels;
        [SerializeField] Transform board;
        [SerializeField] RectTransform header;
        [SerializeField] RectTransform titleButtons;
        [SerializeField] Transform LevelSelction;

        void Start()
        {
            Boundries = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, transform.position.z));
            pixels.x = Camera.main.pixelWidth;
            pixels.y = Camera.main.pixelHeight;
            makePositions();
        }

        void makePositions()
        {
            if (Boundries.x > 5.3)
            {
                board.localScale = new Vector3(Boundries.x / 10.6f, Boundries.y / 20, 0); ;
                board.position = new Vector3(Boundries.x / 2, Boundries.y / 2.3f, 0);
            }
            else
            {
                board.localScale = new Vector3(Boundries.x / 11.4f, Boundries.y / 21, 0); ;
                board.position = new Vector3(Boundries.x / 2, Boundries.y / 2.3f, 0);
                LevelSelction.localScale = new Vector3(0.9f, 0.9f, 1);
            }

            if (pixels.y == 2160)
            {
                header.sizeDelta = new Vector2(960, 180);
                titleButtons.localScale = new Vector3(0.9f, 0.9f, 1);

            }
            else if (pixels.y == 2960)
            {
                header.sizeDelta = new Vector2(940, 180);
                titleButtons.localScale = new Vector3(0.9f, 0.9f, 1);
            }
            else
            {
                header.sizeDelta = new Vector2(1080, 180);
                titleButtons.sizeDelta = new Vector2(100, 100);
            }
        }
    }
}