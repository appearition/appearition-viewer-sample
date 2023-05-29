// -----------------------------------------------------------------------
// Company:"Appearition Pty Ltd"
// File: AppearitionBaseRequest.cs
// Copyright (c) 2019. All rights reserved.
// -----------------------------------------------------------------------

#pragma warning disable 693
#pragma warning disable 1717
using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;
using Appearition.API;
using Appearition.Common;
using Appearition.Common.ObjectExtensions;

namespace Appearition.Internal
{
    public abstract class AppearitionBaseRequest<T> where T : BaseApi, new()
    {
        #region Constants

        public enum RequestState
        {
            None,
            InProgress,
            SuccessOnline,
            SuccessOffline,
            Failure
        }

        #endregion

        #region Events

        public delegate void RequestCompleted(T response);

        public event RequestCompleted OnRequestCompleted;

        public delegate void RequestCompletedJson(string jsonResponse);

        public event RequestCompletedJson OnRequestCompletedJson;

        #endregion

        #region Request Properties

        protected int ProductId { get; set; }
        protected object PostBody { get; set; }
        protected BaseRequestContent RequestContent { get; set; }

        #endregion

        #region Process Properties

        /// <summary>
        /// Timeout for the request, in milliseconds. Default value is 180,000 (3minutes)
        /// </summary>
        public int Timeout = 30000;

        public RequestState CurrentState { get; protected set; }

        /// <summary>
        /// Returns whether the current request has completed or not.
        /// </summary>
        /// <value><c>true</c> if is done; otherwise, <c>false</c>.</value>
        public virtual bool IsDone => CurrentState == RequestState.SuccessOnline || CurrentState == RequestState.SuccessOffline || CurrentState == RequestState.Failure;

        public virtual float Progress { get; protected set; }

        public virtual long SizeOfFileBeingUploaded { get; protected set; }

        private EmsError _errors;

        /// <summary>
        /// Contains all the _errors and messages from the EMS obtained during this request.
        /// Do note that it content will not be available until the _isDone flag is set to true.
        /// </summary>
        /// <value>The _errors.</value>
        public EmsError Errors
        {
            get { return _errors; }
        }

        #endregion

        #region Response Properties

        string _jsonDownloaded = "";

        /// <summary>
        /// Contains the JSON response from this request once complete.
        /// </summary>
        /// <value>The json downloaded.</value>
        public virtual string JsonDownloaded
        {
            get { return _jsonDownloaded; }
        }

        private T _requestResponseObject;

        /// <summary>
        /// Contains the response of the download extracted. This variable might be null until the download has complete.
        /// </summary>
        /// <value>The request response object.</value>
        public virtual T RequestResponseObject
        {
            get { return _requestResponseObject; }
        }

        private byte[] _downloadedBytes;

        /// <summary>
        /// Bytes downloaded by the current request. If the API Call doesn't feature download of any attachments, this value will be null.
        /// </summary>
        /// <value>The downloaded bytes.</value>
        public virtual byte[] DownloadedBytes
        {
            get { return _downloadedBytes; }
        }

        string _responseCode = "";

        /// <summary>
        /// Response code for this current request.
        /// </summary>
        /// <value>The response code.</value>
        public virtual string ResponseCode
        {
            get { return _responseCode; }
        }

        protected virtual string JsonSavePath { get; set; }
        protected virtual string DownloadedBytesSavePath { get; set; }

        #endregion

        #region Requests

        #region GET Overloads

        /// <summary>
        /// Launches an API of the type GET and returns the download process ApiData class.
        /// </summary>
        /// <returns>The API call GE.</returns>
        /// <param name="productId">Product identifier.</param>
        /// <param name="requestContent"></param>
        /// <param name="callback">Callback.</param>
        /// <param name="reusableObject"></param>
        /// <typeparam name="T">The 1st type parameter.</typeparam>
        public static AppearitionRequest<T> LaunchAPICall_GET<K>(int productId, T reusableObject, BaseRequestContent requestContent = null, Action<T> callback = null) where K : BaseHandler
        {
            //Setup request
            var request = new AppearitionRequest<T>();
            AppearitionGate.Instance.StartCoroutine(request.HandleAPICall_GET<K>(productId, reusableObject, requestContent, callback));
            return request;
        }

