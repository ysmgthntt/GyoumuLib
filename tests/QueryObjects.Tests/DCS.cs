namespace QueryObjects.Tests
{
    internal static class DCS
    {
        public static T? SerializeAndDeserialize<T>(T graph)
        {
            var dcs = new DataContractSerializer(typeof(T));
            using var ms = new MemoryStream();
            dcs.WriteObject(ms, graph);
            ms.Position = 0;
            var result = (T?)dcs.ReadObject(ms);
            return result;
        }
    }
}
