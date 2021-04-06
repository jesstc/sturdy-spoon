using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Swagger.Models;
using Swagger.Service;

namespace Swagger.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PostController : Controller
    {
        private readonly DictionaryService _dbdata;

        public PostController(DictionaryService dictionaryService)
        {
            _dbdata = dictionaryService;
        }

        [HttpGet("GetPostByUserId")]
        public ActionResult<List<Post>> GetPostByUserId(Guid userId)
        {
            var result = _dbdata.postData.Where(x => x.Key.Equals(userId)).ToList();
            return result.Count == 0 ? Ok("Cannot find match data") : Ok(result);
        }

        [HttpGet("GetAllPost")]
        public ActionResult GetAllPost()
        {
            return Ok(_dbdata.postData.Select(x => new { x.Key, x.Value }));
        }

        [HttpPost("CreatePost")]
        public ActionResult<List<Post>> CreatePost(Guid AuthorId, string PostContent)
        {
            var newPost = new Post() { PostId = Guid.NewGuid(), Content = PostContent, LikeName = null };

            if (_dbdata.postData != null)
            {
                if (_dbdata.postData.Keys.Contains(AuthorId))
                {
                    _dbdata.postData[AuthorId].Add(newPost);
                }
                else
                {
                    _dbdata.postData.Add(AuthorId, new List<Post>());
                    _dbdata.postData[AuthorId].Add(newPost);
                }
            }

            return Ok(_dbdata.postData[AuthorId]);
        }

        [HttpPut("LikePost")]
        public ActionResult LikePost(Guid likePostId, Guid likeUserId)
        {
            foreach (var posts in _dbdata.postData.Values)
            {
                foreach (var post in posts)
                {
                    if (post.PostId.Equals(likePostId))
                    {
                        var likeUser = _dbdata.accountData.Find(x => x.UserId.Equals(likeUserId));
                        post.LikeName ??= new List<string>();
                        if (!post.LikeName.Contains(likeUser.Name))
                        {
                            post.LikeName.Add(likeUser.Name);
                            return Ok(post);
                        }
                        else
                        {
                            return Ok("You are already like the post");
                        }
                    }
                }
            }

            return Ok("Cannot find the post by this id");
        }

        [HttpPut("EditPost")]
        public ActionResult EditPost(Guid postId, string newContent)
        {
            foreach (var posts in _dbdata.postData.Values)
            {
                foreach (var post in posts)
                {
                    post.Content = newContent;
                    return Ok(post);
                }
            }

            return Ok("Cannot find the post by this id");
        }

        [HttpDelete("DeletePost")]
        public ActionResult DeletePost(Guid postId)
        {
            var delRaw = 0;
            foreach (var (guid, posts) in _dbdata.postData)
            {
                delRaw += posts.RemoveAll(x => x.PostId.Equals(postId));
            }

            return delRaw == 0 ? Ok("Cannot find match data") : Ok(_dbdata.postData.Select(x => new { x.Key, x.Value }));
        }

        [HttpGet("GetAllComment")]
        public ActionResult GetAllComment()
        {
            return Ok(_dbdata.commentData.Select(x => new { x.Key, x.Value }));
        }

        [HttpGet("GetCommentByPostId")]
        public ActionResult GetCommentById(Guid postId)
        {
            return Ok(_dbdata.commentData.Select(x => new { x.Key, x.Value }).Where(i => i.Key.Equals(postId)));
        }

        [HttpPost("AddCommentToPost")]
        public ActionResult AddCommentToPost(Guid postId, Guid authorId, string commentContent)
        {
            var authorAccount = _dbdata.accountData.Find(x => x.UserId.Equals(authorId));
            var newComment = new Comment() { CommentId = Guid.NewGuid(), Author = authorAccount.Name, Content = commentContent, LikeName = new List<string>() };

            if (_dbdata.commentData != null)
            {
                if (!_dbdata.commentData.Keys.Contains(postId))
                {
                    _dbdata.commentData.Add(postId, new List<Comment>());

                }
                _dbdata.commentData[postId].Add(newComment);
            }

            return Ok(_dbdata.commentData[postId]);
        }

        [HttpPut("LikeComment")]
        public ActionResult LikeComment(Guid likeCommentId, Guid likeUserId)
        {
            foreach (var comments in _dbdata.commentData.Values)
            {
                foreach (var comment in comments)
                {
                    if (comment.CommentId.Equals(likeCommentId))
                    {
                        var likeUser = _dbdata.accountData.Find(x => x.UserId.Equals(likeUserId));
                        comment.LikeName ??= new List<string>();

                        if (!comment.LikeName.Contains(likeUser.Name))
                        {
                            comment.LikeName.Add(likeUser.Name);
                            return Ok(comment);
                        }
                        else
                        {
                            return Ok("You are already like the comment");
                        }
                    }
                }
            }

            return Ok("Cannot find the comment by this id");
        }

        [HttpPut("EditComment")]
        public ActionResult EditComment(Guid commentId, string newContent)
        {
            foreach (var comments in _dbdata.commentData.Values)
            {
                foreach (var comment in comments)
                {
                    comment.Content = newContent;
                    return Ok(comment);
                }
            }

            return Ok("Cannot find the comment by this id");
        }

        [HttpDelete("DeleteComment")]
        public ActionResult DeleteComment(Guid commentId)
        {
            var delRaw = 0;
            foreach (var (guid, comments) in _dbdata.commentData)
            {
                delRaw += comments.RemoveAll(x => x.CommentId.Equals(commentId));
            }

            return delRaw == 0 ? Ok("Cannot find match data") : Ok(_dbdata.postData.Select(x => new { x.Key, x.Value }));
        }
    }
}
