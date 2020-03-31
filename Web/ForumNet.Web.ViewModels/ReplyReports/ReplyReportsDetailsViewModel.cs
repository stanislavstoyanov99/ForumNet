﻿namespace ForumNet.Web.ViewModels.ReplyReports
{
    using Ganss.XSS;

    using Infrastructure;

    public class ReplyReportsDetailsViewModel
    {
        private readonly IHtmlSanitizer htmlSanitizer;

        public ReplyReportsDetailsViewModel()
        {
            this.htmlSanitizer = new HtmlSanitizer();
            this.htmlSanitizer.AllowedTags.Add(ModelConstants.IFrameAllowedTag);
        }

        public int Id { get; set; }

        public string Description { get; set; }

        public string SanitizedDescription
            => this.htmlSanitizer.Sanitize(this.Description);

        public string CreatedOn { get; set; }

        public string AuthorId { get; set; }

        public string AuthorUserName { get; set; }

        public string AuthorProfilePicture { get; set; }

        public int ReplyId { get; set; }

        public string ReplyDescription { get; set; }

        public string SanitizedReplyDescription
            => this.htmlSanitizer.Sanitize(this.ReplyDescription);
    }
}
