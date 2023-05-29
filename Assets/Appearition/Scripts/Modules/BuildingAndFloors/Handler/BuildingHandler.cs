#pragma warning disable 0162
using System.Collections;
using System.Collections.Generic;
using Appearition.Common;
using System;
using System.IO;
using Appearition.BuildingsAndFloors.API;
using Appearition.Common.ObjectExtensions;
using Appearition.Internal;

namespace Appearition.BuildingsAndFloors
{
    /// <summary>
    /// Handler responsible for wrapping the functionalities of Buildings, Floors, Building Equipments and Building Materials.
    /// </summary>
    public class BuildingHandler : BaseHandler
    {
        #region Handler Settings

        /// <summary>
        /// Path to a building data entry.
        /// </summary>
        /// <param name="building"></param>
        /// <returns></returns>
        public static string GetPathToBuildingDirectory(Building building)
        {
            return string.Format("{0}/{1}", GetHandlerStoragePath<BuildingHandler>(), building.name.ToLower().Replace(" ", "_").Trim());
        }

        /// <summary>
        /// Path to the Building Media directory (usually located inside its Building folder).
        /// </summary>
        /// <param name="building"></param>
        /// <returns></returns>
        public static string GetPathToBuildingMediaDirectory(Building building)
        {
            return $"{GetPathToBuildingDirectory(building)}/Media";
        }

        /// <summary>
        /// Path to a Building Media file.
        /// </summary>
        /// <param name="building"></param>
        /// <param name="media"></param>
        public static string GetPathToBuildingMedia(Building building, BuildingMedia media)
        {
            return $"{GetPathToBuildingMediaDirectory(building)}/{media.fileName.Trim()}";
        }

        #endregion

        #region Building

        #region Create

        /// <summary>
        /// Creates a new building.
        /// </summary>
        /// <param name="buildingName">Building name</param>
        /// <param name="onSuccess">Contains the newly created building. Only called if the request is successful.</param>
        /// <param name="onFailure">Contains any error obtained during the request. Only called if the request has failed.</param>
        /// <param name="onComplete">Always called at the end of the request, defines whether the request was successful or not.</param>
        public static void CreateBuilding(string buildingName, Action<Building> onSuccess = null, Action<EmsError> onFailure = null,
            Action<bool> onComplete = null)
        {
            AppearitionGate.Instance.StartCoroutine(CreateBuildingProcess(AppearitionGate.Instance.CurrentUser.selectedChannel,
                new Building {name = buildingName}, onSuccess, onFailure, onComplete));
        }

        /// <summary>
        /// Creates a new building.
        /// </summary>
        /// <param name="buildingName">Building name</param>
        /// <param name="onSuccess">Contains the newly created building. Only called if the request is successful.</param>
        /// <param name="onFailure">Contains any error obtained during the request. Only called if the request has failed.</param>
        /// <param name="onComplete">Always called at the end of the request, defines whether the request was successful or not.</param>
        public static IEnumerator CreateBuildingProcess(string buildingName, Action<Building> onSuccess = null, Action<EmsError> onFailure = null,
            Action<bool> onComplete = null)
        {
            yield return CreateBuildingProcess(AppearitionGate.Instance.CurrentUser.selectedChannel,
                new Building {name = buildingName}, onSuccess, onFailure, onComplete);
        }

        /// <summary>
        /// Creates a new building.
        /// </summary>
        /// <param name="channelId">The target channel id</param>
        /// <param name="buildingName">Building name</param>
        /// <param name="onSuccess">Contains the newly created building. Only called if the request is successful.</param>
        /// <param name="onFailure">Contains any error obtained during the request. Only called if the request has failed.</param>
        /// <param name="onComplete">Always called at the end of the request, defines whether the request was successful or not.</param>
        public static void CreateBuilding(int channelId, string buildingName, Action<Building> onSuccess = null, Action<EmsError> onFailure = null,
            Action<bool> onComplete = null)
        {
            AppearitionGate.Instance.StartCoroutine(CreateBuildingProcess(channelId, new Building {name = buildingName}, onSuccess, onFailure, onComplete));
        }

        /// <summary>
        /// Creates a new building.
        /// </summary>
        /// <param name="channelId">The target channel id</param>
        /// <param name="buildingName">Building name</param>
        /// <param name="onSuccess">Contains the newly created building. Only called if the request is successful.</param>
        /// <param name="onFailure">Contains any error obtained during the request. Only called if the request has failed.</param>
        /// <param name="onComplete">Always called at the end of the request, defines whether the request was successful or not.</param>
        public static IEnumerator CreateBuildingProcess(int channelId, string buildingName, Action<Building> onSuccess = null, Action<EmsError> onFailure = null,
            Action<bool> onComplete = null)
        {
            yield return CreateBuildingProcess(channelId, new Building {name = buildingName}, onSuccess, onFailure, onComplete);
        }

        /// <summary>
        /// Creates a new building.
        /// </summary>
        /// <param name="building">Building data</param>
        /// <param name="onSuccess">Contains the newly created building. Only called if the request is successful.</param>
        /// <param name="onFailure">Contains any error obtained during the request. Only called if the request has failed.</param>
        /// <param name="onComplete">Always called at the end of the request, defines whether the request was successful or not.</param>
        public static void CreateBuilding(Building building, Action<Building> onSuccess = null, Action<EmsError> onFailure = null,
            Action<bool> onComplete = null)
        {
            AppearitionGate.Instance.StartCoroutine(CreateBuildingProcess(AppearitionGate.Instance.CurrentUser.selectedChannel, building, onSuccess, onFailure, onComplete));
        }

        /// <summary>
        /// Creates a new building.
        /// </summary>
        /// <param name="building">Building data</param>
        /// <param name="onSuccess">Contains the newly created building. Only called if the request is successful.</param>
        /// <param name="onFailure">Contains any error obtained during the request. Only called if the request has failed.</param>
        /// <param name="onComplete">Always called at the end of the request, defines whether the request was successful or not.</param>
        public static IEnumerator CreateBuildingProcess(Building building, Action<Building> onSuccess = null, Action<EmsError> onFailure = null,
            Action<bool> onComplete = null)
        {
            yield return CreateBuildingProcess(AppearitionGate.Instance.CurrentUser.selectedChannel, building, onSuccess, onFailure, onComplete);
        }

        /// <summary>
        /// Creates a new building.
        /// </summary>
        /// <param name="channelId">The target channel id</param>
        /// <param name="building">Building data</param>
        /// <param name="onSuccess">Contains the newly created building. Only called if the request is successful.</param>
        /// <param name="onFailure">Contains any error obtained during the request. Only called if the request has failed.</param>
        /// <param name="onComplete">Always called at the end of the request, defines whether the request was successful or not.</param>
        public static void CreateBuilding(int channelId, Building building, Action<Building> onSuccess = null, Action<EmsError> onFailure = null,
            Action<bool> onComplete = null)
        {
            AppearitionGate.Instance.StartCoroutine(CreateBuildingProcess(channelId, building, onSuccess, onFailure, onComplete));
        }

        /// <summary>
        /// Creates a new building.
        /// </summary>
        /// <param name="channelId">The target channel id</param>
        /// <param name="building">Building data</param>
        /// <param name="onSuccess">Contains the newly created building. Only called if the request is successful.</param>
        /// <param name="onFailure">Contains any error obtained during the request. Only called if the request has failed.</param>
        /// <param name="onComplete">Always called at the end of the request, defines whether the request was successful or not.</param>
        public static IEnumerator CreateBuildingProcess(int channelId, Building building, Action<Building> onSuccess = null, Action<EmsError> onFailure = null,
            Action<bool> onComplete = null)
        {
            if (building == null)
            {
                string message = string.Format(BuildingConstants.BUILDING_CREATE_NULL_BUILDING);
                AppearitionLogger.LogError(message);
                onFailure?.Invoke(new EmsError(message));
                onComplete?.Invoke(false);
                yield break;
            }

            //Launch request
            var createBuildingRequest = AppearitionRequest<Building_Create>.LaunchAPICall_POST<BuildingHandler>(channelId, GetReusableApiRequest<Building_Create>(), building);

            //Wait for request..
            while (!createBuildingRequest.IsDone)
                yield return null;

            //All done!
            if (createBuildingRequest.RequestResponseObject.Data != null && createBuildingRequest.RequestResponseObject.IsSuccess)
            {
                Building outcome = createBuildingRequest.RequestResponseObject.Data;
                outcome.CopyFrom(outcome);

                AppearitionLogger.LogInfo(string.Format(BuildingConstants.BUILDING_CREATE_SUCCESS, outcome.buildingId, channelId));

                //Callback it out ~
                if (onSuccess != null)
                    onSuccess(building);
            }
            else
            {
                AppearitionLogger.LogError(string.Format(BuildingConstants.BUILDING_CREATE_FAILURE, channelId));

                //Request failed =(
                if (onFailure != null)
                    onFailure(createBuildingRequest.Errors);
            }

            if (onComplete != null)
                onComplete(createBuildingRequest.RequestResponseObject.Data != null && createBuildingRequest.RequestResponseObject.IsSuccess);
        }

        #endregion

        #region Get

        /// <summary>
        /// Fetches a single building with matching IDs.
        /// </summary>
        /// <param name="buildingId">Target building</param>
        /// <param name="onSuccess">Contains the building, if found. Only called if the request is successful.</param>
        /// <param name="onFailure">Contains any error obtained during the request. Only called if the request has failed.</param>
        /// <param name="onComplete">Always called at the end of the request, defines whether the request was successful or not.</param>
        public static void GetSingleBuilding(int buildingId, Action<Building> onSuccess = null, Action<EmsError> onFailure = null,
            Action<bool> onComplete = null)
        {
            AppearitionGate.Instance.StartCoroutine(GetSingleBuildingProcess(AppearitionGate.Instance.CurrentUser.selectedChannel, buildingId, onSuccess, onFailure, onComplete));
        }

        /// <summary>
        /// Fetches a single building with matching IDs.
        /// </summary>
        /// <param name="buildingId">Target building</param>
        /// <param name="onSuccess">Contains the building, if found. Only called if the request is successful.</param>
        /// <param name="onFailure">Contains any error obtained during the request. Only called if the request has failed.</param>
        /// <param name="onComplete">Always called at the end of the request, defines whether the request was successful or not.</param>
        public static IEnumerator GetSingleBuildingProcess(int buildingId, Action<Building> onSuccess = null, Action<EmsError> onFailure = null,
            Action<bool> onComplete = null)
        {
            yield return GetSingleBuildingProcess(AppearitionGate.Instance.CurrentUser.selectedChannel, buildingId, onSuccess, onFailure, onComplete);
        }

        /// <summary>
        /// Fetches a single building with matching IDs.
        /// </summary>
        /// <param name="channelId">The target channel id</param>
        /// <param name="buildingId">Target building</param>
        /// <param name="onSuccess">Contains the building, if found. Only called if the request is successful.</param>
        /// <param name="onFailure">Contains any error obtained during the request. Only called if the request has failed.</param>
        /// <param name="onComplete">Always called at the end of the request, defines whether the request was successful or not.</param>
        public static void GetSingleBuilding(int channelId, int buildingId, Action<Building> onSuccess = null, Action<EmsError> onFailure = null,
            Action<bool> onComplete = null)
        {
            AppearitionGate.Instance.StartCoroutine(GetSingleBuildingProcess(channelId, buildingId, onSuccess, onFailure, onComplete));
        }

        /// <summary>
        /// Fetches a single building with matching IDs.
        /// </summary>
        /// <param name="channelId">The target channel id</param>
        /// <param name="buildingId">Target building</param>
        /// <param name="onSuccess">Contains the building, if found. Only called if the request is successful.</param>
        /// <param name="onFailure">Contains any error obtained during the request. Only called if the request has failed.</param>
        /// <param name="onComplete">Always called at the end of the request, defines whether the request was successful or not.</param>
        public static IEnumerator GetSingleBuildingProcess(int channelId, int buildingId, Action<Building> onSuccess = null, Action<EmsError> onFailure = null,
            Action<bool> onComplete = null)
        {
            var requestParams = new Building_Get.RequestContent() {buildingId = buildingId};

            //Launch request
            var getSingleBuildingRequest = AppearitionRequest<Building_Get>.LaunchAPICall_GET<BuildingHandler>(channelId, GetReusableApiRequest<Building_Get>(), requestParams);

            //Wait for request..
            while (!getSingleBuildingRequest.IsDone)
                yield return null;

            //All done!
            if (getSingleBuildingRequest.RequestResponseObject.IsSuccess)
            {
                Building outcome = getSingleBuildingRequest.RequestResponseObject.Data;

                if (outcome == null)
                    AppearitionLogger.LogInfo(string.Format(BuildingConstants.BUILDING_GET_NOT_FOUND, buildingId, channelId));
                else
                    AppearitionLogger.LogInfo(string.Format(BuildingConstants.BUILDING_GET_SUCCESS, outcome.buildingId, channelId));

                //Callback it out ~
                if (onSuccess != null)
                    onSuccess(outcome);
            }
            else
            {
                AppearitionLogger.LogError(string.Format(BuildingConstants.BUILDING_GET_FAILURE, buildingId, channelId));

                //Request failed =(
                if (onFailure != null)
                    onFailure(getSingleBuildingRequest.Errors);
            }

            if (onComplete != null)
                onComplete(getSingleBuildingRequest.RequestResponseObject.Data != null && getSingleBuildingRequest.RequestResponseObject.IsSuccess);
        }

        #endregion

        #region List

        /// <summary>
        /// Fetches all the buildings in a given channel.
        /// </summary>
        /// <param name="onSuccess">Contains all the buildings found using the given query. Only called if the request is successful.</param>
        /// <param name="onFailure">Contains any error obtained during the request. Only called if the request has failed.</param>
        /// <param name="onComplete">Always called at the end of the request, defines whether the request was successful or not.</param>
        public static void GetChannelBuildings(Action<List<Building>> onSuccess = null, Action<EmsError> onFailure = null,
            Action<bool> onComplete = null)
        {
            AppearitionGate.Instance.StartCoroutine(GetChannelBuildingsProcess(AppearitionGate.Instance.CurrentUser.selectedChannel,
                BuildingConstants.GetDefaultBuildingListQuery(), onSuccess, onFailure, onComplete));
        }

        /// <summary>
        /// Fetches all the buildings in a given channel.
        /// </summary>
        /// <param name="onSuccess">Contains all the buildings found using the given query. Only called if the request is successful.</param>
        /// <param name="onFailure">Contains any error obtained during the request. Only called if the request has failed.</param>
        /// <param name="onComplete">Always called at the end of the request, defines whether the request was successful or not.</param>
        public static IEnumerator GetChannelBuildingsProcess(Action<List<Building>> onSuccess = null, Action<EmsError> onFailure = null,
            Action<bool> onComplete = null)
        {
            yield return GetChannelBuildingsProcess(AppearitionGate.Instance.CurrentUser.selectedChannel,
                BuildingConstants.GetDefaultBuildingListQuery(), onSuccess, onFailure, onComplete);
        }

        /// <summary>
        /// Fetches all the buildings in a given channel.
        /// </summary>
        /// <param name="channelId">The target channel id</param>
        /// <param name="onSuccess">Contains all the buildings found using the given query. Only called if the request is successful.</param>
        /// <param name="onFailure">Contains any error obtained during the request. Only called if the request has failed.</param>
        /// <param name="onComplete">Always called at the end of the request, defines whether the request was successful or not.</param>
        public static void GetChannelBuildings(int channelId, Action<List<Building>> onSuccess = null, Action<EmsError> onFailure = null,
            Action<bool> onComplete = null)
        {
            AppearitionGate.Instance.StartCoroutine(GetChannelBuildingsProcess(channelId, BuildingConstants.GetDefaultBuildingListQuery(), onSuccess, onFailure, onComplete));
        }

