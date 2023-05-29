// // -----------------------------------------------------------------------
// // Company:"Appearition Pty Ltd"
// // File: LocationHandler.cs
// // Copyright (c) 2019. All rights reserved.
// // -----------------------------------------------------------------------

using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Appearition.Common;
using Appearition.Location.API;
using UnityEngine;

namespace Appearition.Location
{
    /// <summary>
    /// Handles the API requests for the Location module.
    /// </summary>
    public sealed class LocationHandler : BaseHandler
    {
        #region Handler Settings

        /// <summary>
        /// Path to the container of a single Location entry.
        /// </summary>
        /// <param name="location">Given location data.</param>
        /// <returns>Full path to the location entry directory.</returns>
        public static string GetPathToLocationDirectory(Location location)
        {
            return string.Format("{0}/{1}", GetHandlerStoragePath<LocationHandler>(), location.Name);
        }

        /// <summary>
        /// Path to the Label Image directory for a given location and PointOfInterest.
        /// </summary>
        /// <param name="location">Given location data.</param>
        /// <param name="poi">Given PointOfInterest.</param>
        /// <returns>Full path to the label directory for the given location data.</returns>
        public static string GetPathToLabelDirectory(Location location, PointOfInterest poi)
        {
            return GetPathToLocationDirectory(location);
        }

        /// <summary>
        /// Path to the Marker Image directory for a given location and PointOfInterest.
        /// </summary>
        /// <param name="location">Given location data.</param>
        /// <param name="poi">Given PointOfInterest.</param>
        /// <returns>Full path to the marker directory for the given location data.</returns>
        public static string GetPathToMarkerImageDirectory(Location location, PointOfInterest poi)
        {
            return GetPathToLocationDirectory(location);
        }

        /// <summary>
        /// Path to the Info Image directory for a given location and PointOfInterest.
        /// </summary>
        /// <param name="location">Given location data.</param>
        /// <param name="poi">Given PointOfInterest.</param>
        /// <returns>Full path to the given info directory for the given location data.</returns>
        public static string GetPathToInfoImageDirectory(Location location, PointOfInterest poi)
        {
            return GetPathToLocationDirectory(location);
        }

        /// <summary>
        /// Path to the Label Image for a given location and PointOfInterest.
        /// </summary>
        /// <param name="location">Given location data.</param>
        /// <param name="poi">Given PointOfInterest.</param>
        /// <returns>Full path to the given label image for the given location data.</returns>
        public static string GetPathToLabelImage(Location location, PointOfInterest poi)
        {
            return string.Format("{0}/{1}", GetPathToLabelDirectory(location, poi), poi.LabelImageFileName);
        }

        /// <summary>
        /// Path to the Marker Image for a given location and PointOfInterest.
        /// </summary>
        /// <param name="location">Given location data.</param>
        /// <param name="poi">Given PointOfInterest.</param>
        /// <returns>Full path to the given marker image for the given location data.</returns>
        public static string GetPathToMarkerImage(Location location, PointOfInterest poi)
        {
            return string.Format("{0}/{1}", GetPathToLabelDirectory(location, poi), poi.MarkerImageFileName);
        }

        /// <summary>
        /// Path to the Info Image for a given location and PointOfInterest.
        /// </summary>
        /// <param name="location">Given location data.</param>
        /// <param name="poi">Given PointOfInterest.</param>
        /// <returns>Full path to the given info image for the given location data.</returns>
        public static string GetPathToInfoImage(Location location, PointOfInterest poi)
        {
            return string.Format("{0}/{1}", GetPathToLabelDirectory(location, poi), poi.InfoImageFileName);
        }

        #endregion

        #region Get Location Data

        /// <summary>
        /// Fetches all the ArLocations for the channel selected by the current user.
        ///
        /// API Requirement: Anonymous Token. Offline Capability.
        /// </summary>
        /// <param name="onSuccess">Contains the channel ApiData of the selected channel. Only called if the request is successful.</param>
        /// <param name="onFailure">Contains any error obtained during the request. Only called if the request has failed.</param>
        /// <param name="onComplete">Always called at the end of the request, defines whether the request was successful or not.</param>
        public static void GetChannelArLocations(Action<List<Location>> onSuccess = null, Action<EmsError> onFailure = null, Action<bool> onComplete = null)
        {
            GetChannelArLocations
                (AppearitionGate.Instance.CurrentUser.selectedChannel, false, false, false, onSuccess, onFailure, onComplete);
        }

        /// <summary>
        /// Fetches all the ArLocations for the channel selected by the current user.
        ///
        /// API Requirement: Anonymous Token. Offline Capability.
        /// </summary>
        /// <param name="onSuccess">Contains the channel ApiData of the selected channel. Only called if the request is successful.</param>
        /// <param name="onFailure">Contains any error obtained during the request. Only called if the request has failed.</param>
        /// <param name="onComplete">Always called at the end of the request, defines whether the request was successful or not.</param>
        public static IEnumerator GetChannelArLocationsProcess(Action<List<Location>> onSuccess = null, Action<EmsError> onFailure = null, Action<bool> onComplete = null)
        {
            yield return GetChannelArLocationsProcess(AppearitionGate.Instance.CurrentUser.selectedChannel, false, false, false, onSuccess, onFailure, onComplete);
        }

