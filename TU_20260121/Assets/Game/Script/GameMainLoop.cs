using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using VContainer.Unity;
using UnityEngine;
using UnityEngine.UI;
using Game;
using Game.LifetimeScopes;
using Game.Redux;
using Game.Runtime.ReduxUtility;
using System.Collections.Generic;
using System.Diagnostics;

namespace Game.Runtime
{
    internal sealed class GameMainLoop : IAsyncStartable//神クラス:VCountainerがよくわかってないせい
    {
        float RightScreenPositionOffset = 7.66f;

        ///
        private readonly PlayerConfig _playerConfig;
        private readonly GameConfig _gameConfig;
        private readonly EnemyConfig _enemyConfig;
        private readonly ItemConfig _itemConfig;
        private readonly List<UIData> _playSceneUI;
        private readonly GameObject _playerObject;
        private readonly GameObject _cameraObject;
        private readonly RawImage _background;
        private readonly Store<GameState> _gameStateStore;
        private readonly Store<GameGlobalState> _gameGlobalStore;
        private readonly Store<GamePlayerState> _gamePlayerStateStore;
        private readonly int _groundLayerMask; // Groundレイヤー
        private readonly int _enemyLayerMask;  // Enemyレイヤー

        // コンストラクタで受け取る（VContainerが自動注入）
        public GameMainLoop(
            PlayerConfig playerConfig,
            GameConfig gameConfig,
            EnemyConfig enemyConfig,
            ItemConfig itemConfig,
            List<UIData> playSceneUI,
            GameObject playerObject,
            GameObject cameraObject,
            RawImage background,
            Store<GameState> gameStateStore,
            Store<GameGlobalState> gameGlobalStore,
            Store<GamePlayerState> gamePlayerStateStore,
            int groundLayerMask,
            int enemyLayerMask)
        {
            _playerConfig = playerConfig;
            _gameConfig = gameConfig;
            _enemyConfig = enemyConfig;
            _itemConfig = itemConfig;
            _playSceneUI = playSceneUI;
            _playerObject = playerObject;
            _cameraObject = cameraObject;
            _background = background;
            _gameStateStore = gameStateStore;
            _gameGlobalStore = gameGlobalStore;
            _gamePlayerStateStore = gamePlayerStateStore;
            _groundLayerMask = groundLayerMask;
            _enemyLayerMask = enemyLayerMask;
        }

        public async UniTask StartAsync(CancellationToken ctx)
        {
            // ゲームメインループの処理をここに記述
            _gamePlayerStateStore.Dispatch(new GamePlayerState(false, 0f, false, 0f, 0f, false));
            _gameGlobalStore.Dispatch(new GameGlobalState(0, 0, 0));
            _gameStateStore.Dispatch(new GameState(0, _gameConfig.TimeLimit, false));
            
            ChangeUITextOnPlayScene();
            _playSceneUI.Find(ui => ui.Type == UIType.GameOver).Object.SetActive(false);

            /// 入場
            await GameStarting(ctx);
            HideReadyCount(ctx).Forget();
            
            /// プレイ中
            EnemyGenerator(ctx).Forget();
            ItemGenerator(ctx).Forget();
            await PlayAsync(ctx);

            /// 退場
            
            /// リザルトへ移行
            SceneLoader.SceneLoad("ResultScene");
        }

