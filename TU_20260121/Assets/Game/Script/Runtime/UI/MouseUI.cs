using TMPro;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.InputSystem;
using VContainer;
using Game;
using Game.Redux;
using Game.Runtime.ReduxUtility;
using R3;

namespace Game.Runtime
{
    public class MouseUI : MonoBehaviour
    {
        [SerializeField] GameObject mouseNormalObject;
        [SerializeField] GameObject mouseRightClickObject;

        float maxSpeed;
        float minSpeed;
        float speedRange;

        internal Store<GamePlayerState> _gamePlayerStateStore;
        internal GameConfig _gameConfig;
        internal PlayerConfig _playerConfig;

        [Inject]
        internal void Construct(
            Store<GamePlayerState> gamePlayerStateStore,
            GameConfig gameConfig,
            PlayerConfig playerConfig)
        {
            _gamePlayerStateStore = gamePlayerStateStore;
            _gameConfig = gameConfig;
            _playerConfig = playerConfig;
        }

        void Start()
        {
            maxSpeed = _gameConfig.Speed * _playerConfig.SpeedRateCurve.Evaluate(2f);
            minSpeed = _gameConfig.Speed * _playerConfig.SpeedRateCurve.Evaluate(-1f);

            speedRange = maxSpeed - minSpeed;
        }

        void Update()
        {
            var mouseText = mouseNormalObject.GetComponentInChildren<TextMeshProUGUI>();
            
            // 右クリックされていないときだけ表示
            bool rightClick = Input.GetMouseButton(1);
            if (mouseNormalObject.activeSelf == rightClick)
                mouseNormalObject.SetActive(!rightClick);
            
            if(mouseRightClickObject.activeSelf != rightClick)
                mouseRightClickObject.SetActive(rightClick);
            
            if(mouseNormalObject.activeSelf == true)
            {
                mouseText.text = _gamePlayerStateStore.State.CurrentValue.JumpCount.ToString();
                if(0f < _gamePlayerStateStore.State.CurrentValue.NotJumpableTime)
                    mouseText.text = "X";
            }

            if(mouseRightClickObject.activeSelf == true)
            {
                //Speedが-1から1までの範囲で変化するためこのようなコードになっている
                mouseRightClickObject.GetComponent<Image>().fillAmount = (_gamePlayerStateStore.State.CurrentValue.Speed - minSpeed) / speedRange;
            }


            if(_gamePlayerStateStore.State.CurrentValue.IsGameOver)
            {
                mouseNormalObject.SetActive(false);
                mouseRightClickObject.SetActive(false);
            }
        }
    }
}