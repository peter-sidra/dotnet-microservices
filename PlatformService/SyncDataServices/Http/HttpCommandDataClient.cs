using System;
using System.Formats.Asn1;
using System.Net.Http;
using System.Net.Mime;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using PlatformService.Dtos;

namespace PlatformService.SyncDataServices.Http {
    public class HttpCommandDataClient : ICommandDataClient {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _config;

        // Because we're injecting an HttpClient, we need an http client factory
        public HttpCommandDataClient(HttpClient httpClient, IConfiguration config) {
            _httpClient = httpClient;
            _config = config;
        }

        public async Task SendPlatformToCommand(PlatformReadDto plat) {
            var httpContent = new StringContent(
                JsonSerializer.Serialize(plat),
                encoding: Encoding.UTF8,
                mediaType: MediaTypeNames.Application.Json
            );

            var response = await _httpClient.PostAsync(
                _config["CommandService"],
                httpContent
            );

            if (response.IsSuccessStatusCode) {
                Console.WriteLine("--> Sync POST to CommandService was OK!");
            }
            else {
                Console.WriteLine("--> Sync POST to CommandService was not OK!");
            }
        }
    }
}