        #endregion

        #region POST Overloads

        /// <summary>
        /// Launches an API of the type POST and returns the download process ApiData class.
        /// </summary>
        /// <returns>The API call position.</returns>
        /// <param name="productId">Product identifier.</param>
        /// <param name="reusableObject"></param>
        /// <param name="postRequestBody">Post request body.</param>
        /// <param name="requestContent">Request content.</param>
        /// <param name="callback">Callback.</param>
        public static AppearitionRequest<T> LaunchAPICall_POST<K>(int productId, T reusableObject, object postRequestBody, BaseRequestContent requestContent = null,
            Action<T> callback = null) where K : BaseHandler
        {
            //Setup request
            var request = new AppearitionRequest<T>();
            AppearitionGate.Instance.StartCoroutine(request.HandleAPICall_POST<K>(productId, reusableObject, postRequestBody, requestContent, callback));
            return request;
        }

        #endregion

        #region Post Multipart Overloads

        /// <summary>
        /// Launches an API of the type POST with MultiPart Form-ApiData, mainly for uploading ApiData, and returns the process ApiData class.
        /// This class doesn't include a _progress for the time being.
        /// </summary>
        /// <returns>The API call multi part position.</returns>
        /// <param name="productId">Product identifier.</param>
        /// <param name="reusableObject"></param>
        /// <param name="requestContent">Request content.</param>
        /// <param name="postRequestData">Post request ApiData.</param>
        /// <param name="mediaType"></param>
        /// <param name="callback">Callback.</param>
        public static AppearitionRequest<T> LaunchAPICall_MultiPartPOST<K>(int productId, T reusableObject, BaseRequestContent requestContent = null,
            List<MultiPartFormParam> postRequestData = null, string mediaType = "", Action<T> callback = null) where K : BaseHandler
        {
            //Setup request
            var request = new AppearitionRequest<T>();
            AppearitionGate.Instance.StartCoroutine(request.HandleAPICall_POSTMultiPartFormData<K>(productId, reusableObject, requestContent, postRequestData, mediaType, callback));
            return request;
        }

        #endregion

        #endregion

        #region Request Processes

        #region GET