        /// <summary>
        /// Fetches all the buildings in a given channel.
        /// </summary>
        /// <param name="channelId">The target channel id</param>
        /// <param name="onSuccess">Contains all the buildings found using the given query. Only called if the request is successful.</param>
        /// <param name="onFailure">Contains any error obtained during the request. Only called if the request has failed.</param>
        /// <param name="onComplete">Always called at the end of the request, defines whether the request was successful or not.</param>
        public static IEnumerator GetChannelBuildingsProcess(int channelId, Action<List<Building>> onSuccess = null, Action<EmsError> onFailure = null,
            Action<bool> onComplete = null)
        {
            yield return GetChannelBuildingsProcess(channelId, BuildingConstants.GetDefaultBuildingListQuery(), onSuccess, onFailure, onComplete);
        }

        /// <summary>
        /// Fetches all the buildings matching a given query and given channel.
        /// </summary>
        /// <param name="query">Paging query</param>
        /// <param name="onSuccess">Contains all the buildings found using the given query. Only called if the request is successful.</param>
        /// <param name="onFailure">Contains any error obtained during the request. Only called if the request has failed.</param>
        /// <param name="onComplete">Always called at the end of the request, defines whether the request was successful or not.</param>
        public static void GetChannelBuildings(PostFilterQuery query, Action<List<Building>> onSuccess = null, Action<EmsError> onFailure = null,
            Action<bool> onComplete = null)
        {
            AppearitionGate.Instance.StartCoroutine(GetChannelBuildingsProcess(AppearitionGate.Instance.CurrentUser.selectedChannel, query, onSuccess, onFailure, onComplete));
        }

        /// <summary>
        /// Fetches all the buildings matching a given query and given channel.
        /// </summary>
        /// <param name="query">Paging query</param>
        /// <param name="onSuccess">Contains all the buildings found using the given query. Only called if the request is successful.</param>
        /// <param name="onFailure">Contains any error obtained during the request. Only called if the request has failed.</param>
        /// <param name="onComplete">Always called at the end of the request, defines whether the request was successful or not.</param>
        public static IEnumerator GetChannelBuildingsProcess(PostFilterQuery query, Action<List<Building>> onSuccess = null, Action<EmsError> onFailure = null,
            Action<bool> onComplete = null)
        {
            yield return GetChannelBuildingsProcess(AppearitionGate.Instance.CurrentUser.selectedChannel, query, onSuccess, onFailure, onComplete);
        }

        /// <summary>
        /// Fetches all the buildings matching a given query and given channel.
        /// </summary>
        /// <param name="channelId">The target channel id</param>
        /// <param name="query">Paging query</param>
        /// <param name="onSuccess">Contains all the buildings found using the given query. Only called if the request is successful.</param>
        /// <param name="onFailure">Contains any error obtained during the request. Only called if the request has failed.</param>
        /// <param name="onComplete">Always called at the end of the request, defines whether the request was successful or not.</param>
        public static void GetChannelBuildings(int channelId, PostFilterQuery query, Action<List<Building>> onSuccess = null, Action<EmsError> onFailure = null,
            Action<bool> onComplete = null)
        {
            AppearitionGate.Instance.StartCoroutine(GetChannelBuildingsProcess(channelId, query, onSuccess, onFailure, onComplete));
        }

        /// <summary>
        /// Fetches all the buildings matching a given query and given channel.
        /// </summary>
        /// <param name="channelId">The target channel id</param>
        /// <param name="query">Paging query</param>
        /// <param name="onSuccess">Contains all the buildings found using the given query. Only called if the request is successful.</param>
        /// <param name="onFailure">Contains any error obtained during the request. Only called if the request has failed.</param>
        /// <param name="onComplete">Always called at the end of the request, defines whether the request was successful or not.</param>
        public static IEnumerator GetChannelBuildingsProcess(int channelId, PostFilterQuery query, Action<List<Building>> onSuccess = null, Action<EmsError> onFailure = null,
            Action<bool> onComplete = null)
        {
            if (query == null)
                query = BuildingConstants.GetDefaultBuildingListQuery();

            //Launch request
            var listBuildingsRequest = AppearitionRequest<Building_List>.LaunchAPICall_POST<BuildingHandler>(channelId, GetReusableApiRequest<Building_List>(), query);

            //Wait for request..
            while (!listBuildingsRequest.IsDone)
                yield return null;

            bool isSuccess = listBuildingsRequest.RequestResponseObject.Data != null && listBuildingsRequest.RequestResponseObject.Data.Buildings != null
                                                                                     && listBuildingsRequest.RequestResponseObject.IsSuccess;
            //All done!
            if (isSuccess)
            {
                List<Building> outcome = listBuildingsRequest.RequestResponseObject.Data.Buildings;

                AppearitionLogger.LogInfo(string.Format(BuildingConstants.BUILDING_LIST_SUCCESS, outcome.Count, query));

                //Callback it out ~
                if (onSuccess != null)
                    onSuccess(outcome);
            }
            else
            {
                AppearitionLogger.LogError(string.Format(BuildingConstants.BUILDING_LIST_FAILURE, channelId, query.ToString()));

                //Request failed =(
                if (onFailure != null)
                    onFailure(listBuildingsRequest.Errors);
            }

            if (onComplete != null)
                onComplete(isSuccess);
        }

        #endregion

        #region Update

        /// <summary>
        /// Update the data of the given building on the EMS.
        /// </summary>
        /// <param name="building">Target building</param>
        /// <param name="onSuccess">Only called if the request is successful.</param>
        /// <param name="onFailure">Contains any error obtained during the request. Only called if the request has failed.</param>
        /// <param name="onComplete">Always called at the end of the request, defines whether the request was successful or not.</param>
        public static void UpdateBuildingData(Building building, Action onSuccess = null, Action<EmsError> onFailure = null,
            Action<bool> onComplete = null)
        {
            AppearitionGate.Instance.StartCoroutine(UpdateBuildingDataProcess(AppearitionGate.Instance.CurrentUser.selectedChannel, building, onSuccess, onFailure, onComplete));
        }

        /// <summary>
        /// Update the data of the given building on the EMS.
        /// </summary>
        /// <param name="building">Target building</param>
        /// <param name="onSuccess">Only called if the request is successful.</param>
        /// <param name="onFailure">Contains any error obtained during the request. Only called if the request has failed.</param>
        /// <param name="onComplete">Always called at the end of the request, defines whether the request was successful or not.</param>
        public static IEnumerator UpdateBuildingDataProcess(Building building, Action onSuccess = null, Action<EmsError> onFailure = null,
            Action<bool> onComplete = null)
        {
            yield return UpdateBuildingDataProcess(AppearitionGate.Instance.CurrentUser.selectedChannel, building, onSuccess, onFailure, onComplete);
        }

        /// <summary>
        /// Update the data of the given building on the EMS.
        /// </summary>
        /// <param name="channelId">The target channel id</param>
        /// <param name="building">Target building</param>
        /// <param name="onSuccess">Only called if the request is successful.</param>
        /// <param name="onFailure">Contains any error obtained during the request. Only called if the request has failed.</param>
        /// <param name="onComplete">Always called at the end of the request, defines whether the request was successful or not.</param>
        public static void UpdateBuildingData(int channelId, Building building, Action onSuccess = null, Action<EmsError> onFailure = null,
            Action<bool> onComplete = null)
        {
            AppearitionGate.Instance.StartCoroutine(UpdateBuildingDataProcess(channelId, building, onSuccess, onFailure, onComplete));
        }

        /// <summary>
        /// Update the data of the given building on the EMS.
        /// </summary>
        /// <param name="channelId">The target channel id</param>
        /// <param name="building">Target building</param>
        /// <param name="onSuccess">Only called if the request is successful.</param>
        /// <param name="onFailure">Contains any error obtained during the request. Only called if the request has failed.</param>
        /// <param name="onComplete">Always called at the end of the request, defines whether the request was successful or not.</param>
        public static IEnumerator UpdateBuildingDataProcess(int channelId, Building building, Action onSuccess = null, Action<EmsError> onFailure = null,
            Action<bool> onComplete = null)
        {
            if (building == null)
            {
                string message = string.Format(BuildingConstants.BUILDING_NULL_BUILDING);
                AppearitionLogger.LogError(message);
                onFailure?.Invoke(new EmsError(message));
                onComplete?.Invoke(false);
                yield break;
            }

            //Launch request
            var updateBuildingProcess = AppearitionRequest<Building_Update>.LaunchAPICall_POST<BuildingHandler>(channelId, GetReusableApiRequest<Building_Update>(), building);

            //Wait for request..
            while (!updateBuildingProcess.IsDone)
                yield return null;

            //All done!
            if (updateBuildingProcess.RequestResponseObject.IsSuccess)
            {
                AppearitionLogger.LogInfo(string.Format(BuildingConstants.BUILDING_UPDATE_SUCCESS, building.buildingId, channelId));

                //Callback it out ~
                if (onSuccess != null)
                    onSuccess();
            }
            else
            {
                AppearitionLogger.LogError(string.Format(BuildingConstants.BUILDING_UPDATE_FAILURE, building.buildingId, channelId));

                //Request failed =(
                if (onFailure != null)
                    onFailure(updateBuildingProcess.Errors);
            }

            if (onComplete != null)
                onComplete(updateBuildingProcess.RequestResponseObject.IsSuccess);
        }

        #endregion

        #region Delete (Not implemented in EMS during creation of processes)

        /// <summary>
        /// Delete the data of the given building and all associated floors from the EMS.
        /// </summary>
        /// <param name="building">Target building</param>
        /// <param name="onSuccess">Only called if the request is successful.</param>
        /// <param name="onFailure">Contains any error obtained during the request. Only called if the request has failed.</param>
        /// <param name="onComplete">Always called at the end of the request, defines whether the request was successful or not.</param>
        public static void DeleteBuildingData(Building building, Action onSuccess = null, Action<EmsError> onFailure = null,
            Action<bool> onComplete = null)
        {
            AppearitionGate.Instance.StartCoroutine(DeleteBuildingDataProcess(AppearitionGate.Instance.CurrentUser.selectedChannel, building, onSuccess, onFailure, onComplete));
        }

        /// <summary>
        /// Delete the data of the given building and all associated floors from the EMS.
        /// </summary>
        /// <param name="building">Target building</param>
        /// <param name="onSuccess">Only called if the request is successful.</param>
        /// <param name="onFailure">Contains any error obtained during the request. Only called if the request has failed.</param>
        /// <param name="onComplete">Always called at the end of the request, defines whether the request was successful or not.</param>
        public static IEnumerator DeleteBuildingDataProcess(Building building, Action onSuccess = null, Action<EmsError> onFailure = null,
            Action<bool> onComplete = null)
        {
            yield return DeleteBuildingDataProcess(AppearitionGate.Instance.CurrentUser.selectedChannel, building, onSuccess, onFailure, onComplete);
        }

        /// <summary>
        /// Delete the data of the given building and all associated floors from the EMS.
        /// </summary>
        /// <param name="channelId">The target channel id</param>
        /// <param name="building">Target building</param>
        /// <param name="onSuccess">Only called if the request is successful.</param>
        /// <param name="onFailure">Contains any error obtained during the request. Only called if the request has failed.</param>
        /// <param name="onComplete">Always called at the end of the request, defines whether the request was successful or not.</param>
        public static void DeleteBuildingData(int channelId, Building building, Action onSuccess = null, Action<EmsError> onFailure = null,
            Action<bool> onComplete = null)
        {
            AppearitionGate.Instance.StartCoroutine(DeleteBuildingDataProcess(channelId, building, onSuccess, onFailure, onComplete));
        }

        /// <summary>
        /// Delete the data of the given building and all associated floors from the EMS.
        /// </summary>
        /// <param name="channelId">The target channel id</param>
        /// <param name="building">Target building</param>
        /// <param name="onSuccess">Only called if the request is successful.</param>
        /// <param name="onFailure">Contains any error obtained during the request. Only called if the request has failed.</param>
        /// <param name="onComplete">Always called at the end of the request, defines whether the request was successful or not.</param>
        public static IEnumerator DeleteBuildingDataProcess(int channelId, Building building, Action onSuccess = null, Action<EmsError> onFailure = null,
            Action<bool> onComplete = null)
        {
            if (building == null)
            {
                string message = string.Format(BuildingConstants.BUILDING_NULL_BUILDING);
                AppearitionLogger.LogError(message);
                onFailure?.Invoke(new EmsError(message));
                onComplete?.Invoke(false);
                yield break;
            }

            var requestParams = new Building_Remove.RequestContent() {buildingId = building.buildingId};

            //Launch request
            var deleteBuildingProcess =
                AppearitionRequest<Building_Remove>.LaunchAPICall_POST<BuildingHandler>(channelId, GetReusableApiRequest<Building_Remove>(), null, requestParams);

            //Wait for request..
            while (!deleteBuildingProcess.IsDone)
                yield return null;

            //All done!
            if (deleteBuildingProcess.RequestResponseObject.IsSuccess)
            {
                AppearitionLogger.LogInfo(string.Format(BuildingConstants.BUILDING_REMOVE_SUCCESS, building.buildingId, channelId));

                //Callback it out ~
                if (onSuccess != null)
                    onSuccess();
            }
            else
            {
                AppearitionLogger.LogError(string.Format(BuildingConstants.BUILDING_REMOVE_FAILURE, building.buildingId, channelId));

                //Request failed =(
                if (onFailure != null)
                    onFailure(deleteBuildingProcess.Errors);
            }

            if (onComplete != null)
                onComplete(deleteBuildingProcess.RequestResponseObject.IsSuccess);
        }

        #endregion

        #endregion


        #region Floor

        #region Create

        /// <summary>
        /// Creates a new floor for the given building.
        /// </summary>
        /// <param name="name">Name of the new floor.</param>
        /// <param name="buildingId">Target building</param>
        /// <param name="onSuccess">Contains the newly created floor. Only called if the request is successful.</param>
        /// <param name="onFailure">Contains any error obtained during the request. Only called if the request has failed.</param>
        /// <param name="onComplete">Always called at the end of the request, defines whether the request was successful or not.</param>
        public static void CreateFloor(string name, int buildingId, Action<Floor> onSuccess = null, Action<EmsError> onFailure = null,
            Action<bool> onComplete = null)
        {
            AppearitionGate.Instance.StartCoroutine(CreateFloorProcess(AppearitionGate.Instance.CurrentUser.selectedChannel,
                new Floor {name = name, buildingId = buildingId}, onSuccess, onFailure, onComplete));
        }

        /// <summary>
        /// Creates a new floor using the given data. Do note that the floor data's buildingFloorId will be overwritten.
        /// </summary>
        /// <param name="name">Name of the new floor.</param>
        /// <param name="buildingId">Target building</param>
        /// <param name="onSuccess">Contains the newly created floor. Only called if the request is successful.</param>
        /// <param name="onFailure">Contains any error obtained during the request. Only called if the request has failed.</param>
        /// <param name="onComplete">Always called at the end of the request, defines whether the request was successful or not.</param>
        public static IEnumerator CreateFloorProcess(string name, int buildingId, Action<Floor> onSuccess = null, Action<EmsError> onFailure = null,
            Action<bool> onComplete = null)
        {
            yield return CreateFloorProcess(AppearitionGate.Instance.CurrentUser.selectedChannel,
                new Floor {name = name, buildingId = buildingId}, onSuccess, onFailure, onComplete);
        }