        private async UniTask GameStarting(
            CancellationToken ctx)
        {
            // 初期位置設定
            _playerObject.transform.position = new Vector3(-9f, _playerConfig.InitialPosition.y, 0f);
            var _entranceTime = _gameConfig.EntranceTime;
            var _entranceSpeed = (_playerConfig.InitialPosition.x + 9f) / _entranceTime;
            Animator anim = _playerObject.GetComponent<Animator>();
            anim.speed = _playerConfig.EntranceAnimSpeed;

            var _readyCount = _playSceneUI.FindAll(ui => ui.Type == UIType.ReadyCount);
            foreach (var ui in _readyCount)
            {
                ui.Object.SetActive(true);
                ui.Text.color = new Color(ui.Text.color.r, ui.Text.color.g, ui.Text.color.b, 1f);
            }
            
            while (_entranceTime > 0 && !ctx.IsCancellationRequested)
            {
                _entranceTime -= Time.deltaTime;
                
                // カウントダウン表示（小数点切り上げ）
                int countdown = Mathf.CeilToInt(_entranceTime);
                foreach (var ui in _readyCount)
                    ui.Text.text = countdown.ToString();
                
                _playerObject.transform.position = new Vector2(
                    _playerObject.transform.position.x + _entranceSpeed * Time.deltaTime,
                    _playerObject.transform.position.y);
                await UniTask.Yield(ctx);
            }

            foreach (var ui in _readyCount)
                ui.Text.text = "Go!";
            _gameStateStore.Dispatch(new ChangeIsRunningAction(true));
        }

        private async UniTask HideReadyCount(
            CancellationToken ctx)
        {
            // "Go!"を表示する時間を確保
            UnityEngine.Debug.Log($"HideReadyCount開始 待機時間:{_gameConfig.ShowReadyCountTime*0.5f}秒");
            await UniTask.Delay(TimeSpan.FromSeconds(_gameConfig.ShowReadyCountTime*0.3f), cancellationToken: ctx);
            UnityEngine.Debug.Log("フェードアウト開始");
            
            var _showReadyCountTime = _gameConfig.ShowReadyCountTime*0.7f;
            var _readyCount = _playSceneUI.FindAll(ui => ui.Type == UIType.ReadyCount);
            
            while (_showReadyCountTime > 0 && !ctx.IsCancellationRequested)
            {
                _showReadyCountTime -= Time.deltaTime;
                var alpha = _showReadyCountTime / (_gameConfig.ShowReadyCountTime*0.7f);
                foreach (var ui in _readyCount)
                    ui.Text.color = new Color(ui.Text.color.r, ui.Text.color.g, ui.Text.color.b, alpha);
                await UniTask.Yield(ctx);
            }
            UnityEngine.Debug.Log("ReadyCount非表示");

            foreach (var ui in _readyCount)
                ui.Object.SetActive(false);
        }

        private async UniTask EnemyGenerator(CancellationToken ctx)
        {
            var TimeLimit = _gameConfig.TimeLimit;
            var NowTime = 0f;
            var enemyGenerateProbableCurve = _enemyConfig.EnemyAppearRateCurve;
            var enemyGenerateList = _enemyConfig.GenerateEnemyDataList;

            // 敵生成ロジックをここに記述
            while (!ctx.IsCancellationRequested)
            {
                if(NowTime >= TimeLimit && _gamePlayerStateStore.State.CurrentValue.IsGameOver)
                    break;
                
                // 敵を生成する処理
                NowTime += Time.deltaTime;
                GenerateEnemy(enemyGenerateProbableCurve.Evaluate(NowTime / TimeLimit));
                //GenerateEnemy(0.1f);
                await UniTask.Yield(ctx);
            }

            void GenerateEnemy(float enemyGenerateProbability = 0.02f)
            {
                enemyGenerateProbability *= _enemyConfig.EnemyAppearRate;
                var rand = UnityEngine.Random.Range(0f, 1f);

                // 敵生成の具体的な実装をここに記述
                var generateEnemyPositionX = _gameStateStore.State.CurrentValue.GameCenter + 11f;
                for(int i = 0; i < enemyGenerateList.Length; i++)
                {
                    var enemyData = enemyGenerateList[i];

                    if(rand <= enemyGenerateProbability * enemyData.Probability)
                    {
                        var enemy = UnityEngine.Object.Instantiate(
                            enemyData.Obj,
                            new Vector3(
                                generateEnemyPositionX,
                                LowestPoint(generateEnemyPositionX).y + enemyData.Obj.transform.localScale.y/2f,
                                0f),
                            Quaternion.identity);

                        switch(enemyData.Type)
                        {
                            case EnemyType.Dust:
                                break;
                            case EnemyType.Bird:
                                enemy.GetComponent<CrowGenerator>().Init(_gameStateStore, _enemyConfig);
                                var yOffset = UnityEngine.Random.Range(LowestPoint(generateEnemyPositionX).y, 4f);
                                enemy.transform.position = new Vector3(
                                    enemy.transform.position.x,
                                    yOffset,
                                    enemy.transform.position.z);
                                //generateEnemyPositionX += UnityEngine.Random.Range(1f, 3f);
                                break;
                            case EnemyType.Mole:
                                enemy.GetComponent<MoleGenerator>().Init(_gameStateStore, _playerObject, _enemyConfig);
                                break;
                            default:
                                break;
                        }

                        UnityEngine.Debug.Log($"敵{i}を生成しました: 確率 {enemyGenerateProbability} (乱数: {rand})");
                        break; // 一度に一体だけ生成
                    }
                }
                
            }
        }
        
