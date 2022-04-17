using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using NetCoreRabbitMQ.Web.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Routing.Template;
using Microsoft.Extensions.Configuration;
using NetCoreRabbitMQ.Web.Services;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace NetCoreRabbitMQ.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly RabbitMQProducer _rabbitMQProducer;

        public HomeController(RabbitMQProducer rabbitMqProducer)
        {
            _rabbitMQProducer = rabbitMqProducer;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Index(Customer customer)
        {
            _rabbitMQProducer.Publish(customer);

            ViewBag.Info = "Sipariş oluşturuldu.";
            return View(customer);
        }

        public IActionResult Privacy()
        {
            return View();
        }
    }
}
