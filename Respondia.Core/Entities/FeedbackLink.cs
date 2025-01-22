using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Respondia.Core.Entities
{
    /// <summary>
    /// Represents a link associated with a feedback form, identified by a unique token.
    /// </summary>
    public class FeedbackLink : BaseEntity
    {
        /// <summary>
        /// Gets or sets the identifier of the associated feedback form.
        /// </summary>
        public int FeedbackFormId { get; set; }

        /// <summary>
        /// Gets or sets the associated feedback form.
        /// </summary>
        public FeedbackForm FeedbackForm { get; set; } = default!;

        /// <summary>
        /// Gets or sets the unique token for accessing the feedback form.
        /// </summary>
        public string Token { get; set; } = Guid.NewGuid().ToString();

        /// <summary>
        /// Gets or sets the expiry date of the feedback link.
        /// </summary>
        public DateTime ExpiryDate { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the feedback link is active.
        /// </summary>
        public bool IsActive { get; set; } = true;
    }
}
