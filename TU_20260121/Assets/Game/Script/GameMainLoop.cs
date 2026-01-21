using System;
using System.Linq;
using System.Threading;
using Cysharp.Threading.Tasks;
using VContainer.Unity;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Game.Runtime
{
    internal sealed class GameMainLoop : IAsyncStartable
    {
        public async UniTask StartAsync(CancellationToken cancellationToken)
        {
            // ゲームメインループの処理をここに記述
            await UniTask.Yield(cancellationToken);
        }
    }
}