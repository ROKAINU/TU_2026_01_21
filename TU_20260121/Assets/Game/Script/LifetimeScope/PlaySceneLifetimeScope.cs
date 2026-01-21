using VContainer;
using VContainer.Unity;
using UnityEngine;

namespace Game.LifetimeScopes
{
    public class PlaySceneLifetimeScope : LifetimeScope
    {
        [SerializeField] private PlayerConfig playerConfig;
        protected override void Configure(IContainerBuilder builder)
        {
            // ScriptableObject をインスタンスとして登録
            builder.RegisterInstance(playerConfig);
            
            // シーン内のコンポーネントを登録
            // builder.RegisterComponentInHierarchy<PlayerController>();
            
            // エントリーポイントを登録（自動で Start が呼ばれる）
            // builder.RegisterEntryPoint<GameLoop>(Lifetime.Scoped);
        }
    }
}