        /// <summary>
        /// Process of a standard API GET call compatible with the Appearition EMS.
        /// </summary>
        /// <returns>The API call GE.</returns>
        /// <param name="productId">Product identifier.</param>
        /// <param name="reusableObject"></param>
        /// <param name="requestContent">Request content.</param>
        /// <param name="callback">Callback.</param>
        public IEnumerator HandleAPICall_GET<K>(int productId, T reusableObject, BaseRequestContent requestContent, Action<T> callback) where K : BaseHandler
        {
            //Init
            if (reusableObject == null)
                reusableObject = new T();
            else
                reusableObject.ResetToDefault<T>();

            _requestResponseObject = reusableObject;

            //Catch bad credentials
            if (!AreCredentialCorrect(this, reusableObject))
            {
                CurrentState = RequestState.Failure;
                callback?.Invoke(reusableObject);
                yield break;
            }

            //Store data
            ProductId = productId;
            PostBody = null;
            RequestContent = requestContent;

            //Prepare request
            JsonSavePath = BaseHandler.GetApiJsonFullPath<T, K>(reusableObject, AppearitionGate.Instance.CurrentUser,
                productId, reusableObject.GetJsonFileExtraParams(PostBody, RequestContent));
            DownloadedBytesSavePath = BaseHandler.GetApiDownloadedBytesFullPath<T, K>(reusableObject, AppearitionGate.Instance.CurrentUser,
                productId, reusableObject.GetJsonFileExtraParams(PostBody, RequestContent));
            CurrentState = RequestState.InProgress;
            reusableObject.IsBeingUsed = true;

            //Setup request
            var request = UnityWebRequest.Get(BaseApi.GetApiCall<T>
            (AppearitionGate.Instance.CurrentUser.SelectedEndPoint.endPointURL,
                AppearitionGate.Instance.CurrentUser.selectedTenant, productId, requestContent));

            request.SetRequestHeader("Content-Type", "application/json");
            //if (Application.platform != RuntimePlatform.WebGLPlayer)
            //    request.SetRequestHeader("charset", "utf-8");

            request.timeout = Timeout;
            request.certificateHandler = new AppearitionCertificateHandler();


            reusableObject.IsBeingUsed = true;

            request.SetRequestHeader("API-Version", reusableObject.ApiVersion.ToString());

            //Authentication headers
            if (!reusableObject.OverrideAuthenticationHeader)
            {
                foreach (var authenticationHeader in AppearitionGate.Instance.CurrentUser.GetAuthenticationHeaders(reusableObject.AuthenticationOverride, reusableObject))
                {
                    request.SetRequestHeader(authenticationHeader.Key, authenticationHeader.Value);
                }
            }

            //Add extra headers
            foreach (var extraHeader in reusableObject.GetRequestExtraHeaders())
                request.SetRequestHeader(extraHeader.Key, extraHeader.Value);

            //Handle offline and no-internet.
            if (!reusableObject.BypassInternetCheck)
            {
                while (!AppearitionGate.HasInternetAccessToEms.HasValue)
                    yield return null;

                if (!AppearitionGate.HasInternetAccessToEms.GetValueOrDefault())
                {
                    if (TryToLoadOfflineData(out string offlineJson))
                    {
                        CompleteRequest(offlineJson, reusableObject, callback, false, request.url, ApiConstants.API_CALL_SUCCESS);
                    }
                    else
                    {
                        AppearitionLogger.LogError(AppearitionConstants.EMS_UNREACHABLE_NO_LOCAL_DATA_ERROR_MESSAGE);
                        _errors = new EmsError(AppearitionConstants.EMS_UNREACHABLE_NO_LOCAL_DATA_ERROR_MESSAGE, 0);
                        CurrentState = RequestState.Failure;
                        callback?.Invoke(reusableObject);
                    }

                    request.Dispose();
                    yield break;
                }
            }

            //Send the request
            var sentRequest = request.SendWebRequest();

            while (!sentRequest.isDone)
            {
                Progress = sentRequest.progress;
                yield return null;
            }

            //Store the bytes, if any
            _downloadedBytes = request.downloadHandler.data;

            //Handle response
            CompleteRequest(request.downloadHandler.text, reusableObject, callback, true, request.url, request.responseCode.ToString(), request.error);
            request.Dispose();
        }

        #endregion

        #region POST

