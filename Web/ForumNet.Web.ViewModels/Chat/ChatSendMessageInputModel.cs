﻿namespace ForumNet.Web.ViewModels.Chat
{
    using System.ComponentModel.DataAnnotations;

    using Common;

    public class ChatSendMessageInputModel
    {
        [Required]
        [MaxLength(GlobalConstants.MessageContentMaxLength)]
        public string Content { get; set; }

        [Required]
        public string ReceiverId { get; set; }
    }
}
