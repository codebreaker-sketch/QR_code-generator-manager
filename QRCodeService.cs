

//using QRCoder;
//using qr_website.Models;
//using qr_website.Data;
//using Microsoft.EntityFrameworkCore;

//namespace qr_website.Services
//{
//    public class QRCodeService
//    {
//        private readonly QRCodeDbContext _context;

//        public QRCodeService(QRCodeDbContext context)
//        {
//            _context = context;
//        }

//        public async Task<IEnumerable<QRCodeEntry>> GetAll() =>
//            await _context.QRCodes.ToListAsync();

//        public async Task<QRCodeEntry?> GetById(Guid id) =>
//            await _context.QRCodes.FirstOrDefaultAsync(q => q.Id == id);

//        public async Task<QRCodeEntry> AddQRCode(string name, string data, QRCodeType type)
//        {
//            using var qrGenerator = new QRCodeGenerator();

//            // Build type-specific QR payload
//            string qrPayload = type switch
//            {
//                QRCodeType.WiFi => BuildWiFiPayload(data),
//                QRCodeType.Email => $"mailto:{data}",
//                QRCodeType.Phone => $"tel:{data}",
//                _ => data
//            };

//            var qrData = qrGenerator.CreateQrCode(qrPayload, QRCodeGenerator.ECCLevel.Q);
//            using var qrCode = new PngByteQRCode(qrData);
//            var qrBytes = qrCode.GetGraphic(20);
//            var base64 = Convert.ToBase64String(qrBytes);

//            var newEntry = new QRCodeEntry
//            {
//                Id = Guid.NewGuid(),
//                Name = name,
//                Data = qrPayload,
//                ImageBase64 = base64,
//                Type = type,
//                CreatedAt = DateTime.Now
//            };

//            _context.QRCodes.Add(newEntry);
//            await _context.SaveChangesAsync();

//            return newEntry;
//        }

//        public async Task<IEnumerable<QRCodeEntry>> Search(string keyword) =>
//            await _context.QRCodes
//                .Where(q =>
//                    q.Name.Contains(keyword) ||
//                    q.Id.ToString().Contains(keyword))
//                .ToListAsync();

//        private string BuildWiFiPayload(string rawData)
//        {
//            // Expects data in format: SSID;PASSWORD;AUTH
//            var parts = rawData.Split(';');
//            var ssid = parts.Length > 0 ? parts[0] : "";
//            var password = parts.Length > 1 ? parts[1] : "";
//            var auth = parts.Length > 2 ? parts[2] : "WPA";

//            return $"WIFI:S:{ssid};T:{auth};P:{password};;";
//        }
//    }
//}


using QRCoder;
using qr_website.Models;
using qr_website.Data;
using Microsoft.EntityFrameworkCore;

namespace qr_website.Services
{
    public class QRCodeService
    {
        private readonly QRCodeDbContext _context;

        public QRCodeService(QRCodeDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<QRCodeEntry>> GetAll() =>
            await _context.QRCodes.ToListAsync();

        public async Task<QRCodeEntry?> GetById(Guid id) =>
            await _context.QRCodes.FirstOrDefaultAsync(q => q.Id == id);

        public async Task<QRCodeEntry> AddQRCode(string name, string data, QRCodeType type)
        {
            using var qrGenerator = new QRCodeGenerator();

            // Build type-specific QR payload
            string qrPayload = type switch
            {
                QRCodeType.WiFi => BuildWiFiPayload(data),
                QRCodeType.Email => $"mailto:{data}",
                QRCodeType.Phone => $"tel:{data}",
                _ => data
            };

            var qrData = qrGenerator.CreateQrCode(qrPayload, QRCodeGenerator.ECCLevel.Q);
            using var qrCode = new PngByteQRCode(qrData);
            var qrBytes = qrCode.GetGraphic(20);
            var base64 = Convert.ToBase64String(qrBytes);

            var newEntry = new QRCodeEntry
            {
                Id = Guid.NewGuid(),
                Name = name,
                Data = qrPayload,
                ImageBase64 = base64,
                Type = type,
                CreatedAt = DateTime.Now
            };

            _context.QRCodes.Add(newEntry);
            await _context.SaveChangesAsync();

            return newEntry;
        }

        public async Task<IEnumerable<QRCodeEntry>> Search(string keyword) =>
            await _context.QRCodes
                .Where(q =>
                    q.Name.Contains(keyword) ||
                    q.Id.ToString().Contains(keyword))
                .ToListAsync();

        public async Task DeleteQRCode(Guid id)
        {
            var qrCode = await _context.QRCodes.FindAsync(id);
            if (qrCode != null)
            {
                _context.QRCodes.Remove(qrCode);
                await _context.SaveChangesAsync();
            }
        }


        private string BuildWiFiPayload(string rawData)
        {
            // Expects data in format: SSID;PASSWORD;AUTH
            var parts = rawData.Split(';');
            var ssid = parts.Length > 0 ? parts[0] : "";
            var password = parts.Length > 1 ? parts[1] : "";
            var auth = parts.Length > 2 ? parts[2] : "WPA";

            return $"WIFI:S:{ssid};T:{auth};P:{password};;";
        }
    }
}
