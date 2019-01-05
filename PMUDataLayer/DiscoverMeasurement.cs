using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Text;  // for class Encoding
using System.IO;    // for StreamReader
using System.Security.Cryptography;
using System.Xml.Linq;

namespace PMUDataLayer
{
    /**
     * https://social.msdn.microsoft.com/Forums/vstudio/en-US/b02d4373-4b25-48a3-b942-e56f2004a6c2/reading-http-basic-authentication-usernamepassword-from-service-implementation?forum=wcf
     * https://stackoverflow.com/questions/896901/wcf-adding-nonce-to-usernametoken
     * **/
    public class DiscoverMeasurement
    {
        public XDocument GetMeasTree(string username, string password)
        {
            string res = "";
            XDocument doc = new XDocument();
            string securityHeaderStr = GetSecurityHeader(username, password);
            string bodyStr = "<soap:Envelope xmlns:soap=\"http://www.w3.org/2003/05/soap-envelope\" xmlns:dat=\"http://www.eterra.com/public/services/data/dataTypes\">" +
     $"<soap:Header>{securityHeaderStr}</soap:Header>" +
      "<soap:Body>" +
          "<dat:DiscoverServerRequest>?</dat:DiscoverServerRequest>" +
        "</soap:Body>" +
      "</soap:Envelope>";
            var request = (HttpWebRequest)WebRequest.Create("https://172.16.183.131:24721/eterra-ws/HistoricalDataProvider");
            request.ServerCertificateValidationCallback += (sender, certificate, chain, sslPolicyErrors) => true;
            var data = Encoding.ASCII.GetBytes(bodyStr);
            request.Method = "POST";
            request.ContentType = "application/soap+xml; charset=\"utf-8\"";
            request.ContentLength = data.Length;

            using (var stream = request.GetRequestStream())
            {
                stream.Write(data, 0, data.Length);
            }

            var response = (HttpWebResponse)request.GetResponse();

            res = new StreamReader(response.GetResponseStream()).ReadToEnd();
            //Console.WriteLine(res);
            try {
                // extract xml from the 6th line in the response text
                res = res.Split('\n')[5];
                doc = XDocument.Parse(res);
            }
            catch (Exception e)
            {
                Console.WriteLine($"Error in xml parsing. {e.Message}");
            }
            return doc;
        }

        public string GetSecurityHeader(string userName, string password)
        {
            Random r = new Random();
            DateTime created = DateTime.Now;
            string createdStr = created.ToString("yyyy-MM-ddTHH:mm:ss.fff")+"+05:30";
            string nonce = Convert.ToBase64String(Encoding.ASCII.GetBytes(SHA1Encrypt(created + r.Next().ToString())));
            string securityHeaderStr = "<wsse:Security xmlns:wsse=\"http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-wssecurity-secext-1.0.xsd\" xmlns:wsu=\"http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-wssecurity-utility-1.0.xsd\">" +
        $"<wsse:UsernameToken wsu:Id = \"UsernameToken-329D41BF01D5F8E66114867472731424\">" +
        $"<wsse:Username>{userName}</wsse:Username>" +
        $"<wsse:Password Type = \"http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-username-token-profile-1.0#PasswordText\">{password}</wsse:Password>" +
        $"<wsse:Nonce EncodingType = \"http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-soap-message-security-1.0#Base64Binary\">{nonce}</wsse:Nonce>" +
        $"<wsu:Created>{createdStr}</wsu:Created>" +
        "</wsse:UsernameToken>" +
        "</wsse:Security>";
            return securityHeaderStr;
        }

        protected String ByteArrayToString(byte[] inputArray)
        {
            StringBuilder output = new StringBuilder("");
            for (int i = 0; i < inputArray.Length; i++)
            {
                output.Append(inputArray[i].ToString("X2"));
            }
            return output.ToString();
        }
        protected String SHA1Encrypt(String phrase)
        {
            UTF8Encoding encoder = new UTF8Encoding();
            SHA1CryptoServiceProvider sha1Hasher = new SHA1CryptoServiceProvider();
            byte[] hashedDataBytes = sha1Hasher.ComputeHash(encoder.GetBytes(phrase));
            return ByteArrayToString(hashedDataBytes);
        }
    }
}