        private async UniTask ItemGenerator(CancellationToken ctx)
        {
            var itemGenerateProbability = _itemConfig.ItemAppearRate;

            while (!ctx.IsCancellationRequested)
            {
                // アイテム生成の具体的な実装をここに記述
                var rand = UnityEngine.Random.Range(0f, 1f);

                // 敵生成の具体的な実装をここに記述
                var generateEnemyPositionX = _gameStateStore.State.CurrentValue.GameCenter + 11f;
                if(rand <= itemGenerateProbability)
                {
                    UnityEngine.Object.Instantiate(
                        _itemConfig.GenerateItemPrefab,
                        new Vector3(
                            generateEnemyPositionX,
                            LowestPoint(generateEnemyPositionX).y + _itemConfig.GenerateItemPrefab.transform.localScale.y/2f + _itemConfig.GenerateItemOffsetY,
                            0f),
                        Quaternion.identity);
                }
                await UniTask.Yield(ctx);
            }
        }

        private Vector2 LowestPoint(float x)
        {
            // 基準となる地点
            Vector2 origin = new Vector2(x, 10f);
            int layerMask = (1 << _groundLayerMask) | (1 << _enemyLayerMask);
            // 下方向に十分長いRayを飛ばす
            float maxDistance = 20f;
            RaycastHit2D hit = Physics2D.Raycast(origin, Vector2.down, maxDistance, layerMask);

            // 何にも当たらなければ、maxDistance分下が最低点
            Vector2 lowestPoint;
            if (hit.collider == null)
            {
                lowestPoint = origin + Vector2.down * maxDistance;
            }
            else
            {
                lowestPoint = hit.point; // 何かに当たった場合はその地点
            }

            return lowestPoint;
        }

