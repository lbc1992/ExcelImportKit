﻿/******************************************************************
** auth: wei.huazhong
** date: 9/17/2018 12:05:33 PM
** desc:
******************************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace ExcelService
{
    public class ErrorMessageHandler
    {
        private Dictionary<string, string> errorMessages;

        private ErrorMessageHandler()
        {
            errorMessages = new Dictionary<string, string>();
            string configPath = GetConfigPath();
            LoadConfig(configPath, errorMessages);
        }

        private String GetConfigPath()
        {
            Uri baseUri = new Uri(System.Reflection.Assembly.GetCallingAssembly().CodeBase);

            Uri sheetConfigUri = new Uri(baseUri, @"..\Configs\ErrorMessage.cfg.xml");

            return sheetConfigUri.LocalPath;
        }

        private void LoadConfig(string path, Dictionary<string, string> errorMessages)
        {
            var xdoc = XDocument.Load(path);
            var elements = xdoc.Root.Elements();
            foreach (var e in elements)
            {
                var errorCode = e.Attribute("code").Value;
                if (!errorMessages.ContainsKey(errorCode))
                {
                    errorMessages.Add(errorCode, e.Attribute("message").Value);
                }
            }
        }

        class Nested
        {
            static Nested() { }
            internal static readonly ErrorMessageHandler instance = new ErrorMessageHandler();
        }

        public static ErrorMessageHandler Instance
        {
            get
            {
                return Nested.instance;
            }
        }

        public string GetErrorMessage(string errorCode, params object[] formatParams)
        {
            if (errorMessages.ContainsKey(errorCode))
            {
                if (formatParams != null && formatParams.Length > 0)
                    return string.Format(errorMessages[errorCode], formatParams);
                else
                    return errorMessages[errorCode];
            }
            throw new ArgumentNullException(string.Format("couldn't find errorcode:{0}", errorCode));
        }
    }

}
