using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Appearition.Analytics.Activities
{
    /// <summary>
    /// Main activity for a classic application UI menu. Should contains a name, as well as what previous menu triggered it.
    /// Also contains events concerning what UI is interacted with, and how this App Panel was closed.
    /// </summary>
    public class AppPanelActivity : Activity
    {
        //Shared constants
        const string NEXT_PANEL_NAME = "NEXT_PANEL_NAME";

        public AppPanelActivity(string panelName) : this("", panelName)
        {
        }

        public AppPanelActivity(string previousPanelName, string panelName)
        {
            if (!string.IsNullOrEmpty(previousPanelName))
                ActivityData.Add(new AnalyticsData("PREVIOUS_PANEL_NAME", previousPanelName));
            ActivityData.Add(new AnalyticsData("PANEL_NAME", panelName));

            ActivityData.Add(new AnalyticsData($"{DeviceInfoConstants.DEVICE_INFO_BATTERY_STATUS}_START", SystemInfo.batteryStatus.ToString()));
            ActivityData.Add(new AnalyticsData($"{DeviceInfoConstants.DEVICE_INFO_BATTERY_LEVEL}_START", SystemInfo.batteryLevel.ToString()));

            FinalizeActivityCreation();
        }

        public override void SubmitActivity()
        {
            //Try to find the next panel. If found, bind it!
            if (Events.Count > 0)
            {
                ActivityEvent nextPanelEvent =
                    Events.FirstOrDefault(o => o.EventCode == ActivityEventContainer<GoToAppPanel>.ConvertContainerTypeToEventCode<GoToAppPanel>());
                if (nextPanelEvent != null)
                    ActivityData.Add(nextPanelEvent.EventData.First(o => o.Key == NEXT_PANEL_NAME));
            }

            ActivityData.Add(new AnalyticsData($"{DeviceInfoConstants.DEVICE_INFO_BATTERY_STATUS}_END", SystemInfo.batteryStatus.ToString()));
            ActivityData.Add(new AnalyticsData($"{DeviceInfoConstants.DEVICE_INFO_BATTERY_LEVEL}_END", SystemInfo.batteryLevel.ToString()));

            base.SubmitActivity();
        }

        #region Event Containers

        /// <summary>
        /// Should occur when a UI Element has been interacted with.
        /// The first parameter should be the type of UI, such as button, slider, etc. The second parameter should be the name of the UI element.
        /// </summary>
        public class UiElementInteracted : ActivityEventContainer<UiElementInteracted>
        {
            public UiElementInteracted(string typeOfUi, string uiElementName)
            {
                analyticEvents.Add(new AnalyticsData("TYPE_OF_UI_OBJECT", typeOfUi));
                analyticEvents.Add(new AnalyticsData("UI_ELEMENT_NAME", uiElementName));
            }
        }

        /// <summary>
        /// Should occur when the application is about to swap to a new UI panel, as well as how the transition was made (button click, etc).
        /// This even most likely should be the last of this activity.
        /// If this event is part of this activity, the activity will take that information as part of its ActivityData.
        /// </summary>
        public class GoToAppPanel : ActivityEventContainer<GoToAppPanel>
        {
            public GoToAppPanel(string newUiPanelName, string method)
            {
                analyticEvents.Add(new AnalyticsData(NEXT_PANEL_NAME, newUiPanelName));
                analyticEvents.Add(new AnalyticsData("PANEL_CHANGE_METHOD", method));
            }
        }

        public class CustomUserAction : ActivityEventContainer<CustomUserAction>
        {
            public CustomUserAction(string actionType, string actionOutcome)
            {
                analyticEvents.Add(new AnalyticsData("USER_ACTION_TYPE", actionType));
                analyticEvents.Add(new AnalyticsData("USER_ACTION_OUTCOME", actionOutcome));
            }
        }

        #endregion
    }
}