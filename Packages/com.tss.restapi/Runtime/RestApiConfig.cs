using TSS.Utils;
using UnityEngine;

namespace TSS.Rest
{
    [CreateSingletonAsset("Assets/TSS/RestConfig.asset", "Rest Config")]
    public class RestApiConfig : ScriptableObject
    {
        public string ApiAddress => _apiAddress;
        public string JoinUserApi => _joinUserApi;
        public string UsersCountApi => _usersCountApi;
        
        [SerializeField] private string _apiAddress = "http://109.73.196.225:3000/api/";
        [SerializeField] private string _joinUserApi = "join-user";
        [SerializeField] private string _usersCountApi = "users-count";
    }
}