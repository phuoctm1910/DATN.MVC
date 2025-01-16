using DATN.MVC.Request.MarketPlaces;
using Newtonsoft.Json;
using System.Net.Http.Headers;

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

        public static T1 PostMethodWithFileAsync<T1, T2>(string apiUrl, T2 data, IEnumerable<IFormFile> files, string fileKeyName)
        {
            try
            {
                using (var client = new HttpClient())
                using (var form = new MultipartFormDataContent())
                {
                    // Thêm các thuộc tính của đối tượng data vào form-data
                    foreach (var property in typeof(T2).GetProperties())
                    {
                        var value = property.GetValue(data)?.ToString();
                        if (value != null) // Bỏ qua trường null
                        {
                            form.Add(new StringContent(value), property.Name);
                        }
                    }


                    // Thêm tất cả các tệp vào form-data với cùng tên fileKeyName
                    if (files != null)
                    {
                        foreach (var file in files)
                        {
                            var fileContent = new StreamContent(file.OpenReadStream());
                            fileContent.Headers.ContentType = new MediaTypeHeaderValue(file.ContentType);
                            form.Add(fileContent, fileKeyName, file.FileName); // Dùng fileKeyName mà không có chỉ số
                        }
                    }

                    // Gửi yêu cầu POST
                    var response = client.PostAsync(apiUrl, form).Result;

                    if (response.IsSuccessStatusCode)
                    {
                        var responseContent = response.Content.ReadAsStringAsync().Result;
                        return JsonConvert.DeserializeObject<T1>(responseContent);
                    }
                    else
                    {
                        var errorContent = response.Content.ReadAsStringAsync().Result;
                        throw new Exception($"Failed to post data to API. Status Code: {response.StatusCode}, Error: {errorContent}");
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Error posting data with file: {ex.Message}");
            }
        }

        public static T1 PutMethodAsyncV2<T1, T2>(string apiUrl, T2 data)
        {
            try
            {
                using (var client = new HttpClient())
                using (var formContent = new MultipartFormDataContent())
                {
                    // Add properties of the object to form-data
                    foreach (var property in typeof(T2).GetProperties())
                    {
                        var value = property.GetValue(data);
                        if (value != null)
                        {
                            // Ensure that values are serialized to string format if necessary
                            formContent.Add(new StringContent(value.ToString()), property.Name);
                        }
                    }

                    // Send the PUT request
                    var response = client.PutAsync(apiUrl, formContent).Result;

                    if (response.IsSuccessStatusCode)
                    {
                        var responseContent = response.Content.ReadAsStringAsync().Result;
                        return JsonConvert.DeserializeObject<T1>(responseContent);
                    }
                    else
                    {
                        var errorContent = response.Content.ReadAsStringAsync().Result;
                        throw new Exception($"API call failed. Status Code: {response.StatusCode}, Error: {errorContent}");
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Error in PutMethodAsyncV2: {ex.Message}", ex);
            }
        }

        public static T1 PutMethodWithFileAsync<T1, T2>(string apiUrl, T2 data, IEnumerable<IFormFile> files, string fileKeyName = "ImageFile")
        {
            try
            {
                using (var client = new HttpClient())
                using (var form = new MultipartFormDataContent())
                {
                    // Add properties of the data object to form-data
                    foreach (var property in typeof(T2).GetProperties())
                    {
                        var value = property.GetValue(data)?.ToString();
                        if (value != null) // Skip null properties
                        {
                            form.Add(new StringContent(value), property.Name);
                        }
                    }

                    // Add all files to form-data with the same fileKeyName
                    if (files != null && files.Any())
                    {
                        foreach (var file in files)
                        {
                            var fileContent = new StreamContent(file.OpenReadStream());
                            fileContent.Headers.ContentType = new MediaTypeHeaderValue(file.ContentType);
                            form.Add(fileContent, fileKeyName, file.FileName);
                        }
                    }


                    // Send PUT request
                    var response = client.PutAsync(apiUrl, form).Result;

                    if (response.IsSuccessStatusCode)
                    {
                        var responseContent = response.Content.ReadAsStringAsync().Result;
                        return JsonConvert.DeserializeObject<T1>(responseContent);
                    }
                    else
                    {
                        var errorContent = response.Content.ReadAsStringAsync().Result;
                        throw new Exception($"Failed to put data to API. Status Code: {response.StatusCode}, Error: {errorContent}");
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Error putting data with file: {ex.Message}");
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

        public static T1 PostMethodAsyncV2<T1, T2>(string apiUrl, T2 data)
        {
            try
            {
                using (var client = new HttpClient())
                using (var formContent = new MultipartFormDataContent())
                {
                    // Add properties of the object to form-data
                    foreach (var property in typeof(T2).GetProperties())
                    {
                        var value = property.GetValue(data);
                        if (value != null)
                        {
                            // Ensure that values are serialized to string format if necessary
                            formContent.Add(new StringContent(value.ToString()), property.Name);
                        }
                    }

                    // Send the PUT request
                    var response = client.PostAsync(apiUrl, formContent).Result;

                    if (response.IsSuccessStatusCode)
                    {
                        var responseContent = response.Content.ReadAsStringAsync().Result;
                        return JsonConvert.DeserializeObject<T1>(responseContent);
                    }
                    else
                    {
                        var errorContent = response.Content.ReadAsStringAsync().Result;
                        throw new Exception($"API call failed. Status Code: {response.StatusCode}, Error: {errorContent}");
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Error in PutMethodAsyncV2: {ex.Message}", ex);
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

        public static T1 DeleteMethodAsync<T1, T2>(string apiUrl, T2 data)
        {
            try
            {
                var requestMessage = GetHttpRequestMessage(HttpMethod.Delete, apiUrl);
                requestMessage.Content = new StringContent(JsonConvert.SerializeObject(data), System.Text.Encoding.UTF8, "application/json");

                using (HttpClient client = new HttpClient())
                {
                    HttpResponseMessage response = client.Send(requestMessage);
                    if (response.IsSuccessStatusCode)
                    {
                        var responseContent = response.Content.ReadAsStringAsync().Result;
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

        private static HttpRequestMessage GetHttpRequestMessage(HttpMethod method, string url)
        {
            return new HttpRequestMessage(method, url);
        }

        internal static Task<T1> PostMethodWithFileAsync<T1, T2>(string v, Request.MarketPlaces.CreateSalceplaces request, IEnumerable<IFormFile> imageFiles, T2 newPost)
        {
            throw new NotImplementedException();
        }

        internal static T1 PostMethodWithFileAsync<T1, T2>(string v, CreateSalceplaces request, IEnumerable<IFormFile> imageFiles, string fileKeyName)
        {
            throw new NotImplementedException();
        }
    }
}
