﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

/// Name:   Cory Siebler
/// ASUID:  1000832292
/// Email:  csiebler@asu.edu
/// Class:  ASU CSE 445 (#11845)
namespace HotelBookingSystem
{
    /// <summary>
    /// The OrderProcessingThread processes the order, e.g., checks the credit card number and calculates the amount.Creates a Thread to process
    /// the order. Returns a rate based on the following: unitPrice*NoOfRooms + Tax + LocationCharge. The OrderProcessingThread sends a
    /// confirmation to the travel agency and prints the order (on screen).
    /// </summary>
    public class OrderProcessing
    {
        private const double TAX = 0.08;
        private const double LOCATION_CHARGE = 50.00;
        private const string CC_REGEX
            = "^(?:4[0-9]{12}(?:[0-9]{3})?"         // Visa
            + "|  5[1-5][0-9]{14}"                  // MasterCard
            + "|  3[47][0-9]{13}"                   // American Express
            + "|  3(?:0[0-5]|[68][0-9])[0-9]{11}"   // Diners Club
            + "|  6(?:011|5[0-9]{2})[0-9]{12}"      // Discover
            + "|  (?:2131|1800|35\\d{3})\\d{11}"    // JCB
            + ")$";

        private const string CC_REGEX2
            = "^(?:4[0-9]{12}(?:[0-9]{3})?|5[1-5][0-9]{14}|6(?:011|5[0-9][0-9])[0-9]{12}|3[47][0-9]{13}|3(?:0[0-5]|[68][0-9])[0-9]{11}|(?:2131|1800|35\\d{3})\\d{11})$";

        private OrderClass order;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="order"></param>
        public OrderProcessing(OrderClass order)
        {
            this.Order = order;
        }

        /// <summary>
        /// 
        /// </summary>
        public void ProcessOrder()
        {
            if (order != null)
            {
                Console.WriteLine("PROCESSING: Travel Agency Order ({0})", order.ToString());

                // Check for a valid credit card number
                if (ValidateCreditCard(order.CardNo))
                {
                    Console.WriteLine("VALIDATED: Credit Card Number Valid");
                }
                else
                {
                    Console.WriteLine("INVALIDATED: Credit Card Number Not Valid");
                    return;
                }

                Console.WriteLine("PROCESSED: Travel Agency Order ({0})", order.ToString());
            }
            else
            {
                Console.WriteLine("PROCESSING: No order received");
            }
        }

        /// <summary>
        /// Validates a Credit Card number using a Regular Expression. Allows for Visa, Mastercard, AMEX, Discover, JCB, and Diner's Club cards.
        /// A list of valid numbers is found in the TravelAgency class.
        /// </summary>
        /// <param name="ccNum"></param>
        /// <returns></returns>
        private bool ValidateCreditCard(long ccNum)
        {
            return Regex.IsMatch(ccNum.ToString(), CC_REGEX2);
        }

        /// <summary>
        /// 
        /// </summary>
        public OrderClass Order
        {
            get { return order; }
            private set { order = value; }
        }

    }
}
