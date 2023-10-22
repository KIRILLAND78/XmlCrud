using System.Xml;
using System.Xml.Serialization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using WebApplication2.Models;
using WebApplication2.Models.Constants;
using Formatting = Newtonsoft.Json.Formatting;

namespace WebApplication2.Controllers;
[Route("xml")]
public class XmlController : Controller
{
    private IWebHostEnvironment Environment;
    public string Document;
    
    public XmlController(IWebHostEnvironment environment)
    {
        Environment = environment;
        Document = getXML();
    }
    public string getXML()
    {
        string xmlFilePath = this.Environment.ContentRootPath + Constants.PATH + Constants.XML;
        string xmlData = System.IO.File.ReadAllText(xmlFilePath);
        return xmlData;
    }
    
    [HttpGet("api/Reports")]
    public IActionResult Reports()
    {
        XmlSerializer serializer = new XmlSerializer(typeof(Reports));
        
        using (StringReader reader = new StringReader(Document))
        {   
            Reports reports = (Reports)serializer.Deserialize(reader);
            string jsonData = JsonConvert.SerializeObject(reports, Formatting.Indented);
            return Ok(jsonData);
        }
    }


    [Route("DeleteNode/{ReportId:int}")]
    public async Task<IActionResult> DeleteNode([FromRoute] int ReportId, RequestModel requestModel)
    {
        var XML = Constants.XML;
        var PATH = Constants.PATH;
        XmlDocument document = new XmlDocument();
        document.Load(string.Concat(this.Environment.ContentRootPath, PATH, XML));
        var XPATH = Constants.XPATH;
        
        foreach (XmlNode node in document.SelectNodes(XPATH))
        {
            if (ReportId == int.Parse(node["ReportId"].InnerText))
            {
                XmlNode parent = node.ParentNode;
                parent.RemoveChild(node);
                document.Save(string.Concat(this.Environment.ContentRootPath, "/Services", "/RQList.xml"));
                return Ok();
                
            }
        }
        return NoContent();
    }
    //
    //
    // [Route("EditNode/{ReportId:int}")]
    // public IActionResult EditNode([FromRoute] int ReportId)
    // {
    //     var XPATH = Constants.XPATH;
    //     var document = getXML();
    //     
    //     foreach (XmlNode node in document.SelectNodes(XPATH))
    //     {
    //         if (ReportId == int.Parse(node["ReportId"].InnerText))
    //         {
    //             var editNode = new RequestModel
    //             {
    //                 Server = node["Server"]?.InnerText,
    //                 DataBase = node["DataBase"]?.InnerText,
    //                 UserName = node["UserName"]?.InnerText,
    //                 UserPassword = node["UserPassword"]?.InnerText,
    //                 DataSourceType = int.TryParse(node["DataSourceType"]?.InnerText, out int DataSourceType) ? DataSourceType : null,
    //                 FileQueryColName = node["FileQueryColName"]?.InnerText,
    //                 FileQueryOutputColName = node["FileQueryOutputColName"]?.InnerText,
    //                 FileQueryTempPath = node["FileQueryTempPath"]?.InnerText,
    //                 UsePivotData = bool.TryParse(node["UsePivotData"]?.InnerText, out bool usePivotData) ? usePivotData : null,
    //                 PivotCol = node["PivotCol"]?.InnerText,
    //                 PivotCols = node["PivotCols"]?.InnerText,
    //                 PivotRows = node["PivotRows"]?.InnerText,
    //                 PivotData = node["PivotData"]?.InnerText,
    //                 DefaultComplex = node["DefaultComplex"]?.InnerText, 
    //             };
    //             return View(editNode);
    //         }
    //
    //     }
    //
    //     return View();
    // }
    //
    // [Route("EditNode/{ReportId:int}")]
    // [HttpPost]
    // public IActionResult EditNode(RequestModel updatedModel)
    // {
    //     if (ModelState.IsValid)
    //     {
    //         var XPATH = Constants.XPATH;
    //         var document = getXML();
    //         foreach (XmlNode node in document.SelectNodes(XPATH))
    //         {
    //             if (updatedModel.ReportId == int.Parse(node["ReportId"].InnerText))
    //             {
    //                 node["Server"].InnerText = updatedModel.Server;
    //                 document.Save(string.Concat(this.Environment.ContentRootPath, "/Services", "/RQList.xml"));
    //             }
    //         }
    //
    //     }
    //     return View(updatedModel);
    // }
}