        /// <summary>
        /// Process of a standard API POST call compatible with the Appearition EMS. This process is not compatible with multipart form ApiData POST requests.
        /// </summary>
        /// <returns>The API call position.</returns>
        /// <param name="productId">Product identifier.</param>
        /// <param name="reusableObject"></param>
        /// <param name="postRequestBody">Post request body.</param>
        /// <param name="requestContent">Request content.</param>
        /// <param name="callback">Callback.</param>
        public IEnumerator HandleAPICall_POST<K>(int productId, T reusableObject, object postRequestBody, BaseRequestContent requestContent,
            Action<T> callback = null) where K : BaseHandler
        {
            //Init
            if (reusableObject == null)
                reusableObject = new T();
            else
                reusableObject.ResetToDefault<T>();

            _requestResponseObject = reusableObject;

            //Catch bad credentials
            if (!AreCredentialCorrect(this, reusableObject))
            {
                CurrentState = RequestState.Failure;
                callback?.Invoke(reusableObject);
                yield break;
            }

            //Store data
            ProductId = productId;
            PostBody = postRequestBody;
            RequestContent = requestContent;

            //Prepare request
            JsonSavePath = BaseHandler.GetApiJsonFullPath<T, K>(reusableObject, AppearitionGate.Instance.CurrentUser, 
                productId, reusableObject.GetJsonFileExtraParams(PostBody, RequestContent));
            DownloadedBytesSavePath = BaseHandler.GetApiDownloadedBytesFullPath<T, K>(reusableObject, AppearitionGate.Instance.CurrentUser, 
                productId, reusableObject.GetJsonFileExtraParams(PostBody, RequestContent));
            CurrentState = RequestState.InProgress;
            reusableObject.IsBeingUsed = true;

            //Get body
            string body = AppearitionConstants.SerializeJson(postRequestBody);

            //JsonReplace
            foreach (var kvp in reusableObject.JsonReplaceKvp)
                body = body.Replace(kvp.Key, kvp.Value);

            var request = new UnityWebRequest(BaseApi.GetApiCall<T>
            (AppearitionGate.Instance.CurrentUser.SelectedEndPoint.endPointURL,
                AppearitionGate.Instance.CurrentUser.selectedTenant, productId, requestContent));

            request.downloadHandler = new DownloadHandlerBuffer();
            request.method = UnityWebRequest.kHttpVerbPOST;

            if (string.IsNullOrEmpty(body))
                body = "null";

            //Create the upload body of the request based on the JSON
            request.uploadHandler = new UploadHandlerRaw(System.Text.Encoding.ASCII.GetBytes(body)) {
                contentType = "application/json"
            };

            request.SetRequestHeader("Accept", "application/json");

            request.timeout = Timeout;
            request.certificateHandler = new AppearitionCertificateHandler();

            //Authenfication headers
            if (!reusableObject.OverrideAuthenticationHeader)
            {
                foreach (var authenticationHeader in AppearitionGate.Instance.CurrentUser.GetAuthenticationHeaders(reusableObject.AuthenticationOverride, reusableObject))
                {
                    request.SetRequestHeader(authenticationHeader.Key, authenticationHeader.Value);
                }
            }

            //request.chunkedTransfer = false;

            foreach (var extraHeader in reusableObject.GetRequestExtraHeaders())
            {
                request.SetRequestHeader(extraHeader.Key, extraHeader.Value);
            }

            //Additional headers
            request.SetRequestHeader("API-Version", reusableObject.ApiVersion.ToString());

            //Handle offline and no-internet.
            if (!reusableObject.BypassInternetCheck)
            {
                while (!AppearitionGate.HasInternetAccessToEms.HasValue)
                    yield return null;

                if (!AppearitionGate.HasInternetAccessToEms.GetValueOrDefault())
                {
                    if (TryToLoadOfflineData(out string offlineJson))
                    {
                        CompleteRequest(offlineJson, reusableObject, callback, false, request.url, ApiConstants.API_CALL_SUCCESS);
                    }
                    else
                    {
                        AppearitionLogger.LogError(AppearitionConstants.EMS_UNREACHABLE_NO_LOCAL_DATA_ERROR_MESSAGE);
                        _errors = new EmsError(AppearitionConstants.EMS_UNREACHABLE_NO_LOCAL_DATA_ERROR_MESSAGE, 0);
                        CurrentState = RequestState.Failure;
                        callback?.Invoke(reusableObject);
                    }

                    request.Dispose();
                    yield break;
                }
            }

            //Send the request
            var sentRequest = request.SendWebRequest();

            while (!sentRequest.isDone)
            {
                Progress = sentRequest.progress;
                yield return null;
            }

            //Store the bytes, if any
            _downloadedBytes = request.downloadHandler.data;

            //Handle response
            CompleteRequest(request.downloadHandler.text, reusableObject, callback, true, request.url, request.responseCode.ToString(), request.error);
            request.Dispose();
        }

