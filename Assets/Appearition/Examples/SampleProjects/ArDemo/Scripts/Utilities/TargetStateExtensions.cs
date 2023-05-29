using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Appearition.ArDemo
{
    public static class TargetStateExtensions
    {
        public static bool IsTracking(this AppearitionArHandler.TargetState state)
        {
            return (state & AppearitionArHandler.TargetState.Tracking) != 0;
        }

        public static bool IsNotTracking(this AppearitionArHandler.TargetState state)
        {
            return !state.IsTracking();
        }
    }
}