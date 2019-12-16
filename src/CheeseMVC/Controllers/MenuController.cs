using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using CheeseMVC.Data;
using CheeseMVC.Models;
using CheeseMVC.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace CheeseMVC.Controllers
{
    public class MenuController : Controller
    {
        private CheeseDbContext context;

        public MenuController(CheeseDbContext dbContext)
        {
            context = dbContext;
        }

        public IActionResult Index()
        {
            IList<Menu> menus = context.Menus.ToList();
            return View(menus);
        }

        public IActionResult Add()
        {
            AddMenuViewModel menu = new AddMenuViewModel();
            return View(menu);
        }

        [HttpPost]
        public IActionResult Add(AddMenuViewModel addMenuViewModel)
        {
            if (ModelState.IsValid)
            {
                Menu newMenu = new Menu
                {
                    Name = addMenuViewModel.Name
                };
                context.Menus.Add(newMenu);
                context.SaveChanges();
                return Redirect("/Menu/ViewMenu/" + newMenu.ID);
            }

            return View(addMenuViewModel);
        }

        public IActionResult ViewMenu(int id)
        {
            Menu menu = context.Menus.Single(m => m.ID == id);

            List<CheeseMenu> items = context
                .CheeseMenus
                .Include(item => item.Cheese)
                .Include(item => item.Cheese.Category)
                .Where(cm => cm.MenuID == id)
                .ToList();

            ViewMenuViewModel viewMenuViewModel = new ViewMenuViewModel
            {
                Menu = menu,
                Items = items
            };

            return View(viewMenuViewModel);
        }

        public IActionResult AddItem(int id)
        {
            Menu menu = context.Menus.Single(m => m.ID == id);
            IList<Cheese> cheeses = context.Cheeses.ToList();

            AddMenuItemViewModel addMenuItemViewModel = new AddMenuItemViewModel(menu, cheeses);

            return View(addMenuItemViewModel);
        }

        [HttpPost]
        public IActionResult AddItem(int CheeseID, int MenuID)
        {
            IList<CheeseMenu> existingItems = context.CheeseMenus
                    .Where(cm => cm.CheeseID == CheeseID)
                    .Where(cm => cm.MenuID == MenuID).ToList();

            if (existingItems.Count == 0)
            {
                Menu newMenu = context.Menus.Single(m => m.ID == MenuID);
                Cheese newCheese = context.Cheeses.Single(c => c.ID == CheeseID);
                

                CheeseMenu newCheeseMenu = new CheeseMenu
                {
                    MenuID = MenuID,
                    Menu = newMenu,
                    CheeseID = CheeseID,
                    Cheese = newCheese
                  
                };

                context.CheeseMenus.Add(newCheeseMenu);
                context.SaveChanges();

                return Redirect("/Menu/ViewMenu/" + MenuID); ;
            }
            Menu menu = context.Menus.Single(m => m.ID == MenuID);
            IList<Cheese> cheeses = context.Cheeses.ToList();
            AddMenuItemViewModel addMenuItemViewModel = new AddMenuItemViewModel(menu, cheeses);
            return View(addMenuItemViewModel);
        }
    }
}