        /// <summary>
        /// Fetches all the ArLocations for the channel selected by the current user.
        ///
        /// API Requirement: Anonymous Token. Offline Capability.
        /// </summary>
        /// <param name="channelId">The id of the targeted channel.</param>
        /// <param name="onSuccess">Contains the channel ApiData of the selected channel. Only called if the request is successful.</param>
        /// <param name="onFailure">Contains any error obtained during the request. Only called if the request has failed.</param>
        /// <param name="onComplete">Always called at the end of the request, defines whether the request was successful or not.</param>
        public static void GetChannelArLocations(int channelId, Action<List<Location>> onSuccess = null, Action<EmsError> onFailure = null, Action<bool> onComplete = null)
        {
            AppearitionGate.Instance.StartCoroutine(GetChannelArLocationsProcess(channelId, false, false, false, onSuccess, onFailure, onComplete));
        }

        /// <summary>
        /// Fetches all the ArLocations for the channel selected by the current user.
        ///
        /// API Requirement: Anonymous Token. Offline Capability.
        /// </summary>
        /// <param name="channelId">The id of the targeted channel.</param>
        /// <param name="onSuccess">Contains the channel ApiData of the selected channel. Only called if the request is successful.</param>
        /// <param name="onFailure">Contains any error obtained during the request. Only called if the request has failed.</param>
        /// <param name="onComplete">Always called at the end of the request, defines whether the request was successful or not.</param>
        public static IEnumerator GetChannelArLocationsProcess(int channelId, Action<List<Location>> onSuccess, Action<EmsError> onFailure, Action<bool> onComplete)
        {
            yield return GetChannelArLocationsProcess(channelId, false, false, false, onSuccess, onFailure, onComplete);
        }

        /// <summary>
        /// Fetches all the ArLocations for the channel selected by the current user.
        /// 
        /// API Requirement: Anonymous Token. Offline Capability.
        /// </summary>
        /// <param name="downloadLabelImageIfAny">Downloads or loads the label images of the PointOfInterests.</param>
        /// <param name="downloadMarkerImageIfAny">Downloads or loads the marker images of the PointOfInterests.</param>
        /// <param name="downloadInfoImageIfAny">Downloads or loads the info images of the PointOfInterests.</param>
        /// <param name="onSuccess">Contains the channel ApiData of the selected channel. Only called if the request is successful.</param>
        /// <param name="onFailure">Contains any error obtained during the request. Only called if the request has failed.</param>
        /// <param name="onComplete">Always called at the end of the request, defines whether the request was successful or not.</param>
        public static void GetChannelArLocations(bool downloadLabelImageIfAny, bool downloadMarkerImageIfAny, bool downloadInfoImageIfAny, Action<List<Location>> onSuccess = null,
            Action<EmsError> onFailure = null, Action<bool> onComplete = null)
        {
            GetChannelArLocations
                (AppearitionGate.Instance.CurrentUser.selectedChannel, downloadLabelImageIfAny, downloadMarkerImageIfAny, downloadInfoImageIfAny, onSuccess, onFailure, onComplete);
        }

        /// <summary>
        /// Fetches all the ArLocations for the channel selected by the current user.
        ///
        /// API Requirement: Anonymous Token. Offline Capability.
        /// </summary>
        /// <param name="downloadLabelImageIfAny">Downloads or loads the label images of the PointOfInterests.</param>
        /// <param name="downloadMarkerImageIfAny">Downloads or loads the marker images of the PointOfInterests.</param>
        /// <param name="downloadInfoImageIfAny">Downloads or loads the info images of the PointOfInterests.</param>
        /// <param name="onSuccess">Contains the channel ApiData of the selected channel. Only called if the request is successful.</param>
        /// <param name="onFailure">Contains any error obtained during the request. Only called if the request has failed.</param>
        /// <param name="onComplete">Always called at the end of the request, defines whether the request was successful or not.</param>
        public static IEnumerator GetChannelArLocationsProcess(bool downloadLabelImageIfAny, bool downloadMarkerImageIfAny, bool downloadInfoImageIfAny, Action<List<Location>> onSuccess = null,
            Action<EmsError> onFailure = null, Action<bool> onComplete = null)
        {
            yield return GetChannelArLocationsProcess(AppearitionGate.Instance.CurrentUser.selectedChannel, downloadLabelImageIfAny, downloadMarkerImageIfAny, downloadInfoImageIfAny, onSuccess,
                onFailure, onComplete);
        }

        /// <summary>
        /// Fetches all the ArLocations for the channel selected by the current user.
        ///
        /// API Requirement: Anonymous Token. Offline Capability.
        /// </summary>
        /// <param name="channelId">The id of the targeted channel.</param>
        /// <param name="downloadLabelImageIfAny">Downloads or loads the label images of the PointOfInterests.</param>
        /// <param name="downloadMarkerImageIfAny">Downloads or loads the marker images of the PointOfInterests.</param>
        /// <param name="downloadInfoImageIfAny">Downloads or loads the info images of the PointOfInterests.</param>
        /// <param name="onSuccess">Contains the channel ApiData of the selected channel. Only called if the request is successful.</param>
        /// <param name="onFailure">Contains any error obtained during the request. Only called if the request has failed.</param>
        /// <param name="onComplete">Always called at the end of the request, defines whether the request was successful or not.</param>
        public static void GetChannelArLocations(int channelId, bool downloadLabelImageIfAny, bool downloadMarkerImageIfAny, bool downloadInfoImageIfAny, Action<List<Location>> onSuccess = null,
            Action<EmsError> onFailure = null, Action<bool> onComplete = null)
        {
            AppearitionGate.Instance.StartCoroutine(
                GetChannelArLocationsProcess(channelId, downloadLabelImageIfAny, downloadMarkerImageIfAny, downloadInfoImageIfAny, onSuccess, onFailure, onComplete));
        }


