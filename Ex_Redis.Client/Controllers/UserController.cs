﻿using Microsoft.AspNetCore.Mvc;

namespace Ex_Redis.Client.Controllers
{
	public class UserController : Controller
	{
		public IActionResult Index()
		{
			return View();
		}
	}
}
