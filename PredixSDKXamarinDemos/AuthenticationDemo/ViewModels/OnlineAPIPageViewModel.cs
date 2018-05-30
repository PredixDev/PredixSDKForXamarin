using System;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using PredixSDKForWindows.Networking;

namespace AuthenticationDemo
{
	public class OnlineAPIPageViewModel
    {
		public async Task<ResponseData> SendOnlineRequest(string url)
        {
            // Construct a PredixRequestMessage. If authenticated, this request prefills 
            // the Authorization header with the retrieved access token.
            var request = new PredixRequestMessage()
            {
                RequestUri = new Uri(url),
                Method = HttpMethod.Get
            };

            HttpResponseMessage response = await PredixHttpClient.Client.SendAsync(request);
            string jsonStr = await response.Content.ReadAsStringAsync();
            var responseData = JsonConvert.DeserializeObject<Data>(jsonStr);

            var documentData = new ResponseData();
            documentData.URLResponseData = responseData.Text;
            documentData.URLStatusData = response.StatusCode.ToString();

            return documentData;
        }
    }
}