        /// <summary>
        /// Fetches all the ArLocations for the channel selected by the current user.
        ///
        /// API Requirement: Anonymous Token. Offline Capability.
        /// </summary>
        /// <param name="channelId">The id of the targeted channel.</param>
        /// <param name="downloadLabelImageIfAny">Downloads or loads the label images of the PointOfInterests.</param>
        /// <param name="downloadMarkerImageIfAny">Downloads or loads the marker images of the PointOfInterests.</param>
        /// <param name="downloadInfoImageIfAny">Downloads or loads the info images of the PointOfInterests.</param>
        /// <param name="onSuccess">Contains the channel ApiData of the selected channel. Only called if the request is successful.</param>
        /// <param name="onFailure">Contains any error obtained during the request. Only called if the request has failed.</param>
        /// <param name="onComplete">Always called at the end of the request, defines whether the request was successful or not.</param>
        public static IEnumerator GetChannelArLocationsProcess(int channelId, bool downloadLabelImageIfAny, bool downloadMarkerImageIfAny, bool downloadInfoImageIfAny,
            Action<List<Location>> onSuccess, Action<EmsError> onFailure, Action<bool> onComplete)
        {
            //Online request
            var getLocationByChannelRequest =
                AppearitionRequest<ArLocation_ListByChannel>.LaunchAPICall_GET<LocationHandler>(channelId, GetReusableApiRequest<ArLocation_ListByChannel>());

            while (!getLocationByChannelRequest.IsDone)
                yield return null;

            if (getLocationByChannelRequest.RequestResponseObject.IsSuccess && getLocationByChannelRequest.RequestResponseObject.Data != null)
            {
                //Download the picture contents if required.
                if (downloadLabelImageIfAny || downloadMarkerImageIfAny || downloadInfoImageIfAny)
                {
                    for (int i = 0; i < getLocationByChannelRequest.RequestResponseObject.Data.locations.Count; i++)
                    {
                        for (int k = 0; k < getLocationByChannelRequest.RequestResponseObject.Data.locations[i].Pois.Count; k++)
                            yield return LoadPointOfInterestImagesProcess(getLocationByChannelRequest.RequestResponseObject.Data.locations[i],
                                getLocationByChannelRequest.RequestResponseObject.Data.locations[i].Pois[k], downloadLabelImageIfAny, downloadMarkerImageIfAny, downloadInfoImageIfAny);
                    }
                }

                AppearitionLogger.LogInfo(string.Format("ArLocations successfully fetched for the channel of id {0}.", channelId));
                if (onSuccess != null)
                    onSuccess(getLocationByChannelRequest.RequestResponseObject.Data.locations);
            }
            else
            {
                AppearitionLogger.LogError(string.Format("An error occured when trying to fetch the ArLocations from the channel of id {0}.", channelId));

                if (onFailure != null)
                    onFailure(getLocationByChannelRequest.Errors);
            }

            if (onComplete != null)
                onComplete(getLocationByChannelRequest.RequestResponseObject.IsSuccess);
        }

        #endregion

        #region Address - Lat/Long

        /// <summary>
        /// For a given address, fetches the latitude and longitude.
        /// </summary>
        /// <param name="address">Paging query</param>
        /// <param name="onSuccess">Contains the location data fetched from the EMS.</param>
        /// <param name="onFailure">Contains any error obtained during the request. Only called if the request has failed.</param>
        /// <param name="onComplete">Always called at the end of the request, defines whether the request was successful or not.</param>
        public static void AddressToLatLon(string address, Action<LocationAddress> onSuccess = null, Action<EmsError> onFailure = null,
            Action<bool> onComplete = null)
        {
            AppearitionGate.Instance.StartCoroutine(AddressToLatLonProcess(address, onSuccess, onFailure, onComplete));
        }

        /// <summary>
        /// For a given address, fetches the latitude and longitude.
        /// </summary>
        /// <param name="address">Paging query</param>
        /// <param name="onSuccess">Contains the location data fetched from the EMS.</param>
        /// <param name="onFailure">Contains any error obtained during the request. Only called if the request has failed.</param>
        /// <param name="onComplete">Always called at the end of the request, defines whether the request was successful or not.</param>
        public static IEnumerator AddressToLatLonProcess(string address, Action<LocationAddress> onSuccess = null, Action<EmsError> onFailure = null,
            Action<bool> onComplete = null)
        {
            var request = new Location_LatLongByAddress.RequestContent {address = address};

            //Launch request
            var latLonByAddressRequest = AppearitionRequest<Location_LatLongByAddress>.LaunchAPICall_POST<LocationHandler>(0, GetReusableApiRequest<Location_LatLongByAddress>(), null, request);

            //Wait for request..
            while (!latLonByAddressRequest.IsDone)
                yield return null;

            bool isSuccess = latLonByAddressRequest.RequestResponseObject.Data != null && latLonByAddressRequest.RequestResponseObject.IsSuccess;

            //All done!
            if (isSuccess)
            {
                AppearitionLogger.LogInfo($"Location of address {latLonByAddressRequest.RequestResponseObject.Data.address} " +
                                          $"resulted with geolocation {latLonByAddressRequest.RequestResponseObject.Data.latitude},{latLonByAddressRequest.RequestResponseObject.Data.longitude}");

                //Callback it out ~
                if (onSuccess != null)
                    onSuccess(latLonByAddressRequest.RequestResponseObject.Data);
            }
            else
            {
                AppearitionLogger.LogError($"An error occurred when trying to find the latitude and longitude for the address: {address}\n{latLonByAddressRequest.Errors}");

                //Request failed =(
                if (onFailure != null)
                    onFailure(latLonByAddressRequest.Errors);
            }

            if (onComplete != null)
                onComplete(isSuccess);
        }

