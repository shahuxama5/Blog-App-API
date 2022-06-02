using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TheBlogApp.Data;
using TheBlogApp.Models.DTO;
using TheBlogApp.Models.Entities;

namespace TheBlogApp.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PostsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public PostsController (ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllPosts()
        {
            var posts = await _context.Posts.ToListAsync();
            return Ok(posts);
        }

        [HttpGet]
        [Route("{id:guid}")]
        [ActionName("GetPostById")]
        public async Task<IActionResult> GetPostById(Guid id)
        {
            var post = await _context.Posts.FirstOrDefaultAsync(x => x.Id == id);
            if(post == null)
            {
                return NotFound();
            }
            return Ok(post);
        }

        [HttpPost]
        public async Task<IActionResult> AddPost(AddPostRequest addPostRequest)
        {
            var post = new Post()
            {
                Title = addPostRequest.Title,
                Content = addPostRequest.Content,
                Author = addPostRequest.Author,
                FeaturedImageUrl = addPostRequest.FeaturedImageUrl,
                Summary = addPostRequest.Summary,
                PublishDate = addPostRequest.PublishDate,
                UpdatedDate = addPostRequest.UpdatedDate,
                Visible = addPostRequest.Visible,
                UrlHandle = addPostRequest.UrlHandle,
            };
            post.Id = Guid.NewGuid();
            await _context.Posts.AddAsync(post);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetPostById), new {id = post.Id}, post);
        }

        [HttpPut]
        [Route("{id:guid}")]
        public async Task<IActionResult> UpdatePost([FromRoute] Guid id, UpdatePostRequest updatePostRequest)
        {
            var existingPost = await _context.Posts.FindAsync(id);

            if (existingPost != null)
            {
                existingPost.Title = updatePostRequest.Title;
                existingPost.Content = updatePostRequest.Content;
                existingPost.Author = updatePostRequest.Author;
                existingPost.FeaturedImageUrl = updatePostRequest.FeaturedImageUrl;
                existingPost.Summary = updatePostRequest.Summary;
                existingPost.PublishDate = updatePostRequest.PublishDate;
                existingPost.UpdatedDate = updatePostRequest.UpdatedDate;
                existingPost.Visible = updatePostRequest.Visible;
                existingPost.UrlHandle = updatePostRequest.UrlHandle;

                await _context.SaveChangesAsync();

                return Ok(existingPost);
            };

            return NotFound();
        }

        [HttpDelete]
        [Route("{id:guid}")]
        public async Task<IActionResult> DeletePost(Guid id)
        {
            var existingpost = await _context.Posts.FindAsync(id);
            if (existingpost == null)
            {
                return NotFound();
            }

            _context.Remove(existingpost);
            await _context.SaveChangesAsync();
            return Ok(existingpost);
        }

    }
}
