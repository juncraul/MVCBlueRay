using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MVCBlueRay.Models
{
    public class ReCaptcha
    {
        private string m_success;
        [JsonProperty("success")]
        public string Success
        {
            get { return m_success; }
            set { m_success = value; }
        }

        private List<string> m_errorCodes;
        [JsonProperty("error-codes")]
        public List<string> ErrorCodes
        {
            get { return m_errorCodes; }
            set { m_errorCodes = value; }
        }

        public static string Validate(string encodedResponse)
        {
            var client = new System.Net.WebClient();
            string privateKey = "6LehohoUAAAAAA3jvUBF1UeIsYtmN2Y0doCxixcF";
            var reply = client.DownloadString(string.Format("https://www.google.com/recaptcha/api/siteverify?secret={0}&response={1}", privateKey, encodedResponse));
            var captchaResponse = JsonConvert.DeserializeObject<ReCaptcha>(reply);
            return captchaResponse.Success;
        }
    }
}