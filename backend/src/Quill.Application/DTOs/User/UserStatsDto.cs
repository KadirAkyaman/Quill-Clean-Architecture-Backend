using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Quill.Application.DTOs.User
{
    public class UserStatsDto // post count, subscriber count, subscribed count
    {
        public int PostsCount { get; set; }
        public int SubscriberCount { get; set; }
        public int SubscriptionsCount  { get; set; }
    }
}