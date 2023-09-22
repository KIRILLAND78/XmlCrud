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
    public XmlDocument getXML()
    {
        var XML = Constants.XML;
        var PATH = Constants.PATH;
        XmlDocument document = new XmlDocument();
        document.Load(string.Concat(this.Environment.ContentRootPath, PATH, XML));
        return document; 
    }
    
    [HttpGet("api/Reports")]
    public IActionResult Reports()
    {
        string xmlFilePath = this.Environment.ContentRootPath + Constants.PATH + Constants.XML;
        string xmlData = System.IO.File.ReadAllText(xmlFilePath);
        
        XmlSerializer serializer = new XmlSerializer(typeof(Reports));
        
        using (StringReader reader = new StringReader(xmlData))
        {
            Reports reports = (Reports)serializer.Deserialize(reader);

            // Преобразуйте объекты в JSON
            string jsonData = JsonConvert.SerializeObject(reports, Newtonsoft.Json.Formatting.Indented);

            // Теперь у вас есть JSON данные, которые вы можете передать
            return Ok(jsonData);
        }
    }


    [Route("/Home/DeleteNode/{ReportId:int}")]
    public async Task<IActionResult> DeleteNode([FromRoute] int ReportId, RequestModel requestModel)
    {
        var XPATH = Constants.XPATH;
        var document = getXML();
        foreach (XmlNode node in document.SelectNodes(XPATH))
        {
            if (ReportId == int.Parse(node["ReportId"].InnerText))
            {
                XmlNode parent = node.ParentNode;
                parent.RemoveChild(node);
                document.Save(string.Concat(this.Environment.ContentRootPath, "/Services", "/RQList.xml"));
                break;
            }
        }
        return NoContent();
    }


    [Route("Home/EditNode/{ReportId:int}")]
    public IActionResult EditNode([FromRoute] int ReportId)
    {
        var XPATH = Constants.XPATH;
        var document = getXML();
        
        foreach (XmlNode node in document.SelectNodes(XPATH))
        {
            if (ReportId == int.Parse(node["ReportId"].InnerText))
            {
                var editNode = new RequestModel
                {
                    Server = node["Server"]?.InnerText,
                    DataBase = node["DataBase"]?.InnerText,
                    UserName = node["UserName"]?.InnerText,
                    UserPassword = node["UserPassword"]?.InnerText,
                    DataSourceType = int.TryParse(node["DataSourceType"]?.InnerText, out int DataSourceType) ? DataSourceType : null,
                    FileQueryColName = node["FileQueryColName"]?.InnerText,
                    FileQueryOutputColName = node["FileQueryOutputColName"]?.InnerText,
                    FileQueryTempPath = node["FileQueryTempPath"]?.InnerText,
                    UsePivotData = bool.TryParse(node["UsePivotData"]?.InnerText, out bool usePivotData) ? usePivotData : null,
                    PivotCol = node["PivotCol"]?.InnerText,
                    PivotCols = node["PivotCols"]?.InnerText,
                    PivotRows = node["PivotRows"]?.InnerText,
                    PivotData = node["PivotData"]?.InnerText,
                    DefaultComplex = node["DefaultComplex"]?.InnerText, 
                };
                return View(editNode);
            }

        }

        return View();
    }
    
    [Route("Home/EditNode/{ReportId:int}")]
    [HttpPost]
    public IActionResult EditNode(RequestModel updatedModel)
    {
        if (ModelState.IsValid)
        {
            var XPATH = Constants.XPATH;
            var document = getXML();
            foreach (XmlNode node in document.SelectNodes(XPATH))
            {
                if (updatedModel.ReportId == int.Parse(node["ReportId"].InnerText))
                {
                    node["Server"].InnerText = updatedModel.Server;
                    document.Save(string.Concat(this.Environment.ContentRootPath, "/Services", "/RQList.xml"));
                }
            }

        }
        return View(updatedModel);
    }
    
    
    public IActionResult Index()
    {
        return View();
    }
    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
    
}