using System;
using Trusense.Tools;
using Unity.Services.Core;
using Unity.Services.Core.Environments;
using UnityEngine;

namespace Trusense.Managers
{
    public class GameManager : Singleton<GameManager>
    {
        public string environment = "production";
        protected async override void Awake()
        {
            base.Awake();
            try
            {
                var options = new InitializationOptions().SetEnvironmentName(environment);
                await UnityServices.InitializeAsync();
            }
            catch (Exception error)
            {
                Debug.LogException(error);
            }
        }
    }
}