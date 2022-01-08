using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Directory.Report.Application.RestClients.Model;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RestSharp;

namespace Directory.Report.Application.RestClients
{
    public interface IContactApiClient
    {
        Task<List<ContactModel>> All();
    }
    
    public class ContactApiClient : IContactApiClient
    {
        public async Task<List<ContactModel>> All()
        {
            var client = new RestSharp.RestClient("http://contact-api:80/Contact/List");
            var request = new RestRequest
            {
                Method = Method.Get
            };
            var response = await client.ExecuteAsync(request);
            Console.Write(JsonConvert.SerializeObject(response));
            if (response.Content == null) return null;
            var jObject = JObject.Parse(response.Content);
            if (jObject["model"] == null) return null;
            var contactList = JsonConvert.DeserializeObject<List<ContactModel>>(jObject["model"].ToString());
            return await Task.FromResult(contactList);
        }
    }
}