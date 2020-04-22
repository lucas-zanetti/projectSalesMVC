﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SalesWebMvc.Services;

namespace SalesWebMvc.Controllers
{
    public class SalesRecordsController : Controller
    {
        private readonly SalesRecordService _salesRecordService;

        public SalesRecordsController(SalesRecordService salesRecordService)
        {
            _salesRecordService = salesRecordService;
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> SimpleSearchAsync(DateTime? minDate, DateTime? maxDate)
        {
            if (minDate.HasValue == false)
                minDate = new DateTime(DateTime.Now.Year, 1, 1);
            if (maxDate.HasValue == false)
                maxDate = DateTime.Now;
            ViewData["minDate"] = minDate.Value.ToString("yyyy-MM-dd");
            ViewData["maxDate"] = maxDate.Value.ToString("yyyy-MM-dd");
            var sales = await _salesRecordService.FindByDateAsync(minDate, maxDate);
            return View(sales);
        }

        public IActionResult GroupingSearch()
        {
            return View();
        }
    }
}