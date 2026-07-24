using System.Collections.Generic;
using System.Text.Json;

public class Jiexi2
{
    // key = PathID
    public static Dictionary<long, Jiexi2> Assets { get; } = new();

    public long PathID { get; set; }

    private string _json = "";
    public int Version { get; set; } = 0;
    public string Json
    {
        get => _json;
        set
        {
            _json = value;
            TargetID = GetPathIDs(value);
        }
    }

    public string Name { get; set; } = "";



    public List<long> TargetID { get; private set; } = new();

    public static List<long>? GetTarget(long pathID)
    {
        if (Assets.TryGetValue(pathID, out Jiexi2? jiex))
        {
            return jiex.TargetID;
        }

        return null;
    }
    private static List<long> GetPathIDs(string json)
    {
        List<long> ids = new();

        if (string.IsNullOrEmpty(json))
            return ids;

        using JsonDocument doc = JsonDocument.Parse(json);

        FindPathID(doc.RootElement, ids);

        return ids;
    }


    private static void FindPathID(JsonElement element, List<long> ids)
    {
        if (element.ValueKind == JsonValueKind.Object)
        {
            foreach (var property in element.EnumerateObject())
            {
                if (property.Name == "m_PathID" &&
                    property.Value.ValueKind == JsonValueKind.Number)
                {
                    ids.Add(property.Value.GetInt64());
                }

                FindPathID(property.Value, ids);
            }
        }
        else if (element.ValueKind == JsonValueKind.Array)
        {
            foreach (var item in element.EnumerateArray())
            {
                FindPathID(item, ids);
            }
        }
    }
}