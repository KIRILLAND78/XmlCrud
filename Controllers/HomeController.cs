using System.Diagnostics;
using System.Reflection.Metadata;
using System.Xml;
using Microsoft.AspNetCore.Mvc;
using WebApplication2.Models;

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
        var XML = "/RQList.xml";
        var PATH = "/Services";
        XmlDocument document = new XmlDocument();
        document.Load(string.Concat(this.Environment.ContentRootPath, PATH, XML));
        return document;
    }
    
    [Route("Reports")]
    public IActionResult Reports()
    {
        List<RequestModel> requests = new List<RequestModel>();
        var XPATH = "/Reports/Data";
        var document = getXML();
        
        foreach (XmlNode node in document.SelectNodes(XPATH))
        {
            requests.Add(new RequestModel
            {
                ReportId = int.Parse(node["ReportId"]?.InnerText),
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
            });
        }
        return View(requests);
    }


    [Route("/Home/DeleteNode/{ReportId:int}")]
    public async Task<IActionResult> DeleteNode([FromRoute] int ReportId, RequestModel requestModel)
    {
        
        var XPATH = "/Reports/Data";
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
        var XPATH = "/Reports/Data";
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

    /*[HttpPost]
    public IActionResult EditNode(RequestModel updatedModel)
    {
        if (ModelState.IsValid)
        {
            var XPATH = "/Reports/Data";
            var document = getXML();
            foreach (XmlNode node in document.SelectNodes(XPATH))
            {
                if (updatedModel.ReportId == int.Parse(node["ReportId"].InnerText))
                {
                    XmlNode parent = node.ParentNode;
                    document.Save(string.Concat(this.Environment.ContentRootPath, "/Services", "/RQList.xml"));
                }
            }

        }
    }*/
    
    
    
    
    
    
    
    
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