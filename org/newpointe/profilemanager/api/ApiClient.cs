using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace org.newpointe.profilemanager.api
{

    class ApiClient
    {

        private readonly HttpClient client;

        public ApiClient(string controllerAddress)
        {
            client = new HttpClient();
            client.BaseAddress = new System.Uri(controllerAddress);
            client.DefaultRequestHeaders.Add("User-Agent", "org.newpointe.profilemanager.api.ApiClient");
        }

        public async Task<HttpResponseMessage> Login(string username, string password)
        {
            return await client.PostAsJsonAsync(
                KnownEndpoints.API_AUTH,
                new {
                    username = username,
                    password = password,
                    rememberMe = false
                }
            );
        }

        // Library Actions
        
        public async Task<HttpResponseMessage> StartLibraryTask(string target_class, int target_id, string task_type) {
            return await this.StartLibraryTaskWithParams(target_class, target_id, task_type, new object {});
        }
        public async Task<HttpResponseMessage> StartLibraryTaskWithParams(string target_class, int target_id, string task_type, object @params) {
            return await client.PostAsJsonAsync(
                KnownEndpoints.API_PROFILEMANAGER_MAGIC_DO_MAGIC,
                new {
                    library_item_task = new {
                        start_task = new [] {
                            new object[] {
                                new { target_class, target_id, task_type, @params },
                                ""
                            }
                        },
                    }
                }
            );
        }

        // Device Actions

        public async Task<HttpResponseMessage> LockDevice(int deviceId) {
            return await this.StartLibraryTask("Device", deviceId, "DeviceLock");
        }
        public async Task<HttpResponseMessage> ShutDownDevice(int deviceId) {
            return await this.StartLibraryTask("Device", deviceId, "ShutDownDevice");
        }
        public async Task<HttpResponseMessage> RestartDevice(int deviceId) {
            return await this.StartLibraryTask("Device", deviceId, "RestartDevice");
        }

        // Device Group Actions

        public async Task<HttpResponseMessage> LockDeviceGroup(int deviceGroupId) {
            return await this.StartLibraryTask("DeviceGroup", deviceGroupId, "DeviceLock");
        }
        public async Task<HttpResponseMessage> ShutDownDeviceGroup(int deviceGroupId) {
            return await this.StartLibraryTask("DeviceGroup", deviceGroupId, "ShutDownDevice");
        }
        public async Task<HttpResponseMessage> RestartDeviceGroup(int deviceGroupId) {
            return await this.StartLibraryTask("DeviceGroup", deviceGroupId, "RestartDevice");
        }

    }
}