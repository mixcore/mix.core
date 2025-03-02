using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mix.Shared.Extensions
{
    public static class JObjectExtensions
    {
        public static JToken FindDiff(this JToken fromValue, JToken toValue)
        {
            var diff = new JObject();

            string valueToPrint = string.Empty;
            if (JToken.DeepEquals(fromValue, toValue)) return diff;
            else if (fromValue == null || toValue == null || fromValue.Type != toValue.Type)
            {
                diff["from"] = fromValue;
                diff["to"] = toValue;
                return diff;
            }

            switch (fromValue.Type)
            {
                case JTokenType.Object:
                    {
                        var Initial = fromValue as JObject;
                        var Updated = toValue as JObject;
                        var addedKeys = Initial.Properties().Select(c => c.Name).Except(Updated.Properties().Select(c => c.Name));
                        var removedKeys = Updated.Properties().Select(c => c.Name).Except(Initial.Properties().Select(c => c.Name));
                        var unchangedKeys = Initial.Properties().Where(c => JToken.DeepEquals(c.Value, toValue[c.Name])).Select(c => c.Name);
                        foreach (var k in addedKeys)
                        {
                            diff[k] = new JObject
                            {
                                ["to"] = fromValue[k]
                            };
                        }
                        foreach (var k in removedKeys)
                        {
                            diff[k] = new JObject
                            {
                                ["from"] = toValue[k]
                            };
                        }
                        var potentiallyModifiedKeys = Initial.Properties().Select(c => c.Name).Except(addedKeys).Except(unchangedKeys);
                        foreach (var k in potentiallyModifiedKeys)
                        {
                            var foundDiff = Initial[k].FindDiff(Updated[k]);
                            if (foundDiff == null)
                                return foundDiff;

                            if (foundDiff.HasValues && (foundDiff["from"] != null || foundDiff["to"] != null))
                            {
                                diff[k] = foundDiff;
                            }
                        }
                    }
                    break;
                //"to" indicate the Original Value
                //"-" indicate the Updated/Modified Value
                case JTokenType.Array:
                    {
                        var current = fromValue as JArray;
                        var newValue = toValue as JArray;

                        if (!JToken.DeepEquals(current, newValue))
                        {
                            diff["from"] = current;
                            diff["to"] = newValue;
                        }
                        //    var minus = new JArray(current.ExceptAll(newValue, new JTokenEqualityComparer()));
                        //var plus = new JArray(newValue.ExceptAll(current, new JTokenEqualityComparer()));
                        //if (minus.HasValues) diff["from"] = minus;
                        //if (plus.HasValues) diff["to"] = plus;
                    }
                    break;
                default:
                    diff["from"] = fromValue;
                    diff["to"] = toValue;
                    break;
            }

            return diff;
        }

        public static bool IsArrayChanged(JToken foundDiff)
        {
            return foundDiff["from"] != null && foundDiff["from"].Type == JTokenType.Array
                || foundDiff["to"] != null && foundDiff["to"].Type == JTokenType.Array;
        }
        public static bool IsArrayObjectChanged(JToken foundDiff)
        {
            return foundDiff["from"] != null && foundDiff["from"][0] != null && foundDiff["from"][0].Type == JTokenType.Object
                || foundDiff["to"] != null && foundDiff["to"][0] != null && foundDiff["to"][0].Type == JTokenType.Object;
        }

        public static IEnumerable<TSource> ExceptAll<TSource>(
        this IEnumerable<TSource> first,
        IEnumerable<TSource> second)
        {
            return first.ExceptAll(second, null);
        }

        public static IEnumerable<TSource> ExceptAll<TSource>(
            this IEnumerable<TSource> first,
            IEnumerable<TSource> second,
            IEqualityComparer<TSource> comparer)
        {
            if (first == null) { throw new ArgumentNullException("first"); }
            if (second == null) { throw new ArgumentNullException("second"); }


            var secondCounts = new Dictionary<TSource, int>(comparer ?? EqualityComparer<TSource>.Default);
            int count;
            int nullCount = 0;

            // Count the values fromValue second
            foreach (var item in second)
            {
                if (item == null)
                {
                    nullCount++;
                }
                else
                {
                    if (secondCounts.TryGetValue(item, out count))
                    {
                        secondCounts[item] = count + 1;
                    }
                    else
                    {
                        secondCounts.Add(item, 1);
                    }
                }
            }

            // Yield the values fromValue first
            foreach (var item in first)
            {
                if (item == null)
                {
                    nullCount--;
                    if (nullCount < 0)
                    {
                        yield return item;
                    }
                }
                else
                {
                    if (secondCounts.TryGetValue(item, out count))
                    {
                        if (count == 0)
                        {
                            secondCounts.Remove(item);
                            yield return item;
                        }
                        else
                        {
                            secondCounts[item] = count - 1;
                        }
                    }
                    else
                    {
                        yield return item;
                    }
                }
            }
        }
    }
}
