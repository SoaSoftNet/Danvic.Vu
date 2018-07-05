//-----------------------------------------------------------------------
// <copyright file= "SwaggerDocDescription.cs">
//     Copyright (c) Danvic712. All rights reserved.
// </copyright>
// Author: Danvic712
// Date Created: 2018-07-05 18:00:02
// Modified by:
// Description: Add additional Swagger UI Comments
//-----------------------------------------------------------------------
using Swashbuckle.AspNetCore.Swagger;
using Swashbuckle.AspNetCore.SwaggerGen;
using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;

namespace Danvic.Vu.Tools
{
    public class SwaggerDocDescription : IDocumentFilter
    {
        /// <summary>
        /// Add comments
        /// </summary>
        /// <param name="swaggerDoc"></param>
        /// <param name="context"></param>
        public void Apply(SwaggerDocument swaggerDoc, DocumentFilterContext context)
        {
            swaggerDoc.Tags = GetControllerDescription();
        }

        /// <summary>
        /// Get comments info from controller
        /// </summary>
        /// <returns></returns>
        private IList<Tag> GetControllerDescription()
        {
            IList<Tag> tagsList = new List<Tag>();

            //Check whether the xml document has existed
            //
            var basePath = Path.GetDirectoryName(AppContext.BaseDirectory);
            var xmlPath = Path.Combine(basePath, "Danvic.Vu.xml");
            if (!File.Exists(xmlPath))
            {
                return tagsList;
            }

            //Get xml document node information
            //
            XmlDocument xmlDocument = new XmlDocument();
            xmlDocument.Load(xmlPath);

            string memeberName = string.Empty;//thrid node's name
            string controllerName = string.Empty;//controller's name
            string keyName = string.Empty;//controller's name without controller
            string controllerComment = string.Empty;//comment of controller

            foreach (XmlNode item in xmlDocument.SelectNodes("//member"))
            {
                memeberName = item.Attributes["name"].Value;
                if (memeberName.StartsWith("T:"))
                {
                    string[] strArray = memeberName.Split('.');
                    controllerName = strArray[strArray.Length - 1];

                    if (controllerName.EndsWith("Controller"))
                    {
                        XmlNode commentNode = item.SelectSingleNode("summary");
                        keyName = controllerName.Remove(controllerName.Length - "Controller".Length, "Controller".Length);

                        if (commentNode != null && !string.IsNullOrEmpty(commentNode.InnerText) && !tagsList.Contains(new Tag { Name = keyName }))
                        {
                            controllerComment = commentNode.InnerText.Trim();

                            tagsList.Add(new Tag { Name = keyName, Description = controllerComment });
                        }
                    }

                }
            }

            return tagsList;
        }
    }
}
