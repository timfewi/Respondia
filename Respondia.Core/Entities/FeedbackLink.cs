using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Respondia.Core.Entities
{
    public class FeedbackLink : BaseEntity
    {
        public int FeedbackFormId { get; set; }
        public FeedbackForm FeedbackForm { get; set; } = default!;
        public string Token { get; set; } = Guid.NewGuid().ToString();
        public DateTime ExpiryDate { get; set; }
        public bool IsActive { get; set; } = true;


    }
}
