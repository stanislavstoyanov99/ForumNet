﻿namespace ForumNet.Web.Areas.Administration.Controllers
{
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Mvc;

    using Services.Contracts;
    using ViewModels.Categories;

    public class CategoriesController : AdminController
    {
        private readonly ICategoriesService categoriesService;

        public CategoriesController(ICategoriesService categoriesService)
        {
            this.categoriesService = categoriesService;
        }

        public IActionResult Create()
        {
            return this.View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(CategoriesCreateInputModel input)
        {
            if (!this.ModelState.IsValid)
            {
                return this.View(input);
            }

            await this.categoriesService.CreateAsync(input.Name);

            return this.RedirectToAction("All", "Categories", new { area = string.Empty });
        }

        public async Task<IActionResult> Edit(int id)
        {
            var category = await this.categoriesService.GetByIdAsync<CategoriesEditInputModel>(id);
            if (category == null)
            {
                return this.NotFound();
            }

            return this.View(category);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(CategoriesEditInputModel input)
        {
            if (!this.ModelState.IsValid)
            {
                return this.View(input);
            }

            await this.categoriesService.EditAsync(input.Id, input.Name);

            return this.RedirectToAction("Details", "Categories", new { area = string.Empty });
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var isExisting = await this.categoriesService.IsExisting(id);
            if (!isExisting)
            {
                return this.NotFound();
            }

            await this.categoriesService.DeleteAsync(id);

            return this.RedirectToAction("All", "Categories", new { area = string.Empty });
        }
    }
}