        /// <summary>
        /// Creates a new floor for the given building.
        /// </summary>
        /// <param name="channelId">The target channel id</param>
        /// <param name="name">Name of the new floor.</param>
        /// <param name="buildingId">Target building</param>
        /// <param name="onSuccess">Contains the newly created floor. Only called if the request is successful.</param>
        /// <param name="onFailure">Contains any error obtained during the request. Only called if the request has failed.</param>
        /// <param name="onComplete">Always called at the end of the request, defines whether the request was successful or not.</param>
        public static void CreateFloor(int channelId, string name, int buildingId, Action<Floor> onSuccess = null, Action<EmsError> onFailure = null,
            Action<bool> onComplete = null)
        {
            AppearitionGate.Instance.StartCoroutine(CreateFloorProcess(channelId, new Floor {name = name, buildingId = buildingId}, onSuccess, onFailure, onComplete));
        }

        /// <summary>
        /// Creates a new floor using the given data. Do note that the floor data's buildingFloorId will be overwritten.
        /// </summary>
        /// <param name="channelId">The target channel id</param>
        /// <param name="name">Name of the new floor.</param>
        /// <param name="buildingId">Target building</param>
        /// <param name="onSuccess">Contains the newly created floor. Only called if the request is successful.</param>
        /// <param name="onFailure">Contains any error obtained during the request. Only called if the request has failed.</param>
        /// <param name="onComplete">Always called at the end of the request, defines whether the request was successful or not.</param>
        public static IEnumerator CreateFloorProcess(int channelId, string name, int buildingId, Action<Floor> onSuccess = null, Action<EmsError> onFailure = null,
            Action<bool> onComplete = null)
        {
            yield return CreateFloorProcess(channelId, new Floor {name = name, buildingId = buildingId}, onSuccess, onFailure, onComplete);
        }

        /// <summary>
        /// Creates a new floor using the given data. Do note that the floor data's buildingFloorId will be overwritten.
        /// </summary>
        /// <param name="floor">Floor data</param>
        /// <param name="onSuccess">Contains the newly created floor. Only called if the request is successful.</param>
        /// <param name="onFailure">Contains any error obtained during the request. Only called if the request has failed.</param>
        /// <param name="onComplete">Always called at the end of the request, defines whether the request was successful or not.</param>
        public static void CreateFloor(Floor floor, Action<Floor> onSuccess = null, Action<EmsError> onFailure = null,
            Action<bool> onComplete = null)
        {
            AppearitionGate.Instance.StartCoroutine(CreateFloorProcess(AppearitionGate.Instance.CurrentUser.selectedChannel, floor, onSuccess, onFailure, onComplete));
        }

        /// <summary>
        /// Creates a new floor using the given data. Do note that the floor data's buildingFloorId will be overwritten.
        /// </summary>
        /// <param name="floor">Floor data</param>
        /// <param name="onSuccess">Contains the newly created floor. Only called if the request is successful.</param>
        /// <param name="onFailure">Contains any error obtained during the request. Only called if the request has failed.</param>
        /// <param name="onComplete">Always called at the end of the request, defines whether the request was successful or not.</param>
        public static IEnumerator CreateFloorProcess(Floor floor, Action<Floor> onSuccess = null, Action<EmsError> onFailure = null,
            Action<bool> onComplete = null)
        {
            yield return CreateFloorProcess(AppearitionGate.Instance.CurrentUser.selectedChannel, floor, onSuccess, onFailure, onComplete);
        }

        /// <summary>
        /// Creates a new floor using the given data. Do note that the floor data's buildingFloorId will be overwritten.
        /// </summary>
        /// <param name="channelId">The target channel id</param>
        /// <param name="floor">Floor data</param>
        /// <param name="onSuccess">Contains the newly created floor. Only called if the request is successful.</param>
        /// <param name="onFailure">Contains any error obtained during the request. Only called if the request has failed.</param>
        /// <param name="onComplete">Always called at the end of the request, defines whether the request was successful or not.</param>
        public static void CreateFloor(int channelId, Floor floor, Action<Floor> onSuccess = null, Action<EmsError> onFailure = null,
            Action<bool> onComplete = null)
        {
            AppearitionGate.Instance.StartCoroutine(CreateFloorProcess(channelId, floor, onSuccess, onFailure, onComplete));
        }

        /// <summary>
        /// Creates a new floor using the given data. Do note that the floor data's buildingFloorId will be overwritten.
        /// </summary>
        /// <param name="channelId">The target channel id</param>
        /// <param name="floor">Floor data</param>
        /// <param name="onSuccess">Contains the newly created floor. Only called if the request is successful.</param>
        /// <param name="onFailure">Contains any error obtained during the request. Only called if the request has failed.</param>
        /// <param name="onComplete">Always called at the end of the request, defines whether the request was successful or not.</param>
        public static IEnumerator CreateFloorProcess(int channelId, Floor floor, Action<Floor> onSuccess = null, Action<EmsError> onFailure = null,
            Action<bool> onComplete = null)
        {
            if (floor == null)
            {
                string message = string.Format(BuildingConstants.BUILDINGFLOOR_CREATE_NULL_FLOOR);
                AppearitionLogger.LogError(message);
                onFailure?.Invoke(new EmsError(message));
                onComplete?.Invoke(false);
                yield break;
            }

            //Launch request
            var createFloorRequest = AppearitionRequest<BuildingFloor_Create>.LaunchAPICall_POST<BuildingHandler>(channelId, GetReusableApiRequest<BuildingFloor_Create>(), floor);

            //Wait for request..
            while (!createFloorRequest.IsDone)
                yield return null;

            //All done!
            if (createFloorRequest.RequestResponseObject.Data != null && createFloorRequest.RequestResponseObject.IsSuccess)
            {
                Floor outcome = createFloorRequest.RequestResponseObject.Data;
                floor.CopyFrom(outcome);

                AppearitionLogger.LogInfo(string.Format(BuildingConstants.BUILDINGFLOOR_CREATE_SUCCESS, outcome.buildingId, outcome.buildingFloorId, channelId));

                //Callback it out ~
                if (onSuccess != null)
                    onSuccess(floor);
            }
            else
            {
                AppearitionLogger.LogError(string.Format(BuildingConstants.BUILDINGFLOOR_CREATE_FAILURE, floor.buildingId, channelId));

                //Request failed =(
                if (onFailure != null)
                    onFailure(createFloorRequest.Errors);
            }

            if (onComplete != null)
                onComplete(createFloorRequest.RequestResponseObject.Data != null && createFloorRequest.RequestResponseObject.IsSuccess);
        }

        #endregion

        #region Get

        /// <summary>
        /// Fetches a single floor with matching IDs.
        /// </summary>
        /// <param name="buildingId">Target building</param>
        /// <param name="buildingFloorId">Target building</param>
        /// <param name="onSuccess">Contains the building floor, if found. Only called if the request is successful.</param>
        /// <param name="onFailure">Contains any error obtained during the request. Only called if the request has failed.</param>
        /// <param name="onComplete">Always called at the end of the request, defines whether the request was successful or not.</param>
        public static void GetSingleFloor(int buildingId, int buildingFloorId, Action<Floor> onSuccess = null, Action<EmsError> onFailure = null,
            Action<bool> onComplete = null)
        {
            AppearitionGate.Instance.StartCoroutine(GetSingleFloorProcess(AppearitionGate.Instance.CurrentUser.selectedChannel, buildingId, buildingFloorId, onSuccess, onFailure, onComplete));
        }

        /// <summary>
        /// Fetches a single floor with matching IDs.
        /// </summary>
        /// <param name="buildingId">Target building</param>
        /// <param name="buildingFloorId">Target building</param>
        /// <param name="onSuccess">Contains the building floor, if found. Only called if the request is successful.</param>
        /// <param name="onFailure">Contains any error obtained during the request. Only called if the request has failed.</param>
        /// <param name="onComplete">Always called at the end of the request, defines whether the request was successful or not.</param>
        public static IEnumerator GetSingleFloorProcess(int buildingId, int buildingFloorId, Action<Floor> onSuccess = null, Action<EmsError> onFailure = null,
            Action<bool> onComplete = null)
        {
            yield return GetSingleFloorProcess(AppearitionGate.Instance.CurrentUser.selectedChannel, buildingId, buildingFloorId, onSuccess, onFailure, onComplete);
        }

        /// <summary>
        /// Fetches a single floor with matching IDs.
        /// </summary>
        /// <param name="channelId">The target channel id</param>
        /// <param name="buildingId">Target building</param>
        /// <param name="buildingFloorId">Target building</param>
        /// <param name="onSuccess">Contains the building floor, if found. Only called if the request is successful.</param>
        /// <param name="onFailure">Contains any error obtained during the request. Only called if the request has failed.</param>
        /// <param name="onComplete">Always called at the end of the request, defines whether the request was successful or not.</param>
        public static void GetSingleFloor(int channelId, int buildingId, int buildingFloorId, Action<Floor> onSuccess = null, Action<EmsError> onFailure = null,
            Action<bool> onComplete = null)
        {
            AppearitionGate.Instance.StartCoroutine(GetSingleFloorProcess(channelId, buildingId, buildingFloorId, onSuccess, onFailure, onComplete));
        }

        /// <summary>
        /// Fetches a single floor with matching IDs.
        /// </summary>
        /// <param name="channelId">The target channel id</param>
        /// <param name="buildingId">Target building</param>
        /// <param name="buildingFloorId">Target building</param>
        /// <param name="onSuccess">Contains the building floor, if found. Only called if the request is successful.</param>
        /// <param name="onFailure">Contains any error obtained during the request. Only called if the request has failed.</param>
        /// <param name="onComplete">Always called at the end of the request, defines whether the request was successful or not.</param>
        public static IEnumerator GetSingleFloorProcess(int channelId, int buildingId, int buildingFloorId, Action<Floor> onSuccess = null, Action<EmsError> onFailure = null,
            Action<bool> onComplete = null)
        {
            var requestParams = new BuildingFloor_Get.RequestContent() {buildingId = buildingId, buildingFloorId = buildingFloorId};

            //Launch request
            var getSingleFloorRequest = AppearitionRequest<BuildingFloor_Get>.LaunchAPICall_GET<BuildingHandler>(channelId, GetReusableApiRequest<BuildingFloor_Get>(), requestParams);

            //Wait for request..
            while (!getSingleFloorRequest.IsDone)
                yield return null;

            //All done!
            if (getSingleFloorRequest.RequestResponseObject.IsSuccess)
            {
                Floor outcome = getSingleFloorRequest.RequestResponseObject.Data;

                if (outcome == null)
                    AppearitionLogger.LogInfo(string.Format(BuildingConstants.BUILDINGFLOOR_GET_NOT_FOUND, buildingFloorId, buildingId, channelId));
                else
                    AppearitionLogger.LogInfo(string.Format(BuildingConstants.BUILDINGFLOOR_GET_SUCCESS, outcome.buildingFloorId, outcome.buildingId, channelId));

                //Callback it out ~
                if (onSuccess != null)
                    onSuccess(outcome);
            }
            else
            {
                AppearitionLogger.LogError(string.Format(BuildingConstants.BUILDINGFLOOR_GET_FAILURE, buildingFloorId, buildingId, channelId));

                //Request failed =(
                if (onFailure != null)
                    onFailure(getSingleFloorRequest.Errors);
            }

            if (onComplete != null)
                onComplete(getSingleFloorRequest.RequestResponseObject.Data != null && getSingleFloorRequest.RequestResponseObject.IsSuccess);
        }

        #endregion

        #region List

        /// <summary>
        /// Fetches all from the given channel.
        /// </summary>
        /// <param name="onSuccess">Contains all the floors found using the given query. Only called if the request is successful.</param>
        /// <param name="onFailure">Contains any error obtained during the request. Only called if the request has failed.</param>
        /// <param name="onComplete">Always called at the end of the request, defines whether the request was successful or not.</param>
        public static void GetFloors(Action<List<Floor>> onSuccess = null, Action<EmsError> onFailure = null,
            Action<bool> onComplete = null)
        {
            AppearitionGate.Instance.StartCoroutine(GetFloorsProcess(AppearitionGate.Instance.CurrentUser.selectedChannel,
                BuildingConstants.GetDefaultBuildingFloorListQuery(), onSuccess, onFailure, onComplete));
        }

        /// <summary>
        /// Fetches all from the given channel.
        /// </summary>
        /// <param name="onSuccess">Contains all the floors found using the given query. Only called if the request is successful.</param>
        /// <param name="onFailure">Contains any error obtained during the request. Only called if the request has failed.</param>
        /// <param name="onComplete">Always called at the end of the request, defines whether the request was successful or not.</param>
        public static IEnumerator GetFloorsProcess(Action<List<Floor>> onSuccess = null, Action<EmsError> onFailure = null,
            Action<bool> onComplete = null)
        {
            return GetFloorsProcess(AppearitionGate.Instance.CurrentUser.selectedChannel,
                BuildingConstants.GetDefaultBuildingFloorListQuery(), onSuccess, onFailure, onComplete);
        }

        /// <summary>
        /// Fetches all from the given channel.
        /// </summary>
        /// <param name="channelId">The target channel id</param>
        /// <param name="onSuccess">Contains all the floors found using the given query. Only called if the request is successful.</param>
        /// <param name="onFailure">Contains any error obtained during the request. Only called if the request has failed.</param>
        /// <param name="onComplete">Always called at the end of the request, defines whether the request was successful or not.</param>
        public static void GetFloors(int channelId, Action<List<Floor>> onSuccess = null, Action<EmsError> onFailure = null,
            Action<bool> onComplete = null)
        {
            AppearitionGate.Instance.StartCoroutine(GetFloorsProcess(channelId, BuildingConstants.GetDefaultBuildingFloorListQuery(), onSuccess, onFailure, onComplete));
        }

        /// <summary>
        /// Fetches all from the given channel.
        /// </summary>
        /// <param name="channelId">The target channel id</param>
        /// <param name="onSuccess">Contains all the floors found using the given query. Only called if the request is successful.</param>
        /// <param name="onFailure">Contains any error obtained during the request. Only called if the request has failed.</param>
        /// <param name="onComplete">Always called at the end of the request, defines whether the request was successful or not.</param>
        public static IEnumerator GetFloorsProcess(int channelId, Action<List<Floor>> onSuccess = null, Action<EmsError> onFailure = null,
            Action<bool> onComplete = null)
        {
            return GetFloorsProcess(channelId, BuildingConstants.GetDefaultBuildingFloorListQuery(), onSuccess, onFailure, onComplete);
        }

        /// <summary>
        /// Fetches all the floors matching a given query and given channel.
        /// </summary>
        /// <param name="query">Paging query</param>
        /// <param name="onSuccess">Contains all the floors found using the given query. Only called if the request is successful.</param>
        /// <param name="onFailure">Contains any error obtained during the request. Only called if the request has failed.</param>
        /// <param name="onComplete">Always called at the end of the request, defines whether the request was successful or not.</param>
        public static void GetFloors(PostFilterQuery query, Action<List<Floor>> onSuccess = null, Action<EmsError> onFailure = null,
            Action<bool> onComplete = null)
        {
            AppearitionGate.Instance.StartCoroutine(GetFloorsProcess(AppearitionGate.Instance.CurrentUser.selectedChannel, query, onSuccess, onFailure, onComplete));
        }

        /// <summary>
        /// Fetches all the floors matching a given query and given channel.
        /// </summary>
        /// <param name="query">Paging query</param>
        /// <param name="onSuccess">Contains all the floors found using the given query. Only called if the request is successful.</param>
        /// <param name="onFailure">Contains any error obtained during the request. Only called if the request has failed.</param>
        /// <param name="onComplete">Always called at the end of the request, defines whether the request was successful or not.</param>
        public static IEnumerator GetFloorsProcess(PostFilterQuery query, Action<List<Floor>> onSuccess = null, Action<EmsError> onFailure = null,
            Action<bool> onComplete = null)
        {
            yield return GetFloorsProcess(AppearitionGate.Instance.CurrentUser.selectedChannel, query, onSuccess, onFailure, onComplete);
        }