        /// <summary>
        /// For a given geolocation, tries to get the associated address.
        /// </summary>
        /// <param name="latitude">Given latitude</param>
        /// <param name="longitude">Given longitude</param>
        /// <param name="onSuccess">Contains the location data fetched from the EMS.</param>
        /// <param name="onFailure">Contains any error obtained during the request. Only called if the request has failed.</param>
        /// <param name="onComplete">Always called at the end of the request, defines whether the request was successful or not.</param>
        public static void LatLonToAddress(double latitude, double longitude, Action<LocationAddress> onSuccess = null, Action<EmsError> onFailure = null,
            Action<bool> onComplete = null)
        {
            AppearitionGate.Instance.StartCoroutine(LatLonToAddressProcess(latitude, longitude, onSuccess, onFailure, onComplete));
        }

        /// <summary>
        /// For a given geolocation, tries to get the associated address.
        /// </summary>
        /// <param name="latitude">Given latitude</param>
        /// <param name="longitude">Given longitude</param>
        /// <param name="onSuccess">Contains the location data fetched from the EMS.</param>
        /// <param name="onFailure">Contains any error obtained during the request. Only called if the request has failed.</param>
        /// <param name="onComplete">Always called at the end of the request, defines whether the request was successful or not.</param>
        public static IEnumerator LatLonToAddressProcess(double latitude, double longitude, Action<LocationAddress> onSuccess = null, Action<EmsError> onFailure = null,
            Action<bool> onComplete = null)
        {
            var request = new Location_AddressByLatLong.RequestContent {latitude = latitude, longitude = longitude};

            //Launch request
            var addressByLatLonProcess = AppearitionRequest<Location_AddressByLatLong>.LaunchAPICall_POST<LocationHandler>(0, GetReusableApiRequest<Location_AddressByLatLong>(), null, request);

            //Wait for request..
            while (!addressByLatLonProcess.IsDone)
                yield return null;

            bool isSuccess = addressByLatLonProcess.RequestResponseObject.Data != null && addressByLatLonProcess.RequestResponseObject.IsSuccess;

            //All done!
            if (isSuccess)
            {
                AppearitionLogger.LogInfo($"The geolocation {latitude},{longitude} resulted with the address {addressByLatLonProcess.RequestResponseObject.Data.address}");

                //Callback it out ~
                if (onSuccess != null)
                    onSuccess(addressByLatLonProcess.RequestResponseObject.Data);
            }
            else
            {
                AppearitionLogger.LogError($"An error occurred when trying to find the address for the geolocation: {latitude},{longitude}\n{addressByLatLonProcess.Errors}");

                //Request failed =(
                if (onFailure != null)
                    onFailure(addressByLatLonProcess.Errors);
            }

            if (onComplete != null)
                onComplete(isSuccess);
        }

        #endregion

        #region Location Properties

        #region Get Properties

        /// <summary>
        /// Fetches the properties of the given Location
        /// </summary>
        /// <param name="location"></param>
        /// <param name="onSuccess">Contains the Location as well as the list of all available properties. Only called if the request is successful.</param>
        /// <param name="onFailure">Contains any error obtained during the request. Only called if the request has failed.</param>
        /// <param name="onComplete">Always called at the end of the request, defines whether the request was successful or not.</param>
        public static void GetArTargetProperties(Location location, Action<Location, List<Property>> onSuccess = null, Action<EmsError> onFailure = null,
            Action<bool> onComplete = null)
        {
            AppearitionGate.Instance.StartCoroutine(GetArTargetPropertiesProcess(location, onSuccess, onFailure, onComplete));
        }

        /// <summary>
        /// Fetches the properties of the given Location
        /// </summary>
        /// <param name="location"></param>
        /// <param name="onSuccess">Contains the Location as well as the list of all available properties. Only called if the request is successful.</param>
        /// <param name="onFailure">Contains any error obtained during the request. Only called if the request has failed.</param>
        /// <param name="onComplete">Always called at the end of the request, defines whether the request was successful or not.</param>
        public static IEnumerator GetArTargetPropertiesProcess(Location location, Action<Location, List<Property>> onSuccess = null, Action<EmsError> onFailure = null,
            Action<bool> onComplete = null)
        {
            //Ref Check
            if (location == null)
            {
                AppearitionLogger.LogError(LocationConstants.LOCATION_NULL);
                if (onFailure != null)
                    onFailure(new EmsError(LocationConstants.LOCATION_NULL));
                if (onComplete != null)
                    onComplete(false);
                yield break;
            }

            //Online request. Prepare request content
            var requestContent = new ManageLocation_GetProperties.RequestContent() {arLocationId = location.Id};

            //Launch request
            var getPropertiesRequest =
                AppearitionRequest<ManageLocation_GetProperties>.LaunchAPICall_GET<LocationHandler>(location.ProductId, GetReusableApiRequest<ManageLocation_GetProperties>(), requestContent);

            while (!getPropertiesRequest.IsDone)
                yield return null;

            //Handle success and failure
            if (getPropertiesRequest.RequestResponseObject.IsSuccess)
            {
                bool foundAny = getPropertiesRequest.RequestResponseObject.Data != null && getPropertiesRequest.RequestResponseObject.Data.Count > 0;
                AppearitionLogger.LogInfo(string.Format(LocationConstants.GET_PROPERTY_SUCCESS, foundAny ? getPropertiesRequest.RequestResponseObject.Data.Count.ToString() : "No",
                    location.Id));

                if (onSuccess != null)
                    onSuccess(location, getPropertiesRequest.RequestResponseObject.Data);
            }
            else
            {
                AppearitionLogger.LogError(string.Format(LocationConstants.GET_PROPERTY_FAILURE, location.Id, getPropertiesRequest.Errors));

                if (onFailure != null)
                    onFailure(getPropertiesRequest.Errors);
            }


            if (onComplete != null)
                onComplete(getPropertiesRequest.RequestResponseObject.IsSuccess);
        }