        #endregion

        #region POST Multi-Pat Form

        public IEnumerator HandleAPICall_POSTMultiPartFormData<K>(int productId, T reusableObject, BaseRequestContent requestContent, IEnumerable<MultiPartFormParam> postRequestData,
            string mediaType = "", Action<T> callback = null) where K : BaseHandler
        {
            //Init
            if (reusableObject == null)
                reusableObject = new T();
            else
                reusableObject.ResetToDefault<T>();

            _requestResponseObject = reusableObject;

            //Catch bad credentials
            if (!AreCredentialCorrect(this, reusableObject))
            {
                CurrentState = RequestState.Failure;
                callback?.Invoke(reusableObject);
                yield break;
            }

            //Internet Check. No Offline for Multi Part Form.
            if (!reusableObject.BypassInternetCheck && !AppearitionGate.HasInternetAccessToEms.GetValueOrDefault())
            {
                AppearitionLogger.LogError(AppearitionConstants.EMS_UNREACHABLE_ERROR_MESSAGE);
                _errors = new EmsError(AppearitionConstants.EMS_UNREACHABLE_ERROR_MESSAGE, 0);
                CurrentState = RequestState.Failure;
                callback?.Invoke(reusableObject);
                yield break;
            }

            //Store data
            ProductId = productId;
            PostBody = null;
            RequestContent = requestContent;

            //Prepare request
            CurrentState = RequestState.InProgress;
            reusableObject.IsBeingUsed = true;

            //Prepare the data
            byte[] data;

            //Convert the MultiPartForms to Unity's
            List<IMultipartFormSection> multiPartForms = new List<IMultipartFormSection>();

            foreach (var multi in postRequestData)
            {
                var paramStream = multi.value as Stream;

                if (paramStream != null)
                {
                    //Write the stream to byte array
                    data = paramStream.ToByteArray();
                }
                else if (multi.value is byte[])
                {
                    data = (byte[]) multi.value;
                }
                else if (multi.value is Sprite)
                {
                    data = ((Sprite) multi.value).texture.EncodeToJPG();
                }
                else
                {
                    data = ((Texture2D) multi.value).EncodeToJPG();
                }

                multiPartForms.Add(new MultipartFormFileSection(multi.name, data, multi.fileName, multi.mimeType));
            }


            //Finalize the request body
            byte[] boundary = UnityWebRequest.GenerateBoundary();
            byte[] formSections = UnityWebRequest.SerializeFormSections(multiPartForms, boundary);
            byte[] terminate = Encoding.UTF8.GetBytes(String.Concat("\r\n--", Encoding.UTF8.GetString(boundary), "--"));
            byte[] body = new byte[formSections.Length + terminate.Length];
            Buffer.BlockCopy(formSections, 0, body, 0, formSections.Length);
            Buffer.BlockCopy(terminate, 0, body, formSections.Length, terminate.Length);


            //Create the request from scratch
            var request = new UnityWebRequest(BaseApi.GetApiCall<T>
            (AppearitionGate.Instance.CurrentUser.SelectedEndPoint.endPointURL,
                AppearitionGate.Instance.CurrentUser.selectedTenant, productId, requestContent));

            request.uploadHandler = new UploadHandlerRaw(body) {contentType = String.Concat("multipart/form-data; boundary=", Encoding.UTF8.GetString(boundary))};

            request.downloadHandler = new DownloadHandlerBuffer();
            request.method = UnityWebRequest.kHttpVerbPOST;
            request.SetRequestHeader("Accept", "application/json");
            //request.SetRequestHeader("Content-Type", "multipart/form-data");
            request.timeout = Timeout;
            //request.chunkedTransfer = false;
            request.certificateHandler = new AppearitionCertificateHandler();

            //Set file size
            SizeOfFileBeingUploaded = request.uploadHandler.data.LongLength;

            //Authentication headers
            foreach (var authenticationHeader in AppearitionGate.Instance.CurrentUser.GetAuthenticationHeaders(reusableObject.AuthenticationOverride, reusableObject))
            {
                request.SetRequestHeader(authenticationHeader.Key, authenticationHeader.Value);
            }


            foreach (var extraHeader in reusableObject.GetRequestExtraHeaders())
                request.SetRequestHeader(extraHeader.Key, extraHeader.Value);

            //Additional headers
            request.SetRequestHeader("API-Version", reusableObject.ApiVersion.ToString());

            if (!string.IsNullOrEmpty(mediaType))
            {
                request.SetRequestHeader("MediaType", mediaType);
                request.SetRequestHeader("IsPrivate", "false");
            }

            //Create the upload body of the request based on the JSON
            //request.uploadHandler = new UploadHandlerRaw(postBytes)
            //{ contentType = string.Format("multipart/form-ApiData; boundary={0}", Boundary) };

            //Send the request
            var sentRequest = request.SendWebRequest();

            while (!sentRequest.isDone)
            {
                Progress = sentRequest.progress;
                yield return null;
            }

            //Store the bytes, if any
            _downloadedBytes = request.downloadHandler.data;

            //Handle response
            CompleteRequest(request.downloadHandler.text, reusableObject, callback, true, request.url, request.responseCode.ToString(), request.error);
            request.Dispose();
        }

