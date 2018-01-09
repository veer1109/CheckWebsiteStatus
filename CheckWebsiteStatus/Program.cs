using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using System.Net.Mail;
using System.IO;

namespace CheckWebsiteStatus
{
    class Program
    {

       // private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public static ArrayList chkurl = new ArrayList();
        public static ArrayList WebStatus = new ArrayList();
       public static List<string> WebURL = new List<string>();
        public static int ErrorStatus;
    
        static void Main(string[] args)
        {
            WebURL = Convert.ToString(ConfigurationManager.AppSettings["weburl"]).Split(',').ToList();

           // Console.Write("The list element" + WebURL.Count);
            foreach (string url in WebURL)
            {
                string status = CheckStatus(url);

                WebStatus.Add(status);
               // Console.WriteLine(status);
            }

                using (StreamWriter w = File.AppendText("log.txt"))
                {
                    w.Write("\r\nLog Entry : ");
                    w.WriteLine("{0} {1}", DateTime.Now.ToLongTimeString(),DateTime.Now.ToLongDateString());
                    w.WriteLine("------------------------------------------");
                    w.WriteLine("Website                             Status");
                    w.WriteLine("------------------------------------------");
                for (int i = 0; i < WebURL.Count; i++)
                    {
                    Log(WebURL[i] + "    " + WebStatus[i], w);
                    Console.WriteLine("URL: " + WebURL[i] + "Status: " + WebStatus[i]);
                    }
                    w.WriteLine("------------------------------------------");   
                }
            string Status = Convert.ToString(ConfigurationManager.AppSettings["MailRequest"]);
            if (Status == "true" || ErrorStatus==1)
            {
                SendMail();
            }
            //Console.ReadLine();

        }

        public static string CheckStatus(string URL)
        {
            UriBuilder uri = new UriBuilder(URL);
            HttpWebResponse response;
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(uri.Uri);
            request.AllowAutoRedirect = false;
           ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            try
            {
                response = (HttpWebResponse)request.GetResponse();

                int i = (int)response.StatusCode;

                if (i == 301)
                {
                    Console.WriteLine(i);
                    return response.StatusCode.ToString();
                }
                else if (i == 200)
                {
                    Console.WriteLine(i);
                    return response.StatusCode.ToString();
                }
                else if (i == 404)
                {
                    Console.WriteLine(i);
                    return response.StatusCode.ToString();
                }
                else if (i == 400)
                {
                    Console.WriteLine(i);
                    return response.StatusCode.ToString();
                }
                else if (i == 401)
                {
                    Console.WriteLine(i);
                    return response.StatusCode.ToString();
                }
                else if (i == 403)
                {
                    Console.WriteLine(i);
                    return response.StatusCode.ToString();
                }
                else if (i == 405)
                {
                    Console.WriteLine(i);
                    return response.StatusCode.ToString();
                }
                else if (i == 500)
                {
                    Console.WriteLine(i);
                    return response.StatusCode.ToString();
                }



            }
            catch (System.Net.WebException ex)
            {
                  Console.WriteLine(ex.Message);
                    ErrorStatus = 1;
                  return ex.Message.ToString();
               
            }
            return "NoStatus";

        }

        public static string BuildMessage()
        {
                  
            StringBuilder myBuilder = new StringBuilder();
            var appsettings = System.Configuration.ConfigurationManager.AppSettings;
            StringBuilder Message = new StringBuilder();

            Message.AppendLine("<p style='font-family:verdana;font-size: x-small;'>" + ConfigurationManager.AppSettings["msg1"] + "</p>");
            Message.AppendLine("<p style='font-family:verdana;font-size: x-small;'>" + ConfigurationManager.AppSettings["msg2"] + "</p>");
          
            myBuilder.Append("<table border='1px' cellpadding='5' cellspacing='0' ");
            myBuilder.Append("style='border: solid 1px Black; font-size: x-small;font-family:verdana; '>");
            myBuilder.Append("<tr align='left' valign='top' >");
            myBuilder.Append("<td align='left' valign='top' style='border: solid 1px ; color:black;background-color:#C2E0FF;' >");
            myBuilder.Append("Website URL");
            myBuilder.Append("</td>");
            myBuilder.Append("<td align='left' valign='top' style='border: solid 1px ; color:black;background-color:#C2E0FF;'>");
            myBuilder.Append("Status");
           
           
            for (int i =0; i < WebURL.Count;i++)
            {
                
                    myBuilder.Append("</tr>");
                    myBuilder.Append("<td align='left' valign='top' style='border: solid 1px black;'>");
                    myBuilder.Append(WebURL[i].ToString());
                    myBuilder.Append("</td>");
                    myBuilder.Append("<td align='left' valign='top' style='border: solid 1px black;'>");
                    myBuilder.Append(WebStatus[i].ToString());
                    myBuilder.Append("</td>");
                   
                
            }


            myBuilder.Append("</tr>");
            myBuilder.Append("</table>");

            Message.AppendLine(myBuilder.ToString());

            return Message.ToString();
        
    }


        public static void SendMail()
        {

            MailMessage mail = new MailMessage();
            SmtpClient client = new SmtpClient();
            NetworkCredential crd = new NetworkCredential();
            client.Host = Convert.ToString(ConfigurationManager.AppSettings["emailhost"]);
            client.Port = Convert.ToInt32(ConfigurationManager.AppSettings["portNo"]);
            client.UseDefaultCredentials = false;
            client.Credentials = crd;
            client.EnableSsl = false;

            var appsettings = System.Configuration.ConfigurationManager.AppSettings;

            string Emailto = Convert.ToString(ConfigurationManager.AppSettings["to"]);
            string Emailfrom = Convert.ToString(ConfigurationManager.AppSettings["from"]);
            // string[] addresslist = new string[] { "preeti.kale@ash-software.com", "veer.patil@ash-software.com" ,"mukund.vaidya@ash-software.com"};
            string[] addresslist = new string[] { Emailto };

            foreach (var address in addresslist)
            {
                mail.To.Add(address);
            }
            MailAddress from = new MailAddress(Emailfrom);
            mail.Subject = Convert.ToString(ConfigurationManager.AppSettings["subject"]);
           
            mail.Body = BuildMessage();
            mail.IsBodyHtml = true;
            mail.From = from;
            client.Send(mail);
            // Console.ReadLine();
        }


        public static void Log(string logMessage, TextWriter w)
        {
            
            w.WriteLine("{0}", logMessage);
          
        }
    }
}
