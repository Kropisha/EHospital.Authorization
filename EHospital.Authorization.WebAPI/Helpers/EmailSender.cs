using EHospital.Shared.HttpClientWrapper;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace EHospital.Authorization.BusinessLogic.EmailAction
{
    /// <summary>
    /// Class for verify by email
    /// </summary>
    public class EmailSender
    {
        private const string URI_EMAIL = "https://localhost:44384/api/SendingEmails";

        static readonly HttpClient Client = new HttpClient();
        private readonly IHttpClientWrapper _httpClientWrapper = new HttpClientWrapper(Client);

        /// <summary>
        /// Send email to user
        /// </summary>
        /// <param name="email">user's email</param>
        /// <param name="subject">topic of letter</param>
        /// <param name="message">link</param>
        public async Task SendEmail(string email, string topic, string text)
        {
            var body = new { email = email, topic = topic, text = text };
            var helper = await _httpClientWrapper.SendPostRequestAsync(URI_EMAIL, body);
        }

        public string GenerateKey()
        {
            DateTime _now = DateTime.Now;
            string _dd = _now.ToString("dd");
            string _mm = _now.ToString("MM");
            string _yy = _now.ToString("yyyy");
            string _hh = _now.Hour.ToString();
            string _min = _now.Minute.ToString();
            string _ss = _now.Second.ToString();

            string _uniqueId = _dd + _hh + _mm + _min + _ss + _yy;
            return _uniqueId;
        }
    }
}
