using System;
using Trusense.Tools;
using UnityEngine.Events;

namespace Trusense.Managers
{
    /// <summary>
    /// EventManager is a singleton class that manages events in the Trusense application.
    /// It can be used to register, unregister, and invoke events throughout the application.
    /// 
    /// Author: Nguyễn Duy Khánh
    /// Created: August 6, 2025
    /// Last Modified: August 6, 2025
    /// Version: 1.0.0
    /// </summary>
    public class EventManager : Singleton<EventManager>
    {
        public UnityEvent<string> OnLanguageChanged = new UnityEvent<string>();
    }

}