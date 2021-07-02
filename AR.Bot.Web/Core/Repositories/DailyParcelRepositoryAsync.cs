﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AR.Bot.Core.Data;
using AR.Bot.Domain;

// ReSharper disable once CheckNamespace
namespace AR.Bot.Repositories
{
    // TODO: Refactor
    public interface IDailyParcelRepository : IRepository<SentActivity>
    {
        Task Create(Guid userId, Guid activityId);

        IEnumerable<SentActivity> GetTodayParcel(Guid userId);
        IEnumerable<SentActivity> GetTodayParcel();
    }

    public class DailyParcelRepositoryAsync : RepositoryAsync<SentActivity>, IDailyParcelRepository
    {
        public DailyParcelRepositoryAsync(DataContext context) : base(context) {}

        public Task Create(Guid userId, Guid activityId)
        {
            return AddAsync(new SentActivity(userId, activityId));
        }

        public IEnumerable<SentActivity> GetTodayParcel(Guid userId) => GetTodayParcel().Where(e => e.TelegramUserId == userId);

        public IEnumerable<SentActivity> GetTodayParcel()
        {
            var currentDate = DateTime.Now.ToMoscowTime();

            return GetWithInclude(e => e.SentDate.CompareTo(currentDate) == 0);
        }
    }
}