        #endregion

        /// <summary>
        /// Concludes the current request, including storing the standard ApiData, deserializing the json, and calling the events/callback.		
        /// </summary>
        /// <returns>The request.</returns>
        /// <param name="responseJson">Response json.</param>
        /// <param name="reusableObject">Reusable object.</param>
        /// <param name="callback">Callback.</param>
        /// <param name="isLiveData">Whether the data is freshly downloaded or offline loaded</param>
        /// <param name="url">URL.</param>
        /// <param name="requestResponseCode">Request response code.</param>
        /// <param name="error">Error.</param>
        public virtual T CompleteRequest(string responseJson, T reusableObject,
            Action<T> callback, bool isLiveData, string url = "", string requestResponseCode = "", string error = "")
        {
            //Flag the object up prior to losing its memory pointer.
            reusableObject.IsBeingUsed = false;
            _errors.ResetToDefault();

            //Check for login outcome
            //if ((requestResponseCode.Length < 1 || requestResponseCode == ApiConstants.API_CALL_SUCCESS) && responseJson.Length > 1)
            if (requestResponseCode.Length < 1 || requestResponseCode == ApiConstants.API_CALL_SUCCESS)
            {
                //Success downloading !
                AppearitionLogger.Log("Request " + url + " has successfully been  downloaded with code " + requestResponseCode + ".\n Result: " + responseJson,
                    AppearitionLogger.LogLevel.Debug);

                try
                {
                    reusableObject = AppearitionConstants.DeserializeJson<T>(responseJson);
                    reusableObject.IsSuccess = reusableObject.IsSuccess;
                } catch (Exception e)
                {
                    //Handle weird requests like ping.
                    if (reusableObject == null)
                        reusableObject = new T();

                    if (requestResponseCode == ApiConstants.API_CALL_SUCCESS)
                        reusableObject.IsSuccess = true;
                    else
                        Debug.LogError("The ApiData was downloaded successfully but the EMS input was incorrect. " + e);
                    //Debug.Log("Oh naw " + e);
                }

                //Fill in the error files
                if (reusableObject.Errors != null && reusableObject.Errors.Length > 0)
                    _errors.Errors.AddRange(reusableObject.Errors);

                if (reusableObject.IsSuccess && requestResponseCode.Length < 1)
                    requestResponseCode = ApiConstants.API_CALL_SUCCESS;

                CurrentState = isLiveData ? RequestState.SuccessOnline : RequestState.SuccessOffline;

                //If online success, delete the local files and update them
                if (CurrentState == RequestState.SuccessOnline)
                    UpdateOfflineData(reusableObject);
            }
            else
            {
                //Handle Failure
                _errors.Errors.Add(error);
                AppearitionLogger.Log("Error while requesting " + url + ".\n Code: " + requestResponseCode + ".\n Error: " + _errors, AppearitionLogger.LogLevel.Error);
                CurrentState = RequestState.Failure;
            }

            //Force in max _progress
            Progress = 1;

            _jsonDownloaded = responseJson;
            _requestResponseObject = reusableObject;
            _responseCode = requestResponseCode;
            _errors.errorCode = 0;

            if (int.TryParse(requestResponseCode, out int responseCodeInt))
                _errors.errorCode = responseCodeInt;

            //Finally, callback
            if (OnRequestCompletedJson != null)
                OnRequestCompletedJson(responseJson);

            if (OnRequestCompleted != null)
                OnRequestCompleted(reusableObject);

            if (callback != null)
                callback(reusableObject);

            return reusableObject;
        }