        #endregion

        #region Add Property

        /// <summary>
        /// Adds a new property to a given Location.
        /// </summary>
        /// <param name="location"></param>
        /// <param name="propertyKey"></param>
        /// <param name="propertyValue"></param>
        /// <param name="onSuccess">Contains the Location as well as the list of all available properties. Only called if the request is successful.</param>
        /// <param name="onFailure">Contains any error obtained during the request. Only called if the request has failed.</param>
        /// <param name="onComplete">Always called at the end of the request, defines whether the request was successful or not.</param>
        public static void AddPropertyToLocation(Location location, string propertyKey, object propertyValue, Action<Location, List<Property>> onSuccess = null,
            Action<EmsError> onFailure = null,
            Action<bool> onComplete = null)
        {
            AppearitionGate.Instance.StartCoroutine(AddPropertyToLocationProcess(location, propertyKey, propertyValue, onSuccess, onFailure, onComplete));
        }

        /// <summary>
        /// Adds a new property to a given Location.
        /// </summary>
        /// <param name="location"></param>
        /// <param name="propertyKey"></param>
        /// <param name="propertyValue"></param>
        /// <param name="onSuccess">Contains the Location as well as the list of all available properties. Only called if the request is successful.</param>
        /// <param name="onFailure">Contains any error obtained during the request. Only called if the request has failed.</param>
        /// <param name="onComplete">Always called at the end of the request, defines whether the request was successful or not.</param>
        public static IEnumerator AddPropertyToLocationProcess(Location location, string propertyKey, object propertyValue, Action<Location, List<Property>> onSuccess = null,
            Action<EmsError> onFailure = null,
            Action<bool> onComplete = null)
        {
            //Ref Check
            if (location == null)
            {
                AppearitionLogger.LogError(LocationConstants.LOCATION_NULL);
                if (onFailure != null)
                    onFailure(new EmsError(LocationConstants.LOCATION_NULL));
                if (onComplete != null)
                    onComplete(false);
                yield break;
            }

            //Online request. Prepare request content
            var postContent = new ManageLocation_AddProperty.PostData(propertyKey, propertyValue.ToString());
            var requestContent = new ManageLocation_AddProperty.RequestContent() {arLocationId = location.Id};

            //Launch request
            var addPropertyRequest =
                AppearitionRequest<ManageLocation_AddProperty>.LaunchAPICall_POST<LocationHandler>(location.ProductId, GetReusableApiRequest<ManageLocation_AddProperty>(), postContent,
                    requestContent);

            while (!addPropertyRequest.IsDone)
                yield return null;

            //Handle success and failure
            if (addPropertyRequest.RequestResponseObject.IsSuccess)
            {
                AppearitionLogger.LogInfo(string.Format(LocationConstants.ADD_PROPERTY_SUCCESS, propertyKey, location.Id));
                yield return GetArTargetPropertiesProcess(location, onSuccess);
            }
            else
            {
                AppearitionLogger.LogError(string.Format(LocationConstants.ADD_PROPERTY_FAILURE, propertyKey, location.Id, addPropertyRequest.Errors));

                if (onFailure != null)
                    onFailure(addPropertyRequest.Errors);
            }


            if (onComplete != null)
                onComplete(addPropertyRequest.RequestResponseObject.IsSuccess);
        }

        #endregion

        #region Update Property

        /// <summary>
        /// Updates a single property of a given Location
        /// </summary>
        /// <param name="location"></param>
        /// <param name="propertyKey"></param>
        /// <param name="newPropertyValue"></param>
        /// <param name="onSuccess">Contains the Location as well as the list of all available properties. Only called if the request is successful.</param>
        /// <param name="onFailure">Contains any error obtained during the request. Only called if the request has failed.</param>
        /// <param name="onComplete">Always called at the end of the request, defines whether the request was successful or not.</param>
        public static void UpdateArTargetProperties(Location location, string propertyKey, string newPropertyValue, Action<Location, List<Property>> onSuccess = null,
            Action<EmsError> onFailure = null,
            Action<bool> onComplete = null)
        {
            AppearitionGate.Instance.StartCoroutine(UpdateArTargetPropertiesProcess(location, new Property(propertyKey, newPropertyValue), onSuccess, onFailure, onComplete));
        }

