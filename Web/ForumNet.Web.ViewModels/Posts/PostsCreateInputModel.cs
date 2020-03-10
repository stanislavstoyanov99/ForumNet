﻿namespace ForumNet.Web.ViewModels.Posts
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using Categories;
    using Common;
    using Common.Attributes;
    using Data.Common;
    using Data.Models.Enums;
    using Tags;

    public class PostsCreateInputModel
    {
        [Required]
        [MaxLength(DataConstants.PostTitleMaxLength)]
        public string Title { get; set; }

        [Required]
        [EnumDataType(typeof(PostType))]
        [Display(Name = ModelConstants.PostTypeDisplayName)]
        public PostType PostType { get; set; }

        [DataType(DataType.Url)]
        [Display(Name = ModelConstants.ImageUrlDisplyName)]
        public string ImageUrl { get; set; }

        [YouTubeUrl]
        [Display(Name = ModelConstants.VideoUrlDisplyName)]
        [DataType(DataType.Url)]
        public string VideoUrl { get; set; }

        [Required]
        [MaxLength(DataConstants.PostDescriptionMaxLength)]
        [DataType(DataType.MultilineText)]
        public string Description { get; set; }

        [Required]
        [ValidateCategoryId]
        public int CategoryId { get; set; }

        [Required]
        [ValidateTagIds]
        [Display(Name = ModelConstants.TagsDisplyName)]
        public IEnumerable<int> TagIds { get; set; }

        public IEnumerable<CategoriesInfoViewModel> Categories { get; set; }

        public IEnumerable<TagsInfoViewModel> Tags { get; set; }
    }
}