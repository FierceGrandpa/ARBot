using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AR.Bot.Domain;
using AR.Bot.Repositories;
using AR.Bot.Web.Core.Utils;

// ReSharper disable once CheckNamespace
namespace AR.Bot.Core.Services
{
    public interface IActivityService
    {
        IEnumerable<Activity> GetUnusedActivities(Guid userId);

        Task<Activity> GetRandomActivity(Guid userId); 
        Task<Activity> GetRandomActivityByCategory(Guid userId, Guid categoryId);
    }

    public class ActivityService : IActivityService
    {
        private readonly SecureRandomGenerator _random = new();

        // TODO: Good...
        private const int MaxActivities = 3;

        // TODO: Code Gen [Inject]
        private readonly IRepository<Activity> _activitiesRepository;
        private readonly IRepository<Skill>    _skillsRepository;
        private readonly IDailyParcelRepository     _dailyParcelService;
        private readonly IUnitOfWork _unitOfWork;
        
        public ActivityService(
            IRepository<Activity>     activitiesRepository,
            IRepository<Skill>        skillsRepository,
            IDailyParcelRepository         dailyParcelService,
            IUnitOfWork unitOfWork)
        {
            _activitiesRepository     = activitiesRepository;
            _skillsRepository         = skillsRepository;
            _dailyParcelService       = dailyParcelService;
            _unitOfWork               = unitOfWork;
        }

        // TODO: Refactor
        public IEnumerable<Activity> GetUnusedActivities(Guid userId)
        {
            // TODO: Refactor
            bool UsedByUser(SentActivity e) => e.TelegramUserId == userId;

            static bool Active(Activity e) => e.Status;

            bool NotUsed(Activity activity) => _dailyParcelService
                .GetWithInclude(UsedByUser, e=> e.TelegramUser, e=> e.Activity)
                .All(e => activity.Id != e.ActivityId);

            var activities = _activitiesRepository.GetWithInclude(e=> Active(e) && NotUsed(e),
                e=> e.SentActivities, e=> e.Skills);

            // TODO: Now Activities Can Be Null - This Bad? Make Log and...

            return activities;
        }

        public async Task<Activity> GetRandomActivity(Guid userId)
        {
            if (_dailyParcelService.GetTodayParcel(userId).Count() >= MaxActivities)
            {
                throw new RandomActivityException();
            }

            var activities = GetUnusedActivities(userId);
            if (activities == null)
            {
                throw new RandomActivityException();
            }

            // TODO: Optimize and MAKE OUT MAGIC, WOW!
            var enumerable = activities as Activity[] ?? activities.ToArray();
            var activity = enumerable.ElementAt(_random.Next(enumerable.Length));

            await _dailyParcelService.AddAsync(new SentActivity(userId, activity.Id));
            await _unitOfWork.SaveChangesAsync();

            return activity;
        }

        public async Task<Activity> GetRandomActivityByCategory(Guid userId, Guid categoryId)
        {
            // TODO: This Bad?
            if (_dailyParcelService.GetTodayParcel(userId).Count() >= MaxActivities)
            {
                // TODO: This Bad?
                return null;
            }

            var activities = GetUnusedActivities(userId);
            if (activities == null)
            {
                // TODO: This Bad...
                return null;
            }
            // TODO: Refactor
            var skillsByCategory = _skillsRepository.GetWithInclude(
                e => e.Status && e.CategoryId == categoryId, e => e.Activities, e => e.Category);

            // TODO: Optimize and MAKE OUT MAGIC, WOW!
            var enumerable = activities as Activity[] ?? activities.ToArray();
            var activity = enumerable.Where(e => e.Skills.Any(o => 
                skillsByCategory.Any(s => s.Id == o.Id))).ElementAt(_random.Next(enumerable.Length));

            await _dailyParcelService.AddAsync(new SentActivity(userId, activity.Id));
            await _unitOfWork.SaveChangesAsync();

            return activity;
        }
    }
}
