﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

/// Name:   Cory Siebler
/// ASUID:  1000832292
/// Email:  csiebler@asu.edu
/// Class:  ASU CSE 445 (#11845)
namespace HotelBookingSystem
{
    /// <summary>
    /// A TravelAgency evaluates the price, generates an OrderObject (consisting of multiple values), and sends the order to the Encoder to
    /// convert the order object into a plain string. The TravelAgency sends the encoded string to one of the free cells in the 
    /// MultiCellBuffer.
    /// </summary>
    public class TravelAgency
    {
        private const int BASE_ROOM_ORDER = 10;
        private const int BULK_ROOM_ORDER = 25;

        // Array of Credit Cards for Testing
        private static readonly long[] CC_NUMS =
        {
            4916039504020044,   // Visa
            5289332084535168,   // Mastercard
            374951333742767,    // American Express
            30366099125857,     // Diner's Club
            6011354933529823,   // Discover
            3553991022581867    // JCB
        };

        private static bool hotelsActive = true;
        private static Random random = new Random(); // Random number generator
        private bool roomsNeeded = true;
        private bool bulkOrder = false;
        private double unitPrice;
        private string hotelId;

        /// <summary>
        /// Execution thread for the TravelAgency class. Creates a base order continuously until HotelSupplier threads are no longer active.
        /// </summary>
        public void Run()
        {
            // Continue thread until Hotels are no longer active
            while (hotelsActive)
            {
                // Check if an order needs to be created
                if (roomsNeeded)
                {
                    if (bulkOrder)
                    {
                        CreateBulkOrder(hotelId);
                    }
                    else
                    {
                        CreateBaseOrder(hotelId);
                    }
                }
                else
                {
                    // No orders are needed so sleep the thread for some time
                    Console.WriteLine("WAITING: Travel Agency Thread ({0})", Thread.CurrentThread.Name);
                    Thread.Sleep(1000);
                    roomsNeeded = true;
                }
            }

            Console.WriteLine("CLOSING: Travel Agency Thread ({0})", Thread.CurrentThread.Name);
        }

        /// <summary>
        /// Hook the PriceCut event to the CreateBulkOrder method, so a large order will be placed once the event is fired.
        /// </summary>
        /// <param name="hotel">Hotel to subscibe to price cut events</param>
        public void Subscribe(HotelSupplier hotelSupplier)
        {
            Console.WriteLine("SUBSCRIBING: Price Cut Event");
            hotelSupplier.PriceCut += IssueBulkOrder;
        }

        /// <summary>
        /// Called once the TravelAgency thread has come back from sleeping. Orders BASE_ROOM_ORDER rooms.
        /// </summary>
        /// <param name="hotelId">Receiver ID for the Hotel</param>
        private void CreateBaseOrder(string hotelId)
        {
            Console.WriteLine("CREATING: Base Order ({0})", Thread.CurrentThread.Name);

            // Tell system no order is needed
            roomsNeeded = false;
            OrderClass order = new OrderClass();
            order.Amount = BASE_ROOM_ORDER;
            order.CardNo = CC_NUMS[random.Next(0, CC_NUMS.Length)];
            order.SenderId = Thread.CurrentThread.Name;
            order.ReceiverId = hotelId;

            Program.mb.setOneCell(Encoder.EncodeOrder(order));
        }

        /// <summary>
        /// Called once a PriceCut event occurs. Orders BULK_ROOM_ORDER rooms.
        /// </summary>
        /// <param name="hotelId">Receiver ID for the Hotel</param>
        private void CreateBulkOrder(string hotelId)
        {
            Console.WriteLine("CREATING: Bulk Order ({0})", Thread.CurrentThread.Name);
            
            // Tell system no order is needed
            roomsNeeded = false;
            OrderClass order = new OrderClass();
            order.Amount = BULK_ROOM_ORDER;
            order.CardNo = CC_NUMS[random.Next(0, CC_NUMS.Length)];
            order.SenderId = Thread.CurrentThread.Name;
            order.ReceiverId = hotelId;

            Program.mb.setOneCell(Encoder.EncodeOrder(order));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="e"></param>
        public void IssueBulkOrder(PriceCutEventArgs e)
        {
            bulkOrder = true;
            hotelId = e.Id;
            unitPrice = e.Price;
        }

        /// <summary>
        /// Accessor/Mutator
        /// </summary>
        public static bool HotelsActive
        {
            get { return TravelAgency.hotelsActive; }
            set { TravelAgency.hotelsActive = value; }
        }
    }
}
