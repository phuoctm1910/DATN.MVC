using System;
using System.Security.Claims;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

public class ClaimsIdentityConverter : JsonConverter
{
	public override bool CanConvert(Type objectType)
	{
		return objectType == typeof(ClaimsIdentity);
	}

	public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
	{
		// Bạn có thể triển khai phương thức này nếu cần chuyển đổi ngược từ JSON thành đối tượng ClaimsIdentity
		throw new NotImplementedException();
	}

	public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
	{
		var claimsIdentity = (ClaimsIdentity)value;
		var jObject = new JObject();

		foreach (var claim in claimsIdentity.Claims)
		{
			jObject.Add(claim.Type, claim.Value);
		}

		jObject.WriteTo(writer);
	}
}