        /// <summary>
        /// Fetches all the floors matching a given query and given channel.
        /// </summary>
        /// <param name="channelId">The target channel id</param>
        /// <param name="query">Paging query</param>
        /// <param name="onSuccess">Contains all the floors found using the given query. Only called if the request is successful.</param>
        /// <param name="onFailure">Contains any error obtained during the request. Only called if the request has failed.</param>
        /// <param name="onComplete">Always called at the end of the request, defines whether the request was successful or not.</param>
        public static void GetFloors(int channelId, PostFilterQuery query, Action<List<Floor>> onSuccess = null, Action<EmsError> onFailure = null,
            Action<bool> onComplete = null)
        {
            AppearitionGate.Instance.StartCoroutine(GetFloorsProcess(channelId, query, onSuccess, onFailure, onComplete));
        }

        /// <summary>
        /// Fetches all the floors matching a given query and given channel.
        /// </summary>
        /// <param name="channelId">The target channel id</param>
        /// <param name="query">Paging query</param>
        /// <param name="onSuccess">Contains all the floors found using the given query. Only called if the request is successful.</param>
        /// <param name="onFailure">Contains any error obtained during the request. Only called if the request has failed.</param>
        /// <param name="onComplete">Always called at the end of the request, defines whether the request was successful or not.</param>
        public static IEnumerator GetFloorsProcess(int channelId, PostFilterQuery query, Action<List<Floor>> onSuccess = null, Action<EmsError> onFailure = null,
            Action<bool> onComplete = null)
        {
            if (query == null)
                query = BuildingConstants.GetDefaultBuildingFloorListQuery();

            //Launch request
            var listFloorRequest = AppearitionRequest<BuildingFloor_List>.LaunchAPICall_POST<BuildingHandler>(channelId, GetReusableApiRequest<BuildingFloor_List>(), query);

            //Wait for request..
            while (!listFloorRequest.IsDone)
                yield return null;

            bool isSuccess = listFloorRequest.RequestResponseObject.Data != null && listFloorRequest.RequestResponseObject.Data.Floors != null
                                                                                 && listFloorRequest.RequestResponseObject.IsSuccess;
            //All done!
            if (isSuccess)
            {
                List<Floor> outcome = listFloorRequest.RequestResponseObject.Data.Floors;

                AppearitionLogger.LogInfo(string.Format(BuildingConstants.BUILDINGFLOOR_LIST_SUCCESS, outcome.Count, query));

                //Callback it out ~
                if (onSuccess != null)
                    onSuccess(outcome);
            }
            else
            {
                AppearitionLogger.LogError(string.Format(BuildingConstants.BUILDINGFLOOR_LIST_FAILURE, channelId, query.ToString()));

                //Request failed =(
                if (onFailure != null)
                    onFailure(listFloorRequest.Errors);
            }

            if (onComplete != null)
                onComplete(isSuccess);
        }

        #endregion

        #region Update

        /// <summary>
        /// Update the data of the given floor on the EMS.
        /// </summary>
        /// <param name="floor">Target floor</param>
        /// <param name="onSuccess">Only called if the request is successful.</param>
        /// <param name="onFailure">Contains any error obtained during the request. Only called if the request has failed.</param>
        /// <param name="onComplete">Always called at the end of the request, defines whether the request was successful or not.</param>
        public static void UpdateFloorData(Floor floor, Action onSuccess = null, Action<EmsError> onFailure = null,
            Action<bool> onComplete = null)
        {
            AppearitionGate.Instance.StartCoroutine(UpdateFloorDataProcess(AppearitionGate.Instance.CurrentUser.selectedChannel, floor, onSuccess, onFailure, onComplete));
        }

        /// <summary>
        /// Update the data of the given floor on the EMS.
        /// </summary>
        /// <param name="floor">Target floor</param>
        /// <param name="onSuccess">Only called if the request is successful.</param>
        /// <param name="onFailure">Contains any error obtained during the request. Only called if the request has failed.</param>
        /// <param name="onComplete">Always called at the end of the request, defines whether the request was successful or not.</param>
        public static IEnumerator UpdateFloorDataProcess(Floor floor, Action onSuccess = null, Action<EmsError> onFailure = null,
            Action<bool> onComplete = null)
        {
            yield return UpdateFloorDataProcess(AppearitionGate.Instance.CurrentUser.selectedChannel, floor, onSuccess, onFailure, onComplete);
        }

        /// <summary>
        /// Update the data of the given floor on the EMS.
        /// </summary>
        /// <param name="channelId">The target channel id</param>
        /// <param name="floor">Target floor</param>
        /// <param name="onSuccess">Only called if the request is successful.</param>
        /// <param name="onFailure">Contains any error obtained during the request. Only called if the request has failed.</param>
        /// <param name="onComplete">Always called at the end of the request, defines whether the request was successful or not.</param>
        public static void UpdateFloorData(int channelId, Floor floor, Action onSuccess = null, Action<EmsError> onFailure = null,
            Action<bool> onComplete = null)
        {
            AppearitionGate.Instance.StartCoroutine(UpdateFloorDataProcess(channelId, floor, onSuccess, onFailure, onComplete));
        }

        /// <summary>
        /// Update the data of the given floor on the EMS.
        /// </summary>
        /// <param name="channelId">The target channel id</param>
        /// <param name="floor">Target floor</param>
        /// <param name="onSuccess">Only called if the request is successful.</param>
        /// <param name="onFailure">Contains any error obtained during the request. Only called if the request has failed.</param>
        /// <param name="onComplete">Always called at the end of the request, defines whether the request was successful or not.</param>
        public static IEnumerator UpdateFloorDataProcess(int channelId, Floor floor, Action onSuccess = null, Action<EmsError> onFailure = null,
            Action<bool> onComplete = null)
        {
            if (floor == null)
            {
                string message = string.Format(BuildingConstants.BUILDINGFLOOR_NULL_FLOOR);
                AppearitionLogger.LogError(message);
                onFailure?.Invoke(new EmsError(message));
                onComplete?.Invoke(false);
                yield break;
            }

            //Launch request
            var updateFloorProcess = AppearitionRequest<BuildingFloor_Update>.LaunchAPICall_POST<BuildingHandler>(channelId, GetReusableApiRequest<BuildingFloor_Update>(), floor);

            //Wait for request..
            while (!updateFloorProcess.IsDone)
                yield return null;

            //All done!
            if (updateFloorProcess.RequestResponseObject.IsSuccess)
            {
                AppearitionLogger.LogInfo(string.Format(BuildingConstants.BUILDINGFLOOR_UPDATE_SUCCESS, floor.buildingFloorId, floor.buildingId, channelId));

                //Callback it out ~
                if (onSuccess != null)
                    onSuccess();
            }
            else
            {
                AppearitionLogger.LogError(string.Format(BuildingConstants.BUILDINGFLOOR_UPDATE_FAILURE, floor.buildingFloorId, floor.buildingId, channelId));

                //Request failed =(
                if (onFailure != null)
                    onFailure(updateFloorProcess.Errors);
            }

            if (onComplete != null)
                onComplete(updateFloorProcess.RequestResponseObject.IsSuccess);
        }

        #endregion

        #region Delete

        /// <summary>
        /// Delete the data of the given floor on the EMS.
        /// </summary>
        /// <param name="floor">Target floor</param>
        /// <param name="onSuccess">Only called if the request is successful.</param>
        /// <param name="onFailure">Contains any error obtained during the request. Only called if the request has failed.</param>
        /// <param name="onComplete">Always called at the end of the request, defines whether the request was successful or not.</param>
        public static void DeleteFloorData(Floor floor, Action onSuccess = null, Action<EmsError> onFailure = null,
            Action<bool> onComplete = null)
        {
            AppearitionGate.Instance.StartCoroutine(DeleteFloorDataProcess(AppearitionGate.Instance.CurrentUser.selectedChannel, floor, onSuccess, onFailure, onComplete));
        }

        /// <summary>
        /// Delete the data of the given floor on the EMS.
        /// </summary>
        /// <param name="floor">Target floor</param>
        /// <param name="onSuccess">Only called if the request is successful.</param>
        /// <param name="onFailure">Contains any error obtained during the request. Only called if the request has failed.</param>
        /// <param name="onComplete">Always called at the end of the request, defines whether the request was successful or not.</param>
        public static IEnumerator DeleteFloorDataProcess(Floor floor, Action onSuccess = null, Action<EmsError> onFailure = null,
            Action<bool> onComplete = null)
        {
            yield return DeleteFloorDataProcess(AppearitionGate.Instance.CurrentUser.selectedChannel, floor, onSuccess, onFailure, onComplete);
        }

        /// <summary>
        /// Delete the data of the given floor on the EMS.
        /// </summary>
        /// <param name="channelId">The target channel id</param>
        /// <param name="floor">Target floor</param>
        /// <param name="onSuccess">Only called if the request is successful.</param>
        /// <param name="onFailure">Contains any error obtained during the request. Only called if the request has failed.</param>
        /// <param name="onComplete">Always called at the end of the request, defines whether the request was successful or not.</param>
        public static void DeleteFloorData(int channelId, Floor floor, Action onSuccess = null, Action<EmsError> onFailure = null,
            Action<bool> onComplete = null)
        {
            AppearitionGate.Instance.StartCoroutine(DeleteFloorDataProcess(channelId, floor, onSuccess, onFailure, onComplete));
        }

        /// <summary>
        /// Delete the data of the given floor on the EMS.
        /// </summary>
        /// <param name="channelId">The target channel id</param>
        /// <param name="floor">Target floor</param>
        /// <param name="onSuccess">Only called if the request is successful.</param>
        /// <param name="onFailure">Contains any error obtained during the request. Only called if the request has failed.</param>
        /// <param name="onComplete">Always called at the end of the request, defines whether the request was successful or not.</param>
        public static IEnumerator DeleteFloorDataProcess(int channelId, Floor floor, Action onSuccess = null, Action<EmsError> onFailure = null,
            Action<bool> onComplete = null)
        {
            if (floor == null)
            {
                string message = string.Format(BuildingConstants.BUILDINGFLOOR_NULL_FLOOR);
                AppearitionLogger.LogError(message);
                onFailure?.Invoke(new EmsError(message));
                onComplete?.Invoke(false);
                yield break;
            }

            var requestParams = new BuildingFloor_Remove.RequestContent() {buildingFloorId = floor.buildingFloorId, buildingId = floor.buildingId};

            //Launch request
            var deleteFloorProcess =
                AppearitionRequest<BuildingFloor_Remove>.LaunchAPICall_POST<BuildingHandler>(channelId, GetReusableApiRequest<BuildingFloor_Remove>(), null, requestParams);

            //Wait for request..
            while (!deleteFloorProcess.IsDone)
                yield return null;

            //All done!
            if (deleteFloorProcess.RequestResponseObject.IsSuccess)
            {
                AppearitionLogger.LogInfo(string.Format(BuildingConstants.BUILDINGFLOOR_REMOVE_SUCCESS, floor.buildingFloorId, floor.buildingId, channelId));

                //Callback it out ~
                if (onSuccess != null)
                    onSuccess();
            }
            else
            {
                AppearitionLogger.LogError(string.Format(BuildingConstants.BUILDINGFLOOR_REMOVE_FAILURE, floor.buildingFloorId, floor.buildingId, channelId));

                //Request failed =(
                if (onFailure != null)
                    onFailure(deleteFloorProcess.Errors);
            }

            if (onComplete != null)
                onComplete(deleteFloorProcess.RequestResponseObject.IsSuccess);
        }

        #endregion

        #endregion


        #region Building Material

        #region Get

        /// <summary>
        /// Fetches a single building material with matching IDs.
        /// </summary>
        /// <param name="buildingMaterialId">Target building material</param>
        /// <param name="onSuccess">Contains the building material, if found. Only called if the request is successful.</param>
        /// <param name="onFailure">Contains any error obtained during the request. Only called if the request has failed.</param>
        /// <param name="onComplete">Always called at the end of the request, defines whether the request was successful or not.</param>
        public static void GetSingleBuildingMaterial(int buildingMaterialId, Action<BuildingMaterial> onSuccess = null, Action<EmsError> onFailure = null,
            Action<bool> onComplete = null)
        {
            AppearitionGate.Instance.StartCoroutine(GetSingleBuildingMaterialProcess(AppearitionGate.Instance.CurrentUser.selectedChannel,
                buildingMaterialId, onSuccess, onFailure, onComplete));
        }

        /// <summary>
        /// Fetches a single building material with matching IDs.
        /// </summary>
        /// <param name="buildingMaterialId">Target building material</param>
        /// <param name="onSuccess">Contains the building material, if found. Only called if the request is successful.</param>
        /// <param name="onFailure">Contains any error obtained during the request. Only called if the request has failed.</param>
        /// <param name="onComplete">Always called at the end of the request, defines whether the request was successful or not.</param>
        public static IEnumerator GetSingleBuildingMaterialProcess(int buildingMaterialId, Action<BuildingMaterial> onSuccess = null, Action<EmsError> onFailure = null,
            Action<bool> onComplete = null)
        {
            yield return GetSingleBuildingMaterialProcess(AppearitionGate.Instance.CurrentUser.selectedChannel,
                buildingMaterialId, onSuccess, onFailure, onComplete);
        }

        /// <summary>
        /// Fetches a single building material with matching IDs.
        /// </summary>
        /// <param name="channelId">The target channel id</param>
        /// <param name="buildingMaterialId">Target building material</param>
        /// <param name="onSuccess">Contains the building material, if found. Only called if the request is successful.</param>
        /// <param name="onFailure">Contains any error obtained during the request. Only called if the request has failed.</param>
        /// <param name="onComplete">Always called at the end of the request, defines whether the request was successful or not.</param>
        public static void GetSingleBuildingMaterial(int channelId, int buildingMaterialId, Action<BuildingMaterial> onSuccess = null, Action<EmsError> onFailure = null,
            Action<bool> onComplete = null)
        {
            AppearitionGate.Instance.StartCoroutine(GetSingleBuildingMaterialProcess(channelId, buildingMaterialId, onSuccess, onFailure, onComplete));
        }

        /// <summary>
        /// Fetches a single building material with matching IDs.
        /// </summary>
        /// <param name="channelId">The target channel id</param>
        /// <param name="buildingMaterialId">Target building material</param>
        /// <param name="onSuccess">Contains the building material, if found. Only called if the request is successful.</param>
        /// <param name="onFailure">Contains any error obtained during the request. Only called if the request has failed.</param>
        /// <param name="onComplete">Always called at the end of the request, defines whether the request was successful or not.</param>
        public static IEnumerator GetSingleBuildingMaterialProcess(int channelId, int buildingMaterialId, Action<BuildingMaterial> onSuccess = null, Action<EmsError> onFailure = null,
            Action<bool> onComplete = null)
        {
            var requestParams = new BuildingMaterial_Get.RequestContent() {buildingMaterialId = buildingMaterialId};

            //Launch request
            var getSingleBuildingMaterialRequest = AppearitionRequest<BuildingMaterial_Get>.LaunchAPICall_GET<BuildingHandler>(channelId,
                GetReusableApiRequest<BuildingMaterial_Get>(), requestParams);

            //Wait for request..
            while (!getSingleBuildingMaterialRequest.IsDone)
                yield return null;

            //All done!
            if (getSingleBuildingMaterialRequest.RequestResponseObject.IsSuccess)
            {
                BuildingMaterial outcome = getSingleBuildingMaterialRequest.RequestResponseObject.Data;

                if (outcome == null)
                    AppearitionLogger.LogInfo(string.Format(BuildingConstants.BUILDINGMATERIAL_GET_NOT_FOUND, buildingMaterialId, channelId));
                else
                    AppearitionLogger.LogInfo(string.Format(BuildingConstants.BUILDINGMATERIAL_GET_SUCCESS, outcome.buildingMaterialId, channelId));

                //Callback it out ~
                if (onSuccess != null)
                    onSuccess(outcome);
            }
            else
            {
                AppearitionLogger.LogError(string.Format(BuildingConstants.BUILDINGMATERIAL_GET_FAILURE, buildingMaterialId, channelId));

                //Request failed =(
                if (onFailure != null)
                    onFailure(getSingleBuildingMaterialRequest.Errors);
            }

            if (onComplete != null)
                onComplete(getSingleBuildingMaterialRequest.RequestResponseObject.Data != null && getSingleBuildingMaterialRequest.RequestResponseObject.IsSuccess);
        }

