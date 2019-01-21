using BlogApplication.Models;
using BlogApplication.Models.Comments;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogApplication.Data.Repository
{
    public interface IRepository
    {
       
        Post GetPost(int id);  //gets your post
        
        List<Post> GetAllPosts(); //gets the list of all post
        List<Post> GetAllPosts(String Category); //gets the list of all post with the category
       
        void AddPost(Post post);  //checks if post has been added

        void UpdatePost(Post post);   //return true if post has been updated

        void RemovePost(int id);  //checks if post has been removed

        void AddSubComment(SubComment comment); 
        Task<bool> SaveChangesAsync();
    }
}
