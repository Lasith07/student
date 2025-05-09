using DemoAPI.Models;
using System.Collections.Generic;

namespace DemoAPI.Services.StoryService
{
    public interface IStoryService
    {
        List<StoryModel> GetAllStories();
        StoryModel GetStoryById(long id);
        StoryModel CreateStory(StoryModel storyModel);
        StoryModel UpdateStory(long id, StoryModel storyModel);
        StoryModel DeleteStory(long id);
    }
}
