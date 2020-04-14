using System.Threading.Tasks;
using ImmuneAndReadyToHelp.Core.Model;
using RestSharp;
using RestSharp.Authenticators;
using System;
using Microsoft.Extensions.Configuration;

namespace ImmuneAndReadyToHelp.Core.Services
{
    public class MailGunEmailService : IEmailService
    {
		private string ApiKey { get; set; }
		private string Domain { get; set; }

		public MailGunEmailService(IConfiguration configuration)
		{
			ApiKey = configuration["MailGun:ApiKey"];
			Domain = configuration["MailGun:EmailDomain"];
		}

		public async Task Send(EmailMessage message)
		{
			var client = GetRestClient();
			var request = BuildRequest(message);
			var response = await client.ExecuteAsync(request);
		}

		private RestClient GetRestClient() =>
			new RestClient
			{
				BaseUrl = new Uri("https://api.mailgun.net/v3"),
				Authenticator = new HttpBasicAuthenticator("api", ApiKey)
			};

		private RestRequest BuildRequest(EmailMessage message)
		{
			var FromDisplayName = "Immune And Ready To Help Notifications";
			var request = new RestRequest
			{
				Method = Method.POST,
				Resource = $"mg.{Domain}/messages"
			};

			request.AddParameter("domain", $"mg.{Domain}", ParameterType.UrlSegment);
			request.AddParameter("from", $"{FromDisplayName} <Notifications@{Domain}>");
			request.AddParameter("to", message.To);
			request.AddParameter("subject", message.Subject);
			request.AddParameter("html", message.Body);

			return request;
		}
	}
}