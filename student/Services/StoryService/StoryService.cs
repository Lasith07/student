using DemoAPI.Models;
using DemoAPI.DTOs.Responses;
using Microsoft.EntityFrameworkCore;

namespace DemoAPI.Services.StoryService
{
    public class StoryService : IStoryService
    {
        private readonly ApplicationDbContext _dbContext;

        public StoryService(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public List<StoryModel> GetAllStories()
        {
            return _dbContext.Story.ToList();
        }

        public StoryModel GetStoryById(long id)
        {
            return _dbContext.Story.FirstOrDefault(s => s.storyid == id);
        }

        public StoryModel CreateStory(StoryModel storyModel)
        {
            _dbContext.Story.Add(storyModel);
            _dbContext.SaveChanges();
            return storyModel;
        }

        public StoryModel UpdateStory(long id, StoryModel storyModel)
        {
            var existingStory = _dbContext.Story.FirstOrDefault(s => s.storyid == id);
            if (existingStory == null)
            {
                return null;
            }

            existingStory.title = storyModel.title;
            existingStory.content = storyModel.content;
       

            _dbContext.SaveChanges();
            return existingStory;
        }

        public StoryModel DeleteStory(long id)
        {
            var story = _dbContext.Story.FirstOrDefault(s => s.storyid == id);
            if (story == null)
            {
                return null;
            }

            _dbContext.Story.Remove(story);
            _dbContext.SaveChanges();
            return story;
        }
    }
}
