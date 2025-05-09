using Microsoft.AspNetCore.Mvc;
using DemoAPI.Models;
using DemoAPI.DTOs.Responses;
using DemoAPI.Services.StoryService;

namespace DemoAPI.Controllers
{
    [Route("api/story")]
    [ApiController]
    public class StoryController : ControllerBase
    {
        private readonly IStoryService _storyService;

        public StoryController(IStoryService storyService)
        {
            _storyService = storyService;
        }

     
        [HttpGet("list")]
        public IActionResult GetAllStories()
        {
            var stories = _storyService.GetAllStories();
            return Ok(new BaseResponse(StatusCodes.Status200OK, stories));
        }

    
        [HttpGet("{id}")]
        public IActionResult GetStoryById(long id)
        {
            var story = _storyService.GetStoryById(id);
            if (story == null)
            {
                return NotFound(new BaseResponse(StatusCodes.Status404NotFound, new MessageDTO("Story not found")));
            }
            return Ok(new BaseResponse(StatusCodes.Status200OK, story));
        }

       
        [HttpPost]
        public IActionResult CreateStory([FromBody] StoryModel storyModel)
        {
            if (storyModel == null)
            {
                return BadRequest(new BaseResponse(StatusCodes.Status400BadRequest, new MessageDTO("Invalid story data")));
            }

            var result = _storyService.CreateStory(storyModel);
            if (result == null)
            {
                return BadRequest(new BaseResponse(StatusCodes.Status400BadRequest, new MessageDTO("Failed to create story")));
            }

            return CreatedAtAction(nameof(GetStoryById), new { id = result.storyid }, result);
        }

    
        [HttpPut("{id}")]
        public IActionResult UpdateStory(long id, [FromBody] StoryModel storyModel)
        {
            if (storyModel == null || id != storyModel.storyid)
            {
                return BadRequest(new BaseResponse(StatusCodes.Status400BadRequest, new MessageDTO("Invalid story data")));
            }

            var updatedStory = _storyService.UpdateStory(id, storyModel);
            if (updatedStory == null)
            {
                return NotFound(new BaseResponse(StatusCodes.Status404NotFound, new MessageDTO("Story not found")));
            }

            return Ok(new BaseResponse(StatusCodes.Status200OK, updatedStory));
        }

     
        [HttpDelete("{id}")]
        public IActionResult DeleteStory(long id)
        {
            var deletedStory = _storyService.DeleteStory(id);
            if (deletedStory == null)
            {
                return NotFound(new BaseResponse(StatusCodes.Status404NotFound, new MessageDTO("Story not found")));
            }

            return NoContent();
        }
    }
}
