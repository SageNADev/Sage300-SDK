// The MIT License (MIT) 
// Copyright (c) 2024 The Sage Group plc or its licensors.  All rights reserved.
// 
// Permission is hereby granted, free of charge, to any person obtaining a copy of 
// this software and associated documentation files (the "Software"), to deal in 
// the Software without restriction, including without limitation the rights to use, 
// copy, modify, merge, publish, distribute, sublicense, and/or sell copies of the 
// Software, and to permit persons to whom the Software is furnished to do so, 
// subject to the following conditions:
// 
// The above copyright notice and this permission notice shall be included in all 
// copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, 
// INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A 
// PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT 
// HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF 
// CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE 
// OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.

#region Imports
using System;
using System.Text;
using System.Security.Cryptography;
using System.IO;
#endregion

namespace Sage.CA.SBS.ERP.Sage300.ProxyTester.Utility
{
    /// <summary>
    /// This class has methods to encrypt/decrpyt a string using a 
    /// key string and a salt string
    /// </summary>
    public class StringEncryptor
    {
        #region Private Constants
        private static readonly int SaltTextLengthThreshold = 4;
        #endregion

        /// <summary>
        /// Encrypt a string
        /// </summary>
        /// <param name="clearText">plain text for encryption</param>
        /// <param name="encryptionKey">key</param>
        /// <param name="saltText">salt</param>
        /// <returns></returns>
        public static string Encrypt(string clearText, string encryptionKey, string saltText)
        {
            byte[] clearBytes = Encoding.Unicode.GetBytes(clearText);
            byte[] salt;

            if (saltText.Length < SaltTextLengthThreshold)
            {
                salt = Encoding.Unicode.GetBytes(String.Concat(System.Linq.Enumerable.Repeat(saltText, (4 + saltText.Length - 1) / saltText.Length)));
            }
            else
            {
                salt = Encoding.Unicode.GetBytes(saltText);
            }

            using (var encryptor = Aes.Create())
            {
                Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(encryptionKey, salt);
                encryptor.Key = pdb.GetBytes(32);
                encryptor.IV = pdb.GetBytes(16);
                encryptor.Mode = CipherMode.CFB;
                using (var ms = new MemoryStream())
                {
                    using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateEncryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(clearBytes, 0, clearBytes.Length);
                        cs.Close();
                    }
                    clearText = Convert.ToBase64String(ms.ToArray());
                }
            }
            return clearText;
        }
    }
}