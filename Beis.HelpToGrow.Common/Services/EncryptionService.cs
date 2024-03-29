﻿using Beis.HelpToGrow.Common.Interfaces;
using Microsoft.Extensions.Options;
using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using Beis.HelpToGrow.Common.Config;

namespace Beis.HelpToGrow.Common.Services
{
    public class EncryptionService: IEncryptionService
    {
        private readonly string _salt;
        private readonly int _passwordIterations;
        private readonly string _initialVector;
        private readonly int _keySize;

        public EncryptionService(string salt, int passwordIterations, string initialVector, int keySize)
        {
            _salt = salt;
            _passwordIterations = passwordIterations;
            _initialVector = initialVector;
            _keySize = keySize;
        }

        public EncryptionService(IOptions<EncryptionSettings> options)
        {
            _salt = options.Value.VoucherEncryptionSalt;
            _passwordIterations = options.Value.VoucherEncryptionIteration;
            _initialVector = options.Value.VoucherEncryptionInitialVector;
            _keySize = options.Value.VoucherEncryptionKeySize;
        }

        public string Encrypt(string plainText, string vendorSalt)
        {
            if (string.IsNullOrWhiteSpace(plainText))
            {
                return "";
            }           

            var initialVectorBytes = Encoding.ASCII.GetBytes(_initialVector);
            var saltValueBytes = Encoding.ASCII.GetBytes(_salt);
            var plainTextBytes = Encoding.UTF8.GetBytes(plainText);
            var derivedPassword = new Rfc2898DeriveBytes(vendorSalt, saltValueBytes, _passwordIterations);
            var keyBytes = derivedPassword.GetBytes(_keySize / 8);
            using (var symmetricKey = Aes.Create("AesManaged"))
            {
                symmetricKey.Mode = CipherMode.CBC;
                byte[] cipherTextBytes;
                using (var encryptor = symmetricKey.CreateEncryptor(keyBytes, initialVectorBytes))
                {
                    using (MemoryStream memStream = new MemoryStream())
                    {
                        using (CryptoStream cryptoStream = new CryptoStream(memStream, encryptor, CryptoStreamMode.Write))
                        {
                            cryptoStream.Write(plainTextBytes, 0, plainTextBytes.Length);
                            cryptoStream.FlushFinalBlock();
                            cipherTextBytes = memStream.ToArray();
                            memStream.Close();
                            cryptoStream.Close();
                        }
                    }
                }
                symmetricKey.Clear();

                return Convert.ToBase64String(cipherTextBytes).Replace('+', '-').Replace('/', '_');
            }
                
            
        }

        public string Decrypt(string cipherText, string vendorSalt)
        {
            if (string.IsNullOrWhiteSpace(cipherText))
            {
                return "";
            }

            byte[] initialVectorBytes = Encoding.ASCII.GetBytes(_initialVector);
            byte[] saltValueBytes = Encoding.ASCII.GetBytes(_salt);
            
            cipherText = cipherText.Replace('-', '+').Replace('_', '/');
            byte[] cipherTextBytes = Convert.FromBase64String(cipherText);
            var derivedPassword = new Rfc2898DeriveBytes(vendorSalt, saltValueBytes, _passwordIterations);
            byte[] keyBytes = derivedPassword.GetBytes(_keySize / 8);
            using (var symmetricKey = Aes.Create("AesManaged"))
            {
                symmetricKey.Mode = CipherMode.CBC;
                byte[] initialText = new byte[0];
                int totalRead = 0;
                using (var decryptor = symmetricKey.CreateDecryptor(keyBytes, initialVectorBytes))
                {
                    using (var memStream = new MemoryStream(cipherTextBytes))
                    {
                        using (var cryptoStream = new CryptoStream(memStream, decryptor, CryptoStreamMode.Read))
                        {

                            byte[] decryptedBytes = new byte[cipherTextBytes.Length];
                            var decryptedSpan = decryptedBytes.AsSpan();
                            int read;

                            do
                            {
                                read = cryptoStream.Read(decryptedSpan);
                                decryptedSpan = decryptedSpan.Slice(read);
                                totalRead += read;
                            } while (read != 0);
                            if (totalRead > 0)
                            {
                                //Only use the read bytes else you will have some 0's at the end
                                Array.Resize(ref initialText, totalRead);
                                Array.Copy(decryptedBytes, initialText, totalRead);
                            }
                            System.Diagnostics.Debug.WriteLine(BitConverter.ToString(initialText));
                        }
                    }
                    symmetricKey.Clear();
                    return Encoding.UTF8.GetString(initialText, 0, totalRead);
                }
            }                                
        }
    }
}