using VContainer;
using VContainer.Unity;
using UnityEngine;

namespace Game.LifetimeScopes
{
    /// <summary>
    /// ゲーム全体で共有される依存関係を管理
    /// </summary>
    public class RootLifetimeScope : LifetimeScope
    {
        protected override void Configure(IContainerBuilder builder)
        {
            // ここにグローバルな依存関係を登録
            // 例:  SaveDataRepository, AudioManager など
        }
    }
}
