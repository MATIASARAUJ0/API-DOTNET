using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using API_DOTNET.DTO;

namespace API_DOTNET.Integrations
{
    public class JsonplaceholderAPIIntegration
    {
        private readonly ILogger<JsonplaceholderAPIIntegration> _logger;
        private const string API_URL = "https://jsonplaceholder.typicode.com/posts/";
        private readonly HttpClient httpClient;

        public JsonplaceholderAPIIntegration(ILogger<JsonplaceholderAPIIntegration> logger)
        {
            _logger = logger;
            httpClient = new HttpClient();
        }

        public async Task<PostDTO?> CreatePost(PostDTO post)
        {

            try
            {
                string requestUrl = $"{API_URL}";

                string postJson = JsonSerializer.Serialize(post);

                HttpContent content = new StringContent(postJson, Encoding.UTF8, "application/json");

                HttpResponseMessage response = await httpClient.PostAsync(requestUrl, content);

                if (response.IsSuccessStatusCode)
                {
                    string responseJson = await response.Content.ReadAsStringAsync();
                    PostDTO newPost = JsonSerializer.Deserialize<PostDTO>(responseJson);
                    Console.WriteLine(responseJson);
                    Console.WriteLine("ESTADO: " + response.StatusCode);
                    return newPost;
                }
                else
                {
                    _logger.LogError($"ERROR CODIGO: {response.StatusCode}");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"DESCRIPCIÓN DE ERROR DE API: {ex.Message}");

            }
            return null;
        }


        public async Task<List<PostDTO>> GetAllPost()
        {
            string requestUrl = $"{API_URL}";
            List<PostDTO> lista = new List<PostDTO>();
            try
            {
                HttpResponseMessage response = await httpClient.GetAsync(requestUrl);
                if (response.IsSuccessStatusCode)
                {
                    lista = await response.Content.ReadFromJsonAsync<List<PostDTO>>() ?? new List<PostDTO>();
                }
            }
            catch (Exception ex)
            {
                _logger.LogDebug($"ERROR AL TRATAR DE COMUNICARSE CON EL API: {ex.Message}");
            }

            return lista;
        }

        

        public async Task<String> DeletePost(int id)
        {
            try
            {
                string url = $"{API_URL}{id}";

                HttpResponseMessage response = await httpClient.DeleteAsync(url);

                if (response.IsSuccessStatusCode)
                {
                    string responseJson = await response.Content.ReadAsStringAsync();
                    Console.WriteLine(responseJson);
                    Console.WriteLine("ESTADO: " + response.StatusCode);
                    return response.StatusCode.ToString();
                }
                else
                {
                    _logger.LogError($"ERROR AL TRATAR DE ELIMINAR EL POST. CODIGO: {response.StatusCode}");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"DESCRIPCIÓN DE ERROR DE API: {ex.Message}");
            }

            return "NO SE PUDO CONCRETAR LA ELIMINACIÓN";
        }

        public async Task<PostDTO?> UpdatePost(PostDTO post)
        {

            try
            {
                string requestUrl = $"{API_URL}{post.id}";
                string postJson = JsonSerializer.Serialize(post);
                HttpContent content = new StringContent(postJson, Encoding.UTF8, "application/json");
                HttpResponseMessage response = await httpClient.PutAsync(requestUrl, content);

                if (response.IsSuccessStatusCode)
                {
                    string responseJson = await response.Content.ReadAsStringAsync();

                    PostDTO updatedPost = JsonSerializer.Deserialize<PostDTO>(responseJson);
                    Console.WriteLine(updatedPost);
                    Console.WriteLine(responseJson);
                    Console.WriteLine("ESTADO: " + response.StatusCode);
                    Console.WriteLine(response.Content);

                    return updatedPost;
                }

            }
            catch (Exception ex)
            {
                _logger.LogError($"DESCRIPCIÓN DE ERROR DE API: {ex.Message}");
            }

            return null;
        }

        public async Task<PostDTO?> GetPost(int id)
        {
            try
            {
                string requestUrl = $"{API_URL}{id}";
                HttpResponseMessage response = await httpClient.GetAsync(requestUrl);
                if (response.IsSuccessStatusCode)
                {
                    string post = await response.Content.ReadAsStringAsync();
                    PostDTO p = JsonSerializer.Deserialize<PostDTO>(post);
                    Console.WriteLine(p);
                    return p;
                }
                else
                {
                    _logger.LogError($"ERROR AL CONCRETAR LA BUSQUEDA DEL POST. CODIGO: {response.StatusCode}");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"DESCRIPCIÓN DE ERROR DE API: {ex.Message}");
            }

            return null;
        }

        
    }
}