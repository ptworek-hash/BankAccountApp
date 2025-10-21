using System;
using System.Text;

namespace BT.Model.Customer
{
    internal class Address : IAddress
    {
        public string Street { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Zip { get; set; }

        public string AddressBlock()
        {
            var hasAny = !string.IsNullOrWhiteSpace(Street) ||
                         !string.IsNullOrWhiteSpace(City) ||
                         !string.IsNullOrWhiteSpace(State) ||
                         !string.IsNullOrWhiteSpace(Zip);
            if (!hasAny) return string.Empty;

            var sb = new StringBuilder();
            if (!string.IsNullOrWhiteSpace(Street))
            {
                sb.AppendLine(Street.Trim());
            }

            var parts = new[]
            {
                string.IsNullOrWhiteSpace(City) ? null : City.Trim(),
                string.IsNullOrWhiteSpace(State) ? null : State.Trim(),
                string.IsNullOrWhiteSpace(Zip) ? null : Zip.Trim()
            };

            // City, State Zip formatting
            var cityStateZip = new StringBuilder();
            if (!string.IsNullOrEmpty(parts[0]))
            {
                cityStateZip.Append(parts[0]);
            }
            if (!string.IsNullOrEmpty(parts[1]))
            {
                if (cityStateZip.Length > 0) cityStateZip.Append(", ");
                cityStateZip.Append(parts[1]);
            }
            if (!string.IsNullOrEmpty(parts[2]))
            {
                if (cityStateZip.Length > 0) cityStateZip.Append(" ");
                cityStateZip.Append(parts[2]);
            }

            if (cityStateZip.Length > 0)
            {
                if (sb.Length > 0 && sb[sb.Length - 1] != '\n') sb.AppendLine();
                sb.Append(cityStateZip.ToString());
            }

            return sb.ToString().TrimEnd();
        }
    }
}
