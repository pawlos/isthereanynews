﻿using System.Web.Mvc;
using IsThereAnyNews.Services;

namespace IsThereAnyNews.Mvc.Controllers
{
    public class UsersController : Controller
    {
        private readonly IUsersService usersService;

        public UsersController(IUsersService usersService)
        {
            this.usersService = usersService;
        }

        public ActionResult Index()
        {
            var usersPublicProfileViewModel = this.usersService.LoadAllUsersPublicProfile();
            return this.View("Index", usersPublicProfileViewModel);
        }
    }
}