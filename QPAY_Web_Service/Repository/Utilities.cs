using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;

namespace QPay.BAL.Repository
{

    public class GenericSerializer<T>
    {
        public static T Deserialize(string XML)
        {
            try
            {
                XmlSerializer ser = new XmlSerializer(typeof(T));
                StringReader sr = new StringReader(XML);
                return (T)ser.Deserialize(sr);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static string Serialize(T obj)
        {
            try
            {
                string xmlString = null;
                MemoryStream memoryStream = new MemoryStream();
                XmlSerializer xs = new XmlSerializer(typeof(T));

                XmlWriterSettings settings = new XmlWriterSettings();
                settings.Indent = true;
                settings.Encoding = new UTF8Encoding(false, false);
                settings.OmitXmlDeclaration = true;
                settings.ConformanceLevel = ConformanceLevel.Fragment;

                XmlSerializerNamespaces ns = new XmlSerializerNamespaces();
                ns.Add("", "");

                XmlTextWriter xmlTextWriter = new XmlTextWriter(memoryStream, Encoding.UTF8);
                xs.Serialize(xmlTextWriter, obj, ns);
                memoryStream = (MemoryStream)xmlTextWriter.BaseStream;
                xmlString = UTF8ByteArrayToString(memoryStream.ToArray());

                xmlString = xmlString.Replace("﻿<?xml version=\"1.0\" encoding=\"utf-8\"?>", "");

                return xmlString;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private static string UTF8ByteArrayToString(byte[] characters)
        {
            UTF8Encoding encoding = new UTF8Encoding();
            string constructedString = encoding.GetString(characters);
            return constructedString;
        }
    }
}