        #endregion

        #region Offline Saving/Loading

        private bool TryToLoadOfflineData(out string jsonContent)
        {
            jsonContent = "";

            if (!AppearitionConstants.enableApiResponseStorage)
                return false;

            switch (RequestResponseObject.ResponseSaveType)
            {
                case TypeOfApiStorage.None:
                    return false;

                case TypeOfApiStorage.Response:
                    if (File.Exists(JsonSavePath))
                        jsonContent = File.ReadAllText(JsonSavePath);

                    if (!string.IsNullOrEmpty(jsonContent) && AppearitionConstants.shouldEncryptJson)
                        jsonContent = AppearitionConstants.DecryptData(jsonContent);

                    if (File.Exists(DownloadedBytesSavePath))
                        _downloadedBytes = File.ReadAllBytes(DownloadedBytesSavePath);

                    return !string.IsNullOrEmpty(jsonContent);

                case TypeOfApiStorage.Custom:
                    jsonContent = RequestResponseObject.GetCustomOfflineJsonContent(ProductId, PostBody, RequestContent);

                    Debug.Log(jsonContent);
                    if (File.Exists(DownloadedBytesSavePath))
                        _downloadedBytes = File.ReadAllBytes(DownloadedBytesSavePath);

                    return !string.IsNullOrEmpty(jsonContent);
            }

            return false;
        }

        private void UpdateOfflineData(T responseObject)
        {
            if (!AppearitionConstants.enableApiResponseStorage)
                return;

            if (!string.IsNullOrEmpty(JsonSavePath) && File.Exists(JsonSavePath))
                File.Delete(JsonSavePath);
            if (!string.IsNullOrEmpty(DownloadedBytesSavePath) && File.Exists(DownloadedBytesSavePath))
                File.Delete(DownloadedBytesSavePath);

            switch (RequestResponseObject.ResponseSaveType)
            {
                case TypeOfApiStorage.None:
                    break;

                case TypeOfApiStorage.Response:
                    if (!string.IsNullOrEmpty(JsonSavePath) && !string.IsNullOrEmpty(_jsonDownloaded))
                        File.WriteAllText(JsonSavePath, AppearitionConstants.shouldEncryptJson ? AppearitionConstants.EncryptData(_jsonDownloaded) : _jsonDownloaded);

                    if (!string.IsNullOrEmpty(DownloadedBytesSavePath) && _downloadedBytes != null && _downloadedBytes.Length > 0)
                        File.WriteAllBytes(DownloadedBytesSavePath, _downloadedBytes);
                    break;

                case TypeOfApiStorage.Custom:
                    RequestResponseObject.UpdateCustomOfflineJsonContent(responseObject, ProductId, PostBody, RequestContent);

                    if (!string.IsNullOrEmpty(DownloadedBytesSavePath) && _downloadedBytes != null && _downloadedBytes.Length > 0)
                        File.WriteAllBytes(DownloadedBytesSavePath, _downloadedBytes);

                    break;
            }
        }

