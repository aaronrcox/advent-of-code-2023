

using System;
using System.Runtime.CompilerServices;
using System.Security.Cryptography.X509Certificates;

namespace Day_05
{

    public class MapRange
    {
        public long DstStart { get; set; }
        public long SrcStart { get; set; }
        public long Length { get; set; }

        public MapRange(long dstStart, long srcStart, long length)
        {
            DstStart = dstStart;
            SrcStart = srcStart;
            Length = length;
        }

        public long? MapValue(long val)
        {
            if(val >= SrcStart && val < SrcStart + Length)
            {
                long diff = val - SrcStart;
                return DstStart + diff;
            }
            return null;
        }
    }

    public class Map
    {
        
        public string DestinationCategory { get; set; }
        public string SourceCategory { get; set; }
        public List<MapRange> Ranges { get; set; } = new List<MapRange>();

        public Map()
        {

        }

        public long MapValue(long val)
        {
            foreach (var range in Ranges)
            {
                long? mappedValue = range.MapValue(val);
                if (mappedValue.HasValue)
                    return mappedValue.Value;
            }
            return val;
        }
    }

    public class Alamanac
    {
        public static IEnumerable<long> LongRange(long start, long count)
        {
            Console.WriteLine($"{start} - {count}");

            for (long i = 0; i < count; i++)
            {
                yield return start + i;
            }
        }

        public List<long> Seeds { get; set; } = new List<long>();
        public List<Map> Maps { get; set; } = new List<Map>();

        public Dictionary<string, Dictionary<string, Map>> MapLookup;
        

        public Alamanac(List<string> lines)
        {
            // First Line contains the seeds
            string[] seedsLine = lines[0].Split(":");
            Seeds = seedsLine[1].Trim().Split(" ").Select(s => s.Trim()).Select(long.Parse).ToList();

            Map currentMap = new Map();
            for (int lineIndex = 2; lineIndex < lines.Count; lineIndex++)
            {
                string line = lines[lineIndex];

                // first line contains the src to dst map names
                string[] srcToDst = line.Split("-to-").Select(s => s.Trim().Replace("map:", "")).ToArray();
                currentMap.SourceCategory = srcToDst[0].Trim();
                currentMap.DestinationCategory = srcToDst[1].Trim();

                do
                {
                    lineIndex++;
                    line = lines[lineIndex];

                    if (string.IsNullOrWhiteSpace(line))
                    {
                        Maps.Add(currentMap);
                        currentMap = new();
                        break;
                    }
                        
                    // get the range of numbers
                    var range = line.Split(" ").Select(s => s.Trim()).Select(long.Parse).ToList();
                    currentMap.Ranges.Add(new MapRange(range[0], range[1], range[2]));

                } while (true);
            }

            MapLookup = Maps
                .GroupBy(m => m.SourceCategory)
                .ToDictionary(
                    g => g.Key, 
                    g => g.ToDictionary(m => m.DestinationCategory, m => m)
                );
        }

        public IEnumerable<long> GetSeedLocations()
        {
            var seedsoil = MapLookup["seed"]["soil"];
            var soilfertilizer = MapLookup["soil"]["fertilizer"];
            var fertilizerwater = MapLookup["fertilizer"]["water"];
            var waterlight = MapLookup["water"]["light"];
            var lighttemperature = MapLookup["light"]["temperature"];
            var temperaturehumidity = MapLookup["temperature"]["humidity"];
            var humiditylocation = MapLookup["humidity"]["location"];

            return Seeds
                .Select(seedsoil.MapValue)
                .Select(soilfertilizer.MapValue)
                .Select(fertilizerwater.MapValue)
                .Select(waterlight.MapValue)
                .Select(lighttemperature.MapValue)
                .Select(temperaturehumidity.MapValue)
                .Select(humiditylocation.MapValue);
        }

        public IEnumerable<long> GetSeedLocations2()
        {
            var seedsoil = MapLookup["seed"]["soil"];
            var soilfertilizer = MapLookup["soil"]["fertilizer"];
            var fertilizerwater = MapLookup["fertilizer"]["water"];
            var waterlight = MapLookup["water"]["light"];
            var lighttemperature = MapLookup["light"]["temperature"];
            var temperaturehumidity = MapLookup["temperature"]["humidity"];
            var humiditylocation = MapLookup["humidity"]["location"];

            return Seeds
                .Select((value, index) => new { Index = index, Value = value })
                .GroupBy(x => x.Index / 2).Select(g => g.Select(x => x.Value).ToList()).ToList()
                .SelectMany(pair => LongRange(pair[0], pair[1])).AsParallel()
                .Select(seedsoil.MapValue)
                .Select(soilfertilizer.MapValue)
                .Select(fertilizerwater.MapValue)
                .Select(waterlight.MapValue)
                .Select(lighttemperature.MapValue)
                .Select(temperaturehumidity.MapValue)
                .Select(humiditylocation.MapValue);
        }

    }

    internal class Program
    {
        static void Main(string[] args)
        {
            string filename = "input.txt";
            var lines = File.ReadAllLines(filename).ToList();
            lines.Add("");

            Alamanac alamanac = new Alamanac(lines);

            long part1Answer = alamanac.GetSeedLocations().Min();
            Console.WriteLine($"Part 1: {part1Answer}");
            
            
            long part2Answer = alamanac.GetSeedLocations2().Min();
            Console.WriteLine($"Part 2: {part2Answer}");
        }
    }
}