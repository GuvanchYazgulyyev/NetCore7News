using Microsoft.AspNetCore.RateLimiting;
using Microsoft.Extensions.Options;
using System.Threading.RateLimiting;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

#region ReteLimiter !

#region Fixed Window
//builder.Services.AddRateLimiter(options =>
//{  // Burada AddFixedWindowLimiter Algoritmasý Kullanýlýyor!!!
//    options.AddFixedWindowLimiter("Fixed", _options =>
//    {
//        _options.Window = TimeSpan.FromSeconds(15); // Her 10 Saniyede Buradaki politika geçerli olacaktýr!
//        _options.PermitLimit = 4; // Her 12 Saniyede Ne Kadarlýk Request Gönderme Hakkýmýz Olacak? 4
//        _options.QueueLimit = 2; // 12 saniye içersinde 4 ten fazla req gönderildiyse bunlarýn Kaç Tanesini Kuyruga Alalým?
//        _options.QueueProcessingOrder = QueueProcessingOrder.OldestFirst; // Eski Req den Baþlar
//    });
//});

#endregion

#region Sliding Window!


//builder.Services.AddRateLimiter(options =>
//{
//    options.AddSlidingWindowLimiter("Sliding", _options =>
//    {
//        _options.Window = TimeSpan.FromSeconds(20);// Ne Kadar Periotda Bir Ýþlem Yapýlacak ?
//        _options.PermitLimit = 4;// Her bir 20 saniyede 4 Req Ýþlenecegini Belirtiyoruz!
//        _options.QueueProcessingOrder = QueueProcessingOrder.NewestFirst;// Eskisinden Baþlat
//        _options.QueueLimit = 5; // Fazlalýk Olanlardan En Fazla 5 Tanesini Q' E Alalým
//        _options.SegmentsPerWindow= 5;// Her bir Periodun Kendisinden Önceki Sürece Nekdarlýk Kota harcýycagýný Ýfade Eder!

//    });
//});

#endregion

#region Token Bucket

//builder.Services.AddRateLimiter(options =>
//{
//    options.AddTokenBucketLimiter("Token", _options =>
//    {
//        _options.TokenLimit = 15;// Limiti Belirtiyoruz!
//        _options.TokensPerPeriod = 15; // Her Per Üretilecek Token Max 15
//        _options.QueueProcessingOrder = QueueProcessingOrder.NewestFirst; // Eskisinden Çagýr
//        _options.QueueLimit = 5;
//        _options.ReplenishmentPeriod = TimeSpan.FromSeconds(5); // Ne kdarlýk Süreye Bölüyoruz?
//    });
//});


#endregion


#region Concurrency
//builder.Services.AddRateLimiter(options =>
//{
//    options.AddConcurrencyLimiter("Concurrency", _options =>
//    {
//        _options.PermitLimit = 10;
//        _options.QueueLimit = 5;
//        _options.QueueProcessingOrder = QueueProcessingOrder.NewestFirst;
//    });
//});
#endregion


#endregion


#region OnRejected porperty

//builder.Services.AddRateLimiter(options =>
//{  // Burada AddFixedWindowLimiter Algoritmasý Kullanýlýyor!!!
//    options.AddFixedWindowLimiter("Fixed", _options =>
//    {
//        _options.Window = TimeSpan.FromSeconds(15); // Her 10 Saniyede Buradaki politika geçerli olacaktýr!
//        _options.PermitLimit = 4; // Her 12 Saniyede Ne Kadarlýk Request Gönderme Hakkýmýz Olacak? 4
//        _options.QueueLimit = 2; // 12 saniye içersinde 4 ten fazla req gönderildiyse bunlarýn Kaç Tanesini Kuyruga Alalým?
//        _options.QueueProcessingOrder = QueueProcessingOrder.OldestFirst; // Eski Req den Baþlar
//    });

//    options.OnRejected = (context, cancellationToken) =>
//    {
//        // Looogss...
//        return new();
//    };

//});

#endregion


#region Özelleþtirilmiþ Rate Limite Policy Oluþturma
builder.Services.AddRateLimiter(options =>
{
    options.AddPolicy<string, CustomRateLimiter>("Custom");
});
#endregion



var app = builder.Build();


// Minimal API ' lerde  Rate Limiting
app.MapGet("/", () =>
{

}).RequireRateLimiting(".....");


app.UseRateLimiter();
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
