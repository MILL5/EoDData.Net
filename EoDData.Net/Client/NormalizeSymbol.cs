using System.Linq;
using System.Text.RegularExpressions;

namespace EoDData.Net
{
    public partial class EoDDataClient : IEoDDataClient
    {
        /// <summary>
        /// Regex expression to get the ticker from the name of the Symbol.
        /// </summary>
        private static readonly Regex regexFullName = new(@"^[\s\w\d\.\%\&\]\-]*\[((?<warrants>[\w]+\/W[\w]?)|(?<units>[\w]+\.[U]{1})|(?<preferred>[\w]+\/P[\w]?)|(?<rights>[\w]+\/R)|(?<others>[\w]+\/[\w]))\]$", RegexOptions.Compiled);

        /// <summary>
        /// Regex expression to replace the Units symbols.
        /// </summary>
        private static readonly Regex regexUnits = new(@"^([\w]+)\.(U)", RegexOptions.Compiled);

        /// <summary>
        /// Regex expressions to replace prefered symbols.
        /// </summary>
        private static readonly Regex regexPrefered = new(@"^([\w]+)\/P([\w]+)", RegexOptions.Compiled);

        /// <summary>
        /// Regex expressions to replace rights symbols.
        /// </summary>
        private static readonly Regex regexRights = new(@"^([\w]+)\/R", RegexOptions.Compiled);

        /// <summary>
        /// Regex expressions to replace warrants.
        /// </summary>
        private static readonly Regex regexWarrants = new(@"^([\w]+)\/W", RegexOptions.Compiled);

        /// <summary>
        /// Regex expressions to replace warrants prefered symbols.
        /// </summary>
        private static readonly Regex regexWarrantsPreferred = new(@"^([\w]+)\/W([\w])", RegexOptions.Compiled);

        /// <summary>
        /// Regex expressions to replace other types of symbols.
        /// </summary>
        private static readonly Regex regexOthers = new(@"^([\w]+)\/([\w])", RegexOptions.Compiled);

        /// <summary>
        /// Normalize the Symbol with the NYSE standard.
        /// </summary>
        /// <param name="name">Name of the Symbol.</param>
        /// <param name="code">Code of the Symbol. e.g MSFT</param>
        /// <remarks>
        /// Two things happen for EoDData:
        /// 1. EoDData writes in the Name of the ticker the actual/real stock Symbol, so we need to extract that string.
        ///    e.g. "Chardan Healthcare Acquisition 2 WT [Chaq/W]" => extracted text will be CHAQ.WS.
        ///    We are using precompiled regular expression to improve performance.
        /// 2. Replacing the preferred tickers from "-" to "p".
        /// </remarks>
        /// <returns>The normalized symbol.</returns>
        public static string NormalizeSymbol(string name, string code)
        {
            var openingBracketExists = (name.Length - name.Replace("[", string.Empty).Length) >= 1;
            if (openingBracketExists)
            {
                var regMatch = regexFullName.Match(name);
                if (regMatch.Success)
                {
                    var symbol = regMatch.Groups[1].Value.ToUpper().Trim();
                    var latestNamedMatchGroup = regMatch.Groups.Values.LastOrDefault(x => x.Success);
                    switch (latestNamedMatchGroup.Name.ToLower())
                    {
                        case "warrants":
                            symbol = regexWarrantsPreferred.Replace(symbol, "$1.WS.$2");
                            symbol = regexWarrants.Replace(symbol, "$1.WS");
                            break;

                        case "units":
                            symbol = regexUnits.Replace(symbol, "$1.U");
                            break;

                        case "preferred":
                            symbol = regexPrefered.Replace(symbol, "$1p$2");
                            break;

                        case "rights":
                            symbol = regexRights.Replace(symbol, "$1r");
                            break;

                        default:
                            symbol = regexOthers.Replace(symbol, "$1.$2");
                            break;
                    }

                    code = symbol;
                }
            }
            else
            {
                if (code.Contains('-'))
                {
                    code = code.Replace('-', 'p');
                }
            }

            return code;
        }
    }
}