        /// <summary>
        /// Updates a single property of a given Location
        /// </summary>
        /// <param name="location"></param>
        /// <param name="propertyKey"></param>
        /// <param name="newPropertyValue"></param>
        /// <param name="onSuccess">Contains the Location as well as the list of all available properties. Only called if the request is successful.</param>
        /// <param name="onFailure">Contains any error obtained during the request. Only called if the request has failed.</param>
        /// <param name="onComplete">Always called at the end of the request, defines whether the request was successful or not.</param>
        public static IEnumerator UpdateArTargetPropertiesProcess(Location location, string propertyKey, string newPropertyValue, Action<Location, List<Property>> onSuccess = null,
            Action<EmsError> onFailure = null,
            Action<bool> onComplete = null)
        {
            yield return UpdateArTargetPropertiesProcess(location, new Property(propertyKey, newPropertyValue), onSuccess, onFailure, onComplete);
        }

        /// <summary>
        /// Updates a single property of a given Location
        /// </summary>
        /// <param name="location"></param>
        /// <param name="property"></param>
        /// <param name="onSuccess">Contains the Location as well as the list of all available properties. Only called if the request is successful.</param>
        /// <param name="onFailure">Contains any error obtained during the request. Only called if the request has failed.</param>
        /// <param name="onComplete">Always called at the end of the request, defines whether the request was successful or not.</param>
        public static void UpdateArTargetProperties(Location location, Property property, Action<Location, List<Property>> onSuccess = null, Action<EmsError> onFailure = null,
            Action<bool> onComplete = null)
        {
            AppearitionGate.Instance.StartCoroutine(UpdateArTargetPropertiesProcess(location, property, onSuccess, onFailure, onComplete));
        }

        /// <summary>
        /// Updates a single property of a given Location
        /// </summary>
        /// <param name="location"></param>
        /// <param name="property"></param>
        /// <param name="onSuccess">Contains the Location as well as the list of all available properties. Only called if the request is successful.</param>
        /// <param name="onFailure">Contains any error obtained during the request. Only called if the request has failed.</param>
        /// <param name="onComplete">Always called at the end of the request, defines whether the request was successful or not.</param>
        public static IEnumerator UpdateArTargetPropertiesProcess(Location location, Property property, Action<Location, List<Property>> onSuccess = null, Action<EmsError> onFailure = null,
            Action<bool> onComplete = null)
        {
            //Ref Check
            if (location == null)
            {
                AppearitionLogger.LogError(LocationConstants.LOCATION_NULL);
                if (onFailure != null)
                    onFailure(new EmsError(LocationConstants.LOCATION_NULL));
                if (onComplete != null)
                    onComplete(false);
                yield break;
            }

            //Online request. Prepare request content
            var postContent = new ManageLocation_UpdateProperty.PostData(property);
            var requestContent = new ManageLocation_UpdateProperty.RequestContent() {arLocationId = location.Id};

            //Launch request
            var updatePropertyRequest =
                AppearitionRequest<ManageLocation_UpdateProperty>.LaunchAPICall_POST<LocationHandler>(location.ProductId, GetReusableApiRequest<ManageLocation_UpdateProperty>(), postContent, requestContent);

            while (!updatePropertyRequest.IsDone)
                yield return null;

            //Handle success and failure
            if (updatePropertyRequest.RequestResponseObject.IsSuccess)
            {
                AppearitionLogger.LogInfo(string.Format(LocationConstants.UPDATE_PROPERTY_SUCCESS, property.propertyKey, location.Id));
                yield return GetArTargetPropertiesProcess(location, onSuccess);
            }
            else
            {
                AppearitionLogger.LogError(string.Format(LocationConstants.UPDATE_PROPERTY_FAILURE, property.propertyKey, location.Id, updatePropertyRequest.Errors));

                if (onFailure != null)
                    onFailure(updatePropertyRequest.Errors);
            }


            if (onComplete != null)
                onComplete(updatePropertyRequest.RequestResponseObject.IsSuccess);
        }

        #endregion

        #region Remove Property

        /// <summary>
        /// Deletes a single property of a given Location
        /// </summary>
        /// <param name="location"></param>
        /// <param name="propertyKey"></param>
        /// <param name="onSuccess">Contains the Location as well as the list of all available properties. Only called if the request is successful.</param>
        /// <param name="onFailure">Contains any error obtained during the request. Only called if the request has failed.</param>
        /// <param name="onComplete">Always called at the end of the request, defines whether the request was successful or not.</param>
        public static IEnumerator DeleteArTargetPropertiesProcess(Location location, string propertyKey, Action<Location, List<Property>> onSuccess = null, Action<EmsError> onFailure = null,
        Action<bool> onComplete = null)
        {
            //Ref Check
            if (location == null)
            {
                AppearitionLogger.LogError(LocationConstants.LOCATION_NULL);
                if (onFailure != null)
                    onFailure(new EmsError(LocationConstants.LOCATION_NULL));
                if (onComplete != null)
                    onComplete(false);
                yield break;
            }

            //Online request. Prepare request content
            var requestContent = new ManageLocation_DeleteProperty.RequestContent() { arLocationId = location.Id, propertyKey = propertyKey };

            //Launch request
            var deletePropertyRequest =
                AppearitionRequest<ManageLocation_DeleteProperty>.LaunchAPICall_POST<LocationHandler>(location.ProductId, GetReusableApiRequest<ManageLocation_DeleteProperty>(), null, requestContent);

            while (!deletePropertyRequest.IsDone)
                yield return null;

            //Handle success and failure
            if (deletePropertyRequest.RequestResponseObject.IsSuccess)
            {
                AppearitionLogger.LogInfo(string.Format(LocationConstants.DELETE_PROPERTY_SUCCESS, propertyKey, location.Id));
                yield return GetArTargetPropertiesProcess(location, onSuccess);
            }
            else
            {
                AppearitionLogger.LogError(string.Format(LocationConstants.DELETE_PROPERTY_FAILURE, propertyKey, location.Id, deletePropertyRequest.Errors));

                if (onFailure != null)
                    onFailure(deletePropertyRequest.Errors);
            }


            if (onComplete != null)
                onComplete(deletePropertyRequest.RequestResponseObject.IsSuccess);
        }