        private async UniTask PlayAsync(
            CancellationToken ctx)
        {
            
            float _scrollSpeed = _gameConfig.BackgroundScrollSpeed;
            float _currentOffsetX = 0f;
            var _playerMover = _playerObject.GetComponent<PlayerMover>();
            var anim = _playerObject.GetComponent<Animator>();
            anim.speed = _playerConfig.AnimationSpeed;

            List<GameObject> _playerAfterimage = new ();
            var _afterimageInterval = _playerConfig.AfterimageInterval;
            var _afterimageCount = _playerConfig.AfterimageCount;

            // Updateのように毎フレーム実行されるループ
            while (!ctx.IsCancellationRequested)
            {
                // ここに毎フレームの処理を書く
                _cameraObject.transform.position = new Vector3(_gameStateStore.State.CurrentValue.GameCenter, _cameraObject.transform.position.y, _cameraObject.transform.position.z);
                _gameStateStore.Dispatch(new ChangeTimeLimitAction(-Time.deltaTime));
                _gameStateStore.Dispatch(new ChangeCenterAction(_gameConfig.Speed));
                
                var speedUpRate = 1f;

                #region ScrollBackground
                // スクロールオフセットを計算
                _currentOffsetX += _scrollSpeed * Time.deltaTime;
                
                // UVRectを更新してスクロール
                Rect uvRect = _background.uvRect;
                uvRect.x = _currentOffsetX;
                _background.uvRect = uvRect;
                #endregion

                #region UpdateInvincible
                if(0 < _gamePlayerStateStore.State.CurrentValue.InvincibleTime)
                {
                    
                    // 無敵中
                    if(_gamePlayerStateStore.State.CurrentValue.UseInvincibleItem)
                    {
                        // 無敵アイテム使用中
                        var playerSprite = _playerObject.GetComponent<SpriteRenderer>();
                        float flickerFrequency = _playerConfig.InvincibleItemFlickerFrequency; // 点滅の速さ
                        float alpha = (Mathf.Sin(Time.time * flickerFrequency) + 1f) / 2f; // 0から1の範囲で変化
                        playerSprite.color = new Color(1f, 1f, 0f, alpha);
                    }
                    else
                    {
                        // 敵からのダメージによる無敵中
                        var playerSprite = _playerObject.GetComponent<SpriteRenderer>();
                        playerSprite.color = new Color(1f, 0f, 0f, 0.5f);
                        speedUpRate *= _playerConfig.DamagedSpeedRate;
                    }
                    _gamePlayerStateStore.Dispatch(new UpdateInvincibleAction(Time.deltaTime));
                }
                else
                {
                    // 無敵終了
                    var playerSprite = _playerObject.GetComponent<SpriteRenderer>();
                    playerSprite.color = Color.white;
                    _gamePlayerStateStore.Dispatch(new PlayerInvincibleEndAction());
                }
                #endregion

                #region PlayerJumpableUpdate
                if(0 < _gamePlayerStateStore.State.CurrentValue.NotJumpableTime)
                {
                    // ジャンプ不可時間更新
                    _gamePlayerStateStore.Dispatch(new PlayerJumpableAction(_gamePlayerStateStore.State.CurrentValue.NotJumpableTime - Time.deltaTime));
                    UnityEngine.Debug.Log($"ジャンプ不可時間残り: {_gamePlayerStateStore.State.CurrentValue.NotJumpableTime}");
                }
                #endregion

                #region PlayerSpeedUpUpdate
                if(0 < _gamePlayerStateStore.State.CurrentValue.SpeedUpTime)
                {
                    // スピードアップ中
                    _gamePlayerStateStore.Dispatch(new PlayerSpeedUpAction(_gamePlayerStateStore.State.CurrentValue.SpeedUpTime - Time.deltaTime));
                    speedUpRate = _playerConfig.SpeedUpRate;

                    var playerSprite = _playerObject.GetComponent<SpriteRenderer>();
                    float flickerFrequency = _playerConfig.InvincibleItemFlickerFrequency; // 点滅の速さ
                    float alpha = (Mathf.Sin(Time.time * flickerFrequency) + 1f) / 2f; // 0から1の範囲で変化
                    playerSprite.color = new Color(0.5f, 1f, 1f, alpha);

                    //UnityEngine.Debug.Log($"スピードアップ時間残り: {_gamePlayerStateStore.State.CurrentValue.SpeedUpTime}");
                }
                #endregion
                
                #region PlayerControll
                // 地面に着地したらジャンプ回数リセット
                if (_playerMover._groundCheck.CheckGround())
                {
                    _playerMover._jumpCount = _playerConfig.MaxJumpCount;
                }

                // 横移動
                var xSpeed = _gameConfig.Speed * speedUpRate;
                if (_playerMover.IsWallTouch() || _gamePlayerStateStore.State.CurrentValue.IsGameOver)
                {
                    xSpeed = 0;
                }
                  
                _playerObject.transform.position = new Vector2(
                    _playerObject.transform.position.x + xSpeed,
                    _playerObject.transform.position.y);
                
                if(_playerObject.transform.position.x - _gameStateStore.State.CurrentValue.GameCenter > RightScreenPositionOffset)
                {
                    _playerObject.transform.position = new Vector2(
                    _gameStateStore.State.CurrentValue.GameCenter + RightScreenPositionOffset,
                    _playerObject.transform.position.y);
                }

                if(_playerAfterimage.Count >= _afterimageCount)
                {
                    while(_playerAfterimage.Count >= _afterimageCount)
                    {
                        // 一番古い残像（最初の要素）を削除
                        var oldestAfterimage = _playerAfterimage[0];
                        _playerAfterimage.RemoveAt(0);
                        UnityEngine.Object.Destroy(oldestAfterimage);
                    }
                }

                _afterimageInterval -= Time.deltaTime;
                if(_afterimageInterval <= 0f)
                {
                    var afterimage = new GameObject("Afterimage");
                    afterimage.transform.position = new Vector3(
                        _playerObject.transform.position.x,
                        _playerObject.transform.position.y,
                        _playerObject.transform.position.z + 0.1f);
                    afterimage.transform.rotation = _playerObject.transform.rotation;
                    afterimage.transform.localScale = _playerObject.transform.localScale;
                    var spriteRenderer = afterimage.AddComponent<SpriteRenderer>();
                    spriteRenderer.sprite = _playerObject.GetComponent<SpriteRenderer>().sprite;
                    spriteRenderer.material.SetColor("_Color", new Color(1f, 0.75f, 0.8f, 0.5f));
                    _playerAfterimage.Add(afterimage);
                    _afterimageInterval = _playerConfig.AfterimageInterval;
                }

                // デスゾーンチェック
                var gameCenter = _gameStateStore.State.CurrentValue.GameCenter;
                if (_playerObject.transform.position.x - gameCenter < _gameConfig.DeathZoneOffset)
                {
                    // ゲームオーバー処理
                    _gamePlayerStateStore.Dispatch(new PlayerGameOverAction());
                    UnityEngine.Debug.Log("デスゾーン到達！ゲームオーバー");
                    await GameOver(ctx);
                }
                #endregion

                ChangeUITextOnPlayScene();

                if(_gameStateStore.State.CurrentValue.TimeLimit < 0f) break;
                if(_gamePlayerStateStore.State.CurrentValue.IsGameOver) break;
                
                // 次のフレームまで待機
                await UniTask.Yield(ctx);
            }
        }

