using System;
using System.Diagnostics;
using System.Net;
using System.Text;
using Cysharp.Threading.Tasks;
using JetBrains.Annotations;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.Networking;
using Debug = UnityEngine.Debug;

namespace TSS.Rest
{
    [PublicAPI]
    public static class RestAPI
    {
        private static RestApiConfig _config;
        private static bool _loggedIn;
        private static UniTaskCompletionSource _loginCompletionSource;
        
        internal static void Initialize(RestApiConfig config)
        {
            _config = config;
            WaitLogin().Forget();
        }

        public static string GetUserIdentity()
        {
            if (SystemInfo.unsupportedIdentifier != SystemInfo.deviceUniqueIdentifier)
                return SystemInfo.deviceUniqueIdentifier;

            var id = PlayerPrefs.GetString("user_identity", string.Empty);
            if (string.IsNullOrEmpty(id))
            {
                id = Guid.NewGuid().ToString();
                PlayerPrefs.SetString("user_identity", id);
            }

            return id;
        }
        
        public static async UniTask<int> GetUsersCount()
        {
            if (!await WaitLogin())
                throw new WebException("Login failed");
            var result = await Get<int>(_config.UsersCountApi);
            return result;
        }

        public static async UniTask<T> Get<T>(string api)
        {
            if (!await WaitLogin())
                throw new WebException("Login failed");
            using (UnityWebRequest request = UnityWebRequest.Get(_config.ApiAddress + api))
            {
                await request.SendWebRequest();

                if (request.result == UnityWebRequest.Result.Success)
                {
                    LogSuccess(api, request);
                    return JsonConvert.DeserializeObject<T>(request.downloadHandler.text);
                }
                
                LogFail(api, request);
                throw new WebException(request.error);
            }
        }

        public static async UniTask<T> Post<T>(string api, object data)
        {
            if (!await WaitLogin())
                throw new WebException("Login failed");
            var jsonData = JsonConvert.SerializeObject(data);
            byte[] rawData = Encoding.UTF8.GetBytes(jsonData);
            using (UnityWebRequest request = new UnityWebRequest(_config.ApiAddress + api, "POST"))
            {
                request.uploadHandler = new UploadHandlerRaw(rawData);
                request.downloadHandler = new DownloadHandlerBuffer();
                request.SetRequestHeader("Content-Type", "application/json");
                await request.SendWebRequest();

                if (request.result == UnityWebRequest.Result.Success)
                {
                    LogSuccess(api, request, jsonData);
                    return JsonConvert.DeserializeObject<T>(request.downloadHandler.text);
                }
                
                LogFail(api, request);
                throw new WebException(request.error);
            }
        }
        
        public static async UniTask Post(string api, object data, bool ignoreLogin = false)
        {
            if (!ignoreLogin && !await WaitLogin())
                throw new WebException("Login failed");
            var jsonData = JsonConvert.SerializeObject(data);
            byte[] rawData = Encoding.UTF8.GetBytes(jsonData);
            using (UnityWebRequest request = new UnityWebRequest(_config.ApiAddress + api, "POST"))
            {
                request.uploadHandler = new UploadHandlerRaw(rawData);
                request.downloadHandler = new DownloadHandlerBuffer();
                request.SetRequestHeader("Content-Type", "application/json");
                await request.SendWebRequest();

                if (request.result == UnityWebRequest.Result.Success)
                {
                    LogSuccess(api, request, jsonData);
                    return;
                }
                
                LogFail(api, request);
                throw new WebException(request.error);
            }
        }

        private static async UniTask<bool> WaitLogin()
        {
            if (_loggedIn)
                return true;

            if (_loginCompletionSource != null)
            {
                await _loginCompletionSource.Task;
                return _loggedIn;
            }

            _loginCompletionSource = new UniTaskCompletionSource();
            
            try
            {
                await Post(_config.JoinUserApi, new { id = GetUserIdentity() }, true);
                _loggedIn = true;
            }
            catch (Exception e)
            {
                Debug.LogException(e);
                _loggedIn = false;
            }

            _loginCompletionSource.TrySetResult();
            _loginCompletionSource = null;
            return _loggedIn;
        }

        private static void LogSuccess(string api, UnityWebRequest request, string input = "")
        {
            var str =$"<color=#44ee44>{request.method} ({api}) success:</color>\n";
            if (!string.IsNullOrEmpty(input))
                str += $"<color=#aaaa44>Input: {input}</color>\n";
            str += $"<color=#44aaaa>Response: {request.downloadHandler.text}</color>";
            Debug.Log(str);
        }

        private static void LogFail(string api, UnityWebRequest request)
        {
            Debug.Log($"<color=#ee4444>{request.method} ({api}) failed:</color>\n" +
                      $"<color=#aa4444>Error: {request.error}</color>\n" +
                      $"<color=#44aaaa>Response: {request.downloadHandler.text}</color>");
        }
    }
}
