using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace Assets
{
    public class Manager : MonoBehaviour
    {
        public static Manager Instance => _instance;

        private int _collectibleCount;
        public int collectibleCount {
            get => _collectibleCount; set {
                _collectibleCount = value;
                scoreText.text = $"Score: {_collectibleCount}";
            }
        }
        public Text scoreText;

        public bool gameWon { get; } = false;

        public GameObject gameOverUi;
        public GameObject winningUi;
        private static Manager _instance;

        public void Awake()
        {
            if (_instance != null && _instance != this)
            {
                Destroy(gameObject);
            }
            else
            {
                _instance = this;
            }
        }
    }
}