        #endregion

        #endregion

        #region File Utilities

        #region Location

        /// <summary>
        /// Loads the PointOfInterest's images onto the given PointOfInterest data based on the given parameters and availability.
        /// </summary>
        /// <param name="location">Given Location data which contains the given PointOfInterest.</param>
        /// <param name="poi">Given PointOfInterest.</param>
        /// <param name="loadLabelImageIfAny">Whether or not the label image should be loaded.</param>
        /// <param name="loadMarkerImageIfAny">Whether or not the marker image should be loaded.</param>
        /// <param name="loadInfoImageIfAny">Whether or not hte info image should be loaded.</param>
        /// <param name="onComplete">Occurs when the process is completed. Contains whether or not all the processes successfully completed.</param>
        public static void LoadPointOfInterestImages(Location location, PointOfInterest poi, bool loadLabelImageIfAny = true, bool loadMarkerImageIfAny = true, bool loadInfoImageIfAny = true,
            Action<bool> onComplete = null)
        {
            AppearitionGate.Instance.StartCoroutine(LoadPointOfInterestImagesProcess(location, poi, loadLabelImageIfAny, loadMarkerImageIfAny, loadInfoImageIfAny, onComplete));
        }

        /// <summary>
        /// Loads the PointOfInterest's images onto the given PointOfInterest data based on the given parameters and availability.
        /// </summary>
        /// <param name="location">Given Location data which contains the given PointOfInterest.</param>
        /// <param name="poi">Given PointOfInterest.</param>
        /// <param name="loadLabelImageIfAny">Whether or not the label image should be loaded.</param>
        /// <param name="loadMarkerImageIfAny">Whether or not the marker image should be loaded.</param>
        /// <param name="loadInfoImageIfAny">Whether or not hte info image should be loaded.</param>
        /// <param name="onComplete">Occurs when the process is completed. Contains whether or not all the processes successfully completed.</param>
        public static IEnumerator LoadPointOfInterestImagesProcess(Location location, PointOfInterest poi, bool loadLabelImageIfAny = true, bool loadMarkerImageIfAny = true,
            bool loadInfoImageIfAny = true,
            Action<bool> onComplete = null)
        {
            bool isSuccessful = true;

            if (loadLabelImageIfAny && !string.IsNullOrEmpty(poi.LabelImageUrl))
                yield return LoadLabelImageToPointOfInterestProcess(location, poi, labelSprite => isSuccessful = isSuccessful && labelSprite != null);

            if (loadMarkerImageIfAny && !string.IsNullOrEmpty(poi.MarkerImageUrl))
                yield return LoadMarkerImageToPointOfInterestProcess(location, poi, markerSprite => isSuccessful = isSuccessful && markerSprite != null);

            if (loadInfoImageIfAny && !string.IsNullOrEmpty(poi.InfoImageUrl))
                yield return LoadInfoImageToPointOfInterestProcess(location, poi, infoSprite => isSuccessful = isSuccessful && infoSprite != null);

            if (onComplete != null)
                onComplete(isSuccessful);
        }

        /// <summary>
        /// Clears the cached files of the given PointOfInterest.
        /// </summary>
        /// <param name="location">Given Location data which contains the given PointOfInterest.</param>
        /// <param name="poi">Given PointOfInterest.</param>
        /// <param name="onComplete">Occurs when the process is completed. Contains whether or not all the processes successfully completed.</param>
        public static void ClearLocalPointOfInterestData(Location location, PointOfInterest poi, Action<bool> onComplete)
        {
            AppearitionGate.Instance.StartCoroutine(ClearLocalPointOfInterestDataProcess(location, poi, onComplete));
        }

        /// <summary>
        /// Clears the cached files of the given PointOfInterest.
        /// </summary>
        /// <param name="location">Given Location data which contains the given PointOfInterest.</param>
        /// <param name="poi">Given PointOfInterest.</param>
        /// <param name="onComplete">Occurs when the process is completed. Contains whether or not all the processes successfully completed.</param>
        public static IEnumerator ClearLocalPointOfInterestDataProcess(Location location, PointOfInterest poi, Action<bool> onComplete)
        {
            bool isSuccessful = true;

            if (!string.IsNullOrEmpty(poi.LabelImageFileName))
            {
                string path = GetPathToLabelImage(location, poi);

                if (File.Exists(path))
                    yield return DeleteFileProcess(path, isSuccess => isSuccessful = isSuccessful && isSuccess);
            }

            if (!string.IsNullOrEmpty(poi.MarkerImageFileName))
            {
                string path = GetPathToMarkerImage(location, poi);

                if (File.Exists(path))
                    yield return DeleteFileProcess(path, isSuccess => isSuccessful = isSuccessful && isSuccess);
            }

            if (!string.IsNullOrEmpty(poi.InfoImageFileName))
            {
                string path = GetPathToInfoImage(location, poi);

                if (File.Exists(path))
                    yield return DeleteFileProcess(path, isSuccess => isSuccessful = isSuccessful && isSuccess);
            }

            if (onComplete != null)
                onComplete(isSuccessful);
        }

        #endregion

        #region Images

