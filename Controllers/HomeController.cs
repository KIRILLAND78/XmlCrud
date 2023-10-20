using System.Diagnostics;
using System.Dynamic;
using System.Reflection.Metadata;
using System.Text.Json.Serialization;
using System.Xml;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using WebApplication2.Models;
using WebApplication2.Models.Constants;
using System.IO;
using System.Xml.Serialization;

namespace WebApplication2.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private IWebHostEnvironment Environment;
    
    public HomeController(ILogger<HomeController> logger, IWebHostEnvironment environment)
    {
        _logger = logger;
        Environment = environment;
    }
    public IActionResult Index()
    {
        return View();
    }


    
}