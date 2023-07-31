//BEGIN LICENSE BLOCK 
//Interneuron Synapse

//Copyright(C) 2023  Interneuron Holdings Ltd

//This program is free software: you can redistribute it and/or modify
//it under the terms of the GNU General Public License as published by
//the Free Software Foundation, either version 3 of the License, or
//(at your option) any later version.

//This program is distributed in the hope that it will be useful,
//but WITHOUT ANY WARRANTY; without even the implied warranty of
//MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.

//See the
//GNU General Public License for more details.

//You should have received a copy of the GNU General Public License
//along with this program.If not, see<http://www.gnu.org/licenses/>.
//END LICENSE BLOCK 
﻿//Interneuron Synapse

//Copyright(C) 2021  Interneuron CIC

//This program is free software: you can redistribute it and/or modify
//it under the terms of the GNU General Public License as published by
//the Free Software Foundation, either version 3 of the License, or
//(at your option) any later version.

//This program is distributed in the hope that it will be useful,
//but WITHOUT ANY WARRANTY; without even the implied warranty of
//MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.

//See the
//GNU General Public License for more details.

//You should have received a copy of the GNU General Public License
//along with this program.If not, see<http://www.gnu.org/licenses/>.


﻿using System;
using System.Security.Cryptography.X509Certificates;
using System.Security.Cryptography.Xml;
using System.Xml;

namespace Interneuron.CareRecord.API.AppCode.Formatters
{
    // This code contains parts of the code found at
    // http://www.wiktorzychla.com/2012/12/interoperable-xml-digital-signatures-c_20.html

    public class XmlSignatureHelper
        {
            public static bool VerifySignature(string xml)
            {
                if (xml == null) throw new ArgumentNullException("xml");

                XmlDocument doc = new XmlDocument();
                doc.PreserveWhitespace = true;
                doc.LoadXml(xml);

                // If there's no signature => return that we are "valid"
                XmlNode signatureNode = findSignatureElement(doc);
                if (signatureNode == null) return true;

                SignedXml signedXml = new SignedXml(doc);
                signedXml.LoadXml((XmlElement)signatureNode);

                //var x509Certificates = signedXml.KeyInfo.OfType<KeyInfoX509Data>();
                //var certificate = x509Certificates.SelectMany(cert => cert.Certificates.Cast<X509Certificate2>()).FirstOrDefault();

                //if (certificate == null) throw new InvalidOperationException("Signature does not contain a X509 certificate public key to verify the signature");
                //return signedXml.CheckSignature(certificate, true);

                return signedXml.CheckSignature();
            }


            private static XmlNode findSignatureElement(XmlDocument doc)
            {
                var signatureElements = doc.DocumentElement.GetElementsByTagName("Signature", "http://www.w3.org/2000/09/xmldsig#");
                if (signatureElements.Count == 1)
                    return signatureElements[0];
                else if (signatureElements.Count == 0)
                    return null;
                else
                    throw new InvalidOperationException("Document has multiple xmldsig Signature elements");
            }


            public static bool IsSigned(string xml)
            {
                if (xml == null) throw new ArgumentNullException("xml");

                // First, a quick check, before reading the full document
                if (!xml.Contains("Signature")) return false;

                var doc = new XmlDocument();
                doc.LoadXml(xml);
                return findSignatureElement(doc) != null;
            }


            public static string Sign(string xml, X509Certificate2 certificate)
            {
                if (xml == null) throw new ArgumentNullException("xml");
                if (certificate == null) throw new ArgumentNullException("certificate");
                if (!certificate.HasPrivateKey) throw new ArgumentException("Certificate should have a private key", "certificate");

                XmlDocument doc = new XmlDocument();

                doc.PreserveWhitespace = true;
                doc.LoadXml(xml);

                SignedXml signedXml = new SignedXml(doc);
                signedXml.SigningKey = certificate.PrivateKey;

                // Attach certificate KeyInfo
                KeyInfoX509Data keyInfoData = new KeyInfoX509Data(certificate);
                KeyInfo keyInfo = new KeyInfo();
                keyInfo.AddClause(keyInfoData);
                signedXml.KeyInfo = keyInfo;

                // Attach transforms
                var reference = new Reference("");
                reference.AddTransform(new XmlDsigEnvelopedSignatureTransform(includeComments: false));
                reference.AddTransform(new XmlDsigExcC14NTransform(includeComments: false));
                signedXml.AddReference(reference);

                // Compute signature
                signedXml.ComputeSignature();
                var signatureElement = signedXml.GetXml();

                // Add signature to bundle
                doc.DocumentElement.AppendChild(doc.ImportNode(signatureElement, true));

                return doc.OuterXml;
            }

        }

    }