        #endregion

        #region List

        /// <summary>
        /// Fetches all the building materials matching a given channel.
        /// </summary>
        /// <param name="onSuccess">Contains all the building materials found using the given query. Only called if the request is successful.</param>
        /// <param name="onFailure">Contains any error obtained during the request. Only called if the request has failed.</param>
        /// <param name="onComplete">Always called at the end of the request, defines whether the request was successful or not.</param>
        public static void GetBuildingMaterials(Action<List<BuildingMaterial>> onSuccess = null, Action<EmsError> onFailure = null,
            Action<bool> onComplete = null)
        {
            AppearitionGate.Instance.StartCoroutine(GetBuildingMaterialsProcess(
                AppearitionGate.Instance.CurrentUser.selectedChannel, BuildingConstants.GetDefaultBuildingMaterialListQuery(), onSuccess, onFailure, onComplete));
        }

        /// <summary>
        /// Fetches all the building materials matching a given channel.
        /// </summary>
        /// <param name="onSuccess">Contains all the building materials found using the given query. Only called if the request is successful.</param>
        /// <param name="onFailure">Contains any error obtained during the request. Only called if the request has failed.</param>
        /// <param name="onComplete">Always called at the end of the request, defines whether the request was successful or not.</param>
        public static IEnumerator GetBuildingMaterialsProcess(Action<List<BuildingMaterial>> onSuccess = null, Action<EmsError> onFailure = null,
            Action<bool> onComplete = null)
        {
            yield return GetBuildingMaterialsProcess(
                AppearitionGate.Instance.CurrentUser.selectedChannel, BuildingConstants.GetDefaultBuildingMaterialListQuery(), onSuccess, onFailure, onComplete);
        }

        /// <summary>
        /// Fetches all the building materials matching a given channel.
        /// </summary>
        /// <param name="channelId">The target channel id</param>
        /// <param name="onSuccess">Contains all the building materials found using the given query. Only called if the request is successful.</param>
        /// <param name="onFailure">Contains any error obtained during the request. Only called if the request has failed.</param>
        /// <param name="onComplete">Always called at the end of the request, defines whether the request was successful or not.</param>
        public static void GetBuildingMaterials(int channelId, Action<List<BuildingMaterial>> onSuccess = null, Action<EmsError> onFailure = null,
            Action<bool> onComplete = null)
        {
            AppearitionGate.Instance.StartCoroutine(GetBuildingMaterialsProcess(channelId, BuildingConstants.GetDefaultBuildingMaterialListQuery(), onSuccess, onFailure, onComplete));
        }

        /// <summary>
        /// Fetches all the building materials matching a given channel.
        /// </summary>
        /// <param name="channelId">The target channel id</param>
        /// <param name="onSuccess">Contains all the building materials found using the given query. Only called if the request is successful.</param>
        /// <param name="onFailure">Contains any error obtained during the request. Only called if the request has failed.</param>
        /// <param name="onComplete">Always called at the end of the request, defines whether the request was successful or not.</param>
        public static IEnumerator GetBuildingMaterialsProcess(int channelId, Action<List<BuildingMaterial>> onSuccess = null, Action<EmsError> onFailure = null,
            Action<bool> onComplete = null)
        {
            yield return GetBuildingMaterialsProcess(channelId, BuildingConstants.GetDefaultBuildingMaterialListQuery(), onSuccess, onFailure, onComplete);
        }

        /// <summary>
        /// Fetches all the building materials matching a given query and given channel.
        /// </summary>
        /// <param name="query">Paging query</param>
        /// <param name="onSuccess">Contains all the building materials found using the given query. Only called if the request is successful.</param>
        /// <param name="onFailure">Contains any error obtained during the request. Only called if the request has failed.</param>
        /// <param name="onComplete">Always called at the end of the request, defines whether the request was successful or not.</param>
        public static void GetBuildingMaterials(PostFilterQuery query, Action<List<BuildingMaterial>> onSuccess = null, Action<EmsError> onFailure = null,
            Action<bool> onComplete = null)
        {
            AppearitionGate.Instance.StartCoroutine(GetBuildingMaterialsProcess(AppearitionGate.Instance.CurrentUser.selectedChannel, query, onSuccess, onFailure, onComplete));
        }

        /// <summary>
        /// Fetches all the building materials matching a given query and given channel.
        /// </summary>
        /// <param name="query">Paging query</param>
        /// <param name="onSuccess">Contains all the building materials found using the given query. Only called if the request is successful.</param>
        /// <param name="onFailure">Contains any error obtained during the request. Only called if the request has failed.</param>
        /// <param name="onComplete">Always called at the end of the request, defines whether the request was successful or not.</param>
        public static IEnumerator GetBuildingMaterialsProcess(PostFilterQuery query, Action<List<BuildingMaterial>> onSuccess = null, Action<EmsError> onFailure = null,
            Action<bool> onComplete = null)
        {
            yield return GetBuildingMaterialsProcess(AppearitionGate.Instance.CurrentUser.selectedChannel, query, onSuccess, onFailure, onComplete);
        }

        /// <summary>
        /// Fetches all the building materials matching a given query and given channel.
        /// </summary>
        /// <param name="channelId">The target channel id</param>
        /// <param name="query">Paging query</param>
        /// <param name="onSuccess">Contains all the building materials found using the given query. Only called if the request is successful.</param>
        /// <param name="onFailure">Contains any error obtained during the request. Only called if the request has failed.</param>
        /// <param name="onComplete">Always called at the end of the request, defines whether the request was successful or not.</param>
        public static void GetBuildingMaterials(int channelId, PostFilterQuery query, Action<List<BuildingMaterial>> onSuccess = null, Action<EmsError> onFailure = null,
            Action<bool> onComplete = null)
        {
            AppearitionGate.Instance.StartCoroutine(GetBuildingMaterialsProcess(channelId, query, onSuccess, onFailure, onComplete));
        }

        /// <summary>
        /// Fetches all the building materials matching a given query and given channel.
        /// </summary>
        /// <param name="channelId">The target channel id</param>
        /// <param name="query">Paging query</param>
        /// <param name="onSuccess">Contains all the building materials found using the given query. Only called if the request is successful.</param>
        /// <param name="onFailure">Contains any error obtained during the request. Only called if the request has failed.</param>
        /// <param name="onComplete">Always called at the end of the request, defines whether the request was successful or not.</param>
        public static IEnumerator GetBuildingMaterialsProcess(int channelId, PostFilterQuery query, Action<List<BuildingMaterial>> onSuccess = null, Action<EmsError> onFailure = null,
            Action<bool> onComplete = null)
        {
            if (query == null)
                query = BuildingConstants.GetDefaultBuildingMaterialListQuery();

            //Launch request
            var listBuildingMaterialsRequest = AppearitionRequest<BuildingMaterial_List>.LaunchAPICall_POST<BuildingHandler>(channelId, GetReusableApiRequest<BuildingMaterial_List>(), query);

            //Wait for request..
            while (!listBuildingMaterialsRequest.IsDone)
                yield return null;

            bool isSuccess = listBuildingMaterialsRequest.RequestResponseObject.Data != null && listBuildingMaterialsRequest.RequestResponseObject.Data.Materials != null
                                                                                             && listBuildingMaterialsRequest.RequestResponseObject.IsSuccess;
            //All done!
            if (isSuccess)
            {
                List<BuildingMaterial> outcome = listBuildingMaterialsRequest.RequestResponseObject.Data.Materials;

                AppearitionLogger.LogInfo(string.Format(BuildingConstants.BUILDINGMATERIAL_LIST_SUCCESS, outcome.Count, query));

                //Callback it out ~
                if (onSuccess != null)
                    onSuccess(outcome);
            }
            else
            {
                AppearitionLogger.LogError(string.Format(BuildingConstants.BUILDINGMATERIAL_LIST_FAILURE, channelId, query.ToString()));

                //Request failed =(
                if (onFailure != null)
                    onFailure(listBuildingMaterialsRequest.Errors);
            }

            if (onComplete != null)
                onComplete(isSuccess);
        }

        #endregion

        #endregion


        #region Building Equipment

        #region Get

        /// <summary>
        /// Fetches a single building equipment with matching IDs.
        /// </summary>
        /// <param name="buildingEquipmentId">Target building equipment</param>
        /// <param name="onSuccess">Contains the building equipment, if found. Only called if the request is successful.</param>
        /// <param name="onFailure">Contains any error obtained during the request. Only called if the request has failed.</param>
        /// <param name="onComplete">Always called at the end of the request, defines whether the request was successful or not.</param>
        public static void GetSingleBuildingEquipment(int buildingEquipmentId, Action<BuildingEquipment> onSuccess = null, Action<EmsError> onFailure = null,
            Action<bool> onComplete = null)
        {
            AppearitionGate.Instance.StartCoroutine(GetSingleBuildingEquipmentProcess(AppearitionGate.Instance.CurrentUser.selectedChannel,
                buildingEquipmentId, onSuccess, onFailure, onComplete));
        }

        /// <summary>
        /// Fetches a single building equipment with matching IDs.
        /// </summary>
        /// <param name="buildingEquipmentId">Target building equipment</param>
        /// <param name="onSuccess">Contains the building equipment, if found. Only called if the request is successful.</param>
        /// <param name="onFailure">Contains any error obtained during the request. Only called if the request has failed.</param>
        /// <param name="onComplete">Always called at the end of the request, defines whether the request was successful or not.</param>
        public static IEnumerator GetSingleBuildingEquipmentProcess(int buildingEquipmentId, Action<BuildingEquipment> onSuccess = null, Action<EmsError> onFailure = null,
            Action<bool> onComplete = null)
        {
            yield return GetSingleBuildingEquipmentProcess(AppearitionGate.Instance.CurrentUser.selectedChannel, buildingEquipmentId, onSuccess, onFailure, onComplete);
        }

        /// <summary>
        /// Fetches a single building equipment with matching IDs.
        /// </summary>
        /// <param name="channelId">The target channel id</param>
        /// <param name="buildingEquipmentId">Target building equipment</param>
        /// <param name="onSuccess">Contains the building equipment, if found. Only called if the request is successful.</param>
        /// <param name="onFailure">Contains any error obtained during the request. Only called if the request has failed.</param>
        /// <param name="onComplete">Always called at the end of the request, defines whether the request was successful or not.</param>
        public static void GetSingleBuildingEquipment(int channelId, int buildingEquipmentId, Action<BuildingEquipment> onSuccess = null, Action<EmsError> onFailure = null,
            Action<bool> onComplete = null)
        {
            AppearitionGate.Instance.StartCoroutine(GetSingleBuildingEquipmentProcess(channelId, buildingEquipmentId, onSuccess, onFailure, onComplete));
        }

        /// <summary>
        /// Fetches a single building equipment with matching IDs.
        /// </summary>
        /// <param name="channelId">The target channel id</param>
        /// <param name="buildingEquipmentId">Target building equipment</param>
        /// <param name="onSuccess">Contains the building equipment, if found. Only called if the request is successful.</param>
        /// <param name="onFailure">Contains any error obtained during the request. Only called if the request has failed.</param>
        /// <param name="onComplete">Always called at the end of the request, defines whether the request was successful or not.</param>
        public static IEnumerator GetSingleBuildingEquipmentProcess(int channelId, int buildingEquipmentId, Action<BuildingEquipment> onSuccess = null, Action<EmsError> onFailure = null,
            Action<bool> onComplete = null)
        {
            var requestParams = new BuildingEquipment_Get.RequestContent() {buildingEquipmentId = buildingEquipmentId};

            //Launch request
            var getSingleBuildingEquipmentRequest = AppearitionRequest<BuildingEquipment_Get>.LaunchAPICall_GET<BuildingHandler>(channelId,
                GetReusableApiRequest<BuildingEquipment_Get>(), requestParams);

            //Wait for request..
            while (!getSingleBuildingEquipmentRequest.IsDone)
                yield return null;

            //All done!
            if (getSingleBuildingEquipmentRequest.RequestResponseObject.IsSuccess)
            {
                BuildingEquipment outcome = getSingleBuildingEquipmentRequest.RequestResponseObject.Data;

                if (outcome == null)
                    AppearitionLogger.LogInfo(string.Format(BuildingConstants.BUILDINGEQUIPMENT_GET_NOT_FOUND, buildingEquipmentId, channelId));
                else
                    AppearitionLogger.LogInfo(string.Format(BuildingConstants.BUILDINGEQUIPMENT_GET_SUCCESS, outcome.buildingEquipmentId, channelId));

                //Callback it out ~
                if (onSuccess != null)
                    onSuccess(outcome);
            }
            else
            {
                AppearitionLogger.LogError(string.Format(BuildingConstants.BUILDINGEQUIPMENT_GET_FAILURE, buildingEquipmentId, channelId));

                //Request failed =(
                if (onFailure != null)
                    onFailure(getSingleBuildingEquipmentRequest.Errors);
            }

            if (onComplete != null)
                onComplete(getSingleBuildingEquipmentRequest.RequestResponseObject.Data != null && getSingleBuildingEquipmentRequest.RequestResponseObject.IsSuccess);
        }

        #endregion

        #region List

        /// <summary>
        /// Fetches all the building equipments matching a given channel.
        /// </summary>
        /// <param name="onSuccess">Contains all the building equipments found using the given query. Only called if the request is successful.</param>
        /// <param name="onFailure">Contains any error obtained during the request. Only called if the request has failed.</param>
        /// <param name="onComplete">Always called at the end of the request, defines whether the request was successful or not.</param>
        public static void GetBuildingEquipments(Action<List<BuildingEquipment>> onSuccess = null, Action<EmsError> onFailure = null,
            Action<bool> onComplete = null)
        {
            AppearitionGate.Instance.StartCoroutine(GetBuildingEquipmentsProcess(AppearitionGate.Instance.CurrentUser.selectedChannel,
                BuildingConstants.GetDefaultBuildingEquipmentListQuery(), onSuccess, onFailure, onComplete));
        }

        /// <summary>
        /// Fetches all the building equipments matching a given channel.
        /// </summary>
        /// <param name="onSuccess">Contains all the building equipments found using the given query. Only called if the request is successful.</param>
        /// <param name="onFailure">Contains any error obtained during the request. Only called if the request has failed.</param>
        /// <param name="onComplete">Always called at the end of the request, defines whether the request was successful or not.</param>
        public static IEnumerator GetBuildingEquipmentsProcess(Action<List<BuildingEquipment>> onSuccess = null, Action<EmsError> onFailure = null,
            Action<bool> onComplete = null)
        {
            yield return GetBuildingEquipmentsProcess(AppearitionGate.Instance.CurrentUser.selectedChannel,
                BuildingConstants.GetDefaultBuildingEquipmentListQuery(), onSuccess, onFailure, onComplete);
        }

        /// <summary>
        /// Fetches all the building equipments matching a given channel.
        /// </summary>
        /// <param name="channelId">The target channel id</param>
        /// <param name="onSuccess">Contains all the building equipments found using the given query. Only called if the request is successful.</param>
        /// <param name="onFailure">Contains any error obtained during the request. Only called if the request has failed.</param>
        /// <param name="onComplete">Always called at the end of the request, defines whether the request was successful or not.</param>
        public static void GetBuildingEquipments(int channelId, Action<List<BuildingEquipment>> onSuccess = null, Action<EmsError> onFailure = null,
            Action<bool> onComplete = null)
        {
            AppearitionGate.Instance.StartCoroutine(GetBuildingEquipmentsProcess(channelId, BuildingConstants.GetDefaultBuildingEquipmentListQuery(), onSuccess, onFailure, onComplete));
        }