        /// <summary>
        /// Loads the label image of a given point of interest into the given point of interest data.
        /// The Sprite will both be given in the callback and on the PointOfInterest's LabelImageSprite field.
        /// If this process failed, the callback will contain a null object.
        /// </summary>
        /// <param name="location">Given Location data which contains the given PointOfInterest.</param>
        /// <param name="poi">Given PointOfInterest.</param>
        /// <param name="onComplete">Occurs when the process is completed. Contains the data of the loaded Sprite.</param>
        public static void LoadLabelImageToPointOfInterest(Location location, PointOfInterest poi, Action<Sprite> onComplete = null)
        {
            AppearitionGate.Instance.StartCoroutine(LoadLabelImageToPointOfInterestProcess(location, poi, onComplete));
        }

        /// <summary>
        /// Loads the label image of a given point of interest into the given point of interest data.
        /// The Sprite will both be given in the callback and on the PointOfInterest's LabelImageSprite field.
        /// If this process failed, the callback will contain a null object.
        /// </summary>
        /// <param name="location">Given Location data which contains the given PointOfInterest.</param>
        /// <param name="poi">Given PointOfInterest.</param>
        /// <param name="onComplete">Occurs when the process is completed. Contains the data of the loaded Sprite.</param>
        public static IEnumerator LoadLabelImageToPointOfInterestProcess(Location location, PointOfInterest poi, Action<Sprite> onComplete = null)
        {
            yield return DownloadGenericFile(poi.LabelImageUrl.Trim(), GetPathToLabelImage(location, poi), poi.LabelImageChecksum, true, bytes =>
            {
                if (bytes != null)
                {
                    poi.LabelImageSprite = ImageUtility.LoadOrCreateSprite(bytes);

                    if (onComplete != null)
                        onComplete(poi.LabelImageSprite);
                }
            });
        }

        /// <summary>
        /// Loads the marker image of a given point of interest into the given point of interest data.
        /// The Sprite will both be given in the callback and on the PointOfInterest's MarkerImageSprite field.
        /// If this process failed, the callback will contain a null object.
        /// </summary>
        /// <param name="location">Given Location data which contains the given PointOfInterest.</param>
        /// <param name="poi">Given PointOfInterest.</param>
        /// <param name="onComplete">Occurs when the process is completed. Contains the data of the loaded Sprite.</param>
        public static void LoadMarkerImageToPointOfInterest(Location location, PointOfInterest poi, Action<Sprite> onComplete = null)
        {
            AppearitionGate.Instance.StartCoroutine(LoadMarkerImageToPointOfInterestProcess(location, poi, onComplete));
        }

        /// <summary>
        /// Loads the marker image of a given point of interest into the given point of interest data.
        /// The Sprite will both be given in the callback and on the PointOfInterest's MarkerImageSprite field.
        /// If this process failed, the callback will contain a null object.
        /// </summary>
        /// <param name="location">Given Location data which contains the given PointOfInterest.</param>
        /// <param name="poi">Given PointOfInterest.</param>
        /// <param name="onComplete">Occurs when the process is completed. Contains the data of the loaded Sprite.</param>
        public static IEnumerator LoadMarkerImageToPointOfInterestProcess(Location location, PointOfInterest poi, Action<Sprite> onComplete = null)
        {
            yield return DownloadGenericFile(poi.MarkerImageUrl.Trim(), GetPathToMarkerImage(location, poi), poi.MarkerImageChecksum, true, bytes =>
            {
                if (bytes != null)
                {
                    poi.MarkerImageSprite = ImageUtility.LoadOrCreateSprite(bytes);

                    if (onComplete != null)
                        onComplete(poi.MarkerImageSprite);
                }
            });
        }

        /// <summary>
        /// Loads the info image of a given point of interest into the given point of interest data.
        /// The Sprite will both be given in the callback and on the PointOfInterest's InfoImageSprite field.
        /// If this process failed, the callback will contain a null object.
        /// </summary>
        /// <param name="location">Given Location data which contains the given PointOfInterest.</param>
        /// <param name="poi">Given PointOfInterest.</param>
        /// <param name="onComplete">Occurs when the process is completed. Contains the data of the loaded Sprite.</param>
        public static void LoadInfoImageToPointOfInterest(Location location, PointOfInterest poi, Action<Sprite> onComplete = null)
        {
            AppearitionGate.Instance.StartCoroutine(LoadInfoImageToPointOfInterestProcess(location, poi, onComplete));
        }

        /// <summary>
        /// Loads the info image of a given point of interest into the given point of interest data.
        /// The Sprite will both be given in the callback and on the PointOfInterest's InfoImageSprite field.
        /// If this process failed, the callback will contain a null object.
        /// </summary>
        /// <param name="location">Given Location data which contains the given PointOfInterest.</param>
        /// <param name="poi">Given PointOfInterest.</param>
        /// <param name="onComplete">Occurs when the process is completed. Contains the data of the loaded Sprite.</param>
        public static IEnumerator LoadInfoImageToPointOfInterestProcess(Location location, PointOfInterest poi, Action<Sprite> onComplete = null)
        {
            yield return DownloadGenericFile(poi.InfoImageUrl.Trim(), GetPathToInfoImage(location, poi), poi.InfoImageChecksum, true, bytes =>
            {
                if (bytes != null)
                {
                    poi.InfoImageSprite = ImageUtility.LoadOrCreateSprite(bytes);

                    if (onComplete != null)
                        onComplete(poi.InfoImageSprite);
                }
            });
        }

        #endregion

        #endregion
    }
}