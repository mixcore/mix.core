'use strict';
function CryptoJSCipher(mode, padding, Cipher) {
    var mode = mode;
    var padding = padding;
    var Cipher = Cipher;
    return {
        encode: function(plainValue, base64Key) {
                    if (!plainValue) { return plainValue; }
                    if (base64Key.length<=0) return plainValue;
                    
                    var key = CryptoJS.enc.Base64.parse(base64Key);
                    // this is the decrypted data as a sequence of bytes
                    var encryprtedData = Cipher.encrypt(plainValue, key, {
                        mode: mode,
                        padding: padding
                    });
                    //var encryprtedValue = encryprtedData.toString( CryptoJS.enc.Base64);
                    return encryprtedData.toString();
                },
        decode : function(encryptedValue, base64Key) {
                    if (base64Key.length<=0) return encryptedValue;
                    var key = CryptoJS.enc.Base64.parse(base64Key);
                    // this is the decrypted data as a sequence of bytes
                    var decryptedData = Cipher.decrypt(encryptedValue, key, {
                        mode: mode,
                        padding: padding
                    });
                    return  decryptedData.toString(CryptoJS.enc.Utf8);
                }
    }
}