using BlogApplication.Data.FileManager;
using BlogApplication.Data.Repository;
using BlogApplication.Models;
using BlogApplication.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogApplication.Controllers
{
    [Authorize(Roles = "Admin")]
    public class PanelController:Controller
    {
        private IRepository _repo;
        private IFileManager _fileManager;

        public PanelController(IRepository repo, IFileManager fileManager)
        {
            _repo = repo;
            _fileManager = fileManager;
        }

        // Gets all post and returns a view of all post
        public IActionResult Index()
        {
            var posts = _repo.GetAllPosts();
            return View(posts);
        }

        //Edit passes a nullable id, and checks if id is nulln and returns the post
        //Else it gets the post
        [HttpGet]
        public IActionResult Edit(int? id)
        {
            if (id == null)
            {


                return View(new PostViewModel());
            }
            else
            {
                var post = _repo.GetPost((int)id);
                return View(new PostViewModel
                {
                    id = post.id,
                    Title = post.Title,
                    Body = post.Body,
                    CurrentImage = post.Image,
                    Descriptions = post.Descriptions,
                    Category = post.Category,
                    Tags = post.Tags,

                });
            }
        }

        /* 
         * Takes in a postViewModel
         * It checks the id and sees if its more than 0 if it is it updates the post
         * Else it adds post  
         * If post have saved go to Index else View post
         * It saves images and populates it in the fileManager when it has been uploaded
         */
        [HttpPost]
        public async Task<IActionResult> Edit(PostViewModel postViewModel)
        {
            var post = new Post
            {
                id = postViewModel.id,
                Title = postViewModel.Title,
                Body = postViewModel.Body,
                Descriptions = postViewModel.Descriptions,
                Category = postViewModel.Category,
                Tags = postViewModel.Tags,
               // Image = await _fileManager.SaveImage(postViewModel.Image)
            };

            /*
             * Checks if the picture is the same
             * Save the picture if it is changed
             */
            if(postViewModel == null) 
                post.Image = postViewModel.CurrentImage;
            else
                post.Image = await _fileManager.SaveImage(postViewModel.Image);

            if (post.id > 0)
                _repo.UpdatePost(post);
            else
                _repo.AddPost(post);

            if (await _repo.SaveChangesAsync())
                return RedirectToAction("Index");
            else
                return View(post);

        }

        /* 
         * Removes post with the id
         * and save changes
         */
        [HttpGet]
        public async Task<IActionResult> Remove(int id)
        {
            _repo.RemovePost(id);
            await _repo.SaveChangesAsync();
            return RedirectToAction("Index");
        }
    }
}
