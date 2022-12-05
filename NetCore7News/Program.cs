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
//{  // Burada AddFixedWindowLimiter Algoritmas� Kullan�l�yor!!!
//    options.AddFixedWindowLimiter("Fixed", _options =>
//    {
//        _options.Window = TimeSpan.FromSeconds(15); // Her 10 Saniyede Buradaki politika ge�erli olacakt�r!
//        _options.PermitLimit = 4; // Her 12 Saniyede Ne Kadarl�k Request G�nderme Hakk�m�z Olacak? 4
//        _options.QueueLimit = 2; // 12 saniye i�ersinde 4 ten fazla req g�nderildiyse bunlar�n Ka� Tanesini Kuyruga Alal�m?
//        _options.QueueProcessingOrder = QueueProcessingOrder.OldestFirst; // Eski Req den Ba�lar
//    });
//});

#endregion

#region Sliding Window!


//builder.Services.AddRateLimiter(options =>
//{
//    options.AddSlidingWindowLimiter("Sliding", _options =>
//    {
//        _options.Window = TimeSpan.FromSeconds(20);// Ne Kadar Periotda Bir ��lem Yap�lacak ?
//        _options.PermitLimit = 4;// Her bir 20 saniyede 4 Req ��lenecegini Belirtiyoruz!
//        _options.QueueProcessingOrder = QueueProcessingOrder.NewestFirst;// Eskisinden Ba�lat
//        _options.QueueLimit = 5; // Fazlal�k Olanlardan En Fazla 5 Tanesini Q' E Alal�m
//        _options.SegmentsPerWindow= 5;// Her bir Periodun Kendisinden �nceki S�rece Nekdarl�k Kota harc�ycag�n� �fade Eder!

//    });
//});

#endregion

#region Token Bucket

//builder.Services.AddRateLimiter(options =>
//{
//    options.AddTokenBucketLimiter("Token", _options =>
//    {
//        _options.TokenLimit = 15;// Limiti Belirtiyoruz!
//        _options.TokensPerPeriod = 15; // Her Per �retilecek Token Max 15
//        _options.QueueProcessingOrder = QueueProcessingOrder.NewestFirst; // Eskisinden �ag�r
//        _options.QueueLimit = 5;
//        _options.ReplenishmentPeriod = TimeSpan.FromSeconds(5); // Ne kdarl�k S�reye B�l�yoruz?
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
//{  // Burada AddFixedWindowLimiter Algoritmas� Kullan�l�yor!!!
//    options.AddFixedWindowLimiter("Fixed", _options =>
//    {
//        _options.Window = TimeSpan.FromSeconds(15); // Her 10 Saniyede Buradaki politika ge�erli olacakt�r!
//        _options.PermitLimit = 4; // Her 12 Saniyede Ne Kadarl�k Request G�nderme Hakk�m�z Olacak? 4
//        _options.QueueLimit = 2; // 12 saniye i�ersinde 4 ten fazla req g�nderildiyse bunlar�n Ka� Tanesini Kuyruga Alal�m?
//        _options.QueueProcessingOrder = QueueProcessingOrder.OldestFirst; // Eski Req den Ba�lar
//    });

//    options.OnRejected = (context, cancellationToken) =>
//    {
//        // Looogss...
//        return new();
//    };

//});

#endregion


#region �zelle�tirilmi� Rate Limite Policy Olu�turma
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
