using BlogApplication.Data.FileManager;
using BlogApplication.Data.Repository;
using BlogApplication.Models.Comments;
using BlogApplication.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BlogApplication.Controllers
{
    public class HomeController : Controller
    {
        private IRepository _repo;
        private IFileManager _fileManager;

        public HomeController(IRepository repo, IFileManager fileManager)
        {
           _repo   = repo;
            _fileManager = fileManager;
           
        }

        // Gets all post with the category and returns a view of all post
        public IActionResult Index(string category)
        {
            var posts = string.IsNullOrEmpty(category)?_repo.GetAllPosts():_repo.GetAllPosts(category);
            return View(posts);
        }

        //accept an id and get the post and return the post and the view
        public IActionResult Post(int id)
        {
            var post = _repo.GetPost(id);
            return View(post);
        }

        /*
         * Takes in an image string znd returns it
         */
        [HttpGet("/Image/{image}")]
        public IActionResult Image(string image)
        {
            var mime = image.Substring(image.LastIndexOf('.') + 1);
            return new FileStreamResult(_fileManager.ImageStream(image), $"image/{mime}");
        }

        /*
         * This method checks for a comment and view it
         * If the mainCommentId is 0 then create a mainComment
         * If its O then its a main comment
         * it also view the date and time the comments was created
         */
         [HttpPost]
        public async Task<IActionResult> Comment(CommentViewModel vm)
        {
            if (!ModelState.IsValid)
                return RedirectToAction("Post", new { id = vm.postId });

            var post = _repo.GetPost(vm.postId);
            if(vm.MainCommentId == 0)
            {
                post.MainComments = post.MainComments ?? new List<MainComment>();

                post.MainComments.Add(new MainComment
                {
                    Message = vm.Message,
                    Created = DateTime.Now,

                });

                _repo.UpdatePost(post);
            }
            else
            {
                var comment = new SubComment
                {
                    MainCommentId = vm.MainCommentId,
                    Message = vm.Message,
                    Created = DateTime.Now,
                };
                _repo.AddSubComment(comment);
            }
            await _repo.SaveChangesAsync();

            return RedirectToAction("Post", new { id = vm.postId });
            //return View();
        }
    }

}
