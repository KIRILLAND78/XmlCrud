using System.Reflection;
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
        System.Environment.GetEnvironmentVariable("kdjsfjngkljsdfngjklsd");
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
        XmlDocument document = new XmlDocument();
        document.Load(string.Concat(this.Environment.ContentRootPath, Constants.PATH, Constants.XML));
        
        foreach (XmlNode node in document.SelectNodes(Constants.PARENT_NODE))
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
    
    [Route("api/EditNode")]
    [HttpPut]
    public IActionResult EditNode([FromBody] RequestModel updatedModel)
    {
        if (ModelState.IsValid)
        {
            XmlDocument document = new XmlDocument();
            document.Load(string.Concat(this.Environment.ContentRootPath, Constants.PATH, Constants.XML));
            foreach (XmlNode node in document.SelectNodes(Constants.PARENT_NODE))
            {
                if (updatedModel.ReportId == int.Parse(node["ReportId"].InnerText))
                {
                    foreach (PropertyInfo prop in typeof(RequestModel).GetProperties())
                    {
                        string propName = prop.Name;
                        object propValue = prop.GetValue(updatedModel);
                        
                        if (propValue != null)
                        {
                            string nodeValue = Convert.ToString(propValue);

                            // Проверяем, что узел с таким именем существует
                            XmlNode targetNode = node[propName];
                            if (targetNode == null)
                            {
                                // Если узла нет, создаем его и добавляем значение
                                targetNode = document.CreateElement(propName);
                                node.AppendChild(targetNode);
                            }

                            // Устанавливаем значение узла
                            targetNode.InnerText = nodeValue;
                        }
                        
                        document.Save(string.Concat(this.Environment.ContentRootPath, "/Services", "/RQList.xml"));

                    }
                   }
            }
            return Ok(updatedModel);
        }
        else
        {
            return BadRequest(ModelState);
        }
    }

}