﻿using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace chaeHa_Helper
{
    [Serializable]
    public struct Crypto
    {
        public string iv;
        public string key;

        public Crypto(string _iv, string _key)
        {
            iv = _iv;
            key = _key;
        }

        public bool isEmpty()
        {
            if (string.IsNullOrEmpty(iv) || string.IsNullOrEmpty(key))
                return true;

            return false;
        }

        public void set(Crypto crypto)
        {
            iv = crypto.iv;
            key = crypto.key;
        }

        public static Crypto empty()
        {
            return new Crypto();
        }
    }

    public class chaeHa_AES
    {
        public static string Encode(string strCryptoText, Crypto crypto)
        {
            string strRet;
            byte[] byText;

            if (string.IsNullOrEmpty(strCryptoText) || string.IsNullOrEmpty(strCryptoText))
                return null;

            try
            {
                Encoding encoding = new UTF8Encoding(false);
                byText = encoding.GetBytes(strCryptoText);

                using (RijndaelManaged rijndael = new RijndaelManaged())
                {
                    rijndael.Mode = CipherMode.CBC;
                    rijndael.IV = Convert.FromBase64String(crypto.iv);
                    rijndael.Key = Convert.FromBase64String(crypto.key);

                    ICryptoTransform encryptor = rijndael.CreateEncryptor(rijndael.Key, rijndael.IV);

                    using (MemoryStream memoryStream = new MemoryStream())
                    {
                        using (CryptoStream cryptoStream = new CryptoStream(memoryStream, encryptor, CryptoStreamMode.Write))
                        {
                            cryptoStream.Write(byText, 0, byText.Length);
                            cryptoStream.FlushFinalBlock();

                            byte[] byChiperText = memoryStream.ToArray();

                            strRet = Convert.ToBase64String(byChiperText);
                        }
                    }
                }
            }
            catch
            {
                return null;
            }

            return strRet;
        }

        static public string Decode(string strCryptoText, Crypto crypto)
        {
            string strRet;
            byte[] byText;

            if (string.IsNullOrEmpty(strCryptoText) || string.IsNullOrEmpty(strCryptoText))
                return null;

            try
            {
                byText = Convert.FromBase64String(strCryptoText);

                using (RijndaelManaged rijndael = new RijndaelManaged())
                {
                    rijndael.Mode = CipherMode.CBC;
                    rijndael.IV = Convert.FromBase64String(crypto.iv);
                    rijndael.Key = Convert.FromBase64String(crypto.key);

                    ICryptoTransform decryptor = rijndael.CreateDecryptor(rijndael.Key, rijndael.IV);

                    using (MemoryStream memoryStream = new MemoryStream(byText))
                    {
                        using (CryptoStream cryptoStream = new CryptoStream(memoryStream, decryptor, CryptoStreamMode.Read))
                        {
                            byte[] byChiperText = new byte[byText.Length];

                            int nDecryptLength = cryptoStream.Read(byChiperText, 0, byChiperText.Length);

                            if (isUseBom)
                            {
                                strRet = UTF8Encoding.UTF8.GetString(byChiperText, 0, nDecryptLength);
                            }
                            else
                            {
                                Encoding encoding = new UTF8Encoding(false);
                                strRet = encoding.GetString(byChiperText, 0, nDecryptLength);
                            }
                        }
                    }
                }
            }
            catch
            {
                return null;
            }

            return strRet;
        }
    }
}