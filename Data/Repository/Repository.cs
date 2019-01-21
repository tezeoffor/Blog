using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BlogApplication.Models;
using BlogApplication.Models.Comments;
using Microsoft.EntityFrameworkCore;

namespace BlogApplication.Data.Repository
{
    public class Repository : IRepository
    {
        private AppDbContext _ctx;

        public Repository(AppDbContext ctx)
        {
            _ctx = ctx;
        }

        //Add post to the repository
        public void AddPost(Post post)
        {
            _ctx.Posts.Add(post);
           
        }

        //Get all the post and return the list of post
        public List<Post> GetAllPosts()
        {
            return _ctx.Posts.ToList();

        //Get all the post and return according to its category
        } public List<Post> GetAllPosts(string category)
        {
               
            return _ctx.Posts
                .Where(post => post.Category.ToLower().Equals(category.ToLower()))
                .ToList();       
        }

        //Take a single post, return the first post if the id equals to the id
        public Post GetPost(int id)
        {
            return _ctx.Posts
                .Include(p => p.MainComments)
                .ThenInclude(mc => mc.SubComments)
                .FirstOrDefault(p => p.id == id);
        }

        //Removes post with the id
        public void RemovePost(int id)
        {
            _ctx.Posts.Remove(GetPost(id));
        }


        public void UpdatePost(Post post)
        {
            _ctx.Posts.Update(post);
        }

        //This is called when we save changes, if saveChanges is greater than 0 return true
        public async Task<bool> SaveChangesAsync()
        {
            if (await _ctx.SaveChangesAsync() > 0)
            {
                return true;
            }
            return false;
        }

        public void AddSubComment(SubComment comment)
        {
            _ctx.SubComments.Add(comment);
        }
    }
}
