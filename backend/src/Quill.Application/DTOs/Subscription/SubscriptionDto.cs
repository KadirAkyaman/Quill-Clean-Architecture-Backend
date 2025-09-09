using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Quill.Application.DTOs.User;

namespace Quill.Application.DTOs.Subscription
{
    public class SubscriptionDto
    {
        public int Id { get; set; }
        public UserSummaryDto Subscriber { get; set; }
        public UserSummaryDto SubscribedTo { get; set; }
        public DateTime SubscriptionDate { get; set; }
    }
}