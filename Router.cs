using Nancy;
using System;
using Nancy.IO;
using System.IO;
using PulseAPI;
using Nancy.ModelBinding;

namespace Example
{
	public class Program:Nancy.NancyModule
	{
		private static string protocol = "http://";
		private static string host = "localhost";
		private static int port = 8282;
		static void Main(string[] args) {
			Console.WriteLine("Starting server...");
			var server = new Nancy.Hosting.Self.NancyHost(new System.Uri(protocol + host + ":" + port));
			server.Start();
			Console.WriteLine("Nancy server started on " + protocol + host + ":" + port);
			Console.WriteLine("press any key to exit");
			Console.Read();
		}

		public Program()
		{

			//Create
			Post ["/API/Example/"] = parameters => {
				PulseAPI.Example example = this.Bind();
				string jsonResponse = ExampleController.Create(example);
				return CreateJsonResponse(jsonResponse);
			};

			//Read
			Get ["/API/Example/{id}"] = parameters => {
				var id = parameters.id;
				string jsonResponse = ExampleController.Read(id);
				return CreateJsonResponse(jsonResponse);
			};

			//Update
			Put["/API/Example/{id}"] = parameters => {
				var id = parameters.id;
				var text = this.Request.Query["ExampleText"];
				string jsonResponse = ExampleController.Update(id, text);
				return CreateJsonResponse(jsonResponse);
			};

			//Delete
			Delete["/API/Example/{id}"] = parameters => {
				var id = parameters.id;
				string jsonResponse = ExampleController.Delete(id);
				return CreateJsonResponse(jsonResponse);
			};

			//List
			Get["/API/Example"] = _ => {
				string jsonResponse = ExampleController.List();
				return CreateJsonResponse(jsonResponse); 
			};
		}

		private string GetJsonFromRequest(RequestStream stream) {
			StreamReader sr = new StreamReader(stream);
			string jsonRequest = sr.ReadToEnd();
			sr.Close();
			return jsonRequest;
		}

		private Response CreateJsonResponse(string json) {
			Response response = (Response)json;
			response.ContentType = "application/json";
			return response;
		}
	}
}