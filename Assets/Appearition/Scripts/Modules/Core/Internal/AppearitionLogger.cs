// -----------------------------------------------------------------------
// Company:"Appearition Pty Ltd"
// File: AppearitionLogger.cs
// Copyright (c) 2019. All rights reserved.
// -----------------------------------------------------------------------

using UnityEngine;

namespace Appearition
{
    public static class AppearitionLogger
    {
        public delegate void MessageLogged(object message, LogLevel logType);

        public static event MessageLogged OnMessageLogged;

        /// <summary>
        /// Defines all the types of debug available for debugging. 
        /// This enum is used for both setting what log type if visible, and what log type the current debug is.
        /// </summary>
        public enum LogLevel
        {
            None = -1,
            Debug = 0,
            Info = 1,
            Warning = 2,
            Error = 3
        }

        /// <summary>
        /// Logs a message at a Debug level.
        /// </summary>
        /// <param name="message">Message.</param>
        public static void LogDebug(object message)
        {
            Log(message, LogLevel.Debug);
        }

        /// <summary>
        /// Logs a message at an Info level.
        /// </summary>
        /// <param name="message">Message.</param>
        public static void LogInfo(object message)
        {
            Log(message, LogLevel.Info);
        }

        /// <summary>
        /// Logs a message at a Warning level.
        /// </summary>
        /// <param name="message">Message.</param>
        public static void LogWarning(object message)
        {
            Log(message, LogLevel.Warning);
        }

        /// <summary>
        /// Logs a message at a Error level.
        /// </summary>
        /// <param name="message">Message.</param>
        public static void LogError(object message)
        {
            Log(message, LogLevel.Error);
        }

        /// <summary>
        /// Logs a message to the console.
        /// </summary>
        /// <param name="message">Message.</param>
        /// <param name="logType">Log type.</param>
        public static void Log(object message, LogLevel logType)
        {
            //Event up!
            if (OnMessageLogged != null)
                OnMessageLogged(message, logType);

            //Forbid inferior logs.
            if (logType < AppearitionGate.LogLevel)
                return;

            switch (logType)
            {
                case LogLevel.Debug:
                    Debug.Log(message);
                    break;
                case LogLevel.Info:
                    Debug.Log(message);
                    break;
                case LogLevel.Warning:
                    Debug.LogWarning(message);
                    break;
                case LogLevel.Error:
                    Debug.LogError(message);
                    break;
            }
        }
    }
}