        /// <summary>
        /// Fetches all the building equipments matching a given channel.
        /// </summary>
        /// <param name="channelId">The target channel id</param>
        /// <param name="onSuccess">Contains all the building equipments found using the given query. Only called if the request is successful.</param>
        /// <param name="onFailure">Contains any error obtained during the request. Only called if the request has failed.</param>
        /// <param name="onComplete">Always called at the end of the request, defines whether the request was successful or not.</param>
        public static IEnumerator GetBuildingEquipmentsProcess(int channelId, Action<List<BuildingEquipment>> onSuccess = null, Action<EmsError> onFailure = null,
            Action<bool> onComplete = null)
        {
            yield return GetBuildingEquipmentsProcess(channelId, BuildingConstants.GetDefaultBuildingEquipmentListQuery(), onSuccess, onFailure, onComplete);
        }

        /// <summary>
        /// Fetches all the building equipments matching a given query and given channel.
        /// </summary>
        /// <param name="query">Paging query</param>
        /// <param name="onSuccess">Contains all the building equipments found using the given query. Only called if the request is successful.</param>
        /// <param name="onFailure">Contains any error obtained during the request. Only called if the request has failed.</param>
        /// <param name="onComplete">Always called at the end of the request, defines whether the request was successful or not.</param>
        public static void GetBuildingEquipments(PostFilterQuery query, Action<List<BuildingEquipment>> onSuccess = null, Action<EmsError> onFailure = null,
            Action<bool> onComplete = null)
        {
            AppearitionGate.Instance.StartCoroutine(GetBuildingEquipmentsProcess(AppearitionGate.Instance.CurrentUser.selectedChannel, query, onSuccess, onFailure, onComplete));
        }

        /// <summary>
        /// Fetches all the building equipments matching a given query and given channel.
        /// </summary>
        /// <param name="query">Paging query</param>
        /// <param name="onSuccess">Contains all the building equipments found using the given query. Only called if the request is successful.</param>
        /// <param name="onFailure">Contains any error obtained during the request. Only called if the request has failed.</param>
        /// <param name="onComplete">Always called at the end of the request, defines whether the request was successful or not.</param>
        public static IEnumerator GetBuildingEquipmentsProcess(PostFilterQuery query, Action<List<BuildingEquipment>> onSuccess = null, Action<EmsError> onFailure = null,
            Action<bool> onComplete = null)
        {
            yield return GetBuildingEquipmentsProcess(AppearitionGate.Instance.CurrentUser.selectedChannel, query, onSuccess, onFailure, onComplete);
        }

        /// <summary>
        /// Fetches all the building equipments matching a given query and given channel.
        /// </summary>
        /// <param name="channelId">The target channel id</param>
        /// <param name="query">Paging query</param>
        /// <param name="onSuccess">Contains all the building equipments found using the given query. Only called if the request is successful.</param>
        /// <param name="onFailure">Contains any error obtained during the request. Only called if the request has failed.</param>
        /// <param name="onComplete">Always called at the end of the request, defines whether the request was successful or not.</param>
        public static void GetBuildingEquipments(int channelId, PostFilterQuery query, Action<List<BuildingEquipment>> onSuccess = null, Action<EmsError> onFailure = null,
            Action<bool> onComplete = null)
        {
            AppearitionGate.Instance.StartCoroutine(GetBuildingEquipmentsProcess(channelId, query, onSuccess, onFailure, onComplete));
        }

        /// <summary>
        /// Fetches all the building equipments matching a given query and given channel.
        /// </summary>
        /// <param name="channelId">The target channel id</param>
        /// <param name="query">Paging query</param>
        /// <param name="onSuccess">Contains all the building equipments found using the given query. Only called if the request is successful.</param>
        /// <param name="onFailure">Contains any error obtained during the request. Only called if the request has failed.</param>
        /// <param name="onComplete">Always called at the end of the request, defines whether the request was successful or not.</param>
        public static IEnumerator GetBuildingEquipmentsProcess(int channelId, PostFilterQuery query, Action<List<BuildingEquipment>> onSuccess = null, Action<EmsError> onFailure = null,
            Action<bool> onComplete = null)
        {
            if (query == null)
                query = BuildingConstants.GetDefaultBuildingEquipmentListQuery();

            //Launch request
            var listBuildingEquipmentsRequest = AppearitionRequest<BuildingEquipment_List>.LaunchAPICall_POST<BuildingHandler>(channelId,
                GetReusableApiRequest<BuildingEquipment_List>(), query);

            //Wait for request..
            while (!listBuildingEquipmentsRequest.IsDone)
                yield return null;

            bool isSuccess = listBuildingEquipmentsRequest.RequestResponseObject.Data != null && listBuildingEquipmentsRequest.RequestResponseObject.Data.Equipments != null
                                                                                              && listBuildingEquipmentsRequest.RequestResponseObject.IsSuccess;
            //All done!
            if (isSuccess)
            {
                List<BuildingEquipment> outcome = listBuildingEquipmentsRequest.RequestResponseObject.Data.Equipments;

                AppearitionLogger.LogInfo(string.Format(BuildingConstants.BUILDINGEQUIPMENT_LIST_SUCCESS, outcome.Count, query));

                //Callback it out ~
                if (onSuccess != null)
                    onSuccess(outcome);
            }
            else
            {
                AppearitionLogger.LogError(string.Format(BuildingConstants.BUILDINGEQUIPMENT_LIST_FAILURE, channelId, query.ToString()));

                //Request failed =(
                if (onFailure != null)
                    onFailure(listBuildingEquipmentsRequest.Errors);
            }

            if (onComplete != null)
                onComplete(isSuccess);
        }

        #endregion

        #endregion


        #region Building Floor Equipment

        #region List

        /// <summary>
        /// Fetches all the building materials in a given channel.
        /// </summary>
        /// <param name="floor">Target floor</param>
        /// <param name="onSuccess">Contains all the building floor equipments matching the building and floor ids. Only called if the request is successful.</param>
        /// <param name="onFailure">Contains any error obtained during the request. Only called if the request has failed.</param>
        /// <param name="onComplete">Always called at the end of the request, defines whether the request was successful or not.</param>
        public static void GetBuildingFloorEquipments(Floor floor, Action<List<BuildingFloorEquipment>> onSuccess = null, Action<EmsError> onFailure = null,
            Action<bool> onComplete = null)
        {
            AppearitionGate.Instance.StartCoroutine(GetBuildingFloorEquipmentsProcess(AppearitionGate.Instance.CurrentUser.selectedChannel, floor.buildingId, floor.buildingFloorId, onSuccess,
                onFailure, onComplete));
        }

        /// <summary>
        /// Fetches all the building materials in a given channel.
        /// </summary>
        /// <param name="floor">Target floor</param>
        /// <param name="onSuccess">Contains all the building floor equipments matching the building and floor ids. Only called if the request is successful.</param>
        /// <param name="onFailure">Contains any error obtained during the request. Only called if the request has failed.</param>
        /// <param name="onComplete">Always called at the end of the request, defines whether the request was successful or not.</param>
        public static IEnumerator GetBuildingFloorEquipmentsProcess(Floor floor, Action<List<BuildingFloorEquipment>> onSuccess = null,
            Action<EmsError> onFailure = null, Action<bool> onComplete = null)
        {
            yield return GetBuildingFloorEquipmentsProcess(AppearitionGate.Instance.CurrentUser.selectedChannel, floor.buildingId, floor.buildingFloorId, onSuccess, onFailure, onComplete);
        }

        /// <summary>
        /// Fetches all the building materials in a given channel.
        /// </summary>
        /// <param name="channelId">The target channel id</param>
        /// <param name="floor">Target floor</param>
        /// <param name="onSuccess">Contains all the building floor equipments matching the building and floor ids. Only called if the request is successful.</param>
        /// <param name="onFailure">Contains any error obtained during the request. Only called if the request has failed.</param>
        /// <param name="onComplete">Always called at the end of the request, defines whether the request was successful or not.</param>
        public static void GetBuildingFloorEquipments(int channelId, Floor floor, Action<List<BuildingFloorEquipment>> onSuccess = null, Action<EmsError> onFailure = null,
            Action<bool> onComplete = null)
        {
            AppearitionGate.Instance.StartCoroutine(GetBuildingFloorEquipmentsProcess(channelId, floor.buildingId, floor.buildingFloorId, onSuccess, onFailure, onComplete));
        }

        /// <summary>
        /// Fetches all the building materials in a given channel.
        /// </summary>
        /// <param name="channelId">The target channel id</param>
        /// <param name="floor">Target floor</param>
        /// <param name="onSuccess">Contains all the building floor equipments matching the building and floor ids. Only called if the request is successful.</param>
        /// <param name="onFailure">Contains any error obtained during the request. Only called if the request has failed.</param>
        /// <param name="onComplete">Always called at the end of the request, defines whether the request was successful or not.</param>
        public static IEnumerator GetBuildingFloorEquipmentsProcess(int channelId, Floor floor, Action<List<BuildingFloorEquipment>> onSuccess = null,
            Action<EmsError> onFailure = null, Action<bool> onComplete = null)
        {
            yield return GetBuildingFloorEquipmentsProcess(channelId, floor.buildingId, floor.buildingFloorId, onSuccess, onFailure, onComplete);
        }

        /// <summary>
        /// Fetches all the building materials in a given channel.
        /// </summary>
        /// <param name="buildingId">Target building</param>
        /// <param name="buildingFloorId">Target building floor</param>
        /// <param name="onSuccess">Contains all the building floor equipments matching the building and floor ids. Only called if the request is successful.</param>
        /// <param name="onFailure">Contains any error obtained during the request. Only called if the request has failed.</param>
        /// <param name="onComplete">Always called at the end of the request, defines whether the request was successful or not.</param>
        public static void GetBuildingFloorEquipments(int buildingId, int buildingFloorId, Action<List<BuildingFloorEquipment>> onSuccess = null, Action<EmsError> onFailure = null,
            Action<bool> onComplete = null)
        {
            AppearitionGate.Instance.StartCoroutine(GetBuildingFloorEquipmentsProcess(AppearitionGate.Instance.CurrentUser.selectedChannel,
                buildingId, buildingFloorId, onSuccess, onFailure, onComplete));
        }

        /// <summary>
        /// Fetches all the building materials in a given channel.
        /// </summary>
        /// <param name="buildingId">Target building</param>
        /// <param name="buildingFloorId">Target building floor</param>
        /// <param name="onSuccess">Contains all the building floor equipments matching the building and floor ids. Only called if the request is successful.</param>
        /// <param name="onFailure">Contains any error obtained during the request. Only called if the request has failed.</param>
        /// <param name="onComplete">Always called at the end of the request, defines whether the request was successful or not.</param>
        public static IEnumerator GetBuildingFloorEquipmentsProcess(int buildingId, int buildingFloorId, Action<List<BuildingFloorEquipment>> onSuccess = null,
            Action<EmsError> onFailure = null,
            Action<bool> onComplete = null)
        {
            yield return GetBuildingFloorEquipmentsProcess(AppearitionGate.Instance.CurrentUser.selectedChannel, buildingId, buildingFloorId, onSuccess, onFailure, onComplete);
        }

        /// <summary>
        /// Fetches all the building materials in a given channel.
        /// </summary>
        /// <param name="channelId">The target channel id</param>
        /// <param name="buildingId">Target building</param>
        /// <param name="buildingFloorId">Target building floor</param>
        /// <param name="onSuccess">Contains all the building floor equipments matching the building and floor ids. Only called if the request is successful.</param>
        /// <param name="onFailure">Contains any error obtained during the request. Only called if the request has failed.</param>
        /// <param name="onComplete">Always called at the end of the request, defines whether the request was successful or not.</param>
        public static void GetBuildingFloorEquipments(int channelId, int buildingId, int buildingFloorId, Action<List<BuildingFloorEquipment>> onSuccess = null, Action<EmsError> onFailure = null,
            Action<bool> onComplete = null)
        {
            AppearitionGate.Instance.StartCoroutine(GetBuildingFloorEquipmentsProcess(channelId, buildingId, buildingFloorId, onSuccess, onFailure, onComplete));
        }

        /// <summary>
        /// Fetches all the building materials in a given channel.
        /// </summary>
        /// <param name="channelId">The target channel id</param>
        /// <param name="buildingId">Target building</param>
        /// <param name="buildingFloorId">Target building floor</param>
        /// <param name="onSuccess">Contains all the building floor equipments matching the building and floor ids. Only called if the request is successful.</param>
        /// <param name="onFailure">Contains any error obtained during the request. Only called if the request has failed.</param>
        /// <param name="onComplete">Always called at the end of the request, defines whether the request was successful or not.</param>
        public static IEnumerator GetBuildingFloorEquipmentsProcess(int channelId, int buildingId, int buildingFloorId, Action<List<BuildingFloorEquipment>> onSuccess = null,
            Action<EmsError> onFailure = null,
            Action<bool> onComplete = null)
        {
            var requestParams = new BuildingFloorEquipment_List.RequestContent {buildingId = buildingId, buildingFloorId = buildingFloorId};

            //Launch request
            var listBuildingFloorEquipsRequest = AppearitionRequest<BuildingFloorEquipment_List>.LaunchAPICall_POST<BuildingHandler>(channelId,
                GetReusableApiRequest<BuildingFloorEquipment_List>(), null, requestParams);

            //Wait for request..
            while (!listBuildingFloorEquipsRequest.IsDone)
                yield return null;

            bool isSuccess = listBuildingFloorEquipsRequest.RequestResponseObject.Data != null && listBuildingFloorEquipsRequest.RequestResponseObject.IsSuccess;

            //All done!
            if (isSuccess)
            {
                List<BuildingFloorEquipment> outcome = listBuildingFloorEquipsRequest.RequestResponseObject.Data;

                AppearitionLogger.LogInfo(string.Format(BuildingConstants.BUILDINGFLOOREQUIPMENT_LIST_SUCCESS, outcome.Count));

                //Callback it out ~
                if (onSuccess != null)
                    onSuccess(outcome);
            }
            else
            {
                AppearitionLogger.LogError(string.Format(BuildingConstants.BUILDINGFLOOREQUIPMENT_LIST_FAILURE, channelId));

                //Request failed =(
                if (onFailure != null)
                    onFailure(listBuildingFloorEquipsRequest.Errors);
            }

            if (onComplete != null)
                onComplete(isSuccess);
        }

        #endregion

        #endregion

        #region Building Media

        #region Add

        /// <summary>
        /// Uploads and adds a new media to the given building.
        /// </summary>
        /// <param name="building">Target building</param>
        /// <param name="media">Template for the BuildingMedia data</param>
        /// <param name="fileToUpload">File to be uploaded, whether in byte[] or as a texture.</param>
        /// <param name="onSuccess">Contains the newly created building. Only called if the request is successful.</param>
        /// <param name="onFailure">Contains any error obtained during the request. Only called if the request has failed.</param>
        /// <param name="onComplete">Always called at the end of the request, defines whether the request was successful or not.</param>
        /// <param name="uploadTransferStatus"></param>
        public static void UploadBuildingMedia(Building building, BuildingMedia media, object fileToUpload, Action<BuildingMedia> onSuccess = null,
            Action<EmsError> onFailure = null,
            Action<bool> onComplete = null, DataTransferStatus uploadTransferStatus = null)
        {
            AppearitionGate.Instance.StartCoroutine(UploadBuildingMediaProcess(AppearitionGate.Instance.CurrentUser.selectedChannel, building, media, fileToUpload,
                onSuccess, onFailure, onComplete, uploadTransferStatus));
        }

