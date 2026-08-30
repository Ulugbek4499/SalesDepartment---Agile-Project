using System.Text;
using Newtonsoft.Json;

namespace SalesDepartment.MVC.Services
{
    public class SendSmsApiWithEskiz
    {
        private const string SUCCESS = "200";
        private const string PROCESSING = "102";
        private const string FAILED = "400";
        private const string INVALID_NUMBER = "160";
        private const string MESSAGE_IS_EMPTY = "170";
        private const string SMS_NOT_FOUND = "404";
        private const string SMS_SERVICE_NOT_TURNED = "600";

        private readonly string ESKIZ_EMAIL;
        private readonly string ESKIZ_PASSWORD;

        private string message;
        private string phone;
        private string spend;
        private string email;
        private string password;

        public SendSmsApiWithEskiz(string message, string phone, string email, string password)
        {
            this.message = message;
            this.phone = phone;
            this.spend = null;
            this.email = email;
            this.password = password;
            this.ESKIZ_EMAIL = email;
            this.ESKIZ_PASSWORD = password;
        }

        public async Task<string> Send()
        {
            string statusCode = CustomValidation();
            if (statusCode == SUCCESS)
            {
                string result = CalculationSendSms(message);
                if (result == SUCCESS)
                {
                    return await SendMessageAsync(message);
                }
                else
                {
                    return result;
                }
            }
            return statusCode;
        }

        private string CustomValidation()
        {
            if (phone.Length != 9)
            {
                return INVALID_NUMBER;
            }
            if (string.IsNullOrEmpty(message))
            {
                return MESSAGE_IS_EMPTY;
            }
            else
            {
                message = CleanMessage(message);
            }
            return SUCCESS;
        }

        private string Authorization()
        {
            var client = new HttpClient();
            var content = new StringContent(JsonConvert.SerializeObject(new
            {
                email = email,
                password = password
            }), Encoding.UTF8, "application/json");

            string AUTHORIZATION_URL = "http://notify.eskiz.uz/api/auth/login";

            var response = client.PostAsync(AUTHORIZATION_URL, content).Result;
            if (response.IsSuccessStatusCode)
            {
                var responseContent = response.Content.ReadAsStringAsync().Result;
                dynamic data = JsonConvert.DeserializeObject(responseContent);
                if (data.token != null)
                {
                    return data.token;
                }
            }
            return FAILED;
        }

        private async Task<string> SendMessageAsync(string message)
        {
            string token = Authorization();
            if (token == FAILED)
            {
                return FAILED;
            }

            string SEND_SMS_URL = "http://notify.eskiz.uz/api/message/sms/send";

            var payload = new
            {
                mobile_phone = "998" + phone,
                message = message,
                from = "4546",
                callback_url = "http://afbaf9e5a0a6.ngrok.io/sms-api-result/"
            };

            var headers = new
            {
                Authorization = $"Bearer {token}"
            };

            var client = new HttpClient();

            var response = await client.PostAsync(SEND_SMS_URL, new StringContent(JsonConvert.SerializeObject(payload), Encoding.UTF8, "application/json"));
            string responseContent = await response.Content.ReadAsStringAsync();
            Console.WriteLine($"Eskiz: {responseContent}");
            return ((int)response.StatusCode).ToString();
        }

        public async Task<string> GetStatus(string id)
        {
            string token = Authorization();

            string CHECK_STATUS_URL = $"http://notify.eskiz.uz/api/message/sms/status/{id}";

            var headers = new
            {
                Authorization = $"Bearer {token}"
            };

            var client = new HttpClient();
            var response = await client.GetAsync(CHECK_STATUS_URL);
            if (response.IsSuccessStatusCode)
            {
                var responseContent = await response.Content.ReadAsStringAsync();
                dynamic data = JsonConvert.DeserializeObject(responseContent);
                if (data.status == "success")
                {
                    if (data.message.status == "DELIVRD" || data.message.status == "TRANSMTD")
                    {
                        return SUCCESS;
                    }
                    else if (data.message.status == "EXPIRED")
                    {
                        return FAILED;
                    }
                    else
                    {
                        return PROCESSING;
                    }
                }
            }
            return FAILED;
        }

        private string CleanMessage(string message)
        {
            Console.WriteLine($"Old message: {message}");
            message = message.Replace("ц", "ts").Replace("ч", "ch").Replace("ю", "yu").Replace("а", "a").Replace("б", "b").Replace("қ", "q").Replace("ў", "o'").Replace("ғ", "g'").Replace("ҳ", "h").Replace("х", "x").Replace("в", "v").Replace("г", "g").Replace("д", "d").Replace("е", "e").Replace("ё", "yo").Replace("ж", "j").Replace("з", "z").Replace("и", "i").Replace("й", "y").Replace("к", "k").Replace("л", "l").Replace("м", "m").Replace("н", "n").Replace("о", "o").Replace("п", "p").Replace("р", "r").Replace("с", "s").Replace("т", "t").Replace("у", "u").Replace("ш", "sh").Replace("щ", "sh").Replace("ф", "f").Replace("э", "e").Replace("ы", "i").Replace("я", "ya").Replace("ў", "o'").Replace("ь", "'").Replace("ъ", "'").Replace("’", "'").Replace("“", "\"").Replace("”", "\"").Replace(",", ",").Replace(".", ".").Replace(":", ":");
            // filter upper
            message = message.Replace("Ц", "Ts").Replace("Ч", "Ch").Replace("Ю", "Yu").Replace("А", "A").Replace("Б", "B").Replace("Қ", "Q").Replace("Ғ", "G'").Replace("Ҳ", "H").Replace("Х", "X").Replace("В", "V").Replace("Г", "G").Replace("Д", "D").Replace("Е", "E").Replace("Ё", "Yo").Replace("Ж", "J").Replace("З", "Z").Replace("И", "I").Replace("Й", "Y").Replace("К", "K").Replace("Л", "L").Replace("М", "M").Replace("Н", "N").Replace("О", "O").Replace("П", "P").Replace("Р", "R").Replace("С", "S").Replace("Т", "T").Replace("У", "U").Replace("Ш", "Sh").Replace("Щ", "Sh").Replace("Ф", "F").Replace("Э", "E").Replace("Я", "Ya");
            Console.WriteLine($"Cleaned message: {message}");
            return message;
        }

        private string CalculationSendSms(string message)
        {
            try
            {
                int length = message.Length;
                if (length >= 0 && length <= 160)
                {
                    spend = "1";
                }
                else if (length > 160 && length <= 306)
                {
                    spend = "2";
                }
                else if (length > 306 && length <= 459)
                {
                    spend = "3";
                }
                else if (length > 459 && length <= 612)
                {
                    spend = "4";
                }
                else if (length > 612 && length <= 765)
                {
                    spend = "5";
                }
                else if (length > 765 && length <= 918)
                {
                    spend = "6";
                }
                else if (length > 918 && length <= 1071)
                {
                    spend = "7";
                }
                else if (length > 1071 && length <= 1224)
                {
                    spend = "8";
                }
                else
                {
                    spend = "30";
                }

                Console.WriteLine($"spend: {spend}");
                return SUCCESS;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return FAILED;
            }
        }
    }

    class ProgramStart
    {
        static async Task Main()
        {
            string message = "Салом дунё";
            string phone = "919791999"; // Make sure phone is a string since it might have leading zeros
            string email = "your_email"; // Replace with your email
            string password = "your_password"; // Replace with your password

            SendSmsApiWithEskiz eskizApi = new SendSmsApiWithEskiz(message, phone, email, password);
            string response = await eskizApi.Send();

            Console.WriteLine(response);
        }
    }
}
