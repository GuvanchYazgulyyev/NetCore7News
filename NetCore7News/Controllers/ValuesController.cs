using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using System.Threading.RateLimiting;

namespace NetCore7News.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
   // [EnableRateLimiting("Fixed")]
    //[EnableRateLimiting("Sliding")]
    //[EnableRateLimiting("Token")]
    // [EnableRateLimiting("Concurrency")]
     [EnableRateLimiting("Custom")]
    public class ValuesController : ControllerBase
    {
        public IActionResult Get()
        {
            return Ok();
        }

        [HttpGet("[action]")]
        public async Task<IActionResult> GetAsync()
        {
            await Task.Delay(20000);
            return Ok();
        }

    }
}


#region Reate Limit Algoritmalar

#region Fix Window
// Sabit Bir zaman aralıgı kullanarak istekleri sınırlandıran algoritmadır!!

#endregion

#region Sliding Window
//Fix Win Alogritmasına benzemektedir! Her Sabit Sürede bir zaman aralıgında istekleri sınırlandırmaktadır. 
// Fakat Sürenin yarısından sonra diğer periodun req kotasını Harcayacak şekilde istekleri karşılar!

#endregion


#region Token Bucket
// Her Periotda işlenecek kadar Req sayısı kadar Token üretilmektedir. Eger Tokenler Kullanıldıysa diger periotdan
// borç alınır. Fakat her peryotda Token üretim Miktarı kadar Token üretilecek. 
// ÖNEMLİ (Periodun Max Limiti verilen Sabit sayı Kadar olacaktır!!!!).
#endregion

#region Concurrency

// Concurrency Algoritması Sadece Async (Eşzamanlı Çalısan Req leri Sınırlandırmak için Kullanılır)
// Her İstek Concurrency sınırını bir azaltmakta ve bittikleri taktirde bu sınırı bir arttırmaktadır.

#endregion

#endregion


#region Attribute'ler
#region EnableReteLimiting
// Controller veya action seviyesinde  istenilen politikada RateLimit devreye sokmamızı
// sağlayan bir attribute dir!!!
#endregion

#region DisableRateLimiting
// Controller seviyesinde devreye sokulmuş bir rateLimite politikasının action seviyesinde
// pasifleştirilmesini sağlayan  bir attribite dir!
#endregion

#endregion

#region Minimal API ' lerde  Rate Limiting
// RequireRateLimiting
#endregion

#region OnRejected porperty
// Rate Limit uygulanan operasyonlarda sınırdan dolayı boşa çıkan  req'lerin söz konusu oldugu durumlarda
// loglama vs  gibi işlemler yapabilmek için kullanıdıgımız (Event Mantıgında bir Prop dır).
#endregion


#region Özelleştirilmiş Rate Limite Policy Oluşturma

#endregion

class CustomRateLimiter : IRateLimiterPolicy<string>
{
    public Func<OnRejectedContext, CancellationToken, ValueTask>?
                    OnRejected => (context, cancellationToken) =>
                    {
                        // Looogss...
                        return new();
                    };

    public RateLimitPartition<string> GetPartition(HttpContext httpContext)
    {
        return RateLimitPartition.GetFixedWindowLimiter("", _ => new()
        {
            PermitLimit=10,
            Window=TimeSpan.FromSeconds(10),
            QueueProcessingOrder=QueueProcessingOrder.NewestFirst,
            QueueLimit=4

        });
    }
}