        private async UniTask GameOver(
            CancellationToken ctx)
        {
            // 退場演出などをここに記述
            var _gameOver = _playSceneUI.Find(ui => ui.Type == UIType.GameOver);
            _gameOver.Object.SetActive(true);
            var fadeTime = 2f;

            while (fadeTime > 0 && !ctx.IsCancellationRequested)
            {
                fadeTime -= Time.deltaTime;
                await UniTask.Yield(ctx);
            }
        }

        private void ChangeUITextOnPlayScene()
        {
            foreach (var ui in _playSceneUI)
            {
                switch (ui.Type)
                {
                    case UIType.TimeLimit:
                        ui.Text.text = ui.Prefix + _gameStateStore.State.CurrentValue.TimeLimit.ToString("F2");
                        break;
                    case UIType.ItemCount:
                        ui.Text.text = ui.Prefix + _gameGlobalStore.State.CurrentValue.ItemCount.ToString();
                        break;
                    case UIType.ClearItemCount:
                        ui.Text.text = ui.Prefix + _gameGlobalStore.State.CurrentValue.ClearItemCount.ToString();
                        break;
                    case UIType.JumpCount:
                        ui.Text.text = ui.Prefix + (_playerObject.GetComponent<PlayerMover>()._jumpCount + 1).ToString();
                        break;
                    case UIType.ReadyCount:
                        // ReadyCountは別途処理
                        break;
                    case UIType.GameOver:
                        // GameOverは別途処理
                        break;
                    
                    default:
                        ui.Text.text = ui.Prefix;
                        break;
                }
                //UnityEngine.Debug.Log($"UI更新: {ui.Type} - {ui.Text.text}");
            }
        }
    }
}