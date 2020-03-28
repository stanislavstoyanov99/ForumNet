﻿namespace ForumNet.Services.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using AutoMapper;
    using FluentAssertions;
    using Microsoft.EntityFrameworkCore;
    using Moq;
    using Xunit;

    using Data;
    using Data.Models;
    using Services.Contracts;

    public class CategoriesServiceTests
    {
        [Fact]
        public async Task CreateMethodShouldAddOnlyOneCategoryInDatabase()
        {
            var options = new DbContextOptionsBuilder<ForumDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            var db = new ForumDbContext(options);
            var dateTimeProvider = new Mock<IDateTimeProvider>();
            dateTimeProvider.Setup(dtp => dtp.Now()).Returns(new DateTime(2020, 3, 27));

            var categoriesService = new CategoriesService(db, null, dateTimeProvider.Object);
            await categoriesService.CreateAsync("Test");

            Assert.Equal(1, await db.Categories.CountAsync());
        }

        [Fact]
        public async Task CreateMethodShouldAddRightCategoryInDatabase()
        {
            var options = new DbContextOptionsBuilder<ForumDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            var db = new ForumDbContext(options);
            var dateTimeProvider = new Mock<IDateTimeProvider>();
            dateTimeProvider.Setup(dtp => dtp.Now()).Returns(new DateTime(2020, 3, 27));

            var categoriesService = new CategoriesService(db, null, dateTimeProvider.Object);
            await categoriesService.CreateAsync("Test");

            var expected = new Category
            {
                Id = 1,
                Name = "Test",
                CreatedOn = dateTimeProvider.Object.Now(),
                IsDeleted = false
            };

            var actual = await db.Categories.FirstOrDefaultAsync();

            expected.Should().BeEquivalentTo(actual);
        }

        [Theory]
        [InlineData("Test 1", "Edit 1")]
        [InlineData("Test 2", "Edit 2")]
        [InlineData("Test 3", "Edit 3")]
        public async Task EditMethodShouldChangeCategoryNameAndModifiedOn(string creationName, string editedName)
        {
            var options = new DbContextOptionsBuilder<ForumDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            var db = new ForumDbContext(options);
            var dateTimeProvider = new Mock<IDateTimeProvider>();
            dateTimeProvider.Setup(dtp => dtp.Now()).Returns(new DateTime(2020, 3, 27));

            await db.Categories.AddAsync(new Category
            {
                Name = creationName,
                CreatedOn = dateTimeProvider.Object.Now()
            });
            await db.SaveChangesAsync();

            var categoriesService = new CategoriesService(db, null, dateTimeProvider.Object);
            await categoriesService.EditAsync(1, editedName);

            var expected = new Category
            {
                Name = editedName,
                ModifiedOn = dateTimeProvider.Object.Now()
            };

            var actual = await db.Categories.FirstOrDefaultAsync();

            Assert.Equal(expected.Name, actual.Name);
            Assert.Equal(expected.ModifiedOn, actual.ModifiedOn);
        }

        [Fact]
        public async Task DeleteMethodShouldChangeIsDeletedAndDeletedOn()
        {
            var options = new DbContextOptionsBuilder<ForumDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            var db = new ForumDbContext(options);
            var dateTimeProvider = new Mock<IDateTimeProvider>();
            dateTimeProvider.Setup(dtp => dtp.Now()).Returns(new DateTime(2020, 3, 27));

            await db.Categories.AddAsync(new Category
            {
                Name = "Test",
                CreatedOn = dateTimeProvider.Object.Now()
            });
            await db.SaveChangesAsync();

            var categoriesService = new CategoriesService(db, null, dateTimeProvider.Object);
            await categoriesService.DeleteAsync(1);

            var expected = new Category
            {
                IsDeleted = true,
                DeletedOn = dateTimeProvider.Object.Now()
            };

            var actual = await db.Categories.FirstOrDefaultAsync();

            Assert.Equal(expected.IsDeleted, actual.IsDeleted);
            Assert.Equal(expected.DeletedOn, actual.DeletedOn);
        }

        [Fact]
        public async Task IsExistingMethodWithIdParameterShouldReturnTrueIfExists()
        {
            var options = new DbContextOptionsBuilder<ForumDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            var db = new ForumDbContext(options);
            var dateTimeProvider = new Mock<IDateTimeProvider>();
            dateTimeProvider.Setup(dtp => dtp.Now()).Returns(new DateTime(2020, 3, 27));

            await db.Categories.AddAsync(new Category
            {
                Name = "Test",
                CreatedOn = dateTimeProvider.Object.Now()
            });
            await db.SaveChangesAsync();

            var categoriesService = new CategoriesService(db, null, dateTimeProvider.Object);
            var isExisting = await categoriesService.IsExisting(1);

            Assert.True(isExisting);
        }

        [Fact]
        public async Task IsExistingMethodWithIdParameterShouldReturnFalseIfNotExists()
        {
            var options = new DbContextOptionsBuilder<ForumDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            var db = new ForumDbContext(options);
            var dateTimeProvider = new Mock<IDateTimeProvider>();
            dateTimeProvider.Setup(dtp => dtp.Now()).Returns(new DateTime(2020, 3, 27));

            var categoriesService = new CategoriesService(db, null, dateTimeProvider.Object);
            var isExisting = await categoriesService.IsExisting(1);

            Assert.False(isExisting);
        }

        [Theory]
        [InlineData("Test 1")]
        [InlineData("Test 2")]
        [InlineData("Test 3")]
        public async Task IsExistingMethodWithNameParameterShouldReturnTrueIfExists(string name)
        {
            var options = new DbContextOptionsBuilder<ForumDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            var db = new ForumDbContext(options);
            var dateTimeProvider = new Mock<IDateTimeProvider>();
            dateTimeProvider.Setup(dtp => dtp.Now()).Returns(new DateTime(2020, 3, 27));

            await db.Categories.AddAsync(new Category
            {
                Name = name,
                CreatedOn = dateTimeProvider.Object.Now()
            });
            await db.SaveChangesAsync();

            var categoriesService = new CategoriesService(db, null, dateTimeProvider.Object);
            var isExisting = await categoriesService.IsExisting(name);

            Assert.True(isExisting);
        }

        [Theory]
        [InlineData("Test 1")]
        [InlineData("Test 2")]
        [InlineData("Test 3")]
        public async Task IsExistingMethodWithNameParameterShouldReturnFalseIfNotExists(string name)
        {
            var options = new DbContextOptionsBuilder<ForumDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            var db = new ForumDbContext(options);
            var dateTimeProvider = new Mock<IDateTimeProvider>();
            dateTimeProvider.Setup(dtp => dtp.Now()).Returns(new DateTime(2020, 3, 27));

            var categoriesService = new CategoriesService(db, null, dateTimeProvider.Object);
            var isExisting = await categoriesService.IsExisting(name);

            Assert.False(isExisting);
        }

        [Fact]
        public async Task GetByIdMethodShouldReturnCorrectModel()
        {
            var options = new DbContextOptionsBuilder<ForumDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            var db = new ForumDbContext(options);

            var config = new MapperConfiguration(options =>
            {
                options.CreateMap<Category, Category>();
            });

            var mapper = config.CreateMapper();

            var dateTimeProvider = new Mock<IDateTimeProvider>();
            dateTimeProvider.Setup(dtp => dtp.Now()).Returns(new DateTime(2020, 3, 27));

            await db.Categories.AddAsync(new Category
            {
                Name = "Test",
                CreatedOn = dateTimeProvider.Object.Now()
            });
            await db.SaveChangesAsync();

            var categoriesService = new CategoriesService(db, mapper, dateTimeProvider.Object);
            var expected = await categoriesService.GetByIdAsync<Category>(1);
            var actual = await db.Categories.FirstOrDefaultAsync();

            Assert.Equal(expected.Id, actual.Id);
            Assert.Equal(expected.Name, actual.Name);
            Assert.Equal(expected.CreatedOn, actual.CreatedOn);
        }

        [Fact]
        public async Task GetByIdMethodShouldReturnModelWhichIsNotDeleted()
        {
            var options = new DbContextOptionsBuilder<ForumDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            var db = new ForumDbContext(options);

            var config = new MapperConfiguration(options =>
            {
                options.CreateMap<Category, Category>();
            });

            var mapper = config.CreateMapper();

            var dateTimeProvider = new Mock<IDateTimeProvider>();
            dateTimeProvider.Setup(dtp => dtp.Now()).Returns(new DateTime(2020, 3, 27));

            await db.Categories.AddAsync(new Category
            {
                Name = "Test",
                CreatedOn = dateTimeProvider.Object.Now()
            });
            await db.SaveChangesAsync();

            var categoriesService = new CategoriesService(db, mapper, dateTimeProvider.Object);
            var expected = await categoriesService.GetByIdAsync<Category>(1);
            var actual = await db.Categories.FirstOrDefaultAsync();

            Assert.Equal(expected.IsDeleted, actual.IsDeleted);
        }

        [Fact]
        public async Task GetByIdMethodShouldReturnNullWhenCategoryIsDeleted()
        {
            var options = new DbContextOptionsBuilder<ForumDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            var db = new ForumDbContext(options);

            var config = new MapperConfiguration(options =>
            {
                options.CreateMap<Category, Category>();
            });

            var mapper = config.CreateMapper();

            var dateTimeProvider = new Mock<IDateTimeProvider>();
            dateTimeProvider.Setup(dtp => dtp.Now()).Returns(new DateTime(2020, 3, 27));

            await db.Categories.AddAsync(new Category
            {
                Name = "Test",
                CreatedOn = dateTimeProvider.Object.Now(),
                IsDeleted = true,
                DeletedOn = dateTimeProvider.Object.Now()
            });
            await db.SaveChangesAsync();

            var categoriesService = new CategoriesService(db, mapper, dateTimeProvider.Object);
            var category = await categoriesService.GetByIdAsync<Category>(1);

            Assert.Null(category);
        }

        [Fact]
        public async Task GetByIdMethodShouldReturnNullWhenCategoryIsNotFound()
        {
            var options = new DbContextOptionsBuilder<ForumDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            var db = new ForumDbContext(options);

            var config = new MapperConfiguration(options =>
            {
                options.CreateMap<Category, Category>();
            });

            var mapper = config.CreateMapper();

            var dateTimeProvider = new Mock<IDateTimeProvider>();
            dateTimeProvider.Setup(dtp => dtp.Now()).Returns(new DateTime(2020, 3, 27));

            var categoriesService = new CategoriesService(db, mapper, dateTimeProvider.Object);
            var category = await categoriesService.GetByIdAsync<Category>(1);

            Assert.Null(category);
        }

        [Fact]
        public async Task GetAllMethodShouldReturnCorrectModel()
        {
            var options = new DbContextOptionsBuilder<ForumDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            var db = new ForumDbContext(options);

            var config = new MapperConfiguration(options =>
            {
                options.CreateMap<Category, Category>();
            });

            var mapper = config.CreateMapper();

            var dateTimeProvider = new Mock<IDateTimeProvider>();
            dateTimeProvider.Setup(dtp => dtp.Now()).Returns(new DateTime(2020, 3, 27));

            var expectedCategories = new List<Category>();

            for (int i = 0; i < 10; i++)
            {
                expectedCategories.Add(new Category
                {
                    Name = $"Test {i}",
                    CreatedOn = dateTimeProvider.Object.Now()
                });
            }

            await db.Categories.AddRangeAsync(expectedCategories);
            await db.SaveChangesAsync();

            var categoriesService = new CategoriesService(db, mapper, dateTimeProvider.Object);
            var actualCategories = await categoriesService.GetAllAsync<Category>();

            expectedCategories.Should().BeEquivalentTo(actualCategories);
        }

        [Fact]
        public async Task GetAllMethodShouldReturnZeroItemsIfThereAreNotAnyCategories()
        {
            var options = new DbContextOptionsBuilder<ForumDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            var db = new ForumDbContext(options);

            var config = new MapperConfiguration(options =>
            {
                options.CreateMap<Category, Category>();
            });

            var mapper = config.CreateMapper();

            var dateTimeProvider = new Mock<IDateTimeProvider>();
            dateTimeProvider.Setup(dtp => dtp.Now()).Returns(new DateTime(2020, 3, 27));

            var categoriesService = new CategoriesService(db, mapper, dateTimeProvider.Object);
            var categories = await categoriesService.GetAllAsync<Category>();

            Assert.Empty(categories);
        }
    }
}
