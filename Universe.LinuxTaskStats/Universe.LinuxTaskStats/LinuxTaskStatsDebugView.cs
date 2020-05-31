using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Universe.LinuxTaskStats
{
    public static class LinuxTaskStatsDebugView
    {
        public static string ToDebugString(this IEnumerable<LinuxTaskStats> collection)
        {
            return collection.Select(x => (LinuxTaskStats?) x).ToDebugString();
        }
        
        public static string ToDebugString(this IEnumerable<LinuxTaskStats?> collection)
        {
            var list = collection
                .Where(x => x.HasValue)
                .Select(x => x.GetValueOrDefault())
                .ToArray();
            
#if NETSTANDARD1_1            
            PropertyInfo[] properties = typeof(LinuxTaskStats).GetTypeInfo().DeclaredProperties.ToArray();
#else
            PropertyInfo[] properties = typeof(LinuxTaskStats).GetProperties();
#endif
            var raw = new string[list.Length + 1, properties.Length];
            int col = 0;
            foreach (PropertyInfo property in properties)
            {
                raw[0, col++] = property.Name;
            }

            object[] noArgs = new object[0]; 
            for (int row = 0; row < list.Length; row++)
            {
                for (col = 0; col < properties.Length; col++)
                {
                    object obj = properties[col].GetValue(list[row], noArgs);
                    string v = Convert.ToString(obj);
                    if (obj is long? && ((long?) obj).HasValue) v = ((long?) obj).Value.ToString("n0");
                    if (obj is long) v = ((long) obj).ToString("n0");
                    if (obj is int) v = ((int) obj).ToString("n0");
                    raw[row + 1, col] = v;
                }
            }
            
            int[] width = new int[properties.Length];
            for (col = 0; col < properties.Length; col++)
            {
                for (int row = 0; row < list.Length + 1; row++)
                    width[col] = Math.Max(raw[row, col].Length + 2, width[col]);
            }
            
            StringBuilder ret = new StringBuilder();
            for (int row = 0; row < list.Length + 1; row++)
            {
                for (col = 0; col < properties.Length; col++)
                {
                    ret.AppendFormat("{0,-" + width[col] + "}", raw[row, col]);
                }

                ret.AppendLine();
            }

            return ret.ToString();
        }
        
    }
}