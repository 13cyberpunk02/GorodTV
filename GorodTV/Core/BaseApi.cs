using System.Text;
using Org.BouncyCastle.Crypto.Digests;
using Org.BouncyCastle.Crypto.Macs;
using Org.BouncyCastle.Crypto.Parameters;

namespace GorodTV.Core;

public class BaseApi
{
    const string BaseApiUrl = "https://gorod.tv/api";
    const string PrivateApiKey = "AcJnvy4t359IPdbz";
    private const string UnixTimeRequestString = "https://gorod.tv/api/?operation=unixtime";
    
    public string GetAuthRequestString(string username, string password) => 
        string.Format($"{BaseApiUrl}?operation=auth&login={username}&password={password}&sign={Sign(username, password)}",
            string.Empty);
    public string GetCategoryRequestString(string sessionId) => 
        string.Format($"https://gorod.tv/api?operation=categories&sessionId={sessionId}&sign={CategoriesHmac(sessionId)}",
            string.Empty);

    public string GetChannelsRequestString(string sessionId) => 
        string.Format($"https://gorod.tv/api?operation=channels&sessionId={sessionId}&sign={ChannelsHmac(sessionId)}",
            string.Empty);

    public string GetIsSessionValidString(string sessionId) =>
        string.Format($"https://gorod.tv/api?operation=categories&sessionId={sessionId}&sign={CategoriesHmac(sessionId)}", 
            string.Empty);
     
    public string GetEpgRequestString(string startTime, string channelId, string sessionId) =>
        string.Format($"https://gorod.tv/api?operation=epg&sessionId={sessionId}&channel={channelId}&startTime={startTime}&allday={true}&sign={EpgHmac(channelId, startTime,sessionId)}",
            string.Empty);    
    
    public string GetUnixTimeRequestString => UnixTimeRequestString;
    string Sign(string username, string password) => HashedSign($"auth{username}{password}");
    string CategoriesHmac(string sessionId)  => HashedSign("categories" + sessionId);
    string ChannelsHmac(string sessionId) => HashedSign("channels" + sessionId);
    private string EpgHmac(string channelId, string startTime, string sessionId) => 
        HashedSign("epg" + sessionId + channelId + startTime);
    
    private string HashedSign(string data)
    {
        byte[] keyBytes = Encoding.UTF8.GetBytes(PrivateApiKey);
        byte[] messageBytes = Encoding.UTF8.GetBytes(data);

        var hmac = new HMac(new RipeMD160Digest());
        hmac.Init(new KeyParameter(keyBytes));

        hmac.BlockUpdate(messageBytes, 0, messageBytes.Length);
        byte[] result = new byte[hmac.GetMacSize()];
        hmac.DoFinal(result, 0);

        return BitConverter.ToString(result).Replace("-", "").ToLower();
    }
}