        #endregion

        #region Security Utilities

        /// <summary>
        /// Checks whether or not the authentication information is correct prior to sending the request.
        /// Upon failure, the request is expected to callback and end.
        /// </summary>
        /// <param name="request"></param>
        /// <param name="reusableObject"></param>
        /// <returns></returns>
        private static bool AreCredentialCorrect(AppearitionBaseRequest<T> request, T reusableObject)
        {
            //If an empty profile was given, don't go any further.
            if (AppearitionGate.Instance.CurrentUser == null ||
                //string.IsNullOrEmpty(AppearitionGate.Instance.CurrentUser.selectedTenant) ||
                AppearitionGate.Instance.CurrentUser.SelectedEndPoint == null ||
                string.IsNullOrEmpty(AppearitionGate.Instance.CurrentUser.GetHighestLevelAuthenticationToken))
            {
                AppearitionLogger.LogError(ApiConstants.REQUEST_NO_USER_GIVEN);
                request._errors = new EmsError(ApiConstants.REQUEST_NO_USER_GIVEN, 0);
                return false;
            }

            ////If the authentication level is too low for the request, fail it instantly.
            //if (!AppearitionGate.Instance.CurrentUser.IsUserLoggedIn &&
            //    reusableObject.AuthenticationOverride == AuthenticationOverrideType.SessionToken)
            //{
            //    AppearitionLogger.LogError(ApiConstants.REQUEST_AUTHENTICATION_LEVEL_TOO_LOW);
            //    _errors = new EmsError(ApiConstants.REQUEST_AUTHENTICATION_LEVEL_TOO_LOW);
            //    return false;
            //}

            return true;
        }


        //protected bool MyRemoteCertificateValidationCallback(System.Object sender, System.Security.Cryptography.X509Certificates.X509Certificate certificate,
        //    System.Security.Cryptography.X509Certificates.X509Chain chain, System.Net.Security.SslPolicyErrors sslPolicyErrors)
        //{
        //    bool isOk = true;
        //    // If there are _errors in the certificate chain, look at each error to determine the cause.
        //    if (sslPolicyErrors != System.Net.Security.SslPolicyErrors.None)
        //    {
        //        for (int i = 0; i < chain.ChainStatus.Length; i++)
        //        {
        //            if (chain.ChainStatus[i].Status != System.Security.Cryptography.X509Certificates.X509ChainStatusFlags.RevocationStatusUnknown)
        //            {
        //                chain.ChainPolicy.RevocationFlag = System.Security.Cryptography.X509Certificates.X509RevocationFlag.EntireChain;
        //                chain.ChainPolicy.RevocationMode = System.Security.Cryptography.X509Certificates.X509RevocationMode.Online;
        //                chain.ChainPolicy.UrlRetrievalTimeout = new TimeSpan(0, 1, 0);
        //                chain.ChainPolicy.VerificationFlags = System.Security.Cryptography.X509Certificates.X509VerificationFlags.AllFlags;
        //                bool chainIsValid = chain.Build((System.Security.Cryptography.X509Certificates.X509Certificate2) certificate);
        //                if (!chainIsValid)
        //                {
        //                    isOk = false;
        //                }
        //            }
        //        }
        //    }

        //    return isOk;
        //}

        #endregion
    }

    public class AppearitionCertificateHandler : CertificateHandler
    {
        //// Encoded RSAPublicKey
        //private static string PUB_KEY = "mypublickey";

        protected override bool ValidateCertificate(byte[] certificateData)
        {
            //X509Certificate2 certificate = new X509Certificate2(certificateData);
            // string pk = certificate.GetPublicKeyString();
            //if (pk.ToLower().Equals(PUB_KEY.ToLower()))
            //    return true;
            // Debug.LogError(pk);
            //return false;

            //.. Unsecured connection for now.
            return true;
        }
    }
}