using Newtonsoft.Json;

namespace DATN.MVC.Helpers
{
    public class ApiHelpers
    {
        public static T GetMethod<T>(string apiUrl, Dictionary<string, object> objPara = null)
        {
            try
            {
                var query = objPara != null ? ("?" + objPara.ToQueryString()) : string.Empty;
                var requestMessage = GetHttpRequestMessage(HttpMethod.Get, apiUrl + query);

                using (HttpClient client = new HttpClient())
                {
                    HttpResponseMessage response = client.SendAsync(requestMessage).Result;

                    if (response.IsSuccessStatusCode)
                    {
                        // Read the response content
                        var responseContent = response.Content.ReadAsStringAsync().Result;

                        // Deserialize JSON into the desired object type
                        return JsonConvert.DeserializeObject<T>(responseContent);
                    }
                    else
                    {
                        throw new Exception("Failed to retrieve data from API");
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Error retrieving data: {ex.Message}");
            }
        }

        public static T1 PostMethodAsync<T1, T2>(string apiUrl, T2 data)
        {
            try
            {
                var requestMessage = GetHttpRequestMessage(HttpMethod.Post, apiUrl);
                requestMessage.Content = new StringContent(JsonConvert.SerializeObject(data), System.Text.Encoding.UTF8, "application/json");

                using (HttpClient client = new HttpClient())
                {
                    HttpResponseMessage response =  client.Send(requestMessage);
                    if (response.IsSuccessStatusCode)
                    {
                        var responseContent =  response.Content.ReadAsStringAsync().Result;
                        return JsonConvert.DeserializeObject<T1>(responseContent);
                    }
                    else
                    {
                        throw new Exception("Failed to post data to API");
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Error posting data: {ex.Message}");
            }
        }

        public static T PutMethod<T>(string apiUrl, object data)
        {
            try
            {
                var requestMessage = GetHttpRequestMessage(HttpMethod.Put, apiUrl);
                requestMessage.Content = new StringContent(JsonConvert.SerializeObject(data), System.Text.Encoding.UTF8, "application/json");

                using (HttpClient client = new HttpClient())
                {
                    HttpResponseMessage response = client.SendAsync(requestMessage).Result;

                    if (response.IsSuccessStatusCode)
                    {
                        var responseContent = response.Content.ReadAsStringAsync().Result;
                        return JsonConvert.DeserializeObject<T>(responseContent);
                    }
                    else
                    {
                        throw new Exception("Failed to update data on API");
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Error updating data: {ex.Message}");
            }
        }

        public static T DeleteMethod<T>(string apiUrl)
        {
            try
            {
                var requestMessage = GetHttpRequestMessage(HttpMethod.Delete, apiUrl);

                using (HttpClient client = new HttpClient())
                {
                    HttpResponseMessage response = client.SendAsync(requestMessage).Result;

                    if (response.IsSuccessStatusCode)
                    {
                        var responseContent = response.Content.ReadAsStringAsync().Result;
                        return JsonConvert.DeserializeObject<T>(responseContent);
                    }
                    else
                    {
                        throw new Exception("Failed to delete data from API");
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Error deleting data: {ex.Message}");
            }
        }

        private static HttpRequestMessage GetHttpRequestMessage(HttpMethod method, string url)
        {
            return new HttpRequestMessage(method, url);
        }
    }
}