        /// <summary>
        /// Uploads and adds a new media to the given building.
        /// The filename and mediatype must be set inside the building media.
        /// </summary>
        /// <param name="building">Target building</param>
        /// <param name="media">Template for the BuildingMedia data</param>
        /// <param name="fileToUpload">File to be uploaded, whether in byte[] or as a texture.</param>
        /// <param name="onSuccess">Contains the newly created building. Only called if the request is successful.</param>
        /// <param name="onFailure">Contains any error obtained during the request. Only called if the request has failed.</param>
        /// <param name="onComplete">Always called at the end of the request, defines whether the request was successful or not.</param>
        /// <param name="uploadTransferStatus"></param>
        public static IEnumerator UploadBuildingMediaProcess(Building building, BuildingMedia media, object fileToUpload, Action<BuildingMedia> onSuccess = null,
            Action<EmsError> onFailure = null,
            Action<bool> onComplete = null, DataTransferStatus uploadTransferStatus = null)
        {
            yield return UploadBuildingMediaProcess(AppearitionGate.Instance.CurrentUser.selectedChannel, building, media, fileToUpload, onSuccess, onFailure, onComplete, uploadTransferStatus);
        }

        /// <summary>
        /// Uploads and adds a new media to the given building.
        /// </summary>
        /// <param name="channelId">The target channel id</param>
        /// <param name="building">Target building</param>
        /// <param name="media">Template for the BuildingMedia data</param>
        /// <param name="fileToUpload">File to be uploaded, whether in byte[] or as a texture.</param>
        /// <param name="onSuccess">Contains the newly created building. Only called if the request is successful.</param>
        /// <param name="onFailure">Contains any error obtained during the request. Only called if the request has failed.</param>
        /// <param name="onComplete">Always called at the end of the request, defines whether the request was successful or not.</param>
        /// <param name="uploadTransferStatus"></param>
        public static void UploadBuildingMedia(int channelId, Building building, BuildingMedia media, object fileToUpload, Action<BuildingMedia> onSuccess = null,
            Action<EmsError> onFailure = null,
            Action<bool> onComplete = null, DataTransferStatus uploadTransferStatus = null)
        {
            AppearitionGate.Instance.StartCoroutine(UploadBuildingMediaProcess(channelId, building, media, fileToUpload, onSuccess, onFailure, onComplete, uploadTransferStatus));
        }

        /// <summary>
        /// Uploads and adds a new media to the given building.
        /// The filename and mediatype must be set inside the building media.
        /// </summary>
        /// <param name="channelId">The target channel id</param>
        /// <param name="building">Target building</param>
        /// <param name="media">Template for the BuildingMedia data</param>
        /// <param name="fileToUpload">File to be uploaded, whether in byte[] or as a texture.</param>
        /// <param name="onSuccess">Contains the newly created building. Only called if the request is successful.</param>
        /// <param name="onFailure">Contains any error obtained during the request. Only called if the request has failed.</param>
        /// <param name="onComplete">Always called at the end of the request, defines whether the request was successful or not.</param>
        /// <param name="uploadTransferStatus"></param>
        public static IEnumerator UploadBuildingMediaProcess(int channelId, Building building, BuildingMedia media, object fileToUpload, Action<BuildingMedia> onSuccess = null,
            Action<EmsError> onFailure = null,
            Action<bool> onComplete = null, DataTransferStatus uploadTransferStatus = null)
        {
            if (building == null)
            {
                string message = string.Format(BuildingConstants.BUILDING_NULL_BUILDING);
                AppearitionLogger.LogError(message);
                onFailure?.Invoke(new EmsError(message));
                onComplete?.Invoke(false);
                yield break;
            }

            if (media == null)
            {
                string message = string.Format(BuildingConstants.BUILDING_MEDIA_NULL);
                AppearitionLogger.LogError(message);
                onFailure?.Invoke(new EmsError(message));
                onComplete?.Invoke(false);
                yield break;
            }


            if (fileToUpload == null)
            {
                string message = string.Format(BuildingConstants.BUILDING_MEDIA_FILE_NULL);
                AppearitionLogger.LogError(message);
                onFailure?.Invoke(new EmsError(message));
                onComplete?.Invoke(false);
                yield break;
            }

            //Create the upload multipart form ApiData
            List<MultiPartFormParam> multiPartParam = new List<MultiPartFormParam>() {
                //Don't convert if it's a Texture or Texture2D. Those are not serializable for.. reasons..
                new MultiPartFormParam(media.fileName + "_Media", fileToUpload, media.fileName, media.mimeType)
            };

            //Create the URL extra params
            var requestContent = new BuildingMedia_Add.RequestContent() {
                buildingId = building.buildingId
            };

            //Launch the request
            var uploadRequest =
                AppearitionRequest<BuildingMedia_Add>.LaunchAPICall_MultiPartPOST<BuildingHandler>(channelId, GetReusableApiRequest<BuildingMedia_Add>(), requestContent,
                    multiPartParam,
                    media.mediaType);

            string uploadItemKey = new Guid().ToString();
            bool isTrackingUpload = uploadTransferStatus != null;
            if (isTrackingUpload)
                uploadTransferStatus.AddNewItemProgress(uploadItemKey, media.fileName,
                    uploadRequest.SizeOfFileBeingUploaded > 0 ? uploadRequest.SizeOfFileBeingUploaded : BuildingConstants.FILE_UPLOAD_PLACEHOLDER_SIZE);

            while (!uploadRequest.IsDone)
            {
                if (isTrackingUpload)
                    uploadTransferStatus.UpdateItemProgress(uploadItemKey, uploadRequest.Progress);
                yield return null;
            }

            //Debug output
            if (uploadRequest.RequestResponseObject == null || !uploadRequest.RequestResponseObject.IsSuccess)
            {
                AppearitionLogger.LogError(string.Format(BuildingConstants.BUILDING_MEDIA_ADD_FAILURE, building.buildingId, channelId));

                if (onFailure != null)
                    onFailure(uploadRequest.Errors);
                if (onComplete != null)
                    onComplete(false);
            }
            else
            {
                AppearitionLogger.LogInfo(string.Format(BuildingConstants.BUILDING_MEDIA_ADD_SUCCESS, media.buildingId, building.buildingId, channelId));

                //Copy file to its right location
                yield return SaveContentToFileProcess(fileToUpload.ToByteArrayExtended(), GetPathToBuildingMedia(building, media));
                
                if (onSuccess != null)
                    onSuccess(uploadRequest.RequestResponseObject.Data);
                if (onComplete != null)
                    onComplete(true);
            }
        }

        #endregion

        #region Get

        /// <summary>
        /// Fetches a single building media with matching Id.
        /// </summary>
        /// <param name="building">Target building</param>
        /// <param name="buildingMediaId">Target building media</param>
        /// <param name="onSuccess">Contains the building, if found. Only called if the request is successful.</param>
        /// <param name="onFailure">Contains any error obtained during the request. Only called if the request has failed.</param>
        /// <param name="onComplete">Always called at the end of the request, defines whether the request was successful or not.</param>
        public static void GetSingleBuildingMedia(Building building, int buildingMediaId, Action<BuildingMedia> onSuccess = null, Action<EmsError> onFailure = null,
            Action<bool> onComplete = null)
        {
            AppearitionGate.Instance.StartCoroutine(GetSingleBuildingMediaProcess(AppearitionGate.Instance.CurrentUser.selectedChannel, building, buildingMediaId, false, onSuccess, onFailure,
                onComplete, null));
        }

        /// <summary>
        /// Fetches a single building media with matching Id.
        /// </summary>
        /// <param name="building">Target building</param>
        /// <param name="buildingMediaId">Target building media</param>
        /// <param name="onSuccess">Contains the building, if found. Only called if the request is successful.</param>
        /// <param name="onFailure">Contains any error obtained during the request. Only called if the request has failed.</param>
        /// <param name="onComplete">Always called at the end of the request, defines whether the request was successful or not.</param>
        public static IEnumerator GetSingleBuildingMediaProcess(Building building, int buildingMediaId, Action<BuildingMedia> onSuccess = null,
            Action<EmsError> onFailure = null, Action<bool> onComplete = null)
        {
            yield return GetSingleBuildingMediaProcess(AppearitionGate.Instance.CurrentUser.selectedChannel, building, buildingMediaId, false, onSuccess, onFailure, onComplete, null);
        }

        /// <summary>
        /// Fetches a single building media with matching Id.
        /// </summary>
        /// <param name="channelId">The target channel id</param>
        /// <param name="building">Target building</param>
        /// <param name="buildingMediaId">Target building media</param>
        /// <param name="onSuccess">Contains the building, if found. Only called if the request is successful.</param>
        /// <param name="onFailure">Contains any error obtained during the request. Only called if the request has failed.</param>
        /// <param name="onComplete">Always called at the end of the request, defines whether the request was successful or not.</param>
        public static void GetSingleBuildingMedia(int channelId, Building building, int buildingMediaId, Action<BuildingMedia> onSuccess = null, Action<EmsError> onFailure = null,
            Action<bool> onComplete = null)
        {
            AppearitionGate.Instance.StartCoroutine(GetSingleBuildingMediaProcess(channelId, building, buildingMediaId, false, onSuccess, onFailure, onComplete, null));
        }

        /// <summary>
        /// Fetches a single building media with matching Id.
        /// </summary>
        /// <param name="channelId">The target channel id</param>
        /// <param name="building">Target building</param>
        /// <param name="buildingMediaId">Target building media</param>
        /// <param name="onSuccess">Contains the building, if found. Only called if the request is successful.</param>
        /// <param name="onFailure">Contains any error obtained during the request. Only called if the request has failed.</param>
        /// <param name="onComplete">Always called at the end of the request, defines whether the request was successful or not.</param>
        public static IEnumerator GetSingleBuildingMediaProcess(int channelId, Building building, int buildingMediaId, Action<BuildingMedia> onSuccess = null,
            Action<EmsError> onFailure = null, Action<bool> onComplete = null)
        {
            yield return GetSingleBuildingMediaProcess(channelId, building, buildingMediaId, false, onSuccess, onFailure, onComplete, null);
        }

        /// <summary>
        /// Fetches a single building media with matching Id.
        /// </summary>
        /// <param name="building">Target building</param>
        /// <param name="buildingMediaId">Target building media</param>
        /// <param name="downloadMedia">Whether the media should get downloaded.</param>
        /// <param name="onSuccess">Contains the building, if found. Only called if the request is successful.</param>
        /// <param name="onFailure">Contains any error obtained during the request. Only called if the request has failed.</param>
        /// <param name="onComplete">Always called at the end of the request, defines whether the request was successful or not.</param>
        /// <param name="downloadTransferStatus"></param>
        public static void GetSingleBuildingMedia(Building building, int buildingMediaId, bool downloadMedia, Action<BuildingMedia> onSuccess = null, Action<EmsError> onFailure = null,
            Action<bool> onComplete = null, DataTransferStatus downloadTransferStatus = null)
        {
            AppearitionGate.Instance.StartCoroutine(GetSingleBuildingMediaProcess(AppearitionGate.Instance.CurrentUser.selectedChannel, building, buildingMediaId, downloadMedia, onSuccess, onFailure,
                onComplete, downloadTransferStatus));
        }

        /// <summary>
        /// Fetches a single building media with matching Id.
        /// </summary>
        /// <param name="building">Target building</param>
        /// <param name="buildingMediaId">Target building media</param>
        /// <param name="downloadMedia">Whether the media should get downloaded.</param>
        /// <param name="onSuccess">Contains the building, if found. Only called if the request is successful.</param>
        /// <param name="onFailure">Contains any error obtained during the request. Only called if the request has failed.</param>
        /// <param name="onComplete">Always called at the end of the request, defines whether the request was successful or not.</param>
        /// <param name="downloadTransferStatus"></param>
        public static IEnumerator GetSingleBuildingMediaProcess(Building building, int buildingMediaId, bool downloadMedia, Action<BuildingMedia> onSuccess = null,
            Action<EmsError> onFailure = null, Action<bool> onComplete = null, DataTransferStatus downloadTransferStatus = null)
        {
            yield return GetSingleBuildingMediaProcess(AppearitionGate.Instance.CurrentUser.selectedChannel, building, buildingMediaId, downloadMedia, onSuccess, onFailure, onComplete,
                downloadTransferStatus);
        }

        /// <summary>
        /// Fetches a single building media with matching Id.
        /// </summary>
        /// <param name="channelId">The target channel id</param>
        /// <param name="building">Target building</param>
        /// <param name="buildingMediaId">Target building media</param>
        /// <param name="onSuccess">Contains the building, if found. Only called if the request is successful.</param>
        /// <param name="onFailure">Contains any error obtained during the request. Only called if the request has failed.</param>
        /// <param name="onComplete">Always called at the end of the request, defines whether the request was successful or not.</param>
        /// <param name="downloadTransferStatus"></param>
        public static void GetSingleBuildingMedia(int channelId, Building building, int buildingMediaId, bool downloadMedia, Action<BuildingMedia> onSuccess = null, Action<EmsError> onFailure = null,
            Action<bool> onComplete = null, DataTransferStatus downloadTransferStatus = null)
        {
            AppearitionGate.Instance.StartCoroutine(GetSingleBuildingMediaProcess(channelId, building, buildingMediaId, downloadMedia, onSuccess, onFailure, onComplete, downloadTransferStatus));
        }

        /// <summary>
        /// Fetches a single building media with matching Id.
        /// </summary>
        /// <param name="channelId">The target channel id</param>
        /// <param name="building">Target building</param>
        /// <param name="buildingMediaId">Target building media</param>
        /// <param name="downloadMedia">Whether the media should get downloaded.</param>
        /// <param name="onSuccess">Contains the building, if found. Only called if the request is successful.</param>
        /// <param name="onFailure">Contains any error obtained during the request. Only called if the request has failed.</param>
        /// <param name="onComplete">Always called at the end of the request, defines whether the request was successful or not.</param>
        /// <param name="downloadTransferStatus"></param>
        public static IEnumerator GetSingleBuildingMediaProcess(int channelId, Building building, int buildingMediaId, bool downloadMedia, Action<BuildingMedia> onSuccess = null,
            Action<EmsError> onFailure = null, Action<bool> onComplete = null, DataTransferStatus downloadTransferStatus = null)
        {
            //UnityEngine.Debug.LogError("EMS API not implemented at this point in time.");
            //onFailure?.Invoke(new EmsError("EMS API not implemented at this point in time.", 500));
            //onComplete?.Invoke(false);
            //yield break;

            var requestParams = new BuildingMedia_Get.RequestContent() {buildingId = building.buildingId, buildingMediaId = buildingMediaId};

            //Launch request
            var getSingleBuildingMediaRequest = AppearitionRequest<BuildingMedia_Get>.LaunchAPICall_GET<BuildingHandler>(channelId, GetReusableApiRequest<BuildingMedia_Get>(), requestParams);

            //Wait for request..
            while (!getSingleBuildingMediaRequest.IsDone)
                yield return null;

            //All done!
            if (getSingleBuildingMediaRequest.RequestResponseObject.IsSuccess)
            {
                BuildingMedia outcome = getSingleBuildingMediaRequest.RequestResponseObject.Data;

                if (outcome == null)
                    AppearitionLogger.LogInfo(string.Format(BuildingConstants.BUILDING_MEDIA_GET_NOT_FOUND, buildingMediaId, building.buildingId, channelId));
                else
                    AppearitionLogger.LogInfo(string.Format(BuildingConstants.BUILDING_MEDIA_GET_SUCCESS, buildingMediaId, outcome.buildingId));

                if (downloadMedia && outcome != null && !string.IsNullOrEmpty(outcome.fileName))
                    yield return DownloadBuildingMediaContent(building, new List<BuildingMedia> {outcome}, downloadTransferStatus);

                //Callback it out ~
                if (onSuccess != null)
                    onSuccess(outcome);
            }
            else
            {
                AppearitionLogger.LogError(string.Format(BuildingConstants.BUILDING_MEDIA_GET_FAILURE, buildingMediaId, building.buildingId, channelId));

                //Request failed =(
                if (onFailure != null)
                    onFailure(getSingleBuildingMediaRequest.Errors);
            }

            if (onComplete != null)
                onComplete(getSingleBuildingMediaRequest.RequestResponseObject.Data != null && getSingleBuildingMediaRequest.RequestResponseObject.IsSuccess);
        }

