using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Mix.Lib.Services.Compliance;

namespace Mix.Database.EntityConfigurations.Converters
{
    public class EncryptedStringConverter : ValueConverter<string, string>
    {
        public EncryptedStringConverter(IFieldEncryptionService encryptionService)
            : base(
                v => encryptionService.Encrypt(v),
                v => encryptionService.Decrypt(v))
        {
        }
    }

    public class EncryptedNullableStringConverter : ValueConverter<string?, string?>
    {
        public EncryptedNullableStringConverter(IFieldEncryptionService encryptionService)
            : base(
                v => v != null ? encryptionService.Encrypt(v) : v,
                v => v != null ? encryptionService.Decrypt(v) : v)
        {
        }
    }
}