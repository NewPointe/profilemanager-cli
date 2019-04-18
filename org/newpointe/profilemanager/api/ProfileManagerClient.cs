using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

using Newtonsoft.Json;

using org.newpointe.profilemanager.api.structures;

namespace org.newpointe.profilemanager.api
{

    class ProfileManagerClient
    {

        private readonly HttpClient client;

        public ProfileManagerClient(string controllerAddress)
        {
            client = new HttpClient();
            client.BaseAddress = new System.Uri(controllerAddress);
            client.DefaultRequestHeaders.Add("User-Agent", "org.newpointe.profilemanager.api.ApiClient");
        }


        /// <summary>
        /// Logs in the client.
        /// </summary>
        /// <param name="username">The username.</param>
        /// <param name="password">The password.</param>
        /// <returns>Returns the response for the login.</returns>
        public Task<HttpResponseMessage> Login(string username, string password)
        {
            return client.PostAsJsonAsync(
                KnownEndpoints.API_AUTH,
                new
                {
                    username = username,
                    password = password,
                    rememberMe = false
                }
            );
        }


        // Util

        /// <summary>
        /// Does the Magic. Woosh! ✨🌠🌌💫
        /// </summary>
        /// <param name="item">The item to do magic to.</param>
        /// <param name="method">The type of magic to do.</param>
        /// <param name="parameters">The parameters for the magic.</param>
        /// <typeparam name="T">The Type of the `parameters`.</typeparam>
        /// <returns>Returns the Magic's response.</returns>
        public Task<HttpResponseMessage> DoMagic(string item, string method, params object[] parameters)
        {
            return DoMagicBatch(new[] { new DoMagicRequestItem { Item = item, Method = method, Parameters = parameters } });
        }

        /// <summary>
        /// Does a lot of Magic. The Woosh has been doubled! ✨🌠🌌💫✨🌠🌌💫
        /// 
        /// Note: Duplicate Magic requests (requests with the same `Item` and `Method`) will be ignored.
        /// </summary>
        /// <param name="magicRequests">An array of Magic requests to make.</param>
        /// <returns>Returns the Magic's response.</returns>
        public Task<HttpResponseMessage> DoMagicBatch(DoMagicRequestItem[] magicRequests)
        {
            // Build the Magic request
            var magicRoot = new Dictionary<string, Dictionary<string, object>>();
            foreach (var magicRequest in magicRequests)
            {
                Dictionary<string, object> magicActions;
                if (!magicRoot.TryGetValue(magicRequest.Item, out magicActions))
                {
                    magicActions = new Dictionary<string, object>();
                    magicRoot.Add(magicRequest.Item, magicActions);
                }

                if (!magicActions.ContainsKey(magicRequest.Method))
                {
                    magicActions.Add(magicRequest.Method, new[] { magicRequest.Parameters });
                }
            }

            // Post the Magic ✨
            return client.PostAsJsonAsync(KnownEndpoints.API_PROFILEMANAGER_MAGIC_DO_MAGIC, magicRoot);
        }