        #endregion

        #region List

        /// <summary>
        /// Fetches all the buildings matching a given query and given channel.
        /// </summary>
        /// <param name="building">Target building.</param>
        /// <param name="onSuccess">Contains all the buildings found using the given query. Only called if the request is successful.</param>
        /// <param name="onFailure">Contains any error obtained during the request. Only called if the request has failed.</param>
        /// <param name="onComplete">Always called at the end of the request, defines whether the request was successful or not.</param>
        public static void GetBuildingMedias(Building building, Action<List<BuildingMedia>> onSuccess = null,
            Action<EmsError> onFailure = null, Action<bool> onComplete = null)
        {
            AppearitionGate.Instance.StartCoroutine(GetBuildingMediasProcess(AppearitionGate.Instance.CurrentUser.selectedChannel, building, false, onSuccess, onFailure, onComplete, null));
        }

        /// <summary>
        /// Fetches all the buildings matching a given query and given channel.
        /// </summary>
        /// <param name="building">Target building.</param>
        /// <param name="onSuccess">Contains all the buildings found using the given query. Only called if the request is successful.</param>
        /// <param name="onFailure">Contains any error obtained during the request. Only called if the request has failed.</param>
        /// <param name="onComplete">Always called at the end of the request, defines whether the request was successful or not.</param>
        public static IEnumerator GetBuildingMediasProcess(Building building, Action<List<BuildingMedia>> onSuccess = null,
            Action<EmsError> onFailure = null, Action<bool> onComplete = null)
        {
            yield return GetBuildingMediasProcess(AppearitionGate.Instance.CurrentUser.selectedChannel, building, false, onSuccess, onFailure, onComplete, null);
        }

        /// <summary>
        /// Fetches all the buildings matching a given query and given channel.
        /// </summary>
        /// <param name="channelId">The target channel id</param>
        /// <param name="building">Target building.</param>
        /// <param name="onSuccess">Contains all the buildings found using the given query. Only called if the request is successful.</param>
        /// <param name="onFailure">Contains any error obtained during the request. Only called if the request has failed.</param>
        /// <param name="onComplete">Always called at the end of the request, defines whether the request was successful or not.</param>
        public static void GetBuildingMedias(int channelId, Building building, Action<List<BuildingMedia>> onSuccess = null,
            Action<EmsError> onFailure = null, Action<bool> onComplete = null)
        {
            AppearitionGate.Instance.StartCoroutine(GetBuildingMediasProcess(channelId, building, false, onSuccess, onFailure, onComplete, null));
        }

        /// <summary>
        /// Fetches all the buildings matching a given query and given channel.
        /// </summary>
        /// <param name="channelId">The target channel id</param>
        /// <param name="building">Target building.</param>
        /// <param name="onSuccess">Contains all the buildings found using the given query. Only called if the request is successful.</param>
        /// <param name="onFailure">Contains any error obtained during the request. Only called if the request has failed.</param>
        /// <param name="onComplete">Always called at the end of the request, defines whether the request was successful or not.</param>
        public static IEnumerator GetBuildingMediasProcess(int channelId, Building building, Action<List<BuildingMedia>> onSuccess = null,
            Action<EmsError> onFailure = null, Action<bool> onComplete = null)
        {
            yield return GetBuildingMediasProcess(channelId, building, false, onSuccess, onFailure, onComplete, null);
        }

        /// <summary>
        /// Fetches all the buildings matching a given query and given channel.
        /// </summary>
        /// <param name="building">Target building.</param>
        /// <param name="downloadMedias">Whether the medias should get downloaded.</param>
        /// <param name="onSuccess">Contains all the buildings found using the given query. Only called if the request is successful.</param>
        /// <param name="onFailure">Contains any error obtained during the request. Only called if the request has failed.</param>
        /// <param name="onComplete">Always called at the end of the request, defines whether the request was successful or not.</param>
        /// <param name="downloadTransferStatus"></param>
        public static void GetBuildingMedias(Building building, bool downloadMedias = false, Action<List<BuildingMedia>> onSuccess = null,
            Action<EmsError> onFailure = null,
            Action<bool> onComplete = null, DataTransferStatus downloadTransferStatus = null)
        {
            AppearitionGate.Instance.StartCoroutine(GetBuildingMediasProcess(AppearitionGate.Instance.CurrentUser.selectedChannel, building, downloadMedias, onSuccess, onFailure, onComplete,
                downloadTransferStatus));
        }

        /// <summary>
        /// Fetches all the buildings matching a given query and given channel.
        /// </summary>
        /// <param name="building">Target building.</param>
        /// <param name="downloadMedias">Whether the medias should get downloaded.</param>
        /// <param name="onSuccess">Contains all the buildings found using the given query. Only called if the request is successful.</param>
        /// <param name="onFailure">Contains any error obtained during the request. Only called if the request has failed.</param>
        /// <param name="onComplete">Always called at the end of the request, defines whether the request was successful or not.</param>
        /// <param name="downloadTransferStatus"></param>
        public static IEnumerator GetBuildingMediasProcess(Building building, bool downloadMedias = false, Action<List<BuildingMedia>> onSuccess = null,
            Action<EmsError> onFailure = null,
            Action<bool> onComplete = null, DataTransferStatus downloadTransferStatus = null)
        {
            yield return GetBuildingMediasProcess(AppearitionGate.Instance.CurrentUser.selectedChannel, building, downloadMedias, onSuccess, onFailure, onComplete, downloadTransferStatus);
        }

        /// <summary>
        /// Fetches all the buildings matching a given query and given channel.
        /// </summary>
        /// <param name="channelId">The target channel id</param>
        /// <param name="building">Target building.</param>
        /// <param name="downloadMedias">Whether the medias should get downloaded.</param>
        /// <param name="onSuccess">Contains all the buildings found using the given query. Only called if the request is successful.</param>
        /// <param name="onFailure">Contains any error obtained during the request. Only called if the request has failed.</param>
        /// <param name="onComplete">Always called at the end of the request, defines whether the request was successful or not.</param>
        /// <param name="downloadTransferStatus"></param>
        public static void GetBuildingMedias(int channelId, Building building, bool downloadMedias = false, Action<List<BuildingMedia>> onSuccess = null,
            Action<EmsError> onFailure = null,
            Action<bool> onComplete = null, DataTransferStatus downloadTransferStatus = null)
        {
            AppearitionGate.Instance.StartCoroutine(GetBuildingMediasProcess(channelId, building, downloadMedias, onSuccess, onFailure, onComplete, downloadTransferStatus));
        }

        /// <summary>
        /// Fetches all the buildings matching a given query and given channel.
        /// </summary>
        /// <param name="channelId">The target channel id</param>
        /// <param name="building">Target building.</param>
        /// <param name="downloadMedias">Whether the medias should get downloaded.</param>
        /// <param name="onSuccess">Contains all the buildings found using the given query. Only called if the request is successful.</param>
        /// <param name="onFailure">Contains any error obtained during the request. Only called if the request has failed.</param>
        /// <param name="onComplete">Always called at the end of the request, defines whether the request was successful or not.</param>
        /// <param name="downloadTransferStatus"></param>
        public static IEnumerator GetBuildingMediasProcess(int channelId, Building building, bool downloadMedias = false, Action<List<BuildingMedia>> onSuccess = null,
            Action<EmsError> onFailure = null,
            Action<bool> onComplete = null, DataTransferStatus downloadTransferStatus = null)
        {
            //Create the URL extra params
            var requestContent = new BuildingMedia_Add.RequestContent() {
                buildingId = building.buildingId
            };

            //Launch request
            var listBuildingMediasRequest = AppearitionRequest<BuildingMedia_List>.LaunchAPICall_GET<BuildingHandler>(channelId, GetReusableApiRequest<BuildingMedia_List>(), requestContent);

            //Wait for request..
            while (!listBuildingMediasRequest.IsDone)
                yield return null;

            bool isSuccess = listBuildingMediasRequest.RequestResponseObject.Data != null && listBuildingMediasRequest.RequestResponseObject.IsSuccess;
            //All done!
            if (isSuccess)
            {
                List<BuildingMedia> outcome = new List<BuildingMedia>(listBuildingMediasRequest.RequestResponseObject.Data);

                AppearitionLogger.LogInfo(string.Format(BuildingConstants.BUILDING_MEDIA_LIST_SUCCESS, outcome.Count, building.buildingId));

                if (downloadMedias && outcome.Count > 0)
                    yield return DownloadBuildingMediaContent(building, outcome, downloadTransferStatus);

                //Callback it out ~
                if (onSuccess != null)
                    onSuccess(outcome);
            }
            else
            {
                AppearitionLogger.LogError(string.Format(BuildingConstants.BUILDING_MEDIA_LIST_FAILURE, building.buildingId, channelId));

                //Request failed =(
                if (onFailure != null)
                    onFailure(listBuildingMediasRequest.Errors);
            }

            if (onComplete != null)
                onComplete(isSuccess);
        }

        #endregion

        #region Remove

        /// <summary>
        /// Fetches a single building media with matching Id.
        /// </summary>
        /// <param name="building">Target building</param>
        /// <param name="media">Target building media</param>
        /// <param name="deleteLocally">Whether the file should get deleted locally.</param>
        /// <param name="onSuccess">Contains the building, if found. Only called if the request is successful.</param>
        /// <param name="onFailure">Contains any error obtained during the request. Only called if the request has failed.</param>
        /// <param name="onComplete">Always called at the end of the request, defines whether the request was successful or not.</param>
        public static IEnumerator RemoveBuildingMediaProcess(Building building, BuildingMedia media, bool deleteLocally, Action<BuildingMedia> onSuccess = null,
            Action<EmsError> onFailure = null, Action<bool> onComplete = null)
        {
            var requestParams = new BuildingMedia_Remove.RequestContent() {buildingId = building.buildingId, buildingMediaId = media.buildingId};

            //Launch request
            var removeMediaProcess = AppearitionRequest<BuildingMedia_Get>.LaunchAPICall_GET<BuildingHandler>(media.productId, GetReusableApiRequest<BuildingMedia_Get>(), requestParams);

            //Wait for request..
            while (!removeMediaProcess.IsDone)
                yield return null;

            //All done!
            if (removeMediaProcess.RequestResponseObject.IsSuccess)
            {
                BuildingMedia outcome = removeMediaProcess.RequestResponseObject.Data;

                AppearitionLogger.LogInfo(string.Format(BuildingConstants.BUILDING_MEDIA_REMOVE_SUCCESS, media.id, building.buildingId, media.productId));

                //Also delete it locally
                if (deleteLocally)
                    yield return DeleteFileProcess(GetPathToBuildingMedia(building, media));

                //Callback it out ~
                if (onSuccess != null)
                    onSuccess(outcome);
            }
            else
            {
                AppearitionLogger.LogError(string.Format(BuildingConstants.BUILDING_MEDIA_REMOVE_FAILURE, media.id, media.buildingId, media.productId));

                //Request failed =(
                if (onFailure != null)
                    onFailure(removeMediaProcess.Errors);
            }

            if (onComplete != null)
                onComplete(removeMediaProcess.RequestResponseObject.Data != null && removeMediaProcess.RequestResponseObject.IsSuccess);
        }

        #endregion

        #endregion

        #region File Handling

        #region Building Media

        /// <summary>
        /// Downloads the selected content of a given building media.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="building">Target building</param>
        /// <param name="content"></param>
        /// <param name="downloadTransferStatus"></param>
        /// <returns></returns>
        public static IEnumerator DownloadBuildingMediaContent<T>(Building building, T content,
            DataTransferStatus downloadTransferStatus = null) where T : BuildingMedia
        {
            yield return DownloadBuildingMediaContent(building, new List<T> {content}, downloadTransferStatus);
        }

        /// <summary>
        /// Downloads the selected content of a given building media.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="building">Target building</param>
        /// <param name="contents"></param>
        /// <param name="downloadTransferStatus"></param>
        /// <returns></returns>
        public static IEnumerator DownloadBuildingMediaContent<T>(Building building, IEnumerable<T> contents,
            DataTransferStatus downloadTransferStatus = null) where T : BuildingMedia
        {
            int totalMediaCount = 0;
            int mediaDownloadedCount = 0;

            bool transferStatusCreated = false;
            if (downloadTransferStatus == null)
            {
                downloadTransferStatus = new DataTransferStatus();
                transferStatusCreated = true;
            }


            foreach (T tmpMedia in contents)
            {
                if (tmpMedia == null)
                    continue;

                if (!string.IsNullOrEmpty(tmpMedia.url) &&
                    !string.IsNullOrEmpty(tmpMedia.fileName))
                {
                    //Manually download it.
                    if (BuildingConstants.allowParallelDownloads)
                    {
                        AppearitionGate.Instance.StartCoroutine(DownloadGenericFile(tmpMedia.url.Trim(), GetPathToBuildingMedia(building, tmpMedia), tmpMedia.checksum, true,
                            obj => mediaDownloadedCount++,
                            downloadTransferStatus));
                    }
                    else
                    {
                        yield return DownloadGenericFile(tmpMedia.url.Trim(), GetPathToBuildingMedia(building, tmpMedia), tmpMedia.checksum, true, obj => mediaDownloadedCount++,
                            downloadTransferStatus);
                    }

                    totalMediaCount++;
                }
            }

            //while (totalMediaCount != mediaDownloadedCount)
            //    yield return null;

            //Apply lock
            if (transferStatusCreated)
                downloadTransferStatus.ConfirmLastItemEntered();

            while (totalMediaCount != mediaDownloadedCount)
                yield return null;
        }

        /// <summary>
        /// Deletes the cached content of a building based on given parameters.
        /// </summary>
        /// <typeparam name="T">The type of asset.</typeparam>
        /// <param name="building">The given building.</param>
        /// <param name="deleteBuildingMedias">Whether or not the building medias are to be deleted.</param>
        /// <param name="onComplete">Occurs when the process is completed. Contains whether or not this process has been successfully completed.</param>
        public static void ClearLocalAssetContent<T>(T building, bool deleteBuildingMedias, Action<bool> onComplete) where T : Building
        {
            AppearitionGate.Instance.StartCoroutine(ClearLocalAssetContentProcess<T>(building, deleteBuildingMedias, onComplete));
        }

        /// <summary>
        /// Deletes the cached content of a building based on given parameters.
        /// </summary>
        /// <typeparam name="T">The type of asset.</typeparam>
        /// <param name="building">The given building.</param>
        /// <param name="deleteBuildingMedias">Whether or not the building medias are to be deleted.</param>
        /// <param name="onComplete">Occurs when the process is completed. Contains whether or not this process has been successfully completed.</param>
        public static IEnumerator ClearLocalAssetContentProcess<T>(T building, bool deleteBuildingMedias, Action<bool> onComplete) where T : Building
        {
            bool isSuccessful = true;

            if (deleteBuildingMedias)
            {
                string[] files = Directory.GetFiles(GetPathToBuildingMediaDirectory(building));

                for (int i = 0; i < files.Length; i++)
                {
                    if (!File.Exists(files[i]))
                        continue;

                    yield return DeleteFileProcess(files[i], success => isSuccessful = isSuccessful && success);

                    if (!isSuccessful)
                    {
                        AppearitionLogger.LogError("An error occured when trying to delete the file at path " + files[i]);
                    }
                }
            }

            if (onComplete != null)
                onComplete(isSuccessful);
        }

        #endregion

        #endregion
    }
}