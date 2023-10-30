using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using API_DOTNET.DTO;
using API_DOTNET.Integrations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace API_DOTNET.Controllers.UI
{
    public class PostController : Controller
    {
        private readonly ILogger<PostController> _logger;
        private readonly JsonplaceholderAPIIntegration _jsonplaceholder;

        public PostController(ILogger<PostController> logger, JsonplaceholderAPIIntegration jsonplaceholder)
        {
            _logger = logger;
            _jsonplaceholder = jsonplaceholder;
        }

        public async Task<IActionResult> Index()
        {
            List<PostDTO> lista = await _jsonplaceholder.GetAllPost();
            return View(lista);
        }

        public IActionResult Create()
        {
            return View("Create");
        }

        [HttpPost]
        public async Task<IActionResult> SaveNewPost(PostDTO post)
        {
            var newPost = await _jsonplaceholder.CreatePost(post);
            return RedirectToAction(nameof(DetailPostCreated), post);
        }

        public IActionResult Delete(int id)
        {
            Task<String> res = _jsonplaceholder.DeletePost(id);
            Console.WriteLine(res);
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> GetPost(int id)
        {
            PostDTO post = await _jsonplaceholder.GetPost(id);
            return View("Detail", post);
        }

        public async Task<IActionResult> Update(int id)
        {
            PostDTO post = await _jsonplaceholder.GetPost(id);
            return View("Update", post);
        }

        [HttpPost]
        public async Task<IActionResult> UpdatePost(PostDTO post)
        {
            var newPost = await _jsonplaceholder.UpdatePost(post);
            return RedirectToAction(nameof(DetailPostUpdated), post);
        }

        public IActionResult DetailPostCreated(PostDTO post)
        {
            return View("ViewPostCreate", post);
        }

        public IActionResult DetailPostUpdated(PostDTO post)
        {
            return View("ViewPostUpdate", post);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View("Error!");
        }
    }
}