        /// <summary>
        /// Does Magic to retreive a list of Ids.
        /// </summary>
        /// <param name="item">The item to do magic to.</param>
        /// <param name="method">The type of magic to do.</param>
        /// <returns>Returns a list of Ids.</returns>
        public async Task<int[]> DoMagicForRemoteIds(string item, string method)
        {
            // Do Magic
            var response = await DoMagic(item, method, "__ReturnValue");

            // Check response codes
            response.EnsureSuccessStatusCode();

            // Read response
            var responseJSON = await response.Content.ReadAsStringAsync();
            var responseData = JsonConvert.DeserializeObject<DoMagicForRemoteIdsResponse>(responseJSON);

            // Try extracting the remote response
            if (responseData.remote.TryGetValue("__ReturnValue", out var returnValue))
            {
                if (returnValue.Length > 0)
                {
                    var innerReturnValue = returnValue[0];
                    return innerReturnValue.Skip(1).Select(v => Convert.ToInt32(v)).ToArray();
                }
            }

            throw new Exception(@"Response was not in the expected format. Do you have the right Magic?
Expected Format:
{ ""remote"": { <request_key>: [[<item>, <ids>...]] }}

Actual Response:
" + responseJSON);

        }

        public async Task<T[]> DoMagicForResults<T>(string item, string method, params object[] parameters)
        {
            var response = await DoMagic(item, method, parameters);
            response.EnsureSuccessStatusCode();
            var responseJSON = await response.Content.ReadAsStringAsync();
            var responseData = JsonConvert.DeserializeObject<DoMagicForResultResponse<T>>(responseJSON);

            if (responseData.result.TryGetValue(item, out var returnValue))
            {
                return returnValue.retrieved;
            }

            throw new Exception(@"Response was not in the expected format. Do you have the right Magic?
Expected Format:
{ ""result"": { <item>: { retrieved: [<item_data>...] } } }

Actual Response:
" + responseJSON);

        }


        // Library Actions

        public async Task<int[]> GetActiveLibraryTaskIds()
        {
            return await DoMagicForRemoteIds("library_item_task", "find_all_active");
        }

        public Task<HttpResponseMessage> StartLibraryTask(string target_class, int target_id, string task_type)
        {
            return this.StartLibraryTaskWithParams(target_class, target_id, task_type, new object { });
        }
        public Task<HttpResponseMessage> StartLibraryTaskWithParams(string target_class, int target_id, string task_type, object @params)
        {
            return DoMagic("library_item_task", "start_task", new { target_class, target_id, task_type, @params }, "");
        }

        // Device Actions

        public Task<int[]> GetDeviceIds()
        {
            return DoMagicForRemoteIds("device", "find_all");
        }

        public Task<int[]> GetAppleTvDeviceIds()
        {
            return DoMagicForRemoteIds("device", "find_all_apple_tvs");
        }

        public Task<DevicePartial[]> GetDevices(int[] ids)
        {
            return DoMagicForResults<DevicePartial>("device", "get_details", new { ids });
        }

        public Task<HttpResponseMessage> LockDevice(int deviceId)
        {
            return this.StartLibraryTask("Device", deviceId, "DeviceLock");
        }
        public Task<HttpResponseMessage> ShutDownDevice(int deviceId)
        {
            return this.StartLibraryTask("Device", deviceId, "ShutDownDevice");
        }
        public Task<HttpResponseMessage> RestartDevice(int deviceId)
        {
            return this.StartLibraryTask("Device", deviceId, "RestartDevice");
        }

        // Device Group Actions

        public Task<int[]> GetDeviceGroupIds()
        {
            return DoMagicForRemoteIds("device_group", "find_all");
        }

        public Task<HttpResponseMessage> LockDeviceGroup(int deviceGroupId)
        {
            return this.StartLibraryTask("DeviceGroup", deviceGroupId, "DeviceLock");
        }
        public Task<HttpResponseMessage> ShutDownDeviceGroup(int deviceGroupId)
        {
            return this.StartLibraryTask("DeviceGroup", deviceGroupId, "ShutDownDevice");
        }
        public Task<HttpResponseMessage> RestartDeviceGroup(int deviceGroupId)
        {
            return this.StartLibraryTask("DeviceGroup", deviceGroupId, "RestartDevice");
        }

        // User Actions

        public Task<int[]> GetUserIds()
        {
            return DoMagicForRemoteIds("user", "find_all");
        }

        public async Task<int> GetMyUserId()
        {
            return (await DoMagicForRemoteIds("user", "find_me")).FirstOrDefault();
        }

        // User Group Actions

        public Task<int[]> GetUserGroupIds()
        {
            return DoMagicForRemoteIds("user_group", "find_all");
        }

        public Task<int[]> GetVPPEnabledUserGroupIds()
        {
            return DoMagicForRemoteIds("user_group", "find_all_vpp_enabled");
        }

        // Class Actions

        public Task<int[]> GetClassIds()
        {
            return DoMagicForRemoteIds("edu_class", "find_all");
        }


        // Utility Classes
        private class DoMagicForRemoteIdsResponse
        {
            public Dictionary<string, object[][]> remote { get; set; }
        }

        private class DoMagicForResultResponse<T>
        {
            public Dictionary<string, DoMagicForResultResponseItem<T>> result { get; set; }
        }

        private class DoMagicForResultResponseItem<T>
        {
            public T[] retrieved { get; set; }
        }

    }
}