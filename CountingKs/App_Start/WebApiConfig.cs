using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Formatting;
using System.Net.Http.Headers;
using System.Web.Http;
using System.Web.Http.Dispatcher;
using CountingKs.Filters;
using CountingKs.Services;
using Newtonsoft.Json.Serialization;

namespace CountingKs
{
	public static class WebApiConfig
	{
		public static void Register(HttpConfiguration config)
		{
			config.Routes.MapHttpRoute(
				name: "Food",
				routeTemplate: "api/nutrition/foods/{foodid}",
				defaults: new { controller = "foods", foodid = RouteParameter.Optional }
			);

			config.Routes.MapHttpRoute(
				name: "Measures",
				routeTemplate: "api/nutrition/foods/{foodid}/measures/{id}",
				defaults: new { controller = "measures", id = RouteParameter.Optional }
				);

			config.Routes.MapHttpRoute(
				name: "Measures2",
				routeTemplate: "api/nutrition/foods/{foodid}/measures/{id}",
				defaults: new { controller = "measuresv2", id = RouteParameter.Optional }
				);

			config.Routes.MapHttpRoute(
				name: "Diaries",
				routeTemplate: "api/user/diaries/{diaryid}",
				defaults: new { controller = "diaries", diaryid = RouteParameter.Optional }
			);

			config.Routes.MapHttpRoute(
				name: "DiaryEntries",
				routeTemplate: "api/user/diaries/{diaryid}/entries/{id}",
				defaults: new { controller = "diaryentries", id = RouteParameter.Optional }
			);

			config.Routes.MapHttpRoute(
				name: "DiarySummary",
				routeTemplate: "api/user/diaries/{diaryid}/summary",
				defaults: new { controller = "diarysummary" }
			);

			config.Routes.MapHttpRoute(
				name: "Token",
				routeTemplate: "api/token",
				defaults: new { controller = "token" }
			);

			// Uncomment the following line of code to enable query support for actions with an IQueryable or IQueryable<T> return type.
			// To avoid processing unexpected or malicious queries, use the validation settings on QueryableAttribute to validate incoming queries.
			// For more information, visit http://go.microsoft.com/fwlink/?LinkId=279712.
			//config.EnableQuerySupport();

			var jsonFormatter = config.Formatters.OfType<JsonMediaTypeFormatter>().FirstOrDefault();
			jsonFormatter.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
			CreateMediaTypes(jsonFormatter);


			config.Formatters.JsonFormatter.SupportedMediaTypes.Add(new MediaTypeHeaderValue("text/html"));

			//// Add support JSONP
			//var jsonpFormatter = new WebApiContrib.Formatting.Jsonp.JsonpMediaTypeFormatter(jsonFormatter, "cb");
			//config.Formatters.Insert(0, jsonpFormatter);
			//var jsonpFormatter = new JsonMediaTypeFormatter();
			//config.Formatters.Insert(0, jsonpFormatter);

			// Replace the Controller Configuration
			config.Services.Replace(typeof(IHttpControllerSelector),
				new CountingKsControllerSelector(config));

#if !DEBUG
	// Force HTTPS on entire API
			config.Filters.Add(new RequireHttpsAttribute());
#endif
		}

		/// <summary>
		/// This method ask jsonFormatter to handle additional media types
		/// </summary>
		/// <param name="jsonFormatter"></param>
		private static void CreateMediaTypes(JsonMediaTypeFormatter jsonFormatter)
		{
			// Given these media types,
			var mediaTypes = new[]
			{
				"application/vnd.countingks.food.v1+json",
				"application/vnd.countingks.measure.v1+json",
				"application/vnd.countingks.measure.v2+json",
				"application/vnd.countingks.diary.v1+json",
				"application/vnd.countingks.diaryEntry.v1+json",
			};

			// let jsonFormatter handles above media types
			foreach (var mediaType in mediaTypes)
			{
				jsonFormatter.SupportedMediaTypes.Add(new MediaTypeHeaderValue(mediaType));
			}
		}
	}
}