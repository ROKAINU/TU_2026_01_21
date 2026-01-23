using System;
using VContainer;
using VContainer.Unity;
using UnityEngine;
using UnityEngine.UI;
using Game;
using Game.Redux;
using Game.Runtime;
using Game.Runtime.ReduxUtility;
using System.Collections.Generic;

namespace Game.LifetimeScopes
{
    public class PlaySceneLifetimeScope : LifetimeScope
    {
        [Header("Configs")]
        [SerializeField] private PlayerConfig playerConfig;
        [SerializeField] private GameConfig gameConfig;
        [SerializeField] private EnemyConfig enemyConfig;
        [SerializeField] private ItemConfig itemConfig;
        
        [Header("UI Data")]
        [SerializeField] private List<UIData> dataList;
        
        [Header("Game Objects")]
        [SerializeField] private GameObject playerObject;
        [SerializeField] private GameObject cameraObject;
        [SerializeField] private RawImage background;
        [Header("Layers")]
        [SerializeField] private int groundLayerMask;
        [SerializeField] private int enemyLayerMask;

        protected override void Configure(IContainerBuilder builder)
        {
            // null チェック
            if (playerConfig == null) Debug.LogError("PlayerConfig が設定されていません");
            if (gameConfig == null) Debug.LogError("GameConfig が設定されていません");
            if (itemConfig == null) Debug.LogError("ItemConfig が設定されていません");
            if (enemyConfig == null) Debug.LogError("EnemyConfig が設定されていません");
            if (dataList == null) Debug.LogError("DataList が設定されていません");
            if (playerObject == null) Debug.LogError("PlayerObject が設定されていません");
            if (cameraObject == null) Debug.LogError("CameraObject が設定されていません");
            if (background == null) Debug.LogError("Background が設定されていません");
            if (enemyConfig == null) Debug.LogError("EnemyConfig が設定されていません");

            // ScriptableObject と Component を登録
            builder.RegisterInstance(playerConfig).AsSelf();
            builder.RegisterInstance(gameConfig).AsSelf();
            builder.RegisterInstance(enemyConfig).AsSelf();
            builder.RegisterInstance(itemConfig).AsSelf();
            builder.RegisterInstance(dataList).AsSelf();
            builder.RegisterInstance(background).AsSelf();
            
            // Redux Store を登録（ファクトリメソッドで直接生成）
            builder.Register<Store<GameState>>(resolver => 
                new Store<GameState>(
                    GameState.Default,
                    GameReducer.Reduce
                ), Lifetime.Singleton).AsSelf();

            builder.Register<Store<GameGlobalState>>(resolver => 
                new Store<GameGlobalState>(
                    GameGlobalState.Default,
                    GameGlobalReducer.Reduce
                ), Lifetime.Singleton).AsSelf();

            builder.Register<Store<GamePlayerState>>(resolver => 
                new Store<GamePlayerState>(
                    GamePlayerState.Default,
                    GamePlayerReducer.Reduce
                ), Lifetime.Singleton).AsSelf();

            // PlayerMover と PlayerCollision を登録
            builder.RegisterComponentInHierarchy<PlayerMover>();
            builder.RegisterComponentInHierarchy<PlayerCollision>();

            // エントリーポイントを登録
            // GameMainLoop を登録
            builder.RegisterEntryPoint<GameMainLoop>(resolver => 
                new GameMainLoop(
                    resolver.Resolve<PlayerConfig>(),
                    resolver.Resolve<GameConfig>(),
                    resolver.Resolve<EnemyConfig>(),
                    resolver.Resolve<ItemConfig>(),
                    resolver.Resolve<List<UIData>>(),
                    playerObject,
                    cameraObject,
                    resolver.Resolve<RawImage>(),
                    resolver.Resolve<Store<GameState>>(),
                    resolver.Resolve<Store<GameGlobalState>>(),
                    resolver.Resolve<Store<GamePlayerState>>(),
                    groundLayerMask,
                    enemyLayerMask
                ), Lifetime.Scoped).AsSelf();
        }
    }
}
/*
using VContainer;
using VContainer.Unity;
using UnityEngine;
using Game.Runtime;
using Game.Runtime.Controllers;
using Game.Runtime.Systems;
using Game.Redux;
using Game.Runtime.ReduxUtility;

namespace Game.LifetimeScopes
{
    public class PlaySceneLifetimeScope : LifetimeScope
    {
        [SerializeField] private PlayerConfig playerConfig;
        [SerializeField] private GameConfig gameConfig;
        [SerializeField] private GameObject playerPrefab;
        
        protected override void Configure(IContainerBuilder builder)
        {
            // Config登録
            builder.RegisterInstance(playerConfig);
            builder.RegisterInstance(gameConfig);
            builder.RegisterInstance(playerPrefab);
            
            // Redux Store登録
            builder.Register<Store<GameState>>(Lifetime.Singleton)
                .WithParameter(GameState.Default)
                .WithParameter<System.Func<GameState, object, GameState>>(GameReducer.Reduce);
            
            // Systems登録
            builder.Register<PlayerInputHandler>(Lifetime.Scoped);
            builder.Register<PlayerMover>(Lifetime.Scoped);
            builder.Register<PlayerJumpSystem>(Lifetime.Scoped);
            
            // Controllers登録
            builder.Register<PlayController>(Lifetime.Scoped);
            
            // GameMainLoop登録（エントリーポイント）
            builder.Register<GameMainLoop>(Lifetime.Scoped);
            builder.RegisterEntryPoint<GameMainLoop>();
        }
    }
}
*/