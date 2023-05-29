using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Appearition.AccountAndAuthentication;
using Appearition.ChannelManagement;
using UnityEngine;

namespace Appearition.Common
{
    /// <summary>
    /// Requires the user to be logged in.
    /// Finds the most appropriate channel based on a logged in user, and set it as the current channel in the current user on the AppearitionGate.
    /// </summary>
    public class SetUserChannelAsCurrentProcess : ICommonProcess
    {
        Action<int> _onSuccess;
        Action<EmsError> _onFailure;
        Action<bool> _onComplete;

        public SetUserChannelAsCurrentProcess(
            Action<int> onSuccess = null,
            Action<EmsError> onFailure = null,
            Action<bool> onComplete = null)
        {
            //Store variables
            _onSuccess = onSuccess;
            _onFailure = onFailure;
            _onComplete = onComplete;
        }

        public IEnumerator ExecuteMainProcess()
        {
            //Get all channels. If only one, then that's the one.
            List<Channel> channels = new List<Channel>();

            yield return ChannelHandler.GetAllChannelsProcess(onSuccess => channels.AddRange(onSuccess), _onFailure);

            //No channels = some error occured.
            if (channels.Count == 0)
            {
                AppearitionLogger.LogError("No channels were found with the current user. " +
                                           "Perhaps this user was not given any channel on the EMS, or its package was not selected properly.");

                if (_onComplete != null)
                    _onComplete(false);
                yield break;
            }
            else if (channels.Count == 1 && channels[0].channelId != 0)
            {
                //1 channel, it's the one!
                AppearitionLogger.LogInfo("The most appropriate channel for the logged in user was the channel of id " + channels[0].channelId + ".");
                AppearitionGate.Instance.CurrentUser.selectedChannel = channels[0].channelId;
                if (_onSuccess != null)
                    _onSuccess(channels[0].channelId);
                if (_onComplete != null)
                    _onComplete(true);
                yield break;
            }

            //User with access to multiple channels. Find one that matches the username first.
            ExtendedProfile profile = null;
            yield return AccountHandler.GetProfileDataProcess(onSuccess => profile = onSuccess, _onFailure);

            if (profile == null)
            { 
                AppearitionLogger.LogError("An error happened when trying to fetch the user's UserProfile.");
                if (_onComplete != null)
                    _onComplete(false);
                yield break;
            }

            Channel userChannel = channels.FirstOrDefault(o => o.name.Equals(profile.emailAddress, StringComparison.InvariantCultureIgnoreCase));

            //If found, then it's the one! Otherwise..
            if (userChannel == null)
            {
                //Just select the channel with the lowest ID. First channel is most likely a Sample Experiences channel.
                int lowestId = 9999;
                for (int i = 0; i < channels.Count; i++)
                {
                    if (channels[i].channelId < lowestId)
                        lowestId= channels[i].channelId;
                }
                
                userChannel = channels.First(o=>o.channelId == lowestId);
            }

            AppearitionLogger.LogInfo("The most appropriate channel for the logged in user was the channel of id " + userChannel.channelId + ".");
            AppearitionGate.Instance.CurrentUser.selectedChannel = userChannel.channelId;
            if (_onSuccess != null)
                _onSuccess(userChannel.channelId);
            if (_onComplete != null)
                _onComplete(true);
        }
    }
}