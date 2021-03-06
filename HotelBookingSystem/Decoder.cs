﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

/// Name:   Cory Siebler
/// ASUID:  1000832292
/// Email:  csiebler@asu.edu
/// Class:  ASU CSE 445 (#11845)
namespace HotelBookingSystem
{
    /// <summary>
    /// The Decoder sends the OrderObject to the HotelSupplier. The decoded object must contain the same values generated by the TravelAgency.
    /// </summary>
    public static class Decoder
    {
        /// <summary>
        /// Utilizes the XmlSerializer to decode the encoded string into an OrderClass.
        /// </summary>
        /// <param name="encodedOrder">String representation of the Order from the Travel Agency</param>
        /// <returns>OrderClass from the encoded string</returns>
        public static OrderClass DecodeOrder(string encodedOrder)
        {
            XmlSerializer deserializer = new XmlSerializer(typeof(OrderClass));

            using (TextReader tr = new StringReader(encodedOrder))
            {
                return (OrderClass) deserializer.Deserialize(tr);
            }
        }
    }
}
